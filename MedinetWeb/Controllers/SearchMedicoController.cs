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
    public class SearchMedicoController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Visitador Web")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View();
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Visitador Web")]
        [SessionExpireFilter]
        public ActionResult GetAll(string text, int number, [DataSourceRequest] DataSourceRequest request)
        {

            if (text.Length > 0 && number > 0)
            {
                var lista = db.MW_Medicos.Where(m => m.TX_Nombre1.StartsWith(text) || m.TX_Nombre2.StartsWith(text) ||
                    m.TX_Apellido1.StartsWith(text) || m.TX_Apellido2.StartsWith(text) || m.NU_RegistroSanitario == number);
                return Json(lista.ToDataSourceResult(request, record => new
                {
                    record.ID_Medico,
                    record.NU_RegistroSanitario,
                    record.TX_Nombre1,
                    record.TX_Nombre2,
                    record.TX_Apellido1,
                    record.TX_Apellido2,
                }));
            }
            else if (text.Length > 0)
            {
                var lista = db.MW_Medicos.Where(m => m.TX_Nombre1.StartsWith(text) || m.TX_Nombre2.StartsWith(text) ||
                m.TX_Apellido1.StartsWith(text) || m.TX_Apellido2.StartsWith(text));
                return Json(lista.ToDataSourceResult(request, record => new
                {
                    record.ID_Medico,
                    record.NU_RegistroSanitario,
                    record.TX_Nombre1,
                    record.TX_Nombre2,
                    record.TX_Apellido1,
                    record.TX_Apellido2,
                }));
            }
            else if (number > 0)
            {
                var lista = db.MW_Medicos.Where(m => m.NU_RegistroSanitario == number);
                return Json(lista.ToDataSourceResult(request, record => new
                {
                    record.ID_Medico,
                    record.NU_RegistroSanitario,
                    record.TX_Nombre1,
                    record.TX_Nombre2,
                    record.TX_Apellido1,
                    record.TX_Apellido2,
                }));
            }
            else
            {
                return Json(null);
            }
            // Retrieve all the Registers from database.
            //var lista = _database.Medicos.GetAll();  //_database.Registers.GetAll();
            //var lista = db.MW_Medicos.Where(m => m.TX_Nombre1.StartsWith(text) || m.TX_Nombre2.StartsWith(text) ||
            //    m.TX_Apellido1.StartsWith(text) || m.TX_Apellido2.StartsWith(text) || m.NU_RegistroSanitario == number);  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.

        }
        [Authorize(Roles = "Visitador Web")]
        [SessionExpireFilter]
        public JsonResult GetAuto(string text, [DataSourceRequest] DataSourceRequest request)
        {

            List<string> contenido = new List<string>();
            var lista = db.MW_Medicos.Where(m => m.TX_Nombre1.StartsWith(text) || m.TX_Nombre2.StartsWith(text) || m.TX_Apellido1.StartsWith(text) || m.TX_Apellido2.StartsWith(text));  //_database.Registers.GetAll();
            if (lista.Count() > 0)
            {
                foreach (var item in lista)
                {
                    if (item.TX_Nombre1 != null && item.TX_Nombre1.ToUpper().StartsWith(text.ToUpper()) && !contenido.Contains(item.TX_Nombre1.ToUpper().Trim()))
                        contenido.Add(item.TX_Nombre1.ToUpper().Trim());
                    if (item.TX_Nombre2 != null && item.TX_Nombre2.ToUpper().StartsWith(text.ToUpper()) && !contenido.Contains(item.TX_Nombre2.ToUpper().Trim()))
                        contenido.Add(item.TX_Nombre2.ToUpper().Trim());
                    if (item.TX_Apellido1 != null && item.TX_Apellido1.ToUpper().StartsWith(text.ToUpper()) && !contenido.Contains(item.TX_Apellido1.ToUpper().Trim()))
                        contenido.Add(item.TX_Apellido1.ToUpper().Trim());
                    if (item.TX_Apellido2 != null && item.TX_Apellido2.ToUpper().StartsWith(text.ToUpper()) && !contenido.Contains(item.TX_Apellido2.ToUpper().Trim()))
                        contenido.Add(item.TX_Apellido2.ToUpper().Trim());
                }
            }


            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(contenido.OrderBy(c => c).ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}