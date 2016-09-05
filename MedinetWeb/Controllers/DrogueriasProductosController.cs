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
    public class DrogueriasProductosController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            ViewData["dvDrogueriasAlmacenes"] = db.SP_MW_DrogueriasAlmacenes_DroPro().ToList()
                .Select(e => new
                {
                    ID = e.ID_DrogueriaAlmacen,
                    TX = e.TX_DrogueriaAlmacen
                });

            ViewData["dvProductos"] = db.MW_Productos.ToList()
                .Select(e => new
                {
                    ID = e.ID_Producto,
                    TX = e.TX_Producto
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
            var lista = _database.DrogueriasProductos.GetAll();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_DrogueriaProducto,
                record.ID_DrogueriaAlmacen,
                record.ID_Producto,
                record.TX_IDProductoDrogueria,
                record.TX_ProductoDrogueria,
                record.NU_PrecioProducto,
                record.NU_InvProducto,
                record.TX_DrogueriaRef1,
                record.TX_DrogueriaRef2,
                record.BO_Activo
            }));
        }

        // Action method to update Register record to database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MW_DrogueriasProductos record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Update Register to UoW.
                _database.DrogueriasProductos.Update(record);
                // Save updated Register to database using UoW.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            
            return Json(ModelState.ToDataSourceResult());

        }
    }
}