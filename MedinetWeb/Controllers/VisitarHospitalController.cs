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
    public class VisitarHospitalController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();
        private MedinetWebLimitedEntities db2 = new MedinetWebLimitedEntities();
        private List<SP_MWL_InsertVisitaHospital_Result> recordList;
        private SP_MWL_GetInfoHospital_Result recordInfo;

        string number;

        [Authorize(Roles = "Administrador, Mercadeo, Visitador Web")]
        [SessionExpireFilter]
        public ActionResult Index(string NUM, string F, string T, string HOSP, string S, int N, string AC)
        {
            var cicloAct = db2.SP_MWL_Ciclos_Act().First();
            ViewBag.DiaIni = cicloAct.FE_CicloIni.Day;
            ViewBag.MesIni = cicloAct.FE_CicloIni.Month;
            ViewBag.AnoIni = cicloAct.FE_CicloIni.Year;
            ViewBag.DiaFin = cicloAct.FE_CicloFin.Day;
            ViewBag.MesFin = cicloAct.FE_CicloFin.Month;
            ViewBag.AnoFin = cicloAct.FE_CicloFin.Year;

            ViewBag.NUM = NUM;
            ViewBag.HOSP = HOSP;
            ViewBag.F = F;
            ViewBag.T = T;
            ViewBag.S = S;
            ViewBag.N = N;

            number = NUM;

            int visitadorID = (Int32)Membership.GetUser().ProviderUserKey;

            recordInfo = db2.SP_MWL_GetInfoHospital(visitadorID, NUM).ToList().First();

            ViewBag.HOSPITAL = recordInfo.HOSPITAL;
            ViewBag.RUTA = recordInfo.RUTA;
            ViewBag.INSTITUCION = recordInfo.INSTITUCION;


            ViewBag.ANO = recordInfo.NU_ANO;
            ViewBag.CICLO = recordInfo.CICLO;
            ViewBag.FECHA = recordInfo.FECHA_VISITA;
            ViewBag.TIPO = recordInfo.MOTIVO;

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
                            recordList = db2.SP_MWL_InsertVisitaHospital(visitadorID, NUM, F, T, HOSP, N, S).ToList();
                            if (recordList.Count > 0)
                            {
                                TempData["success"] = "LA VISITA SE HA GUARDADO SATISFACTORIAMENTE";
                                return RedirectToAction("Index", "SelectHospital");
                            }
                            else
                            {
                                TempData["notice"] = "Error al guardar visita. \n\r Por favor intente de nuevo";
                                return RedirectToAction("Index", new { NUM = NUM, F = F, T = T, HOSP = HOSP, S = S, N = N, AC = "R", });
                            }
                        }
                        catch (Exception e)
                        {
                            TempData["notice"] = "Error al guardar visita. \n\r Por favor intente de nuevo";
                            return RedirectToAction("Index", new { NUM = NUM, F = F, T = T, HOSP = HOSP, S = S, N = N, AC = "R", });
                        }
                    }
                    else
                    {
                        TempData["notice"] = "La fecha enviada es inválida";
                        return RedirectToAction("Index", new { NUM = NUM, F = F, T = T, HOSP = HOSP, S = S, N = N, AC = "R", });
                    }
                case "R":
                case "I":
                default:
                    return View();
            }
        }
    }
}