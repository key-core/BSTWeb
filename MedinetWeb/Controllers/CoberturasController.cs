using MedinetWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MedinetWeb.Controllers
{
    public class CoberturasController : Controller
    {
        private MedinetWebLimitedEntities db = new MedinetWebLimitedEntities();
        private MedinetWebEntities db2 = new MedinetWebEntities();
        private SP_MWL_GetCoberturas_Result resultado;
        
        [Authorize(Roles = "Visitador Web")]
        // GET: Coberturas
        public ActionResult Index()
        {

            var cicloAct = db.SP_MWL_Ciclos_Act().First();
            ViewBag.Ciclo = cicloAct.NU_Ano.ToString() + cicloAct.NU_Ciclo.ToString();
            ViewBag.FEIni = cicloAct.FE_CicloIni.Day + "/" + cicloAct.FE_CicloIni.Month + "/" + cicloAct.FE_CicloIni.Year;
            ViewBag.FEFin = cicloAct.FE_CicloFin.Day + "/" + cicloAct.FE_CicloFin.Month + "/" + cicloAct.FE_CicloFin.Year;

            DateTime inicio = DateTime.Now;
            DateTime fin = new DateTime(cicloAct.FE_CicloFin.Year, cicloAct.FE_CicloFin.Month, cicloAct.FE_CicloFin.Day);
            TimeSpan restantes = (fin - inicio);
            if (restantes.Days < 0)
                ViewBag.Restantes = 0;
            else
                ViewBag.Restantes = restantes.Days + 1;
            
            
            int visitadorID = (Int32)Membership.GetUser().ProviderUserKey;
            resultado = db.SP_MWL_GetCoberturas(visitadorID).First();
            ViewBag.FICHADOS = resultado.Fichados;
            ViewBag.REVISITAS = resultado.Revisitas;
            ViewBag.CONTACTOS = resultado.Fichados + resultado.Revisitas;

            ViewBag.PVISITADOS = resultado.PorcentajeFichadoVisitados;
            ViewBag.PREVISITAS = resultado.PorcentajeRevisitasRevisitados;
            ViewBag.MIX = resultado.Mix;

            ViewBag.PDR = resultado.PDR;
            ViewBag.HOSPITALES = resultado.HOSPITALES + "\n(" + (decimal.Round((decimal)(resultado.HOSPITALES * 100) / (decimal)resultado.MINHOSPITALES, 2)) + "%)";
            ViewBag.FARMACIAS = resultado.FARMACIAS + "\n(" + (decimal.Round((decimal)(resultado.FARMACIAS * 100) / (decimal)resultado.MINFARMACIAS, 2)) + "%)";

            ViewBag.MINPDR = resultado.MINPDR;
            ViewBag.MINHOSPITALES = resultado.MINHOSPITALES;
            ViewBag.MINFARMACIAS = resultado.MINFARMACIAS;

            return View();
        }
    }
}