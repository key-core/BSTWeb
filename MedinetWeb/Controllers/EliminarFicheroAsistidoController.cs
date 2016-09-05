using MedinetWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MedinetWeb.Controllers
{
    public class EliminarFicheroAsistidoController : Controller
    {


        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();
        private MedinetWebLimitedEntities db2 = new MedinetWebLimitedEntities();
        //
        // GET: /MoverFicheroAsistido/
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Index()
        {

            if (Membership.GetUser().ToString().ToUpper().Equals("MDNADMIN"))
            {
                return View();
            }
            else {
                return RedirectToAction("Index", "Home");
            }
            
        }
        public JsonResult EliminarFichero(int idCiclo, int visitadorID) {
            try
            {
                db2.SP_MW_EliminarFicheroAsistido(idCiclo, visitadorID);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }
    }
}