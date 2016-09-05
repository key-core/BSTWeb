using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedinetWeb.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MedinetWeb.Helpers;
using System.Web.Security;
using System.Web.Routing;

namespace MedinetWeb.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class UsuariosController : Controller
    {
        public MedinetWebMembershipProvider MembershipService { get; set; }
        public MedinetWebRoleProvider AuthorizationService { get; set; }
        private AccesoRepository repository;

        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null)
                MembershipService = new MedinetWebMembershipProvider();
            if (AuthorizationService == null)
                AuthorizationService = new MedinetWebRoleProvider();

            base.Initialize(requestContext);
        }

        public UsuariosController()
        {
            this.repository = new AccesoRepository();
        }

        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            ViewData["dvEstados"] = db.MW_LocacionEstados.ToList()
                .Select(e => new
                {
                    ID = e.ID_Estado,
                    TX = e.TX_Estado
                });

            ViewData["dvCargos"] = db.MW_Cargos.ToList()
                .Select(e => new
                {
                    ID = e.ID_Cargo,
                    TX = e.TX_Cargo
                });

            return View();
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var lista = _database.Usuarios.GetAll();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_Usuario,
                record.TX_Usuario,
                record.TX_Clave,
                record.TX_Nombre,
                record.TX_Apellido,
                record.NU_Cedula,
                record.ID_Cargo,
                record.TX_Email1,
                record.TX_Email2,
                record.TX_Telefono1,
                record.TX_Telefono2,
                record.TX_Direccion,
                record.ID_Estado,
                record.BO_Expiro,
                record.TX_Imei,
                record.FE_Registro,
                record.BO_Activo
            }));
        }

        // Action method to add new registers to databaes.
        // Accecpt only HTTP POST request.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add([DataSourceRequest] DataSourceRequest request, MW_Usuarios record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Add Register to UoW.
                _database.Usuarios.Add(record);
                // Save Register to database using UoW.
                _database.Save();

                //Encriptamos el password
                ChangePassword(record.TX_Usuario, record.TX_Clave);
            }
            // Return added Register record back to client side in json format and embeded request, modelstate information.
            //return Json(new[] { record }.ToDataSourceResult(request, ModelState));
            return GetAll(request);
        }

        // Action method to update Register record to database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MW_Usuarios record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(record.TX_Usuario, record.TX_Clave) && record.TX_Clave.Length > 30)
                {
                    // Update Register to UoW.
                    _database.Usuarios.Update(record);
                    // Save updated Register to database using UoW.
                    _database.Save();
                }
                else
                {
                    string newClave = record.TX_Clave;

                    // Update Register to UoW.
                    _database.Usuarios.Update(record);
                    // Save updated Register to database using UoW.
                    _database.Save();

                    //Encriptamos el password
                    ChangePassword(record.TX_Usuario, newClave);
                }
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
            //return GetAll(request);
        }

        // Action method to delete Register record from database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, MW_Usuarios record)
        {
            // Test if Register object and modelstate is valid.           
            if (record != null && ModelState.IsValid)
            {
                // Delete Register from UoW.
                _database.Usuarios.Delete(record);
                // Call to database and delete the deleted record.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetUsuarios()
        {
            IEnumerable<SelectListItem> listRecord = db.MW_Usuarios
                                                        .Where(c => c.BO_Activo == true)
                                                        .AsEnumerable().Select(c => new SelectListItem()
                                                        {
                                                            Text = c.TX_Nombre + " " + c.TX_Apellido,
                                                            Value = c.ID_Usuario.ToString()
                                                        });
            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetUsuariosExist(Int16 ID_Usuario, bool newRegistro)
        {
            IEnumerable<SelectListItem> listRecord;

            if (newRegistro)
                listRecord = db.SP_MW_Usuarios_FicAcc()
                            .Where(c => c.BO_Activo == true)
                            .AsEnumerable().Select(c => new SelectListItem()
                            {
                                Text = c.TX_Nombre + " " + c.TX_Apellido,
                                Value = c.ID_Usuario.ToString()
                            });
            else
                listRecord = db.MW_Usuarios
                                .Where(c => c.ID_Usuario == ID_Usuario)
                                .OrderBy(c => c.TX_Nombre)
                                .ThenBy(c => c.TX_Apellido)
                                .AsEnumerable().Select(c => new SelectListItem()
                                {
                                    Text = c.TX_Nombre + " " + c.TX_Apellido,
                                    Value = c.ID_Usuario.ToString()
                                });

            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Metodo realizado para que interactue con KendoUI (Metodo realizado fuera de Membership porque no el mismo 
        /// no soportaba sobrecarga del metodo.)
        /// </summary>
        /// <param name="Nombre de usuario"></param>
        /// <param name="Clave de usuario"></param>
        /// <returns>Boleano que indica si fue exitoso</returns>
        [Authorize]
        [SessionExpireFilter]
        private bool ChangePassword(string username, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword.Trim()))
                return false;

            MW_Usuarios user = repository.GetUser(username);
            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword.Trim(), "md5");
            //string hash = newPassword.Trim();

            user.TX_Clave = hash;
            repository.Save();

            return true;
        }
    }
}