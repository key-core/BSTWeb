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
    public class CoberturaPorEspecialidadController : Controller
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

            ViewData["dvEspecialidades"] = db.MW_Especialidades.ToList()
                .Select(e => new
                {
                    ID = e.ID_Especialidad,
                    TX = e.TX_Especialidad
                });

            return View();
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAllEsp(short cicloID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_CoberturaPorEsp(cicloID).ToList()
                                .OrderBy(e => e.ID_Especialidad);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Especialidad,
                record.Fichados,
                record.Visita,
                record.PCJVisita,
                record.REVisita,
                record.REVisitado,
                record.PCJREVisita,
                record.FmasR,
                record.VmasR,
                record.PCJFVRR,
                record.Proyeccion
            }));
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAllEspReg(short cicloID, short especialidadID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_CoberturaPorEspReg(cicloID, especialidadID).ToList()
                                .OrderBy(e => e.ID_Region);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Especialidad,
                record.ID_Region,
                record.Fichados,
                record.Visita,
                record.PCJVisita,
                record.REVisita,
                record.REVisitado,
                record.PCJREVisita,
                record.FmasR,
                record.VmasR,
                record.PCJFVRR,
                record.Proyeccion
            }));
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAllEspRegLin(short cicloID, short especialidadID, short regionID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_CoberturaPorEspRegLin(cicloID, especialidadID, regionID).ToList()
                                .OrderBy(e => e.ID_Linea);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Especialidad,
                record.ID_Region,
                record.ID_Linea,
                record.Fichados,
                record.Visita,
                record.PCJVisita,
                record.REVisita,
                record.REVisitado,
                record.PCJREVisita,
                record.FmasR,
                record.VmasR,
                record.PCJFVRR,
                record.Proyeccion
            }));
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAllEspRegLinVis(short cicloID, short especialidadID, short regionID, short lineaID,
            [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_CoberturaPorEspRegLinVis(cicloID, especialidadID, regionID, lineaID).ToList()
                                .OrderBy(e => e.ID_Usuario);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Especialidad,
                record.ID_Region,
                record.ID_Linea,
                record.ID_Usuario,
                record.Fichados,
                record.Visita,
                record.PCJVisita,
                record.REVisita,
                record.REVisitado,
                record.PCJREVisita,
                record.FmasR,
                record.VmasR,
                record.PCJFVRR,
                record.Proyeccion
            }));
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAllEspRegLinVisMed(short cicloID, short especialidadID, short regionID, short lineaID,
            short visitadorID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_CoberturaPorEspRegLinVisMed(cicloID, especialidadID, regionID, lineaID, visitadorID).ToList()
                                .OrderBy(e => e.ID_Especialidad);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Especialidad,
                record.ID_Region,
                record.ID_Linea,
                record.ID_Usuario,
                record.Medico,
                record.Fichados,
                record.Visita,
                record.PCJVisita,
                record.REVisita,
                record.REVisitado,
                record.PCJREVisita,
                record.FmasR,
                record.VmasR,
                record.PCJFVRR,
                record.Proyeccion
            }));
        }
    }
}