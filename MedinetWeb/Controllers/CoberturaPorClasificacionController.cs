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
    public class CoberturaPorClasificacionController : Controller
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

            ViewData["dvClasificaciones"] = db.MW_ClasificacionMedicos.ToList()
                .Select(e => new
                {
                    ID = e.ID_Clasificacion,
                    TX = e.TX_Clasificacion
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
        public ActionResult GetAllClasif(short cicloID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_CoberturaPorClasif(cicloID).ToList()
                                .OrderBy(e => e.ID_Clasificacion);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Clasificacion,
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
        public ActionResult GetAllClasifReg(short cicloID, short clasificacionID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_CoberturaPorClasifReg(cicloID, clasificacionID).ToList()
                                .OrderBy(e => e.ID_Region);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Clasificacion,
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
        public ActionResult GetAllClasifRegLin(short cicloID, short clasificacionID, short regionID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_CoberturaPorClasifRegLin(cicloID, clasificacionID, regionID).ToList()
                                .OrderBy(e => e.ID_Linea);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Clasificacion,
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
        public ActionResult GetAllClasifRegLinVis(short cicloID, short clasificacionID, short regionID, short lineaID, 
            [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_CoberturaPorClasifRegLinVis(cicloID, clasificacionID, regionID, lineaID).ToList()
                                .OrderBy(e => e.ID_Usuario);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Clasificacion,
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
        public ActionResult GetAllClasifRegLinVisEsp(short cicloID, short clasificacionID, short regionID, short lineaID, 
            short visitadorID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_CoberturaPorClasifRegLinVisEsp(cicloID, clasificacionID, regionID, lineaID, visitadorID).ToList()
                                .OrderBy(e => e.ID_Especialidad);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Clasificacion,
                record.ID_Region,
                record.ID_Linea,
                record.ID_Usuario,
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
        public ActionResult GetAllClasifRegLinVisEspMed(short cicloID, short clasificacionID, short regionID, short lineaID, 
            short visitadorID, short especialidadID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_CoberturaPorClasifRegLinVisEspMed(cicloID, clasificacionID, regionID, lineaID, visitadorID, especialidadID).ToList()
                                .OrderBy(e => e.ID_Especialidad);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Clasificacion,
                record.ID_Region,
                record.ID_Linea,
                record.ID_Usuario,
                record.ID_Especialidad,
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