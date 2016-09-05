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
    public class ArticulosController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetArticulos(int IDMarca, Int16 IDTipoArticulo)
        {
            IEnumerable<SelectListItem> listRecord = db.MWS_Articulos
                                                        .Where(c => c.ID_Marca == IDMarca && c.ID_TipoArticulo == IDTipoArticulo && c.BO_Activo == true)
                                                        .OrderBy(c => c.TX_Articulo)
                                                        .AsEnumerable().Select(c => new SelectListItem()
                                                        {
                                                            Text = c.TX_Articulo,
                                                            Value = c.ID_Articulo.ToString()
                                                        });
            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }
    }
}