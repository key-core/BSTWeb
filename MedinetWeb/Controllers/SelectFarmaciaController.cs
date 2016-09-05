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
    public class SelectFarmaciaController : Controller
    {
        //
        // GET: /Fichero/

        private readonly DataBase _database = new DataBase();
        //private MedinetWebEntities db = new MedinetWebEntities();
        private MedinetWebLimitedEntities db = new MedinetWebLimitedEntities();
        
        [Authorize(Roles = "Visitador Web")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Visitador Web")]
        [SessionExpireFilter]
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            int visitadorID = (Int32)Membership.GetUser().ProviderUserKey;
            var recordList = db.SP_MWL_GetFarmaciasByZonaAndText(visitadorID).ToList();

            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.NUMERO,
                record.FARMACIA,
                record.FECHA_VISITA,
                record.TIPO
            }));
        }
    }
}
