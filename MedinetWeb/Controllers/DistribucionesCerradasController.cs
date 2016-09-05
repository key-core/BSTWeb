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
using System.Web.Security;

namespace MedinetWeb.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class DistribucionesCerradasController : Controller
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
            var lista = db.SP_MWS_CierreDistribucion_DisCer().ToList();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_CierreDistribucion,
                record.ID_Ciclo,
                record.ID_Usuario,
                record.FE_Registro
            }));
        }

        // Action method to delete Register record from database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, MWS_CierreDistribucion record)
        {
            // Test if Register object and modelstate is valid.           
            if (record != null && ModelState.IsValid)
            {
                // Delete Register from UoW.
                _database.CierreDistribucion.Delete(record);
                // Call to database and delete the deleted record.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }
    }
}