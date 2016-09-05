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
    public class AprobacionAltasBajasController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        public static List<EstatusAprobacion> estatusList = new List<EstatusAprobacion> { 
            new EstatusAprobacion {EstatusID = 1, Estatus = "Postulado"},
            new EstatusAprobacion {EstatusID = 2, Estatus = "Aprobado"},
        };

        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            ViewData["dvRegiones"] = db.MW_Regiones.ToList()
                .Select(e => new
                {
                    ID = e.ID_Region,
                    TX = e.TX_Region
                });

            ViewData["dvLineas"] = db.MW_Lineas.ToList()
                .Select(e => new
                {
                    ID = e.ID_Linea,
                    TX = e.TX_Linea
                });

            ViewData["dvVisitadores"] = db.SP_MW_VisitadoresHistorial_Apr().ToList()
                .Select(e => new
                {
                    ID = e.ID_VisitadoresHistorial,
                    TX = e.TX_Nombre + " " + e.TX_Apellido
                });

            ViewData["dvClasificaciones"] = db.MW_ClasificacionMedicos.ToList()
                .Select(e => new
                {
                    ID = e.ID_Clasificacion,
                    TX = e.TX_Clasificacion
                });

            ViewData["dvEspecialidades"] = db.MW_Especialidades.ToList()
                .Select(e => new
                {
                    ID = e.ID_Especialidad,
                    TX = e.TX_Especialidad
                });

            ViewData["dvEstatus"] = estatusList.Select(e => new
            {
                ID = e.EstatusID,
                TX = e.Estatus
            });
            return View();
        }

        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_VisitadoresAltasBajas((Int32)Membership.GetUser().ProviderUserKey).ToList();
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.ID_Region,
                record.ID_Linea,
                record.ID_VisitadoresHistorial
            }));
        }

        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        public ActionResult GetAllAltas(int visitadorID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_AprobacionAltas(visitadorID).ToList();
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.NroSanitario,
                record.Medico,
                record.Clinica,
                record.ID_Clasificacion,
                record.ID_Especialidad,
                record.Estatus,
                record.BO_GoogleID
            }));
        }

        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        public ActionResult GetAllBajas(int visitadorID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_AprobacionBajas(visitadorID).ToList();
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.Registro,
                record.NroSanitario,
                record.Medico,
                record.Clinica,
                record.ID_Clasificacion,
                record.ID_Especialidad,
                record.Estatus,
                record.BO_GoogleID
            }));
        }

        /// <summary>
        /// Metodo que actualiza las altas MW y MM
        /// </summary>
        /// <param name="visitadorID"></param>
        /// <param name="record"></param>
        /// <returns>Vista actualizada</returns>
        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateAltas(int visitadorID, Models.MirrorEntityModel.MMW_AprobacionAltas record)
        {
            try
            {
                string clasificacion = db.MW_ClasificacionMedicos.Where(r => r.ID_Clasificacion == record.ID_Clasificacion)
                    .Select(r => r.TX_Clasificacion)
                    .First();
                string especialidad = db.MW_Especialidades.Where(r => r.ID_Especialidad == record.ID_Especialidad)
                    .Select(r => r.TX_Especialidad)
                    .First(); ;
                string estatus = record.Estatus == 1 ? "S" : "Y";

                List<string> instrucciones = new List<string>();
                instrucciones.Add("UPDATE Inclusiones SET CLASIFICACION = '" + clasificacion +
                    "', ESPECIALIDAD = '" + especialidad + "', PARA_APROBACION = '" + estatus +
                    "' WHERE ZONA = '" + visitadorID.ToString() + "' AND NRO_SANITARIO = '" + record.NroSanitario + "'");

                List<string> registrationIds = new List<string>();
                registrationIds.Add(db.SP_MW_Usuarios_AltBaj(visitadorID)
                    .Select(r => r.TX_IDRegistroGoogle)
                    .First().ToString());

                // Instancia del y ejecucion del servio(Si resultServicio == true Update MD)

                MedinetService.MedinetCRMService service = new MedinetService.MedinetCRMService();
                if (service.setInstruccionesSql(instrucciones.ToArray(), registrationIds.ToArray()))
                {
                    db.SP_MW_UpdateAltas(clasificacion, especialidad, estatus, visitadorID.ToString(), record.NroSanitario);
                }
                else
                {
                    return JavaScript("mensaje('" + visitadorID + "','GridAltas')");
                    //return Json(ModelState.SerializeErrors());

                }
                return Json(ModelState.ToDataSourceResult());
            }
            catch (Exception ex)
            {
                MedinetLibrary.Excepciones.LogException(ex, "AprobacionAltasBajasController", "UpdateAltas");

                return Json(ModelState.SerializeErrors());
            }
        }

        /// <summary>
        /// Metodo que actualiza las bajas MW y MM
        /// </summary>
        /// <param name="visitadorID"></param>
        /// <param name="record"></param>
        /// <returns>Vista actualizada</returns>
        [Authorize(Roles = "Administrador, Ventas")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateBajas(int visitadorID, Models.MirrorEntityModel.MMW_AprobacionBajas record)
        {
            try
            {
                string clave = "";
                string eliminado = "";

                switch (record.Estatus)
                {
                    case 1:
                        clave = ".";
                        eliminado = ".";
                        break;
                    case 2:
                        clave = "1";
                        eliminado = "SI";
                        break;
                }

                List<string> instrucciones = new List<string>();
                instrucciones.Add("UPDATE Eliminar SET Clave = '" + clave + "', Eliminado = '" + eliminado +
                    "' WHERE ZONA = '" + visitadorID.ToString() + "' AND Registro = " + record.Registro.ToString());

                List<string> registrationIds = new List<string>();
                registrationIds.Add(db.SP_MW_Usuarios_AltBaj(visitadorID)
                    .Select(r => r.TX_IDRegistroGoogle)
                    .First().ToString());

                // Instancia del y ejecucion del servio(Si resultServicio == true Update MD)
                MedinetService.MedinetCRMService service = new MedinetService.MedinetCRMService();
                if (service.setInstruccionesSql(instrucciones.ToArray(), registrationIds.ToArray()))
                {
                    db.SP_MW_UpdateBajas(clave, eliminado, visitadorID.ToString(), record.Registro);
                }
                else
                {
                    return JavaScript("mensaje('" + visitadorID + "','GridBajas')");
                    //return Json(ModelState.SerializeErrors());
                }
                return Json(ModelState.ToDataSourceResult());
            }
            catch (Exception ex)
            {
                MedinetLibrary.Excepciones.LogException(ex, "AprobacionAltasBajasController", "UpdateBajas");

                return Json(ModelState.SerializeErrors());
            }
        }
    }

    public class EstatusAprobacion
    {
        public int EstatusID;
        public string Estatus;
    }
}