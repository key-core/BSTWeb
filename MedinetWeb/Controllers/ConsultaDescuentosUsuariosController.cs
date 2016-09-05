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
    public class ConsultaDescuentosUsuariosController : Controller
    {
        private AccesoRepository repository;

        public ConsultaDescuentosUsuariosController()
        {
            this.repository = new AccesoRepository();
        }

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

            ViewData["dvUsuarios"] = db.MW_Usuarios.ToList()
                .Select(e => new
                {
                    ID = e.ID_Usuario,
                    TX = e.TX_Nombre + " " + e.TX_Apellido
                });

            ViewData["dvCargos"] = db.MW_Cargos.ToList()
                .Select(e => new
                {
                    ID = e.ID_Cargo,
                    TX = e.TX_Cargo
                });

            ViewData["dvConceptoDescuentos"] = db.MW_ConceptoDescuentos.ToList()
                .Select(e => new
                {
                    ID = e.ID_ConceptoDescuento,
                    TX = e.TX_ConceptoDescuento + " Aplica(" + (e.BO_AplicaVehiculo == true ? "Si" : "No") + ")"
                });

            return View();
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAll(short cicloID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_DescuentosUsuarios_ConDesUsu(cicloID).ToList()
                    .OrderBy(e => e.ID_DescuentosUsuarios);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_DescuentosUsuarios,
                record.ID_Ciclo,
                record.ID_Usuario,
                record.ID_Cargo,
                record.ID_ConceptoDescuento,
                record.FE_DescuentoIni,
                record.FE_DescuentoFin,
                record.BO_MedioDia,
                record.NU_TotalDescuento,
                record.TX_Comentario
            }));
        }
    }
}