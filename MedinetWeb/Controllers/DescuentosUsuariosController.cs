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
    public class DescuentosUsuariosController : Controller
    {
        private AccesoRepository repository;

        public DescuentosUsuariosController()
        {
            this.repository = new AccesoRepository();
        }

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

            ViewData["dvUsuarios"] = db.MW_Usuarios.ToList()
                .Select(e => new
                {
                    ID = e.ID_Usuario,
                    TX = e.TX_Nombre + " " + e.TX_Apellido
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
        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_DescuentosUsuarios_DesUsu((Int32)Membership.GetUser().ProviderUserKey).ToList()
                    .OrderByDescending(e => e.ID_DescuentosUsuarios)
                    .ThenByDescending(e => e.ID_Ciclo);
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_DescuentosUsuarios,
                record.ID_Ciclo,
                record.ID_Usuario,
                record.ID_ConceptoDescuento,
                record.FE_DescuentoIni,
                record.FE_DescuentoFin,
                record.BO_MedioDia,
                record.NU_TotalDescuento,
                record.TX_Comentario
            }));
        }

        // Action method to add new registers to databaes.
        // Accecpt only HTTP POST request.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add([DataSourceRequest] DataSourceRequest request, MW_DescuentosUsuarios record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Add Register to UoW.
                _database.DescuentosUsuarios.Add(record);
                // Save Register to database using UoW.
                _database.Save();
            }
            // Return added Register record back to client side in json format and embeded request, modelstate information.
            //return Json(new[] { record }.ToDataSourceResult(request, ModelState));
            return GetAll(request);
        }

        // Action method to update Register record to database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MW_DescuentosUsuarios record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Update Register to UoW.
                _database.DescuentosUsuarios.Update(record);
                // Save updated Register to database using UoW.
                _database.Save();
            }
            // Return added Register record back to client side in json format and embeded request, modelstate information.
            //return Json(new[] { record }.ToDataSourceResult(request, ModelState));
            return GetAll(request);
        }

        // Action method to delete Register record from database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, MW_DescuentosUsuarios record)
        {
            // Test if Register object and modelstate is valid.           
            if (record != null && ModelState.IsValid)
            {
                // Delete Register from UoW.
                _database.DescuentosUsuarios.Delete(record);
                // Call to database and delete the deleted record.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetMinDate(Int16 ciclo)
        {
            if (ciclo == 0)
                return null;

            DateTime recordList = db.MW_Ciclos
                .Where(e => e.ID_Ciclo == ciclo)
                .Select(e => e.FE_CicloIni).First();

            var query = new
            {
                ano = recordList.Year,
                mes = recordList.Month - 1,
                dia = recordList.Day
            };
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetMaxDate(Int16 ciclo)
        {
            if (ciclo == 0)
                return null;

            DateTime recordList = db.MW_Ciclos
                .Where(e => e.ID_Ciclo == ciclo)
                .Select(e => e.FE_CicloFin).First();

            var query = new
            {
                ano = recordList.Year,
                mes = recordList.Month - 1,
                dia = recordList.Day
            };
            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}