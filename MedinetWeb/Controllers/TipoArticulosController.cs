using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedinetWeb.Models;

namespace MedinetWeb.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class TipoArticulosController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetTipoArticulos()
        {
            IEnumerable<SelectListItem> listRecord = db.MWS_TipoArticulos
                                                        .Where(c => c.BO_Activo == true)
                                                        .OrderBy(c => c.TX_TipoArticulo)
                                                        .AsEnumerable().Select(c => new SelectListItem()
                                                        {
                                                            Text = c.TX_TipoArticulo,
                                                            Value = c.ID_TipoArticulo.ToString()
                                                        });
            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }
    }
}