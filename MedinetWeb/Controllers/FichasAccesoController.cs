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
    public class FichasAccesoController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            ViewData["dvUsuarios"] = db.MW_Usuarios.ToList()
                .Select(e => new
                {
                    ID = e.ID_Usuario,
                    TX = e.TX_Nombre.ToString() + " " + e.TX_Apellido.ToString()
                });

            return View();
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var lista = _database.FichasAcceso.GetAll();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_FichaAcceso,
                record.ID_Usuario,
                record.NU_M1,
                record.NU_M2,
                record.NU_M3,
                record.NU_D1,
                record.NU_D2,
                record.NU_D3,
                record.NU_N1,
                record.NU_N2,
                record.NU_N3,
                record.FE_Registro,
                record.BO_Activo
            }));
        }

        // Action method to add new registers to databaes.
        // Accecpt only HTTP POST request.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add([DataSourceRequest] DataSourceRequest request, MW_FichasAcceso record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Add Register to UoW.
                List<int> list = GetRandomNumber();

                record.NU_M1 = Convert.ToInt16(list[0]);
                record.NU_M2 = Convert.ToInt16(list[1]);
                record.NU_M3 = Convert.ToInt16(list[2]);
                record.NU_D1 = Convert.ToInt16(list[3]);
                record.NU_D2 = Convert.ToInt16(list[4]);
                record.NU_D3 = Convert.ToInt16(list[5]);
                record.NU_N1 = Convert.ToInt16(list[6]);
                record.NU_N2 = Convert.ToInt16(list[7]);
                record.NU_N3 = Convert.ToInt16(list[8]);

                _database.FichasAcceso.Add(record);
                // Save Register to database using UoW.
                _database.Save();
            }
            // Return added Register record back to client side in json format and embeded request, modelstate information.
            return Json(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        // Action method to update Register record to database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MW_FichasAcceso record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Update Register to UoW.
                _database.FichasAcceso.Update(record);
                // Save updated Register to database using UoW.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        [Authorize]
        [SessionExpireFilter]
        private List<int> GetRandomNumber()
        {
            Random rg = new Random();
            List<int> list = Enumerable.Range(10, 90).ToList();
            list.Shuffle(rg);

            return list;
        }

        [Authorize]
        [SessionExpireFilter]
        public ActionResult GetFichaAcceso(short fichaAccesoID, [DataSourceRequest] DataSourceRequest request)
        {
            var recordList = db.SP_MW_FichaAcceso(fichaAccesoID).ToList();
            return Json(recordList.ToDataSourceResult(request, record => new
            {
                record.Nro,
                record.M,
                record.D,
                record.N
            }));
        }
    }

    static class IListExtensions
    {
        public static void Shuffle<T>(this IList<T> list, Random rg)
        {
            for (int i = list.Count; i > 1; i--)
            {
                int k = rg.Next(i);
                T temp = list[k];
                list[k] = list[i - 1];
                list[i - 1] = temp;
            }
        }
    }
}