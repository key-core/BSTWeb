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
    public class CierreDistribucionController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            ViewData["dvMarcas"] = db.MW_Marcas.ToList()
                .Select(e => new
                {
                    ID = e.ID_Marca,
                    TX = e.TX_Marca
                });

            ViewData["dvArticulos"] = db.SP_MWS_Articulos_CieDis((Int32)Membership.GetUser().ProviderUserKey).ToList()
                .Select(e => new
                {
                    ID = e.ID_Articulo,
                    TX = e.TX_Articulo
                });

            ViewData["dvTipoArticulos"] = db.MWS_TipoArticulos.ToList()
                .Select(e => new
                {
                    ID = e.ID_TipoArticulo,
                    TX = e.TX_TipoArticulo
                });

            if (db.SP_MWS_GetAprDistribucion_AprDis().First() == 1)
            {
                TempData["notice"] = "Ya fue aprobada la distribución para este ciclo.";
                return RedirectToAction("Index", "Home");
            }
            else if (db.SP_MWS_GetCierreDist_CieDis((Int32)Membership.GetUser().ProviderUserKey).First() == 1)
            {
                TempData["notice"] = "Ya fue cerrada la distribución para este ciclo.";
                return RedirectToAction("Index", "Home");
            }
            else 
                return View();
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var lista = db.SP_MWS_ResumenCierreDistribucion_CieDis((Int32)Membership.GetUser().ProviderUserKey).ToList();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_Marca,
                record.ID_Articulo,
                record.ID_TipoArticulo,
                record.StockInicial,
                record.Dispensado,
                record.StockFinal
            }));
        }

        /// <summary>
        /// Metodo que registra el cierre de la distribución para un gerente en especifico
        /// </summary>
        public void SetCierreDistribucion()
        {
            db.SP_MWS_SetCierreDistribucion_CieDis((Int32)Membership.GetUser().ProviderUserKey);
        }
    }
}