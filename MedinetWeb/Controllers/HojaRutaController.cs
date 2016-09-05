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
    public class HojaRutaController : Controller
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
            var recordList = db.SP_MW_HojaRutaPorReg(cicloID).ToList()
                                .OrderBy(e => e.ID_Region);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Region,
                record.NU_Visitadores,
                record.Creadas
            }));
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAllRegLin(short cicloID, short regionID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_HojaRutaPorRegLin(cicloID, regionID).ToList()
                                .OrderBy(e => e.ID_Linea);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Region,
                record.ID_Linea,
                record.NU_Visitadores,
                record.Creadas
            }));
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAllRegLinVis(short cicloID, short regionID, short lineaID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_HojaRutaPorRegLinVis(cicloID, regionID, lineaID).ToList()
                                .OrderBy(e => e.ID_Visitador);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Region,
                record.ID_Linea,
                record.ID_Visitador,
                record.NU_Visitadores,
                record.Creadas
            }));
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAllRutas(short cicloID, int visitadorID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_HojaRutaPorRegLinVisRutas(cicloID, visitadorID).ToList();
                                //.OrderBy(e => e.Semana)
                                //.ThenBy(e => e.Dia);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.Semana,
                record.Dia,
                record.Am,
                record.Pm
            }));
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAllDetalle(short cicloID, int visitadorID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_HojaRutaPorRegLinVisDetalle(cicloID, visitadorID).ToList()
                                .OrderByDescending(e => e.Tipo)
                                .ThenBy(e => e.Semana)
                                .ThenBy(e => e.Fecha);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.Semana,
                record.Fecha,
                record.Horario,
                record.Hora,
                record.Tipo,
                record.Medico,
                record.Especialidad,
                record.Clinica
            }));
        }
    }
}