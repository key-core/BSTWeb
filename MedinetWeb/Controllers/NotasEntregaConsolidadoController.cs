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
    public class NotasEntregaConsolidadoController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            ViewData["dvVisitadoresHistorial"] = db.SP_MW_VisitadoresHistorial_Apr().ToList()
                .OrderBy(e => e.ID_VisitadoresHistorial)
                .Select(e => new
                {
                    ID = e.ID_VisitadoresHistorial,
                    TX = e.TX_Nombre.ToString() + " " + e.TX_Apellido.ToString()
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
        public ActionResult GetAll(short cicloID, [DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var lista = db.SP_MWS_NotaEntregaConsolidado_NotEntCon(cicloID).ToList();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_NotaEntrega,
                record.ID_VisitadoresHistorial,
                record.ID_Marca,
                record.ID_Articulo,
                record.TX_Articulo,
                record.TX_Lote,
                record.TX_Ubicacion,
                record.NU_UnidadEntregar,
                record.NU_UMEntregar
            }));
        }
    }
}