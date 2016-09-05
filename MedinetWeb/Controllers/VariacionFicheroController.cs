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
    public class VariacionFicheroController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
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

            ViewData["dvVisitadores"] = db.SP_MW_Visitadores().ToList()
                .Select(e => new
                {
                    ID = e.ID_Usuario,
                    TX = e.TX_Nombre + " " + e.TX_Apellido
                });

            return View();
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAllReg(short cicloID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_VarFicheroPorReg(cicloID).ToList()
                                .OrderBy(e => e.ID_Region);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Region,
                record.Fichados,
                record.AltaAprobada,
                record.EnEstudio,
                record.AltaPosterior,
                record.BajaAprobada,
                record.BajaPosterior,
                record.FicheroFinal
            }));
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAllRegLin(short cicloID, short regionID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_VarFicheroPorRegLin(cicloID, regionID).ToList()
                                .OrderBy(e => e.ID_Linea);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Region,
                record.ID_Linea,
                record.Fichados,
                record.AltaAprobada,
                record.EnEstudio,
                record.AltaPosterior,
                record.BajaAprobada,
                record.BajaPosterior,
                record.FicheroFinal
            }));
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAllRegLinVis(short cicloID, short regionID, short lineaID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_VarFicheroPorRegLinVis(cicloID, regionID, lineaID).ToList()
                                .OrderBy(e => e.ID_Usuario);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Region,
                record.ID_Linea,
                record.ID_Usuario,
                record.Fichados,
                record.AltaAprobada,
                record.EnEstudio,
                record.AltaPosterior,
                record.BajaAprobada,
                record.BajaPosterior,
                record.FicheroFinal
            }));
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAllAltas(short cicloID, short visitadorID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_VarFicheroAltas(cicloID, visitadorID).ToList()
                                .OrderBy(e => e.Medico);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.Medico,
                record.Clasificacion,
                record.Especialidad,
                record.AltaAprobada,
                record.EnEstudio,
                record.AltaPosterior
            }));
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAllBajas(short cicloID, short visitadorID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_VarFicheroBajas(cicloID, visitadorID).ToList()
                                .OrderBy(e => e.Medico);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.Medico,
                record.Clasificacion,
                record.Especialidad,
                record.BajaAprobada,
                record.BajaPosterior
            }));
        }
    }
}