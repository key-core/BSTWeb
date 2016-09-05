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
    public class BajaFarmaciaController : Controller
    {
        //
        // GET: /Fichero/

        private readonly DataBase _database = new DataBase();
        //private MedinetWebEntities db = new MedinetWebEntities();
        private MedinetWebLimitedEntities db = new MedinetWebLimitedEntities();
        private System.Data.Entity.Core.Objects.ObjectResult<SP_MWL_EliminarVisitaMedico_Result> resultado;

        [Authorize(Roles = "Visitador Web")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Visitador Web")]
        [SessionExpireFilter]
        public JsonResult Delete(string NUM)
        {
            int visitadorID = (Int32)Membership.GetUser().ProviderUserKey;
            try
            {
                db.SP_MWL_BajaFarmacia(visitadorID, NUM);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }
        [Authorize(Roles = "Visitador Web")]
        [SessionExpireFilter]
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            int visitadorID = (Int32)Membership.GetUser().ProviderUserKey;
            var recordList = db.SP_MWL_GetFicheroFarmacias(visitadorID).ToList();

            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.NUMERO,
                record.FARMACIA,
                record.RUTA,
                record.CADENA
            }));
        }
    }
}
