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

namespace MedinetWeb.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class CiclosController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        public static List<EstatusCiclo> estatusList = new List<EstatusCiclo> { 
            new EstatusCiclo {EstatusID = 1, Estatus = "Abierto"},
            new EstatusCiclo {EstatusID = 2, Estatus = "Cerrado"},
            new EstatusCiclo {EstatusID = 3, Estatus = "Pendiente"},
        };

        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            ViewData["dvEstatus"] = estatusList.Select(e => new
            {
                ID = e.EstatusID,
                TX = e.Estatus
            });

            return View();
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var lista = _database.Ciclos.GetAll();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.NU_Ano,
                record.NU_Ciclo,
                record.FE_CicloIni,
                record.FE_CicloFin,
                record.FE_CicloProrroga,
                record.NU_DiasCancelarVehiculo,
                record.NU_DiasHabiles,
                record.FE_CargaInv,
                record.FE_IniDistribucion,
                record.FE_FinDistribucion,
                record.NU_Estatus,
                record.BO_Activo
            }));
        }

        // Action method to add new registers to databaes.
        // Accecpt only HTTP POST request.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add([DataSourceRequest] DataSourceRequest request, MW_Ciclos record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Add Register to UoW.
                _database.Ciclos.Add(record);
                // Save Register to database using UoW.
                _database.Save();
            }
            // Return added Register record back to client side in json format and embeded request, modelstate information.
            return Json(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        // Action method to update Register record to database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MW_Ciclos record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Update Register to UoW.
                _database.Ciclos.Update(record);
                // Save updated Register to database using UoW.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        // Action method to delete Register record from database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, MW_Ciclos record)
        {
            // Test if Register object and modelstate is valid.           
            if (record != null && ModelState.IsValid)
            {
                // Delete Register from UoW.
                _database.Ciclos.Delete(record);
                // Call to database and delete the deleted record.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetCiclos()
        {
            IEnumerable<SelectListItem> listRecord = db.MW_Ciclos
                                                        .Where(c => c.BO_Activo == true && c.NU_Estatus != 2)
                                                        .AsEnumerable().Select(c => new SelectListItem()
                                                        {
                                                            Text = c.NU_Ano.ToString() + c.NU_Ciclo.ToString(),
                                                            Value = c.ID_Ciclo.ToString()
                                                        });
            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetCiclosUltimos()
        {
            IEnumerable<SelectListItem> listRecord = db.SP_MW_Ciclos_DesUsu().ToList()
                .Where(c => c.BO_Activo == true)
                .AsEnumerable().Select(c => new SelectListItem()
                {
                    Text = c.NU_Ano.ToString() + c.NU_Ciclo.ToString(),
                    Value = c.ID_Ciclo.ToString()
                });

            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetCiclosIni(Int16 ID_CicloIni, bool newRegistro)
        {
            IEnumerable<SelectListItem> listRecord;

            if(newRegistro)
                listRecord = db.MW_Ciclos
                                .Where(c => c.BO_Activo == true && c.NU_Estatus != 2)
                                .AsEnumerable().Select(c => new SelectListItem()
                                {
                                    Text = c.NU_Ano.ToString() + c.NU_Ciclo.ToString(),
                                    Value = c.ID_Ciclo.ToString()
                                });
            else
                listRecord = db.MW_Ciclos
                                .Where(c => c.ID_Ciclo == ID_CicloIni)
                                .AsEnumerable().Select(c => new SelectListItem()
                                {
                                    Text = c.NU_Ano.ToString() + c.NU_Ciclo.ToString(),
                                    Value = c.ID_Ciclo.ToString()
                                });

            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetCiclosFin(Int16 ID_CicloIni)
        {
            IEnumerable<SelectListItem> listRecord = db.MW_Ciclos
                                                        .Where(c => c.ID_Ciclo >= ID_CicloIni && c.BO_Activo == true && c.NU_Estatus != 2)
                                                        .AsEnumerable().Select(c => new SelectListItem()
                                                        {
                                                            Text = c.NU_Ano.ToString() + c.NU_Ciclo.ToString(),
                                                            Value = c.ID_Ciclo.ToString()
                                                        });
            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ciclos seleccionables
        /// </summary>
        /// <returns>Devuelve todos los ciclos menos los pendientes que no poseen data</returns>
        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetCiclosConData()
        {
            var listRecord = db.MW_Ciclos.ToList()
                .Select(e => new
                {
                    ID = e.ID_Ciclo,
                    TX = e.NU_Ano.ToString() + e.NU_Ciclo.ToString(),
                    ST = e.NU_Estatus,
                })
                .Where(e => e.ST != 3)
                .OrderByDescending(e => e.TX);

            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ciclos seleccionables para las distribuciones
        /// </summary>
        /// <returns>Devuelve todos los ciclos menos los pendientes que no poseen data</returns>
        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetCiclosDistribucion()
        {
            var listRecord = db.SP_MWS_CiclosDispensados_NotEnt().ToList()
                .Select(e => new
                {
                    ID = e.ID_Ciclo,
                    TX = e.NU_Ano.ToString() + e.NU_Ciclo.ToString()
                })
                .OrderByDescending(e => e.TX);

            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }
    }

    public class EstatusCiclo
    {
        public int EstatusID;
        public string Estatus;
    }
}