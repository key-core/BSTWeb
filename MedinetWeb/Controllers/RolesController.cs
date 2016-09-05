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
    public class RolesController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View();
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var lista = _database.Roles.GetAll();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_Rol,
                record.TX_Rol,
                record.BO_Activo
            }));
        }

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetRoles()
        {
            IEnumerable<SelectListItem> listRecord = db.MW_Roles
                                                        .Where(c => c.BO_Activo == true)
                                                        .AsEnumerable().Select(c => new SelectListItem()
                                                        {
                                                            Text = c.TX_Rol,
                                                            Value = c.ID_Rol.ToString()
                                                        });
            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }
    }
}