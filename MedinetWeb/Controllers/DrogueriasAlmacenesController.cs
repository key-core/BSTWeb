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
    public class DrogueriasAlmacenesController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
             ViewData["dvDroguerias"] = db.MW_Droguerias.ToList()
                .Select(e => new
                {
                    ID = e.ID_Drogueria,
                    TX = e.TX_Drogueria
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
            var lista = _database.DrogueriasAlmacenes.GetAll();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_DrogueriaAlmacen,
                record.ID_Drogueria,
                record.TX_Almacen,
                record.TX_IDAlmacen,
                record.BO_Activo
            }));
        }

        // Action method to add new registers to databaes.
        // Accecpt only HTTP POST request.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Add([DataSourceRequest] DataSourceRequest request, MW_DrogueriasAlmacenes record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Add Register to UoW.
                _database.DrogueriasAlmacenes.Add(record);
                // Save Register to database using UoW.
                _database.Save();
            }
            // Return added Register record back to client side in json format and embeded request, modelstate information.
            return Json(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        // Action method to update Register record to database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MW_DrogueriasAlmacenes record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Update Register to UoW.
                _database.DrogueriasAlmacenes.Update(record);
                // Save updated Register to database using UoW.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        // Action method to delete Register record from database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, MW_DrogueriasAlmacenes record)
        {
            // Test if Register object and modelstate is valid.           
            if (record != null && ModelState.IsValid)
            {
                // Delete Register from UoW.
                _database.DrogueriasAlmacenes.Delete(record);
                // Call to database and delete the deleted record.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }
    }
}