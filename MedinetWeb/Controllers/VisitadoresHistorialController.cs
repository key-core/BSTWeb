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
    public class VisitadoresHistorialController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();
        private MedinetWebLimitedEntities db2 = new MedinetWebLimitedEntities();

        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            ViewData["dvVisitadores"] = db.SP_MW_Visitadores().ToList()
                .Select(e => new
                {
                    ID = e.ID_Usuario,
                    TX = e.TX_Nombre + " " + e.TX_Apellido
                });

            ViewData["dvRegiones"] = db.MW_Regiones.ToList()
                .Select(e => new
                {
                    ID = e.ID_Region,
                    TX = e.TX_Region
                });

            ViewData["dvLineas"] = db.MW_Lineas.ToList()
                .Select(e => new
                {
                    ID = e.ID_Linea,
                    TX = e.TX_Linea
                });

            ViewData["dvZonas"] = db.SP_MW_Zonas_Zon().ToList()
                .OrderBy(e => e.TX_Zona)
                .Select(c => new
                {
                    TX = c.TX_Zona,
                    ID = c.ID_Zona.ToString()
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
            var lista = _database.VisitadoresHistorial.GetAll();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_VisitadoresHistorial,
                record.ID_Visitador,
                record.ID_Region,
                record.ID_Linea,
                record.ID_Zona,
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
        public ActionResult Add([DataSourceRequest] DataSourceRequest request, MW_VisitadoresHistorial record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Add Register to UoW.
                _database.VisitadoresHistorial.Add(record);
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
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MW_VisitadoresHistorial record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Update Register to UoW.
                _database.VisitadoresHistorial.Update(record);
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
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, MW_VisitadoresHistorial record)
        {
            // Test if Register object and modelstate is valid.           
            if (record != null && ModelState.IsValid)
            {
                // Delete Register from UoW.
                _database.VisitadoresHistorial.Delete(record);
                // Call to database and delete the deleted record.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetVisitadores()
        {
            IEnumerable<SelectListItem> listRecord = db.SP_MW_Visitadores()
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
        public JsonResult GetVisitadores_FicVis(Int16 IDCiclo, Int16 IDLinea, Int16 IDRegion)
        {
            IEnumerable<SelectListItem> listRecord = db.SP_MW_Visitadores_FicVis(IDCiclo, IDLinea, IDRegion).ToList()
                .OrderBy(c => c.TX_Nombre)
                .AsEnumerable().Select(c => new SelectListItem()
                {
                    Text = c.TX_Nombre + " " + c.TX_Apellido,
                    Value = c.ID_Visitador.ToString()
                });

            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetVisitadores_FicVi2(Int16 IDCiclo, Int16 IDLinea, Int16 IDRegion)
        {
            IEnumerable<SelectListItem> listRecord = db2.SP_MW_Visitadores_FicVis2(IDCiclo, IDLinea, IDRegion).ToList()
                .OrderBy(c => c.TX_Nombre)
                .AsEnumerable().Select(c => new SelectListItem()
                {
                    Text = c.TX_Nombre + " " + c.TX_Apellido,
                    Value = c.ID_Visitador.ToString()
                });

            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetVisitadores_VisMed(Int16 IDCiclo, Int16 IDLinea, Int16 IDRegion)
        {
            IEnumerable<SelectListItem> listRecord = db.SP_MW_Visitadores_VisMed(IDCiclo, IDLinea, IDRegion).ToList()
                .OrderBy(c => c.TX_Nombre)
                .AsEnumerable().Select(c => new SelectListItem()
                {
                    Text = c.TX_Nombre + " " + c.TX_Apellido,
                    Value = c.ID_Visitador.ToString()
                });

            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }
    }
}