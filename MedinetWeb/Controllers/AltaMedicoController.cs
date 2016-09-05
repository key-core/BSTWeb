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
using System.Globalization;

namespace MedinetWeb.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class AltaMedicoController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();
        private MedinetWebLimitedEntities db2 = new MedinetWebLimitedEntities();
        private List<SP_MWL_InsertVisitaMedico_Result> recordList;

        int visitadorID = (Int32)Membership.GetUser().ProviderUserKey;

        [Authorize(Roles = "Visitador Web")]
        [SessionExpireFilter]
        public JsonResult FicharMedico(string nroSanitario, string doctor, string especialidad, string clasificacion, string ruta,
            string clinica, string direccion, string revisita)
        {
            try
            {
                int cantidad = db2.SP_MWL_AltaMedicoValidar(visitadorID, nroSanitario.ToUpper()).First().Value;
                if (cantidad <= 0)
                {
                    db2.SP_MWL_AltaMedico(visitadorID, nroSanitario.ToUpper(), doctor.ToUpper(), ruta.ToUpper(), clinica.ToUpper(), especialidad.ToUpper(), clasificacion.ToUpper(), direccion.ToUpper(), revisita.ToUpper());
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception e)
            {
                return Json(false);
            }

        }

        [Authorize(Roles = "Visitador Web")]
        [SessionExpireFilter]
        public ActionResult Index(string NUMERO, string NOMBRE)
        {
            ViewBag.NUMERO = NUMERO;
            ViewBag.DOCTOR = NOMBRE;

            return View();

        }

        [Authorize(Roles = "Visitador Web")]
        [SessionExpireFilter]
        public JsonResult GetRutas(string text, [DataSourceRequest] DataSourceRequest request)
        {
            List<string> rutas = new List<string>();
            if (text.Equals("*"))
            {
                rutas = db2.SP_MWL_GetRutas(visitadorID).OrderBy(r => r).ToList();
            }
            else
            {
                rutas = db2.SP_MWL_GetRutas(visitadorID).Where(r => r.Contains(text.ToUpper())).OrderBy(r => r).ToList();
            }
            return Json(rutas.ToArray(), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Visitador Web")]
        [SessionExpireFilter]
        public JsonResult GetClinicas(string text, [DataSourceRequest] DataSourceRequest request)
        {
            List<string> clinicas = new List<string>();
            if (text.Equals("*"))
            {
                clinicas = db2.SP_MWL_GetClinicas(visitadorID).OrderBy(r => r).ToList();
            }
            else
            {
                clinicas = db2.SP_MWL_GetClinicas(visitadorID).Where(r => r.Contains(text.ToUpper())).OrderBy(r => r).ToList();
            }
            return Json(clinicas.ToArray(), JsonRequestBehavior.AllowGet);

            //var clinicas = db2.SP_MWL_GetClinicas(visitadorID);
            //return Json(clinicas.ToArray(), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Visitador Web")]
        [SessionExpireFilter]
        public JsonResult GetEspecialidades([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<SelectListItem> especialidades = db.MW_Especialidades
                                                        .Where(c => c.BO_Activo)
                                                        .OrderBy(e => e.TX_Especialidad)
                                                        .AsEnumerable().Select(c => new SelectListItem()
                                                        {
                                                            Text = c.TX_Especialidad,
                                                            Value = c.TX_Especialidad.ToString()
                                                        });
            return Json(especialidades, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Visitador Web")]
        [SessionExpireFilter]
        public JsonResult GetClasificaciones([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<SelectListItem> clasificaciones = db.MW_ClasificacionMedicos
                                                        .Where(c => c.BO_Activo)
                                                        .OrderBy(e => e.TX_Clasificacion)
                                                        .AsEnumerable().Select(c => new SelectListItem()
                                                        {
                                                            Text = c.TX_Clasificacion,
                                                            Value = c.TX_Clasificacion.ToString()
                                                        });
            return Json(clasificaciones, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [SessionExpireFilter]
        public JsonResult GetTipoVisitas(string REGISTRO)
        {
            int visitadorID = (Int32)Membership.GetUser().ProviderUserKey;
            IEnumerable<SelectListItem> listRecord = db2.SP_MWL_GetTipoVisitaAllowed(visitadorID, REGISTRO)
                                                        .AsEnumerable().Select(c => new SelectListItem()
                                                        {
                                                            Text = c.TIPO,
                                                            Value = c.CODIGO
                                                        });
            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }



    }
}