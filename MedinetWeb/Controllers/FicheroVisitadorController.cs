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
    public class FicheroVisitadorController : Controller
    {
        //
        // GET: /Fichero/

        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();
        private MedinetWebLimitedEntities db2 = new MedinetWebLimitedEntities();
        
        [Authorize(Roles = "Administrador, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrador, Ventas, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetAll(int cicloID, int lineaID, int regionID, int visitadorID, [DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var recordList = db.SP_MW_FicheroVisitador(cicloID, lineaID, regionID, visitadorID).ToList();
            //var recordLists = db2.SP_MWL_GetFicheroByZonaAndText(214, "AL");

            //return Json(recordLists.ToDataSourceResult(request, record => new {
            //record.REGISTRO,
            //record.DOCTOR,

            //});
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Ciclo,
                record.ID_Visitador,
                record.Registro,
                record.Medico,
                record.Ruta,
                record.Clinica,
                record.Especialidad,
                record.Clasificacion,
                record.Revisita,
                record.Modo,
                record.NroVisitadores,
                record.ColocacionMaleta,
                record.HaceEsperar,
                record.Estado,
                record.Brick,
                record.BrickZona,
                record.Telefono,
                record.Observacion,
                record.Email,
                record.HoraSugerida,
                record.OtroSitio,
                record.Ocupacion,
                record.CostoConsulta,
                record.InfluenciaClinica,
                record.Investigador,
                record.Formador,
                record.Speaker,
                record.Congreso,
                record.ConsideraPrimero,
                record.ConsideraSegundo,
                record.ConsideraTercero,
                record.TratoEspecial,
                record.PatologiaFrecuente1,
                record.PatologiaFrecuente2,
                record.PatologiaFrecuente3,
                record.ProductoAcoplados1,
                record.ProductoAcoplados2,
                record.ProductoAcoplados3,
                record.PerfilPrescriptivo1,
                record.PerfilPrescriptivo2,
                record.PerfilPrescriptivo3,
                record.Secretaria,
                record.NroSanitario,
                record.TipoMedico1,
                record.TipoMedico2,
                record.EstadoCivil,
                record.HijoMasculino,
                record.HijoFemenino,
                record.Deportes,
                record.CumpleanoDia,
                record.CumpleanoMes,
                record.Idiomas1,
                record.Idiomas2,
                record.EdadEnTiempo,
                record.FechaAgenda,
                record.SemanaAgenda,
                record.AmPmAgenda,
                record.RFechaAgenda,
                record.RSemanaAgenda,
                record.RAmPmAgenda
            }));
        }
    }
}
