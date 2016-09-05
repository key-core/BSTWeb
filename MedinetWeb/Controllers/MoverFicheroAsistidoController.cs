using MedinetWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MedinetWeb.Controllers
{
    public class MoverFicheroAsistidoController : Controller
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
        public JsonResult CopiarFichero(int idCicloOrigen, int idCicloDestino, int iDOrigen, int iDDestino) {
            try
            {
                db2.SP_MW_MoverFicheroAsistido(idCicloOrigen, idCicloDestino, iDOrigen, iDDestino);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(false);
            }
        }
    }
}