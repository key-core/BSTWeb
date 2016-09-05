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
    public class AprDistribucionController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            if (db.SP_MWS_GetAprDistribucion_AprDis().First() == 1)
            {
                TempData["notice"] = "Ya fue aprobada la distribución para este ciclo.";
                return RedirectToAction("Index", "Home");
            }
            else
                return View();
        }

        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult ResumenDist()
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
        public ActionResult GetAllResumenDist([DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var lista = db.SP_MWS_ResumenDistribuciones_AprDis().ToList();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_Usuario,
                record.FE_Registro
            }));
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var lista = db.SP_MWS_ArticulosDistribucion_AprDis().ToList();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_Articulo,
                record.TX_IDArticuloCliente,
                record.TX_Articulo,
                record.NU_UnidadEntregar
            }));
        }

        // Action method to delete Register record from database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, SP_MWS_ArticulosDistribucion_AprDis_Result record)
        {
            // Test if Register object and modelstate is valid.           
            if (record != null && ModelState.IsValid)
            {
                // Delete Register from UoW.
                db.SP_MWS_DeleteDistArticulo_AprDis(record.ID_Articulo);
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        public void SetAprDistribucion()
        {
            db.SP_MWS_SetAprDistribucion_AprDis((Int32)Membership.GetUser().ProviderUserKey);
        }
    }
}