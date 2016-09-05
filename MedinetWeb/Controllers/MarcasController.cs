﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedinetWeb.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Web.Security;

namespace MedinetWeb.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class MarcasController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            ViewData["dvLaboratorios"] = db.MW_Laboratorios.ToList()
                .Select(e => new
                {
                    ID = e.ID_Laboratorio,
                    TX = e.TX_Laboratorio
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
            var lista = _database.Marcas.GetAll();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_Marca,
                record.TX_Marca,
                record.ID_Laboratorio,
                record.TX_Posicionamiento,
                record.FE_Registro,
                record.BO_Activo
            }));
        }

        // Action method to add new registers to databaes.
        // Accecpt only HTTP POST request.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add([DataSourceRequest] DataSourceRequest request, MW_Marcas record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Add Register to UoW.
                _database.Marcas.Add(record);
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
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MW_Marcas record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Update Register to UoW.
                _database.Marcas.Update(record);
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
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, MW_Marcas record)
        {
            // Test if Register object and modelstate is valid.           
            if (record != null && ModelState.IsValid)
            {
                // Delete Register from UoW.
                _database.Marcas.Delete(record);
                // Call to database and delete the deleted record.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetMarcas()
        {
            IEnumerable<SelectListItem> listRecord = db.MW_Marcas
                                                        .Where(c => c.BO_Activo == true)
                                                        .AsEnumerable().Select(c => new SelectListItem()
                                                        {
                                                            Text = c.TX_Marca,
                                                            Value = c.ID_Marca.ToString()
                                                        });
            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetMarcasMedinet()
        {
            IEnumerable<SelectListItem> listRecord = db.MW_Marcas
                .Join(db.MW_Laboratorios,
                    a => a.ID_Laboratorio,
                    b => b.ID_Laboratorio,
                    (a, b) => new { Marca = a, Laboratorio = b })
                    .Where(c => c.Marca.BO_Activo == true && c.Laboratorio.BO_LaboratorioMedinet == true)
                    .AsEnumerable().Select(c => new SelectListItem()
                    {
                        Text = c.Marca.TX_Marca,
                        Value = c.Marca.ID_Marca.ToString()
                    });
            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetMarcasCompetencias(Int16 ID_Marca)
        {
            IEnumerable<SelectListItem> listRecord = db.SP_MW_Marcas_MarCom(ID_Marca).ToList()
                .OrderBy(c => c.TX_Marca)
                .AsEnumerable().Select(c => new SelectListItem()
                {
                    Text = c.TX_Marca,
                    Value = c.ID_Marca.ToString()
                });
            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetMarcas_EsqPro(Int16 ID_Linea, Int16 ID_Especialidad, Int16 ID_Ciclo)
        {
            IEnumerable<SelectListItem> listRecord = db.SP_MW_Marcas_EsqPro(ID_Linea, ID_Especialidad, ID_Ciclo).ToList()
                .Where(c => c.BO_Activo == true)
                .OrderBy(c => c.TX_Marca)
                .AsEnumerable().Select(c => new SelectListItem()
                {
                    Text = c.TX_Marca,
                    Value = c.ID_Marca.ToString()
                });

            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetMarcas_ConDis()
        {
            IEnumerable<SelectListItem> listRecord = db.SP_MWS_Marcas_ConDis((Int32)Membership.GetUser().ProviderUserKey).ToList()
                .OrderBy(c => c.TX_Marca)
                .AsEnumerable().Select(c => new SelectListItem()
                {
                    Text = c.TX_Marca,
                    Value = c.ID_Marca.ToString()
                });

            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }
    }
}