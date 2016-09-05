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
using System.Configuration;
using System.Data.SqlClient;
using MedinetLibrary;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MedinetWeb.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class SalidaSIDEXController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        public static List<TipoSalida> TipoSalidaList = new List<TipoSalida> { 
            new TipoSalida {TipoSalidaID = 1, TipoSalidaTX = "Xml"},
            new TipoSalida {TipoSalidaID = 2, TipoSalidaTX = "Txt"},
        };

        public static List<TipoCaracter> TipoCaracterList = new List<TipoCaracter> { 
            new TipoCaracter {TipoCaracterID = 1, TipoCaracterTX = "TAB", TipoCaracterVAL = "\t"},
            new TipoCaracter {TipoCaracterID = 2, TipoCaracterTX = "|", TipoCaracterVAL = "|"},
            new TipoCaracter {TipoCaracterID = 3, TipoCaracterTX = ";", TipoCaracterVAL = ";"},
        };

        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            return View();
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult GetSalidaTxtSIDEX(Int16 cicloID, int tipoCaracter)
        {
            try
            {
                string caract = TipoCaracterList.Where(e => e.TipoCaracterID == tipoCaracter).First().TipoCaracterVAL;

                string cnnString = ConfigurationManager.ConnectionStrings["MedinetWeb"].ConnectionString;
                string cmdString = "SP_MWS_SalidaTxtSIDEX";

                SqlConnection cnn;
                SqlCommand cmd;

                using (cnn = new SqlConnection(cnnString))
                {
                    cmd = new SqlCommand(cmdString, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ciclo", cicloID));
                    cnn.Open();

                    SqlDataReader sqlReader = cmd.ExecuteReader();
                    StringBuilder readerData = new StringBuilder();

                    readerData.Append(sqlReader.GetName(0) + caract + sqlReader.GetName(1) + caract + sqlReader.GetName(2) + 
                        caract + sqlReader.GetName(3) + caract + sqlReader.GetName(4) + caract + sqlReader.GetName(5) + 
                        caract + sqlReader.GetName(6) + caract + sqlReader.GetName(7) + caract + sqlReader.GetName(8) +
                        caract + sqlReader.GetName(9) + caract + sqlReader.GetName(10) + caract + sqlReader.GetName(11));
                    readerData.Append(Environment.NewLine);

                    while (sqlReader.Read())
                    {
                        readerData.Append(sqlReader[0] + caract + sqlReader[1] + caract + sqlReader[2] + caract +
                            sqlReader[3] + caract + sqlReader[4] + caract + sqlReader[5] + caract + sqlReader[6] + caract +
                            sqlReader[7] + caract + sqlReader[8] + caract + sqlReader[9] + caract + sqlReader[10] + caract + 
                            sqlReader[11]);
                        readerData.Append(Environment.NewLine);
                    }

                    byte[] byteArray = Encoding.ASCII.GetBytes(readerData.ToString());
                    MemoryStream stream = new MemoryStream(byteArray);
                    return File(stream, "text/plain", "NotasEntrega.txt");
                }
            }
            catch (Exception ex)
            {
                Excepciones.LogException(ex, "SalidaSIDEXController", "GetSalidaSIDEX");
                return null;
            }
        }

        // Action method to get all the registers.
        // DataSourceRequest is aded into the method for the auto creation of KendoUI grid parameters like page, index etc.
        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public ActionResult GetSalidaXmlSIDEX(Int16 cicloID)
        {
            try
            {
                string cnnString = ConfigurationManager.ConnectionStrings["MedinetWeb"].ConnectionString;
                string cmdString = "SP_MWS_SalidaXmlSIDEX";

                SqlConnection cnn;
                SqlCommand cmd;

                using (cnn = new SqlConnection(cnnString))
                {
                    cmd = new SqlCommand(cmdString, cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ciclo", cicloID));
                    cnn.Open();

                    string readerData = string.Empty;
                    using (XmlReader reader = cmd.ExecuteXmlReader())
                    {
                        while (reader.Read())
                        {
                            readerData += reader.ReadOuterXml();
                        }
                    }
                    XDocument doc = XDocument.Parse(readerData);

                    byte[] byteArray = Encoding.ASCII.GetBytes(doc.ToString());
                    MemoryStream stream = new MemoryStream(byteArray);
                    return File(stream, "application/xml", "NotasEntrega.xml");
                }
            }
            catch (Exception ex)
            {
                Excepciones.LogException(ex, "SalidaSIDEXController", "GetSalidaSIDEX");
                return null;
            }
        }

        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public JsonResult GetTipoSalida()
        {
            var listRecord = TipoSalidaList.Select(e => new
            {
                ID = e.TipoSalidaID,
                TX = e.TipoSalidaTX
            });

            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrador")]
        [SessionExpireFilter]
        public JsonResult GetTipoCaracter()
        {
            var listRecord = TipoCaracterList.Select(e => new
            {
                ID = e.TipoCaracterID,
                TX = e.TipoCaracterTX
            });

            return Json(listRecord, JsonRequestBehavior.AllowGet);
        }
    }

    public class TipoSalida
    {
        public int TipoSalidaID;
        public string TipoSalidaTX;
    }

    public class TipoCaracter
    {
        public int TipoCaracterID;
        public string TipoCaracterTX;
        public string TipoCaracterVAL;
    }
}