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
    public class ValidaVisitadoresController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
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

            ViewData["dvVisitadores"] = db.SP_MW_Visitadores().ToList()
                .Select(e => new
                {
                    ID = e.ID_Usuario,
                    TX = e.TX_Nombre + " " + e.TX_Apellido
                });

            if (db.SP_MWS_GetAprDistribucion_AprDis().First() == 1)
            {
                TempData["notice"] = "Ya fue aprobada la distribución para este ciclo.";
                return RedirectToAction("Index", "Home");
            }
            else
                return View();
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var lista = db.SP_MWS_ValidaVisitadores_ValVis().ToList();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_ValidaVisitadores,
                record.ID_CicloDistribucion,
                record.ID_VisitadoresHistorial,
                record.ID_Visitador,
                record.ID_Region,
                record.ID_Linea,
                record.BO_Activo
            }));
        }

        // Action method to update Register record to database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MWS_ValidaVisitadores record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Update Register to UoW.
                _database.ValidaVisitadores.Update(record);
                // Save updated Register to database using UoW.
                _database.Save();

                db.SP_MWS_SetNotaEntregaCAB_ValVis(record.ID_VisitadoresHistorial, record.BO_Activo);
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }
    }
}