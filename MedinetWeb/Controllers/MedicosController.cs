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
using System.IO;

namespace MedinetWeb.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class MedicosController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador, EditorMedicos")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View();
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, EditorMedicos")]
        [SessionExpireFilter]
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var lista = _database.Medicos.GetAll();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.ID_Medico,
                record.NU_RegistroSanitario,
                record.TX_Nombre1,
                record.TX_Nombre2,
                record.TX_Apellido1,
                record.TX_Apellido2,
                record.TX_Sello,
                record.BO_Activo
            }));
        }

        // Action method to add new registers to databaes.
        // Accecpt only HTTP POST request.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, EditorMedicos")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add([DataSourceRequest] DataSourceRequest request, MW_Medicos record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Add Register to UoW.
                _database.Medicos.Add(record);
                // Save Register to database using UoW.
                _database.Save();
            }
            // Return added Register record back to client side in json format and embeded request, modelstate information.
            return Json(new[] { record }.ToDataSourceResult(request, ModelState));
        }

        // Action method to update Register record to database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, EditorMedicos")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, MW_Medicos record)
        {
            // Test if Register object and modelstate is valid.
            if (record != null && ModelState.IsValid)
            {
                // Update Register to UoW.
                _database.Medicos.Update(record);
                // Save updated Register to database using UoW.
                _database.Save();
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        // Action method to delete Register record from database.
        // Accecpt only HTTP POST request from client side.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador, EditorMedicos")]
        [SessionExpireFilter]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, MW_Medicos record)
        {
            // Test if Register object and modelstate is valid.           
            if (record != null && ModelState.IsValid)
            {
                // Delete Register from UoW.
                _database.Medicos.Delete(record);
                // Call to database and delete the deleted record.
                _database.Save();

                List<string> sello = new List<string>();
                sello.Add(record.TX_Sello);
                RemoveImage(sello.ToArray());
            }
            // Return modelstate info back to client side in json format.
            return Json(ModelState.ToDataSourceResult());
        }

        //public JsonResult habilNroSanitario(int value)
        //{
        //    return Json(db.MW_Medicos.Any(m => m.NU_RegistroSanitario == value), JsonRequestBehavior.AllowGet);
        //}

        [Authorize(Roles = "Administrador, EditorMedicos")]
        [SessionExpireFilter]
        public ActionResult SaveImage(HttpPostedFileBase file)
        {
            string directorio = Server.MapPath("~/Content/SelloMedicos/");
            MedinetLibrary.Utilidades.CrearDirectorio(directorio);

            var fileName = Path.GetFileName(file.FileName);
            var physicalPath = Path.Combine(directorio, fileName);

            file.SaveAs(physicalPath);

            return Json(new { TX_Sello = fileName }, "text/plain");
        }

        [Authorize(Roles = "Administrador, EditorMedicos")]
        [SessionExpireFilter]
        public ActionResult RemoveImage(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    if(fileName != null)
                    {
                        string directorio = Server.MapPath("~/Content/SelloMedicos/");
                        MedinetLibrary.Utilidades.CrearDirectorio(directorio);

                        var physicalPath = Path.Combine(directorio, fileName);

                        // TODO: Verify user permissions
                        if (System.IO.File.Exists(physicalPath))
                        {
                             //The files are not actually removed in this demo
                             System.IO.File.Delete(physicalPath);
                        }
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
    }
}