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
    public class VisitarMedicoController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();
        private MedinetWebLimitedEntities db2 = new MedinetWebLimitedEntities();
        private List<SP_MWL_InsertVisitaMedico_Result> recordList;
        private SP_MWL_GetInfoFichero_Result recordInfo;
        
        string register;

        [Authorize(Roles = "Administrador, Mercadeo, Visitador Web")]
        [SessionExpireFilter]
        public ActionResult Index(string REG, string F, string T, string A, string AC)
        {
            var cicloAct = db2.SP_MWL_Ciclos_Act().First();
            ViewBag.DiaIni = cicloAct.FE_CicloIni.Day;
            ViewBag.MesIni = cicloAct.FE_CicloIni.Month;
            ViewBag.AnoIni = cicloAct.FE_CicloIni.Year;
            ViewBag.DiaFin = cicloAct.FE_CicloFin.Day;
            ViewBag.MesFin = cicloAct.FE_CicloFin.Month;
            ViewBag.AnoFin = cicloAct.FE_CicloFin.Year;

            ViewBag.REG = REG;
            ViewBag.F = F;
            ViewBag.A = A;
            ViewBag.T = T;

            register = REG;

            int visitadorID = (Int32)Membership.GetUser().ProviderUserKey;

            recordInfo = db2.SP_MWL_GetInfoFichero(visitadorID, REG).ToList().First();

            ViewBag.DOCTOR = recordInfo.DOCTOR;
            ViewBag.RUTA = recordInfo.RUTA;
            ViewBag.CLINICA = recordInfo.CLINICA;
            ViewBag.ESPECIALIDAD = recordInfo.ESPECIALIDAD;
            ViewBag.CLASIFICACION = recordInfo.CLASIFICACION;
            ViewBag.REVISITA = recordInfo.REVISITA;

            ViewBag.ANO = recordInfo.NU_ANO;
            ViewBag.CICLO = recordInfo.CICLO;
            ViewBag.FECHA = recordInfo.FECHA_VISITA;
            ViewBag.TIPO = recordInfo.TIPO;

            switch (AC)
            {
                case "V":
                    DateTime temp;
                    string format = "dd/MM/yyyy";
                    if (F.Length == 10 && DateTime.TryParseExact(F, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out temp))
                    {
                        //int visitadorID = (Int32)Membership.GetUser().ProviderUserKey;
                        try
                        {
                            recordList = db2.SP_MWL_InsertVisitaMedico(visitadorID, REG, F, T, A).ToList();
                            if (recordList.Count > 0)
                            {
                                TempData["success"] = "LA VISITA SE HA GUARDADO SATISFACTORIAMENTE";
                                return RedirectToAction("Index", "SelectMedico");
                            }
                            else
                            {
                                TempData["notice"] = "Error al guardar visita. \n\r Una razón puede ser que ya existe una visita de tipo: " + T + " para este médico. \n\r Por favor intente de nuevo";
                                return RedirectToAction("Index", new { REG = REG, F = F, T = T, A = A, AC = "R", });
                            }
                        }
                        catch (Exception e)
                        {
                            TempData["notice"] = "Error al guardar visita. \n\r Una razón puede ser que ya existe una visita de tipo: " + T + " para este médico. \n\r Por favor intente de nuevo";
                            return RedirectToAction("Index", new { REG = REG, F = F, T = T, A = A, AC = "R", });
                        }
                    }
                    else
                    {
                        TempData["notice"] = "La fecha enviada es inválida";
                        return RedirectToAction("Index", new { REG = REG, F = F, T = T, A = A, AC = "R", });
                    }
                    break;
                case "R":
                case "I":
                default:
                    return View();
            }


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