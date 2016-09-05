using MedinetWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace MedinetWeb.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    public class EntradaSIDEXController : Controller
    {
        private readonly DataBase _database = new DataBase();
        private MedinetWebEntities db = new MedinetWebEntities();

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult Index()
        {
            DeleteArtRechazados();
            DeleteExistRechazadas();

            return View();
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult SaveTipoArticulos(HttpPostedFileBase upTipoArticulos)
        {
            var fileName = Path.GetFileName(upTipoArticulos.FileName);
            if (fileName.ToUpper() != "MWS_TIPOARTICULOS.TXT")
                return Content("Has failed item");

            MemoryStream file = StreamToMemoryStream(upTipoArticulos.InputStream);
            string records = Encoding.ASCII.GetString(file.ToArray());
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedinetWeb"].ConnectionString))
                {
                    sqlConn.Open();
                    // bulk send data to database
                    var cmd = new SqlCommand("SP_MWS_BulkInsertTipoArticulos", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    DataTable dtDatos = DatosEntrada("MWS_TipoArticulos", records);
                    if (dtDatos == null)
                        return Content("Has failed item: Uno o varios tipos de dato no coinciden con la estructura.");

                    SqlParameter parameter = cmd.Parameters.AddWithValue("@tipoArticulos", dtDatos);
                    parameter.SqlDbType = SqlDbType.Structured;
                    cmd.ExecuteNonQuery();
                }
                return Json(new { fileName }, "text/plain");
            }
            catch (Exception ex)
            {
                return Content("Has failed item: " + ex.Message.ToString());
            }
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult SaveUnidadesManejo(HttpPostedFileBase upUnidadesManejo)
        {
            var fileName = Path.GetFileName(upUnidadesManejo.FileName);
            if (fileName.ToUpper() != "MWS_UNIDADMANEJO.TXT")
                return Content("Has failed item");

            MemoryStream file = StreamToMemoryStream(upUnidadesManejo.InputStream);
            string records = Encoding.ASCII.GetString(file.ToArray());
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedinetWeb"].ConnectionString))
                {
                    sqlConn.Open();
                    // bulk send data to database
                    var cmd = new SqlCommand("SP_MWS_BulkInsertUnidadManejo", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    DataTable dtDatos = DatosEntrada("MWS_UnidadManejo", records);
                    if (dtDatos == null)
                        return Content("Has failed item: Uno o varios tipos de dato no coinciden con la estructura.");

                    SqlParameter parameter = cmd.Parameters.AddWithValue("@unidadManejo", dtDatos);
                    parameter.SqlDbType = SqlDbType.Structured;
                    cmd.ExecuteNonQuery();
                }
                return Json(new { fileName }, "text/plain");
            }
            catch (Exception ex)
            {
                return Content("Has failed item: " + ex.Message.ToString());
            }
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult SaveMarcas(HttpPostedFileBase upMarcas)
        {
            var fileName = Path.GetFileName(upMarcas.FileName);
            if (fileName.ToUpper() != "MWS_MARCASSIDEX.TXT")
                return Content("Has failed item");

            MemoryStream file = StreamToMemoryStream(upMarcas.InputStream);
            string records = Encoding.ASCII.GetString(file.ToArray());
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedinetWeb"].ConnectionString))
                {
                    sqlConn.Open();
                    // bulk send data to database
                    var cmd = new SqlCommand("SP_MWS_BulkInsertMarcasSIDEX", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    DataTable dtDatos = DatosEntrada("MWS_MarcasSIDEX", records);
                    if (dtDatos == null)
                        return Content("Has failed item: Uno o varios tipos de dato no coinciden con la estructura.");

                    SqlParameter parameter = cmd.Parameters.AddWithValue("@marcasSIDEX", dtDatos);
                    parameter.SqlDbType = SqlDbType.Structured;
                    cmd.ExecuteNonQuery();
                }
                return Json(new { fileName }, "text/plain");
            }
            catch (Exception ex)
            {
                return Content("Has failed item: " + ex.Message.ToString());
            }
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult SaveArticulos(HttpPostedFileBase upArticulos)
        {
            var fileName = Path.GetFileName(upArticulos.FileName);
            if (fileName.ToUpper() != "MWS_ARTICULOS.TXT")
                return Content("Has failed item");

            MemoryStream file = StreamToMemoryStream(upArticulos.InputStream);
            string records = Encoding.ASCII.GetString(file.ToArray());
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedinetWeb"].ConnectionString))
                {
                    sqlConn.Open();
                    // bulk send data to database
                    var cmd = new SqlCommand("SP_MWS_BulkInsertArticulos", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    DataTable dtDatos = DatosEntrada("MWS_Articulos", records);
                    if (dtDatos == null)
                        return Content("Has failed item: Uno o varios tipos de dato no coinciden con la estructura.");

                    SqlParameter parameter = cmd.Parameters.AddWithValue("@articulos", dtDatos);
                    parameter.SqlDbType = SqlDbType.Structured;
                    cmd.ExecuteNonQuery();
                }
                return Json(new { fileName }, "text/plain");
            }
            catch (Exception ex)
            {
                return Content("Has failed item: " + ex.Message.ToString());
            }
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetArtRechazados([DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var lista = _database.ArticulosRechazados.GetAll();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.TX_IDArticuloCliente,
                record.TX_Articulo,
                record.TX_ArticuloAbr,
                record.TX_IDMarcaCliente,
                record.TX_IDTipoArticuloCliente,
                record.TX_IDUnidadManejoClientePri,
                record.NU_UnidadManejo,
                record.TX_IDUnidadManejoClienteSec,
                record.NU_Peso_KG,
                record.NU_Pie_CUB,
                record.BO_Lote,
                record.BO_Activo
            }));
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        private void DeleteArtRechazados()
        {
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE MWS_ArticulosRechazados");
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult SaveExistencias(HttpPostedFileBase upExistencias)
        {
            var fileName = Path.GetFileName(upExistencias.FileName);
            if (fileName.ToUpper() != "MWS_EXISTENCIAS.TXT")
                return Content("Has failed item");

            MemoryStream file = StreamToMemoryStream(upExistencias.InputStream);
            string records = Encoding.ASCII.GetString(file.ToArray());
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedinetWeb"].ConnectionString))
                {
                    sqlConn.Open();
                    // bulk send data to database
                    var cmd = new SqlCommand("SP_MWS_BulkInsertExistencias", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    DataTable dtDatos = DatosEntrada("MWS_Existencias", records);
                    if (dtDatos == null)
                        return Content("Has failed item: Uno o varios tipos de dato no coinciden con la estructura.");

                    SqlParameter parameter = cmd.Parameters.AddWithValue("@existencias", dtDatos);
                    parameter.SqlDbType = SqlDbType.Structured;
                    cmd.CommandTimeout = 60000;
                    cmd.ExecuteNonQuery();
                }
                return Json(new { fileName }, "text/plain");
            }
            catch (Exception ex)
            {
                return Content("Has failed item: " + ex.Message.ToString());
            }
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        public ActionResult GetExistRechazadas([DataSourceRequest] DataSourceRequest request)
        {
            // Retrieve all the Registers from database.
            var lista = _database.ExistenciasRechazadas.GetAll();  //_database.Registers.GetAll();
            // Return the Registers in json format back to KendoUI grid.
            // Anonymous type selector is added to prevent circular reference of json with entity framework.
            return Json(lista.ToDataSourceResult(request, record => new
            {
                record.TX_IDArticuloCliente,
                record.TX_Lote,
                record.TX_Ubicacion,
                record.FE_VencimientoLote,
                record.NU_CantidadPrimaria,
                record.NU_CantidadSecundaria,
                record.NU_UltimoCosto
            }));
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        private void DeleteExistRechazadas()
        {
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE MWS_ExistenciasRechazadas");
        }

        [SessionExpireFilter]
        private MemoryStream StreamToMemoryStream(Stream file)
        {
            MemoryStream ms;
            ms = new MemoryStream();
            file.CopyTo(ms);

            return ms;
        }

        //********************************************METODOS CARGA DE DATOS

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        private DataTable DatosEntrada(string tabla, string datos)
        {
            DataTable dt = new DataTable();
            dt = ModelDataTable(tabla);
            DataRow dr;
            int linea = 0;
            try
            {
                using (StringReader reader = new StringReader(datos))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] s = line.Split(new char[] { ';' });
                        dr = dt.NewRow();

                        for (int i = 0; i <= dt.Columns.Count - 1; i++)
                            if (dt.Columns[i].DataType != typeof(Boolean))
                            {
                                if (dt.Columns[i].DataType != typeof(DateTime))
                                {
                                    dr[i] = s[i];
                                }
                                else
                                {
                                    if (s[i] != "")
                                    {
                                        dr[i] = s[i];
                                    }
                                }
                            }
                            else
                            {
                                dr[i] = s[i] == "0" ? false : true;
                            }

                        dt.Rows.Add(dr);
                        linea++;
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + linea);
                return null;
            }
        }

        [Authorize(Roles = "Administrador, Mercadeo")]
        [SessionExpireFilter]
        private DataTable ModelDataTable(string table)
        {
            string query;
            switch (table)
            {
                case "MWS_TipoArticulos":
                    query = "SET FMTONLY ON; SELECT TX_IDTipoArticuloCliente, TX_TipoArticulo, BO_Activo " +
                        "FROM " + table + " ; SET FMTONLY OFF;";
                    break;
                case "MWS_UnidadManejo":
                    query = "SET FMTONLY ON; SELECT TX_IDUnidadManejoCliente, TX_UnidadManejo, BO_Activo " +
                        "FROM " + table + " ; SET FMTONLY OFF;";
                    break;
                case "MWS_MarcasSIDEX":
                    query = "SET FMTONLY ON; SELECT TX_IDMarcaCliente, TX_Marca, BO_Activo " +
                        "FROM " + table + " ; SET FMTONLY OFF;";
                    break;
                case "MWS_Articulos":
                    query = "SET FMTONLY ON; SELECT TX_IDArticuloCliente, TX_Articulo, TX_ArticuloAbr, " +
                        "CAST(ID_Marca AS varchar(20)) AS TX_IDMarcaCliente, " +
                        "CAST(ID_TipoArticulo AS varchar(10)) AS TX_IDTipoArticuloCliente, " +
                        "CAST(ID_UnidadManejoPri AS varchar(10)) AS TX_IDUnidadManejoClientePri, NU_UnidadManejo, " +
                        "CAST(ID_UnidadManejoSec AS varchar(10)) AS TX_IDUnidadManejoClienteSec, NU_Peso_KG, " +
                        "NU_Pie_CUB, BO_Lote, BO_Activo FROM " + table + " ; SET FMTONLY OFF;";
                    break;
                case "MWS_Existencias":
                    query = "SET FMTONLY ON; SELECT CAST(ID_Articulo AS varchar(30)) AS TX_IDArticuloCliente, " +
                        "TX_Lote, TX_Ubicacion, FE_VencimientoLote, NU_CantidadPrimaria, NU_CantidadSecundaria, " +
                        "NU_UltimoCosto FROM " + table + " ; SET FMTONLY OFF;";
                    break;
                default:
                    query = "";
                    break;
            }

            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedinetWeb"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, sqlConn))
            {
                sqlConn.Open();
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }
    }
}
