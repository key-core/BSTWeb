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
using System.Collections;

namespace MedinetWeb.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class UsuariosResponsabilidadesController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            ViewData["dvUsuarios"] = db.MW_Usuarios.ToList()
                .Select(e => new
                {
                    ID = e.ID_Usuario,
                    TX = e.TX_Nombre + " " + e.TX_Apellido
                });

            ViewData["dvRoles"] = db.MW_Roles.ToList()
                .Select(e => new
                {
                    ID = e.ID_Rol,
                    TX = e.TX_Rol
                });

            ViewData["dvRegiones"] = db.MW_Regiones.ToList()
                .Select(e => new
                {
                    ID = e.ID_Region,
                    TX = e.TX_Region
                });

            ViewData["dvMarcas"] = db.MW_Marcas.ToList()
                .Select(e => new
                {
                    ID = e.ID_Marca,
                    TX = e.TX_Marca
                });

            ViewData["dvLineas"] = db.MW_Lineas.ToList()
                .Select(e => new
                {
                    ID = e.ID_Linea,
                    TX = e.TX_Linea
                });

            ViewData["dvEspecialidades"] = db.MW_Especialidades.ToList()
                .Select(e => new
                {
                    ID = e.ID_Especialidad,
                    TX = e.TX_Especialidad
                });

            ViewData["dvCiclos"] = db.MW_Ciclos.ToList()
                .Select(e => new
                {
                    ID = e.ID_Ciclo,
                    TX = e.NU_Ano.ToString() + e.NU_Ciclo.ToString()
                });

            ViewData["dvCiclosFin"] = db.MW_Ciclos.ToList()
                .Select(e => new
                {
                    ID = e.ID_Ciclo,
                    TX = e.NU_Ano.ToString() + e.NU_Ciclo.ToString()
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
            var lista = _database.UsuariosResponsabilidades.GetAll();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_UsuariosResponsabilidades,
                record.ID_Usuario,
                record.ID_Region,
                record.ID_Marca,
                record.ID_Linea,
                record.ID_CicloIni,
                record.ID_CicloFin,
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
        public ActionResult Add([DataSourceRequest] DataSourceRequest request, MW_UsuariosResponsabilidades record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Add Register to UoW.
                _database.UsuariosResponsabilidades.Add(record);
                // Save Register to database using UoW.
                _database.Save();
            }
            // Return added Register record back to client side in json format and embeded request, modelstate information.
            return Json(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        // Action method to update Register record to database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MW_UsuariosResponsabilidades record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Update Register to UoW.
                _database.UsuariosResponsabilidades.Update(record);
                // Save updated Register to database using UoW.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        // Action method to delete Register record from database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, MW_UsuariosResponsabilidades record)
        {
            // Test if Register object and modelstate is valid.           
            if (record != null && ModelState.IsValid)
            {
                // Delete Register from UoW.
                _database.UsuariosResponsabilidades.Delete(record);
                // Call to database and delete the deleted record.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        //public JsonResult checkUser(string value)
        //{
        //    return Json(db.MW_Usuarios.Any(m => m.TX_Usuario == value), JsonRequestBehavior.AllowGet);
        //}
    }
}