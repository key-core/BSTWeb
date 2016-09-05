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
    public class ConfigDistribucionController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            ViewData["dvCiclos"] = db.MW_Ciclos.ToList()
                .Select(e => new
                {
                    ID = e.ID_Ciclo,
                    TX = e.NU_Ano.ToString() + e.NU_Ciclo.ToString()
                });

            ViewData["dvArticulos"] = db.MWS_Articulos.ToList()
                .Select(e => new
                {
                    ID = e.ID_Articulo,
                    TX = e.TX_Articulo
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

            ViewData["dvClasificaciones"] = db.MW_ClasificacionMedicos.ToList()
                .Select(e => new
                {
                    ID = e.ID_Clasificacion,
                    TX = e.TX_Clasificacion
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

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult PreDispensado(int articuloID, string lineasIDs, string especialidadesIDs, int nuNF)
        {
            ViewBag.articuloID = articuloID;
            ViewBag.lineasIDs = lineasIDs;
            ViewBag.especialidadesIDs = especialidadesIDs;
            ViewBag.nuNF = nuNF;

            ViewData["dvLineas"] = db.MW_Lineas.ToList()
                .Select(e => new
                {
                    ID = e.ID_Linea,
                    TX = e.TX_Linea
                });

            ViewData["dvVisitadoresHistorial"] = db.SP_MWS_VisitadoresHistorial_ConDis(lineasIDs, especialidadesIDs).ToList()
                .OrderBy(e => e.ID_VisitadoresHistorial)
                .Select(e => new
                {
                    ID = e.ID_VisitadoresHistorial,
                    TX = e.TX_Nombre.ToString() + " " + e.TX_Apellido.ToString()
                });

            if (db.SP_MWS_GetCierreDist_CieDis((Int32)Membership.GetUser().ProviderUserKey).First() == 1)
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
        public ActionResult GetAll(int articuloID, string lineasIDs, string especialidadesIDs, [DataSourceRequest] DataSourceRequest request)
        {
            // Actualiza la configuración de la distribución
            db.SP_MWS_LoadConfigDistribucion(articuloID, lineasIDs, especialidadesIDs);

            // Retrieve all the Registers from database.
            var recordList = db.SP_MWS_ConfigDistribucion_ConDis(articuloID, lineasIDs, especialidadesIDs).ToList();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_ConfigDistribucion,
                record.ID_Ciclo,
                record.ID_Articulo,
                record.ID_Linea,
                record.ID_Especialidad,
                record.ID_Clasificacion,
                record.NU_CantidadEntregar,
                record.NU_Fichados,
                record.NU_Revisita,
                record.NU_Contactos
            }));
        }

        // Action method to update Register record to database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MWS_ConfigDistribucion record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Update Register to UoW.
                _database.ConfigDistribucion.Update(record);
                // Save updated Register to database using UoW.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
            //return GetAll(request);
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetExistencias(int articuloID, [DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var recordList = db.SP_MWS_Existencias_ConDis(articuloID).ToList();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Articulo,
                record.NU_UnidadManejo,
                record.ID_Ciclo,
                record.TX_Lote,
                record.TX_Ubicacion,
                record.FE_VencimientoLote,
                record.NU_CantidadPrimaria,
                record.NU_CantidadSecundaria,
                record.NU_UltimoCosto
            }));
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetPreDispensado(int articuloID, string lineasIDs, string especialidadesIDs, int nuNF, [DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var recordList = db.SP_MWS_PreDispensado_ConDis(articuloID, lineasIDs, especialidadesIDs, nuNF).ToList();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_VisitadoresHistorial,
                record.ID_Linea,
                record.NU_Contactos,
                record.NU_UnidadAsignada,
                record.NU_UMCalculada,
                record.NU_UnidadEntregar,
                record.NU_UMEntregar
            }));
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetInvPosDispensado(int articuloID, string lineasIDs, string especialidadesIDs, int nuNF, [DataSourceRequest] DataSourceRequest request)
        {
            return Json(db.SP_MWS_InvPosDispensado_ConDis(articuloID, lineasIDs, especialidadesIDs, nuNF).First().Value, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public void SetNotaEntrega(int articuloID, string lineasIDs, string especialidadesIDs, int nuNF, [DataSourceRequest] DataSourceRequest request)
        {
            db.SP_MWS_DeleteConfigDistribucion_ConDis(articuloID, lineasIDs, especialidadesIDs);

            db.SP_MWS_LoadNotaEntregaCAB_ConDis(lineasIDs, especialidadesIDs);

            db.SP_MWS_LoadNotaEntregaDET_ConDis(articuloID, lineasIDs, especialidadesIDs, nuNF);
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public void SetNoContFichero(int articuloID, int nuNF)
        {
            db.SP_MWS_SetNoContFichero_ConDis(articuloID, nuNF);
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetNoContFichero(int articuloID)
        {
            try
            {
                return Json(db.SP_MWS_GetNoContFichero_ConDis(articuloID).First().NU_Cantidad, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}