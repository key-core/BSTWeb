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
    public class AprobacionesController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            ViewData["dvCiclos"] = db.MW_Ciclos.ToList()
                .Select(e => new
                {
                    ID = e.ID_Ciclo,
                    TX = e.NU_Ano.ToString() + e.NU_Ciclo.ToString()
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

            ViewData["dvVisitadoresHistorial"] = db.SP_MW_VisitadoresHistorial_Apr().ToList()
                .OrderBy(e => e.ID_VisitadoresHistorial)
                .Select(e => new
                {
                    ID = e.ID_VisitadoresHistorial,
                    TX = e.TX_Nombre.ToString() + " " + e.TX_Apellido.ToString()
                });

            return View();
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            // Actualiza las aprobaciones para el usuario
            db.SP_MW_LoadAprobaciones((Int32)Membership.GetUser().ProviderUserKey);

            // Retrieve all the Registers from database.
            var recordList = db.SP_MW_Aprobaciones_Apr((Int32)Membership.GetUser().ProviderUserKey).ToList();
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Aprobacion,
                record.ID_Ciclo,
                record.ID_Region,
                record.ID_Linea,
                record.BO_CierreCiclo,
                record.ID_VisitadoresHistorial,
                record.FE_Transmision,
                record.NU_PorcentajeVisita,
                record.NU_PorcentajeRevisita,
                record.NU_MIX,
                record.NU_PDR,
                record.NU_PP,
                record.NU_AltasAprob,
                record.NU_BajasAprob,
                record.NU_PorcentajeVisitaFarmacia,
                record.NU_PorcentajeVisitaHospital,
                record.NU_DCS,
                record.NU_DCN,
                record.NU_DVN,
                record.NU_Total,
                record.NU_Ajuste,
                record.TX_Observacion
            }));
        }

        // Action method to update Register record to database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MW_Aprobaciones record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Update Register to UoW.
                _database.Aprobaciones.Update(record);
                // Save updated Register to database using UoW.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
            //return GetAll(request);
        }

        /// <summary>
        /// Devuelve el valor maximo para el descuento de vehiculos.
        /// </summary>
        /// <param name="ciclo"></param>
        /// <returns>Maximo valor para el decuento</returns>
        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        public JsonResult GetMaxValue(Int16 ciclo)
        {
            if (ciclo == 0)
                return null;

            Int16 recordList = db.MW_Ciclos
                .Where(e => e.ID_Ciclo == ciclo)
                .Select(e => e.NU_DiasCancelarVehiculo).First();

            var query = new
            {
                maxValue = recordList
            };
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        public void SetAprobacion()
        {
            db.SP_MW_SetAprobacion((Int32)Membership.GetUser().ProviderUserKey);
        }

        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        public JsonResult AprobacionIsAllow()
        {
            return Json(db.SP_MW_AprobacionIsAllow((Int32)Membership.GetUser().ProviderUserKey).First(), JsonRequestBehavior.AllowGet);
        }
    }
}