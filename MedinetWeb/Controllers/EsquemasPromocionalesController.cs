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
    public class EsquemasPromocionalesController : Controller
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

            ViewData["dvPosiciones"] = db.MW_Posiciones.ToList()
                .Select(e => new
                {
                    ID = e.ID_Posicion,
                    TX = e.TX_Posicion
                });

            ViewData["dvMarcas"] = db.MW_Marcas.ToList()
                .Select(e => new
                {
                    ID = e.ID_Marca,
                    TX = e.TX_Marca
                });

            return View();
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var lista = _database.EsquemasPromocionales.GetAll();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_EsquemaPromocional,
                record.ID_Ciclo,
                record.ID_Linea,
                record.ID_Especialidad,
                record.ID_Posicion,
                record.ID_Marca,
                record.FE_Registro,
                record.BO_Activo
            }));
        }

        // Action method to add new registers to databaes.
        // Accecpt only HTTP POST request.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add([DataSourceRequest] DataSourceRequest request, MW_EsquemasPromocionales record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Add Register to UoW.
                _database.EsquemasPromocionales.Add(record);
                // Save Register to database using UoW.
                _database.Save();
            }
            // Return added Register record back to client side in json format and embeded request, modelstate information.
            return Json(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        // Action method to update Register record to database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MW_EsquemasPromocionales record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Update Register to UoW.
                _database.EsquemasPromocionales.Update(record);
                // Save updated Register to database using UoW.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        // Action method to delete Register record from database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, MW_EsquemasPromocionales record)
        {
            // Test if Register object and modelstate is valid.           
            if (record != null && ModelState.IsValid)
            {
                // Delete Register from UoW.
                _database.EsquemasPromocionales.Delete(record);
                // Call to database and delete the deleted record.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }
    }
}