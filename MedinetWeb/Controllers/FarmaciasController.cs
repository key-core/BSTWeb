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

namespace MedinetWeb.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class FarmaciasController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador, EditorFarmacias")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            ViewData["dvEstados"] = db.MW_LocacionEstados.ToList()
                .Select(e => new
                {
                    ID = e.ID_Estado,
                    TX = e.TX_Estado
                });

            ViewData["dvEstados"] = db.MW_LocacionEstados.ToList()
                .Select(e => new
                {
                    ID = e.ID_Estado,
                    TX = e.TX_Estado
                });

            ViewData["dvBricks"] = db.MW_LocacionBricks.ToList()
                .Select(e => new
                {
                    ID = e.ID_Brick,
                    TX = e.TX_Brick
                });

            ViewData["dvCadenas"] = db.MW_CadenaFarmacias.ToList()
                .Select(e => new
                {
                    ID = e.ID_Cadena,
                    TX = e.TX_Cadena
                });

            ViewData["dvClasificaciones"] = db.MW_ClasificacionFarmacias.ToList()
                .Select(e => new
                {
                    ID = e.ID_Clasificacion,
                    TX = e.TX_Clasificacion
                });

            return View();
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, EditorFarmacias")]
        [SessionExpireFilter]
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var lista = _database.Farmacias.GetAll();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_Farmacia,
                record.TX_Farmacia,
                record.TX_Rif,
                record.ID_Estado,
                record.ID_Brick,
                record.TX_Ruta,
                record.TX_Direccion,
                record.TX_Telefono1,
                record.TX_Telefono2,
                record.ID_Cadena,
                record.ID_Clasificacion,
                record.FE_Registro,
                record.BO_Activo
            }));
        }

        // Action method to add new registers to databaes.
        // Accecpt only HTTP POST request.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, EditorFarmacias")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add([DataSourceRequest] DataSourceRequest request, MW_Farmacias record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Add Register to UoW.
                _database.Farmacias.Add(record);
                // Save Register to database using UoW.
                _database.Save();
            }
            // Return added Register record back to client side in json format and embeded request, modelstate information.
            return Json(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        // Action method to update Register record to database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, EditorFarmacias")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MW_Farmacias record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Update Register to UoW.
                _database.Farmacias.Update(record);
                // Save updated Register to database using UoW.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        // Action method to delete Register record from database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, EditorFarmacias")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, MW_Farmacias record)
        {
            // Test if Register object and modelstate is valid.           
            if (record != null && ModelState.IsValid)
            {
                // Delete Register from UoW.
                _database.Farmacias.Delete(record);
                // Call to database and delete the deleted record.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetFarmacias()
        {
            IEnumerable<SelectListItem> listRecord = db.MW_Farmacias
                                                        .Where(c => c.BO_Activo == true)
                                                        .AsEnumerable().Select(c => new SelectListItem()
                                                        {
                                                            Text = c.TX_Farmacia,
                                                            Value = c.ID_Farmacia.ToString()
                                                        });
            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }
    }
}