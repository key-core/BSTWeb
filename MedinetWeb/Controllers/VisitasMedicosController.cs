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
    public class VisitasMedicosController : Controller
    {
        //
        // GET: /VisitasMedicos/

        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAll(int cicloID, int lineaID, int regionID, int visitadorID, [DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var recordList = db.SP_MW_VisMedicos(cicloID, lineaID, regionID, visitadorID).ToList();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_VisitadoresHistorial,
                record.ID_Visitador,
                record.ID_Region,
                record.ID_Linea,
                record.Registro,
                record.Medico,
                record.FechaVisita,
                record.Tipo,
                record.Clinica,
                record.Especialidad,
                record.Motivo,
                record.Ruta,
                record.Clasificacion,
                record.Telefono,
                record.Direccion,
                record.Revisita,
                record.ComentPublicos
            }));
        }

        [Authorize(Roles = "Administrador, Invitado, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAllDetalle(int cicloID, int visitadorID, string registro, [DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var recordList = db.SP_MW_VisMedicosDetalle(cicloID, visitadorID, registro).ToList();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.NroProducto,
                record.ID_Ciclo,
                record.ID_VisitadoresHistorial,
                record.ID_Visitador,
                record.ID_Region,
                record.ID_Linea,
                record.Registro,
                record.Producto,
                record.Muestras,
                record.MaterialPro,
                record.MaterialPOP,
                record.Caracteristica,
                record.Beneficios,
                record.Presentacion,
                record.Solicita,
                record.SolicitaDet,
                record.Prescribe,
                record.NOUtiliza
            }));
        }
    }
}
