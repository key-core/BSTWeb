using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.IO;
using Ionic.Zip;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using MedinetLibrary;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Web.Configuration;
using System.Diagnostics;

namespace MedinetCRMService
{
    /// <summary>
    /// Summary description for MedinetCRMService
    /// </summary>
    [WebService(Namespace = "http://Localhost/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MedinetCRMService : System.Web.Services.WebService
    {

#if DEBUG
        //SqlConnection sqlConexion = new SqlConnection(ConfigurationManager.ConnectionStrings["MedinetWebLOCAL"].ConnectionString);
        SqlConnection sqlConexion = new SqlConnection(ConfigurationManager.ConnectionStrings["MedinetWeb"].ConnectionString);
#else
        SqlConnection sqlConexion = new SqlConnection(ConfigurationManager.ConnectionStrings["MedinetWeb"].ConnectionString);
#endif

        string sqlSelect;
        SqlCommand sqlComando;
        SqlDataReader sqlReader;

        int idVisitadorHistorial = 0;
        int ano = 0;
        int ciclo = 0;
        int idCiclo = 0;

        string rutaBase = ConfigurationManager.AppSettings["rutaBase"].ToString();
        string rutaDesconocidos = ConfigurationManager.AppSettings["rutaDesconocidos"].ToString();
        string rutaDesfasados = ConfigurationManager.AppSettings["rutaDesfasados"].ToString();
        string rutaBackup = ConfigurationManager.AppSettings["rutaBackup"].ToString();
        string rutaCiclos = ConfigurationManager.AppSettings["rutaCiclos"].ToString();
        string rutaFechas = ConfigurationManager.AppSettings["rutaFechas"].ToString();
        string rutaNoProcesados = ConfigurationManager.AppSettings["rutaNoProcesados"].ToString();
        bool bProcesarArchivos = bool.Parse(ConfigurationManager.AppSettings["procesarArchivos"].ToString());
        bool bEnviarSMS = bool.Parse(ConfigurationManager.AppSettings["enviarSMS"].ToString());
        bool bEnviarSMSNumeroMDN = bool.Parse(ConfigurationManager.AppSettings["enviarSMSNumeroMDN"].ToString());

        string fechaActualCorta;
        List<Medico> listadoMedicos;
        List<Farmacia> listadoFarmacias;
        List<Hospital> listadoHospitales;
        List<Ficha> listadoFichas;
        List<MedicoReceta> listadoMedicosRecetas;
        List<EsquemaPromocional> listadoEsquemaPromocional;
        List<Ciclo> listadoCiclos;
        List<CicloVehiculo> listadoCiclosV;
        string nombreArchivoPublico = "";
        Byte[] contenidoArchivoPublico;

        private void modificarProcesamiento(bool opcion)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("/");
            string oldValue = config.AppSettings.Settings["procesarArchivos"].Value;
            config.AppSettings.Settings["procesarArchivos"].Value = opcion.ToString();
            config.Save(ConfigurationSaveMode.Modified);
        }

        /*----------------------MEDINET HIBRIDO---------------------*/

        [WebMethod]
        public Medico[] getMedicosTodos()
        {
            sqlConexion.Open();

            sqlComando = new SqlCommand("SP_CRMServ_MedicosTodos", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlReader = sqlComando.ExecuteReader();

            listadoMedicos = new List<Medico>();
            while (sqlReader.Read())
            {
                listadoMedicos.Add(new Medico(sqlReader.GetInt32(0), sqlReader.GetString(1)));
            }
            sqlConexion.Close();
            return listadoMedicos.ToArray();
        }
        [WebMethod]
        public Medico[] getMedicosNroSanitario(string nroSanitario)
        {
            sqlConexion.Open();
            sqlComando = new SqlCommand("SP_CRMServ_MedicoNroSanitario", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@RegistroSanitario", nroSanitario);
            sqlReader = sqlComando.ExecuteReader();

            listadoMedicos = new List<Medico>();
            while (sqlReader.Read())
            {
                listadoMedicos.Add(new Medico(sqlReader.GetInt32(0), sqlReader.GetString(1)));
            }
            sqlConexion.Close();
            return listadoMedicos.ToArray();
        }
        [WebMethod]
        public Medico[] getMedicosCadena(string cadena)
        {
            sqlConexion.Open();
            sqlComando = new SqlCommand("SP_CRMServ_MedicoEntrada", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@Cadena", cadena);
            sqlReader = sqlComando.ExecuteReader();

            listadoMedicos = new List<Medico>();
            while (sqlReader.Read())
            {
                listadoMedicos.Add(new Medico(sqlReader.GetInt32(0), sqlReader.GetString(1)));
            }
            sqlConexion.Close();
            return listadoMedicos.ToArray();
        }
        [WebMethod]
        public Farmacia[] getMaestraFarmacias(int region)
        {
            sqlConexion.Open();
            sqlComando = new SqlCommand("SP_CRMServ_MaestraFarmacias", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@Region", region);
            sqlReader = sqlComando.ExecuteReader();

            listadoFarmacias = new List<Farmacia>();
            while (sqlReader.Read())
            {
                listadoFarmacias.Add(new Farmacia(sqlReader.GetInt16(0), sqlReader.GetInt16(1), sqlReader.GetString(2), sqlReader.GetString(3), sqlReader.GetString(4), sqlReader.GetString(5), sqlReader.GetString(6), sqlReader.GetString(7), sqlReader.GetString(8), (sqlReader.IsDBNull(9)) ? "S/T" : sqlReader.GetString(9), sqlReader.GetString(10)));
            }
            sqlConexion.Close();
            return listadoFarmacias.ToArray();
        }
        [WebMethod]
        public Hospital[] getMaestraHospitales(int region)
        {
            sqlConexion.Open();
            sqlComando = new SqlCommand("SP_CRMServ_MaestraHospitales", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@Region", region);
            sqlReader = sqlComando.ExecuteReader();

            listadoHospitales = new List<Hospital>();
            while (sqlReader.Read())
            {
                listadoHospitales.Add(new Hospital(sqlReader.GetInt16(0), sqlReader.GetInt16(1), sqlReader.GetString(2), sqlReader.GetString(3), sqlReader.GetString(4), sqlReader.GetString(5), sqlReader.GetBoolean(6), sqlReader.GetBoolean(7)));
            }
            sqlConexion.Close();
            return listadoHospitales.ToArray();
        }
        [WebMethod]
        public EsquemaPromocional[] getEsquemaPromocionalHibrido(int ano, int ciclo, int linea)
        {
            sqlConexion.Open();
            sqlComando = new SqlCommand("SP_CRMServ_EsquemaPromocional", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@Ano", ano);
            sqlComando.Parameters.AddWithValue("@Ciclo", ciclo);
            sqlComando.Parameters.AddWithValue("@Linea", linea);
            sqlReader = sqlComando.ExecuteReader();

            listadoEsquemaPromocional = new List<EsquemaPromocional>();
            while (sqlReader.Read())
            {
                listadoEsquemaPromocional.Add(new EsquemaPromocional(sqlReader.GetInt16(0), sqlReader.GetString(1), sqlReader.GetString(2), sqlReader.GetString(3)));
            }
            sqlConexion.Close();
            return listadoEsquemaPromocional.ToArray();
        }
        [WebMethod]
        public Ciclo[] getCiclosHibrido()
        {
            sqlConexion.Open();
            sqlComando = new SqlCommand("SP_CRMServ_CiclosHibrido", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;

            sqlReader = sqlComando.ExecuteReader();

            listadoCiclos = new List<Ciclo>();
            DataTable sqlDataTable = new DataTable();
            if (sqlReader.HasRows)
            {
                sqlDataTable.Load(sqlReader);
            }

            foreach (DataRow dataRow in sqlDataTable.Rows)
            {
                listadoCiclos.Add(new Ciclo(dataRow[0].ToString().Substring(0, 10), dataRow[1].ToString().Substring(0, 10), Convert.ToInt16(dataRow[2]), convertirStatus(Convert.ToInt16(dataRow[3])), Convert.ToInt16(dataRow[4]), convertirStatus(Convert.ToInt16(dataRow[3]))));//(int)dataRow[2], convertirStatus((int)dataRow[3]), (int)dataRow[4], convertirStatus((int)dataRow[3])));


                //while (sqlReader.Read())
                //{
                //    listadoCiclos.Add(new Ciclo(sqlReader.GetDateTime(0).ToString().Substring(0, 10).Replace("-", "/"), sqlReader.GetDateTime(1).ToString().Substring(0, 10).Replace("-", "/"), sqlReader.GetInt32(2), convertirStatus(sqlReader.GetInt16(3)), sqlReader.GetInt16(4), convertirStatus(sqlReader.GetInt16(3))));
                //}
            }
            sqlConexion.Close();
            return listadoCiclos.ToArray();
        }

        [WebMethod]
        public Ficha[] getFichasHibrido(int idRegion, int idLinea)
        {
            sqlConexion.Open();
            sqlComando = new SqlCommand("SP_CRMServ_FichasHibrido", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@idRegion", idRegion);
            sqlComando.Parameters.AddWithValue("@idLinea", idLinea);
            sqlReader = sqlComando.ExecuteReader();

            listadoFichas = new List<Ficha>();
            DataTable sqlDataTable = new DataTable();
            if (sqlReader.HasRows)
            {
                sqlDataTable.Load(sqlReader);
            }

            foreach (DataRow dataRow in sqlDataTable.Rows)
            {
                listadoFichas.Add(new Ficha(dataRow[0].ToString(), int.Parse(dataRow[1].ToString()), int.Parse(dataRow[2].ToString()), int.Parse(dataRow[3].ToString()), int.Parse(dataRow[4].ToString()), int.Parse(dataRow[5].ToString()), int.Parse(dataRow[6].ToString()), int.Parse(dataRow[7].ToString()), int.Parse(dataRow[8].ToString()), int.Parse(dataRow[9].ToString())));
            }
            sqlConexion.Close();
            return listadoFichas.ToArray();
        }
        [WebMethod]
        public XmlDocument getTablasMaestras(String nombreTabla)
        {
            sqlSelect = "SELECT * FROM " + nombreTabla;
            sqlConexion.Open();
            sqlComando = new SqlCommand(sqlSelect, sqlConexion);
            sqlReader = sqlComando.ExecuteReader();

            DataTable sqlDataTable = new DataTable();

            if (sqlReader.HasRows)
            {
                sqlDataTable.Load(sqlReader);
            }

            XmlDocument memoria = new XmlDocument();
            XmlNode nodoPrincipal = memoria.CreateElement(nombreTabla);
            memoria.AppendChild(nodoPrincipal);

            foreach (DataRow dataRow in sqlDataTable.Rows)
            {
                XmlNode nodoRegistro = memoria.CreateElement("Registro");
                nodoPrincipal.AppendChild(nodoRegistro);
                foreach (DataColumn dataColumn in sqlDataTable.Columns)
                {
                    XmlNode nodoCampo = memoria.CreateElement(dataColumn.ColumnName);
                    nodoRegistro.AppendChild(nodoCampo);

                    switch (dataColumn.DataType.Name)
                    {

                        //case "Boolean":
                        //    nodoCampo.AppendChild(memoria.CreateTextNode(dataRow[dataColumn].ToString()));
                        //    break;
                        case "Date":
                        case "DateTime":
                            String fecha = dataRow[dataColumn].ToString().Substring(0, 10);
                            nodoCampo.AppendChild(memoria.CreateTextNode(fecha));
                            break;
                        default:
                            nodoCampo.AppendChild(memoria.CreateTextNode(dataRow[dataColumn].ToString()));
                            break;
                    }

                    //if (dataColumn.ColumnName.Substring(0, 2) == "FE")
                    //{
                    //    //14/01/2013
                    //    String fecha = dataRow[dataColumn].ToString().Substring(0, 10);
                    //    nodoCampo.AppendChild(memoria.CreateTextNode(fecha));
                    //}
                    //else
                    //{
                    //    nodoCampo.AppendChild(memoria.CreateTextNode(dataRow[dataColumn].ToString()));
                    //}
                }
            }
            sqlConexion.Close();
            return memoria;
        }
        [WebMethod]
        public XmlDocument getInfoPorLinea(String nombreTabla, int ID_Linea)
        {
            sqlSelect = "SELECT * FROM " + nombreTabla + " WHERE ID_Linea=" + ID_Linea;
            sqlConexion.Open();
            sqlComando = new SqlCommand(sqlSelect, sqlConexion);
            sqlReader = sqlComando.ExecuteReader();

            DataTable sqlDataTable = new DataTable();
            if (sqlReader.HasRows)
            {
                sqlDataTable.Load(sqlReader);
            }

            XmlDocument memoria = new XmlDocument();
            XmlNode nodoPrincipal = memoria.CreateElement(nombreTabla);
            memoria.AppendChild(nodoPrincipal);

            foreach (DataRow dataRow in sqlDataTable.Rows)
            {
                XmlNode nodoRegistro = memoria.CreateElement("Registro");
                nodoPrincipal.AppendChild(nodoRegistro);
                foreach (DataColumn dataColumn in sqlDataTable.Columns)
                {
                    XmlNode nodoCampo = memoria.CreateElement(dataColumn.ColumnName);
                    nodoRegistro.AppendChild(nodoCampo);
                    if (dataColumn.ColumnName.Substring(0, 2) == "FE")
                    {
                        //14/01/2013
                        String fecha = dataRow[dataColumn].ToString().Substring(0, 10);
                        nodoCampo.AppendChild(memoria.CreateTextNode(fecha));
                    }
                    else
                    {
                        nodoCampo.AppendChild(memoria.CreateTextNode(dataRow[dataColumn].ToString()));
                    }
                }
            }
            sqlConexion.Close();
            return memoria;
        }
        [WebMethod]
        public XmlDocument getEsquemaPromocional(int ID_Linea, int ID_Ciclo)
        {
            sqlSelect = "SELECT * FROM MW_EsquemasPromocionales WHERE ID_Linea=" + ID_Linea + " AND ID_Ciclo=" + ID_Ciclo;
            sqlConexion.Open();
            sqlComando = new SqlCommand(sqlSelect, sqlConexion);
            sqlReader = sqlComando.ExecuteReader();

            DataTable sqlDataTable = new DataTable();
            if (sqlReader.HasRows)
            {
                sqlDataTable.Load(sqlReader);
            }

            XmlDocument memoria = new XmlDocument();
            XmlNode nodoPrincipal = memoria.CreateElement("EsquemaPromocional");
            memoria.AppendChild(nodoPrincipal);

            foreach (DataRow dataRow in sqlDataTable.Rows)
            {
                XmlNode nodoRegistro = memoria.CreateElement("Registro");
                nodoPrincipal.AppendChild(nodoRegistro);
                foreach (DataColumn dataColumn in sqlDataTable.Columns)
                {
                    XmlNode nodoCampo = memoria.CreateElement(dataColumn.ColumnName);
                    nodoRegistro.AppendChild(nodoCampo);
                    if (dataColumn.ColumnName.Substring(0, 2) == "FE")
                    {
                        //14/01/2013
                        String fecha = dataRow[dataColumn].ToString().Substring(0, 10);
                        nodoCampo.AppendChild(memoria.CreateTextNode(fecha));
                    }
                    else
                    {
                        nodoCampo.AppendChild(memoria.CreateTextNode(dataRow[dataColumn].ToString()));
                    }
                }
            }
            sqlConexion.Close();
            return memoria;
        }
        [WebMethod]
        public Tabla[] getEstructura()
        {
            String miStringFinal = "";
            StringBuilder miString = new StringBuilder();
            DataTable sqlDataTableDB = new DataTable();
            Tabla miTabla;
            List<Tabla> misTablas = new List<Tabla>();

            sqlConexion.Open();
            sqlSelect = "EXEC sp_tables @table_owner='dbo'";
            sqlComando = new SqlCommand(sqlSelect, sqlConexion);
            sqlReader = sqlComando.ExecuteReader();

            if (sqlReader.HasRows)
            {
                sqlDataTableDB.Load(sqlReader);
            }

            foreach (DataRow dataRow in sqlDataTableDB.Rows)
            {
                miTabla = new Tabla();

                miTabla.Nombre = dataRow[2].ToString();
                if (!miTabla.Nombre.StartsWith("sys"))
                {
                    sqlSelect = "SELECT TOP 0 * FROM " + miTabla.Nombre;
                    sqlComando = new SqlCommand(sqlSelect, sqlConexion);
                    sqlReader = sqlComando.ExecuteReader();

                    DataTable sqlDataTable = new DataTable();
                    sqlDataTable.Load(sqlReader);
                    miString.Append("CREATE TABLE " + miTabla.Nombre.ToLower() + " (");
                    foreach (DataColumn dataColumn in sqlDataTable.Columns)
                    {

                        String tipo = "";
                        int longitud = 0;
                        switch (dataColumn.DataType.Name)
                        {
                            case "Int16":
                            case "Int32":
                            case "Int64":
                            case "Boolean":
                                tipo = "INTEGER";
                                break;
                            case "String":
                                tipo = "TEXT";
                                longitud = dataColumn.MaxLength;
                                break;
                            case "Float":
                            case "Double":
                                tipo = "REAL";
                                break;
                            case "Date":
                            case "DateTime":
                                tipo = "TEXT";
                                longitud = 23;
                                break;
                            case "Decimal":
                                tipo = "NUMERIC";
                                break;
                            default:
                                tipo = "BLOB";
                                break;
                        }
                        String texto = " " + tipo;
                        if (longitud > 0)
                        {
                            texto += "(" + longitud + ")";
                        }
                        miString.Append(dataColumn.ColumnName + texto + ", ");
                    }
                    miStringFinal = miString.ToString();
                    miStringFinal = miStringFinal.Substring(0, (miStringFinal.Length - 2)) + ");";
                    miTabla.Estructura = miStringFinal;
                    misTablas.Add(miTabla);
                    miStringFinal = "";
                    miString.Clear();
                }
            }
            sqlConexion.Close();
            return misTablas.ToArray();
        }
        [WebMethod]
        public bool guardarArchivo(Byte[] docbinaryarray, string docname)
        {
            //modificarProcesamiento(true);
            nombreArchivoPublico = docname;
            contenidoArchivoPublico = new Byte[docbinaryarray.Length];
            contenidoArchivoPublico = docbinaryarray;
            if (procesarArchivo())
            {
                if (bEnviarSMS && bProcesarArchivos)
                {
                    prepararSMS();
                }
            }
            return true;
        }
        [WebMethod]
        public int getTamanoArchivo(string DocumentName)
        {
            string strdocPath;
            strdocPath = @"C:\TRANSMISIONES HIBRIDO\" + DocumentName;

            FileStream objfilestream = new FileStream(strdocPath, FileMode.Open, FileAccess.Read);
            int len = (int)objfilestream.Length;
            objfilestream.Close();

            return len;
        }
        [WebMethod]
        public Byte[] getArchivo(string DocumentName)
        {
            string strdocPath;
            strdocPath = @"C:\TRANSMISIONES HIBRIDO\" + DocumentName;

            FileStream objfilestream = new FileStream(strdocPath, FileMode.Open, FileAccess.Read);
            int len = (int)objfilestream.Length;
            Byte[] documentcontents = new Byte[len];
            objfilestream.Read(documentcontents, 0, len);
            objfilestream.Close();

            return documentcontents;
        }
        [WebMethod]
        private bool isVisitadorValido(int identificador)
        {
            bool visitadorActivo = false;
            sqlConexion.Open();
            sqlSelect = "SELECT TOP 1 * FROM MW_VisitadoresHistorial WHERE ID_VisitadoresHistorial=" + identificador + " AND BO_Activo=1";
            sqlComando = new SqlCommand(sqlSelect, sqlConexion);
            sqlReader = sqlComando.ExecuteReader();
            if (sqlReader.HasRows)
            {
                visitadorActivo = true;
            }
            sqlConexion.Close();
            return visitadorActivo;
        }
        private bool isCicloValido(int ano, int ciclo)
        {
            bool cicloValido = false;
            sqlConexion.Open();
            sqlSelect = "SELECT TOP 1 CONVERT(date,FE_CicloIni,103),CONVERT(date,FE_CicloProrroga,103), ID_Ciclo FROM MW_Ciclos WHERE NU_Ano=" + ano + " AND NU_Ciclo=" + ciclo + " AND BO_Activo=1";
            sqlComando = new SqlCommand(sqlSelect, sqlConexion);
            sqlReader = sqlComando.ExecuteReader();
            if (sqlReader.HasRows)
            {
                DataTable sqlDataTable = new DataTable();
                sqlDataTable.Load(sqlReader);
                DateTime fechaActual = DateTime.Today;
                DateTime fechaICiclo = DateTime.Parse(sqlDataTable.Rows[0][0].ToString());
                DateTime fechaPCiclo = DateTime.Parse(sqlDataTable.Rows[0][1].ToString());
                idCiclo = int.Parse(sqlDataTable.Rows[0][2].ToString());
                if (fechaActual >= fechaICiclo && fechaActual <= fechaPCiclo)
                {
                    cicloValido = true;
                }
            }
            sqlConexion.Close();
            return cicloValido;
        }
        private bool isArchivoValido()
        {
            //nombreArchivoPublico = "2013-6-119.zip"; //COMENTAR
            bool bProcesar = false;
            int primerGuion = nombreArchivoPublico.IndexOf("-", 0);
            int segundoGuion = nombreArchivoPublico.IndexOf("-", primerGuion + 1);
            int punto = nombreArchivoPublico.IndexOf(".", segundoGuion + 1);
            ano = int.Parse(nombreArchivoPublico.Substring(0, primerGuion));
            ciclo = int.Parse(nombreArchivoPublico.Substring(primerGuion + 1, segundoGuion - primerGuion - 1));
            idVisitadorHistorial = int.Parse(nombreArchivoPublico.Substring(segundoGuion + 1, punto - segundoGuion - 1));
            crearRutas();
            if (isVisitadorValido(idVisitadorHistorial))
            {
                if (isCicloValido(ano, ciclo))
                {
                    if (bProcesarArchivos)
                    {
                        bProcesar = true;
                        guardarArchivoEnDirectorio(1);
                    }
                    else
                    {
                        guardarArchivoEnDirectorio(5);
                        bProcesar = false;
                    }

                }
                else
                {
                    guardarArchivoEnDirectorio(2);
                }
            }
            else
            {
                guardarArchivoEnDirectorio(3);
            }
            return bProcesar;
        }
        private void guardarArchivoEnDirectorio(int tipo)
        {
            string fechaActual = DateTime.Now.ToLongDateString();
            fechaActual = Convert.ToString(fechaActual);
            string[] month = (fechaActual.Split(' '));
            string annio = month[5];
            string nombreMes = month[3];
            string rutaC = "", rutaF = "";
            FileStream objfilestream; ;
            switch (tipo)
            {
                case 1:
                    rutaC = rutaBase + rutaBackup + rutaCiclos + ano + "\\" + ciclo + "\\";
                    rutaF = rutaBase + rutaBackup + rutaFechas + annio + "\\" + nombreMes + "\\" + fechaActual + "\\";
                    break;
                case 2:
                    rutaC = rutaBase + rutaDesfasados + rutaCiclos + ano + "\\" + ciclo + "\\";
                    rutaF = rutaBase + rutaDesfasados + rutaFechas + annio + "\\" + nombreMes + "\\" + fechaActual + "\\";
                    break;
                case 3:
                    rutaC = rutaBase + rutaDesconocidos + rutaCiclos + ano + "\\" + ciclo + "\\";
                    rutaF = rutaBase + rutaDesconocidos + rutaFechas + annio + "\\" + nombreMes + "\\" + fechaActual + "\\";
                    break;
                case 4:
                    rutaC = rutaBase + rutaBackup + rutaCiclos + ano + "\\" + ciclo + "\\CIERRE\\";
                    rutaF = rutaBase + rutaBackup + rutaFechas + annio + "\\" + nombreMes + "\\" + fechaActual + "\\CIERRE\\";
                    break;
                case 5:
                    rutaC = rutaBase + rutaNoProcesados + rutaCiclos + ano + "\\" + ciclo + "\\";
                    rutaF = rutaBase + rutaNoProcesados + rutaFechas + annio + "\\" + nombreMes + "\\" + fechaActual + "\\";
                    break;
            }

            if (!rutaC.Equals("") && !rutaF.Equals(""))
            {
                Directory.CreateDirectory(rutaC);
                Directory.CreateDirectory(rutaF);
                //POR CICLOS
                rutaC += nombreArchivoPublico;
                objfilestream = new FileStream(rutaC, FileMode.Create, FileAccess.ReadWrite);
                objfilestream.Write(contenidoArchivoPublico, 0, contenidoArchivoPublico.Length);
                objfilestream.Close();

                //POR FECHAS
                rutaF += nombreArchivoPublico;
                objfilestream = new FileStream(rutaF, FileMode.Create, FileAccess.ReadWrite);
                objfilestream.Write(contenidoArchivoPublico, 0, contenidoArchivoPublico.Length);
                objfilestream.Close();
            }
        }
        private Boolean procesarArchivo()
        {
            bool respuesta = true;
            bool bDescomprimidos = false;
            string lineaAux;
            if (isArchivoValido() && bProcesarArchivos)
            {

                string sCadenaDeConexionI = string.Empty;
                string msgError = string.Empty;

                datosEnTablas[] obTa = new datosEnTablas[0];
                using (MemoryStream obMSDatos = new MemoryStream(contenidoArchivoPublico))
                {
                    obMSDatos.Position = 0;
                    using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(obMSDatos))
                    {
                        if (zip.Count > 0)
                        {
                            obTa = new datosEnTablas[zip.Count];
                            int i = 0;

                            ICollection<ZipEntry> obFileEntries = zip.Entries;
                            foreach (ZipEntry obZipEntry in obFileEntries)
                            {
                                using (System.IO.MemoryStream obMemoryStream = new MemoryStream())
                                {
                                    //obZipEntry.ExtractWithPassword(obMemoryStream, "InterS_Issa");
                                    obZipEntry.Extract(obMemoryStream);
                                    obMemoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                                    byte[] ByteArray = obMemoryStream.ToArray();
                                    string sData = System.Text.Encoding.ASCII.GetString(ByteArray, 0, ByteArray.Length);
                                    obTa[i].NombreTabla = obZipEntry.FileName.Replace(".txt", "");
                                    obTa[i].Datos = sData;
                                    i++;
                                }
                            }
                            bDescomprimidos = true;
                        }
                    }
                }
                ///
                //bDescomprimidos = false;
                if (bDescomprimidos)
                {
                    sqlConexion.Open();
                    if (sqlConexion.State == System.Data.ConnectionState.Open)
                    {
                        try
                        {
                            foreach (datosEnTablas item in obTa)
                            {
                                sqlSelect = "SELECT TOP 0 * FROM " + item.NombreTabla;
                                sqlComando = new SqlCommand(sqlSelect, sqlConexion);
                                sqlReader = sqlComando.ExecuteReader();

                                DataTable sqlDataTable = new DataTable();
                                sqlDataTable.Load(sqlReader);
                                StringBuilder insertSql = new StringBuilder();

                                string[] lineas = Regex.Split(item.Datos, "\r\n");
                                int indexLinea = 0;
                                string campoFiltro = "ZONA";
                                if (item.NombreTabla.Equals("MD_PedidosCorreosVisCopia") || item.NombreTabla.Equals("MD_PedidosCodVisDrog") || item.NombreTabla.Equals("MD_PedidosFarmacias"))
                                {
                                    campoFiltro = "ID_VisitadorHistorial";
                                }

                                using (SqlCommand sqlDelete = new SqlCommand("DELETE FROM " + item.NombreTabla + " WHERE " + campoFiltro + "='" + idVisitadorHistorial + "' AND CICLO=" + ciclo + " AND NU_ANO=" + ano + "", sqlConexion))//, obTransaction))
                                {
                                    sqlDelete.ExecuteNonQuery();
                                }
                                try
                                {
                                    string[] nombreColumnas = new string[0];
                                    int campoCiclo = -1;
                                    foreach (string linea in lineas)
                                    {

                                        string[] sCampos = linea.Split('|');


                                        if (indexLinea > 0)
                                        {
                                            if (campoCiclo < 0)
                                            {

                                                List<string> listaCampos = new List<string>();
                                                foreach (string campo in nombreColumnas)
                                                {
                                                    listaCampos.Add(campo.ToUpper());
                                                }
                                                if (listaCampos.Contains("CICLO_VISITA"))
                                                {
                                                    campoCiclo = listaCampos.IndexOf("CICLO_VISITA");
                                                }
                                                else if (listaCampos.Contains("CICLO_VISITADO"))
                                                {
                                                    campoCiclo = listaCampos.IndexOf("CICLO_VISITADO");
                                                }
                                                else if (listaCampos.Contains("CICLO"))
                                                {
                                                    campoCiclo = listaCampos.IndexOf("CICLO");
                                                }
                                            }
                                            lineaAux = linea.ToString();
                                            if (!lineaAux.Equals(""))
                                            {
                                                if (!item.NombreTabla.Equals("MD_Farmacias") && !item.NombreTabla.Equals("MD_Hospital"))
                                                {
                                                    if (int.Parse(sCampos[campoCiclo]) != ciclo)
                                                    {
                                                        lineaAux = "";
                                                    }
                                                }
                                            }
                                            if (!lineaAux.Trim().Equals(""))
                                            {
                                                String cadenaValores = "";
                                                int indexCampo = 0;
                                                insertSql.Clear();
                                                insertSql.Append("INSERT INTO " + item.NombreTabla + " (");
                                                if (sCampos.Length == sqlDataTable.Columns.Count)
                                                {



                                                    foreach (string dataColumn in nombreColumnas)
                                                    {
                                                        try
                                                        {
                                                            string valor = "";
                                                            switch (sqlDataTable.Columns[dataColumn].DataType.Name)
                                                            {

                                                                case "Int16":
                                                                    if (sqlDataTable.Columns[dataColumn].ColumnName.ToUpper().Equals("CICLO") || sqlDataTable.Columns[dataColumn].ColumnName.ToUpper().Equals("CICLO_VISITADO"))
                                                                    {
                                                                        if (item.NombreTabla.Equals("MD_Farmacias") || item.NombreTabla.Equals("MD_Hospital"))
                                                                        {
                                                                            cadenaValores += ciclo;
                                                                        }
                                                                        else
                                                                        {
                                                                            cadenaValores += sCampos[indexCampo].Equals(".") || sCampos[indexCampo].Equals("null") || sCampos[indexCampo].Equals("") ? 0 : Int16.Parse(sCampos[indexCampo]);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        cadenaValores += sCampos[indexCampo].Equals(".") || sCampos[indexCampo].Equals("null") || sCampos[indexCampo].Equals("") ? 0 : Int16.Parse(sCampos[indexCampo]);
                                                                    }
                                                                    //cadenaValores += sCampos[indexCampo] == "." ? 0 : Int16.Parse(sCampos[indexCampo]);
                                                                    break;
                                                                case "Int32":
                                                                    if (sqlDataTable.Columns[dataColumn].ColumnName.ToUpper().Equals("CICLO") || sqlDataTable.Columns[dataColumn].ColumnName.ToUpper().Equals("CICLO_VISITADO"))
                                                                    {
                                                                        if (item.NombreTabla.Equals("MD_Farmacias") || item.NombreTabla.Equals("MD_Hospital"))
                                                                        {
                                                                            cadenaValores += ciclo;
                                                                        }
                                                                        else
                                                                        {
                                                                            cadenaValores += sCampos[indexCampo].Equals(".") || sCampos[indexCampo].Equals("null") || sCampos[indexCampo].Equals("") ? 0 : Int32.Parse(sCampos[indexCampo]);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        cadenaValores += sCampos[indexCampo].Equals(".") || sCampos[indexCampo].Equals("null") || sCampos[indexCampo].Equals("") ? 0 : Int32.Parse(sCampos[indexCampo]);
                                                                    }
                                                                    //cadenaValores += sCampos[indexCampo] == "." ? 0 : Int32.Parse(sCampos[indexCampo]);
                                                                    break;
                                                                case "Int64":
                                                                    if (sqlDataTable.Columns[dataColumn].ColumnName.ToUpper().Equals("CICLO") || sqlDataTable.Columns[dataColumn].ColumnName.ToUpper().Equals("CICLO_VISITADO"))
                                                                    {
                                                                        if (item.NombreTabla.Equals("MD_Farmacias") || item.NombreTabla.Equals("MD_Hospital"))
                                                                        {
                                                                            cadenaValores += ciclo;
                                                                        }
                                                                        else
                                                                        {
                                                                            cadenaValores += sCampos[indexCampo].Equals(".") || sCampos[indexCampo].Equals("null") || sCampos[indexCampo].Equals("") ? 0 : Int64.Parse(sCampos[indexCampo]); ;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        cadenaValores += sCampos[indexCampo].Equals(".") || sCampos[indexCampo].Equals("null") || sCampos[indexCampo].Equals("") ? 0 : Int64.Parse(sCampos[indexCampo]);
                                                                    }
                                                                    //cadenaValores += sCampos[indexCampo] == "." ? 0 : Int64.Parse(sCampos[indexCampo]);
                                                                    break;
                                                                case "Boolean":
                                                                    cadenaValores += sCampos[indexCampo].Equals(".") || sCampos[indexCampo].Equals("null") || sCampos[indexCampo].Equals("") ? false : Boolean.Parse(sCampos[indexCampo]);
                                                                    break;
                                                                case "Float":
                                                                    float mfloat = sCampos[indexCampo].Equals(".") || sCampos[indexCampo].Equals("null") || sCampos[indexCampo].Equals("") ? 0 : float.Parse(sCampos[indexCampo]);
                                                                    cadenaValores += Convert.ToString(mfloat).Replace(",", ".");
                                                                    break;
                                                                case "Double":
                                                                    try
                                                                    {
                                                                        valor = sCampos[indexCampo].Replace(".", ",");
                                                                        float mdoble = valor == "," ? 0 : float.Parse(valor);
                                                                        cadenaValores += Convert.ToString(mdoble).Replace(",", ".");
                                                                        //cadenaValores += sCampos[indexCampo] == "." ? 0 : Double.Parse(sCampos[indexCampo]);
                                                                    }
                                                                    catch (Exception)
                                                                    {
                                                                        cadenaValores += "0.0";
                                                                    }
                                                                    break;
                                                                case "Decimal":
                                                                    Decimal mdecimal = sCampos[indexCampo].Equals(".") || sCampos[indexCampo].Equals("null") || sCampos[indexCampo].Equals("") ? 0 : Decimal.Parse(sCampos[indexCampo]);
                                                                    cadenaValores += Convert.ToString(mdecimal).Replace(",", ".");
                                                                    //cadenaValores += sCampos[indexCampo] == "." ? 0 : Decimal.Parse(sCampos[indexCampo]);
                                                                    break;
                                                                case "String":
                                                                    //cadenaValores += "'" + sCampos[indexCampo] + "'";
                                                                    if (item.NombreTabla.Equals("MD_Resumen_Transmision") && dataColumn.Equals("Fecha"))
                                                                    {
                                                                        if (Regex.IsMatch(sCampos[indexCampo], @"^\d{1,2}\-\d{1,2}\-\d{2,4}$"))
                                                                        {
                                                                            string fecha = sCampos[indexCampo].Substring(6, 4) + "-" + sCampos[indexCampo].Substring(3, 2) + "-" + sCampos[indexCampo].Substring(0, 2);
                                                                            cadenaValores += "'" + fecha + "'";
                                                                        }
                                                                        else
                                                                        {
                                                                            if (sCampos[indexCampo].Length > sqlDataTable.Columns[dataColumn].MaxLength)
                                                                            {
                                                                                cadenaValores += "'" + sCampos[indexCampo].Substring(0, sqlDataTable.Columns[dataColumn].MaxLength - 1).Trim().Replace("\n","").Replace("\r","") + "'";
                                                                            }
                                                                            else
                                                                            {
                                                                                cadenaValores += "'" + sCampos[indexCampo].Trim().Replace("\n","").Replace("\r","") + "'";
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (sCampos[indexCampo].Length > sqlDataTable.Columns[dataColumn].MaxLength)
                                                                        {
                                                                            cadenaValores += "'" + sCampos[indexCampo].Substring(0, sqlDataTable.Columns[dataColumn].MaxLength - 1) + "'";
                                                                        }
                                                                        else
                                                                        {
                                                                            cadenaValores += "'" + sCampos[indexCampo] + "'";
                                                                        }
                                                                    }
                                                                    break;
                                                                case "Date":
                                                                case "DateTime":
                                                                    cadenaValores += "CONVERT(DATETIME,'" + sCampos[indexCampo] + "',103)";
                                                                    break;
                                                                default:
                                                                    cadenaValores += "'" + sCampos[indexCampo] + "'";
                                                                    break;

                                                            }
                                                        }
                                                        catch (Exception e)
                                                        {
                                                            e.Message.ToString();
                                                            string cuerpo = "Este es un mensaje generado por el servicio receptor del archivo de transmisión de MEDINET " + " <br /> " +
                                                                           "----------------------------------------------------------------------------------------------  " + " <br /> " +
                                                                           "<br /> " +
                                                                           "<b>Archivo:</b> " + nombreArchivoPublico + " <br /> " +
                                                                           "<b>Tabla:</b> " + item.NombreTabla + " <br /> " +
                                                                           "<b>Línea " + (indexLinea + 1) + ":</b> " + linea + " <br /> " +
                                                                           "<b>Error:</b> " + e.Message + " <br /> " +
                                                                           "<b>Campo:</b> " + dataColumn + " - " + sCampos[indexCampo] + " <br /> ";

                                                            Utilidades.enviarMail("Falla en linea de archivo", cuerpo, "soporte@medinetla.com");

                                                        }
                                                        if (indexCampo < sqlDataTable.Columns.Count - 1)
                                                        {
                                                            cadenaValores += ", ";
                                                        }
                                                        indexCampo++;
                                                    }
                                                    string cadenaCampos = "";
                                                    for (int i = 0; i < nombreColumnas.Length; i++)
                                                    {
                                                        cadenaCampos += nombreColumnas[i];
                                                        if (i < nombreColumnas.Length - 1)
                                                        {
                                                            cadenaCampos += ", ";
                                                        }
                                                    }

                                                    insertSql.Append(cadenaCampos + ") VALUES (");
                                                    insertSql.Append(cadenaValores + ")");
                                                    insertSql.Replace(",.,", ",'.',");


                                                }

                                                try
                                                {
                                                    SqlCommand sqlInsert = new SqlCommand(insertSql.ToString(), sqlConexion);//, obTransaction);
                                                    int registroInsertado = sqlInsert.ExecuteNonQuery();

                                                    if (item.NombreTabla.Equals("MD_Resumen_Transmision"))
                                                    {
                                                        SqlCommand sqlDelete = new SqlCommand("DELETE FROM " + item.NombreTabla + "_Acumulada WHERE ZONA='" + idVisitadorHistorial + "' AND CICLO=" + ciclo + " AND NU_ANO=" + ano + "AND Fecha='" + fechaActualCorta + "'", sqlConexion);//, obTransaction))
                                                        sqlInsert = new SqlCommand(insertSql.ToString().Replace("Resumen_Transmision", "Resumen_Transmision_Acumulada"), sqlConexion);//, obTransaction);
                                                        registroInsertado = sqlInsert.ExecuteNonQuery();
                                                    }
                                                    if (item.NombreTabla.Equals("MD_Transmision"))
                                                    {
                                                        if (sqlInsert.CommandText.ToString().Contains("CIERRE DE CICLO"))
                                                        {
                                                            guardarArchivoEnDirectorio(4);
                                                        }
                                                    }

                                                }
                                                catch (SqlException se)
                                                {
                                                    se.Errors.ToString();
                                                    string cuerpo = "Este es un mensaje generado por el servicio receptor del archivo de transmisión de MEDINET " + " <br /> " +
                                                                   "----------------------------------------------------------------------------------------------  " + " <br /> " +
                                                                   "<br /> " +
                                                                   "<b>Archivo:</b> " + nombreArchivoPublico + " <br /> " +
                                                                   "<b>Tabla:</b> " + item.NombreTabla + " <br /> " +
                                                                   "<b>Línea " + (indexLinea + 1) + ":</b> " + linea + " <br /> " +
                                                                   "<b>Error:</b> " + se.Message + " <br /> " +
                                                                   "<b>Numero de Error:</b> " + se.Number + " <br /> " +
                                                                   "<b>Instrucción SQL:</b> " + insertSql.ToString() + " <br /> ";

                                                    Utilidades.enviarMail("Falla en linea de archivo", cuerpo, "soporte@medinetla.com");
                                                }

                                            }
                                        }
                                        else
                                        {
                                            nombreColumnas = linea.Split('|');
                                        }
                                        indexLinea++;
                                    }
                                }
                                catch (Exception e)
                                {
                                    respuesta = false;
                                    e.Message.ToString();

                                }
                            }
                        }
                        catch (Exception e)
                        {
                            respuesta = false;
                            e.Message.ToString();
                        }
                    }
                    sqlConexion.Close();
                }
            }
            else
            {
                respuesta = false;
            }
            return respuesta;
        }
        private void crearRutas()
        {

            if (!Directory.Exists(rutaBase))
            {
                Directory.CreateDirectory(rutaBase);
            }
            if (!Directory.Exists(rutaBase + rutaBackup))
            {
                Directory.CreateDirectory(rutaBase + rutaBackup);
            }
            if (!Directory.Exists(rutaBase + rutaDesfasados))
            {
                Directory.CreateDirectory(rutaBase + rutaDesfasados);
            }
            if (!Directory.Exists(rutaBase + rutaDesconocidos))
            {
                Directory.CreateDirectory(rutaBase + rutaDesconocidos);
            }
            if (!Directory.Exists(rutaBase + rutaNoProcesados))
            {
                Directory.CreateDirectory(rutaBase + rutaNoProcesados);
            }
            if (!Directory.Exists(rutaBase + rutaBackup + rutaCiclos))
            {
                Directory.CreateDirectory(rutaBase + rutaBackup + rutaCiclos);
            }
            if (!Directory.Exists(rutaBase + rutaBackup + rutaFechas))
            {
                Directory.CreateDirectory(rutaBase + rutaBackup + rutaFechas);
            }
            string fechaActual = DateTime.Now.ToLongDateString();
            fechaActualCorta = DateTime.Now.ToShortDateString().Replace("/", "-");
            fechaActual = Convert.ToString(fechaActual);
            string[] month = (fechaActual.Split(' '));
            string annio = month[5];
            string nombreMes = month[3];

            if (!Directory.Exists(rutaBase + rutaBackup + rutaFechas + annio + "\\" + nombreMes + "\\" + fechaActual))
            {
                Directory.CreateDirectory(rutaBase + rutaBackup + rutaFechas + annio + "\\" + nombreMes + "\\" + fechaActual);
            }
            if (!Directory.Exists(rutaBase + rutaBackup + rutaCiclos + ano + "\\" + ciclo))
            {
                Directory.CreateDirectory(rutaBase + rutaBackup + rutaCiclos + ano + "\\" + ciclo);
            }
            if (!Directory.Exists(rutaBase + rutaBackup + rutaFechas + annio + "\\" + nombreMes + "\\" + fechaActual + "\\CIERRE"))
            {
                Directory.CreateDirectory(rutaBase + rutaBackup + rutaFechas + annio + "\\" + nombreMes + "\\" + fechaActual + "\\CIERRE");
            }
            if (!Directory.Exists(rutaBase + rutaBackup + rutaCiclos + ano + "\\" + ciclo + "\\CIERRE"))
            {
                Directory.CreateDirectory(rutaBase + rutaBackup + rutaCiclos + ano + "\\" + ciclo + "\\CIERRE");
            }
        }
        private string convertirStatus(int estatus)
        {
            string estatusLetra = "";
            switch (estatus)
            {
                case 1:
                    estatusLetra = "A";
                    break;
                case 2:
                    estatusLetra = "C";
                    break;
                case 3:
                    estatusLetra = "P";
                    break;
            }
            return estatusLetra;
        }
        [WebMethod]
        public void registrarErrorEnCierre(int idVisitadorHistorial)
        {
            sqlConexion.Open();
            sqlComando = new SqlCommand("SP_CRMServ_ValoresBasicosVisitador", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@ID_VisitadorHistorial", idVisitadorHistorial);
            sqlReader = sqlComando.ExecuteReader();
            if (sqlReader.Read())
            {
                string cuerpo = "Este es un mensaje generado por el servicio receptor de MEDINET " + " <br /> " +
                                                                        "----------------------------------------------------------------------------------------------  " + " <br /> " +
                                                                        "<b>Visitador:</b> " + idVisitadorHistorial + " - " + sqlReader.GetString(0) + " " + sqlReader.GetString(1) + " <br /> " +
                                                                        "<b>Región:</b> " + sqlReader.GetString(3) + "<br /> " +
                                                                        "<b>Línea:</b> " + sqlReader.GetString(2) + "<br /> " +
                                                                        "<b>Teléfono:</b> " + sqlReader.GetString(5) + "<br /> " +
                                                                        "<b>Correo Electrónico:</b> " + sqlReader.GetString(4) + "<br /> ";


                Utilidades.enviarMail("Falla en cierre de ciclo (" + sqlReader.GetString(0) + " " + sqlReader.GetString(1) + ")", cuerpo, "soporte@medinetla.com");
            }
            sqlReader.Close();
            sqlConexion.Close();
        }

        /*----------------------MEDINET VEHICULOS RONAVA---------------------*/
        [WebMethod]
        public CicloVehiculo[] getCiclosVehiculos()
        {
            sqlConexion.Open();
            sqlComando = new SqlCommand("SELECT FE_CicloIni, FE_CicloFin, NU_Ciclo, NU_Estatus, NU_DiasHabiles, ID_CICLO FROM MW_CICLOS WHERE NU_ESTATUS IN (1,2) ORDER BY ID_CICLO DESC", sqlConexion);

            sqlReader = sqlComando.ExecuteReader();

            listadoCiclosV = new List<CicloVehiculo>();
            DataTable sqlDataTable = new DataTable();
            if (sqlReader.HasRows)
            {
                sqlDataTable.Load(sqlReader);
            }

            foreach (DataRow dataRow in sqlDataTable.Rows)
            {
                listadoCiclosV.Add(new CicloVehiculo(dataRow[0].ToString().Substring(0, 10), dataRow[1].ToString().Substring(0, 10), Convert.ToInt16(dataRow[2]), convertirStatus(Convert.ToInt16(dataRow[3])), Convert.ToInt16(dataRow[4]), convertirStatus(Convert.ToInt16(dataRow[3])), Convert.ToInt32(dataRow[5])));//(int)dataRow[2], convertirStatus((int)dataRow[3]), (int)dataRow[4], convertirStatus((int)dataRow[3])));


                //while (sqlReader.Read())
                //{
                //    listadoCiclos.Add(new Ciclo(sqlReader.GetDateTime(0).ToString().Substring(0, 10).Replace("-", "/"), sqlReader.GetDateTime(1).ToString().Substring(0, 10).Replace("-", "/"), sqlReader.GetInt32(2), convertirStatus(sqlReader.GetInt16(3)), sqlReader.GetInt16(4), convertirStatus(sqlReader.GetInt16(3))));
                //}
            }
            sqlConexion.Close();
            return listadoCiclosV.ToArray();
        }

        [WebMethod]
        public VehiculoRegistro[] getVehiculos(int idCiclo, decimal monto) {
            List<VehiculoRegistro> listadoRegistros = new List<VehiculoRegistro>();

            sqlConexion.Open();
            sqlComando = new SqlCommand("SP_CRMServ_VehiculosPR", sqlConexion);
            sqlComando.Parameters.AddWithValue("@IdCiclo", idCiclo);
            sqlComando.Parameters.AddWithValue("@Monto", monto);
            sqlComando.CommandType = CommandType.StoredProcedure;

            sqlReader = sqlComando.ExecuteReader();

            
            DataTable sqlDataTable = new DataTable();
            if (sqlReader.HasRows)
            {
                sqlDataTable.Load(sqlReader);
            }

            foreach (DataRow dataRow in sqlDataTable.Rows)
            {
                listadoRegistros.Add(new VehiculoRegistro(dataRow[0].ToString(), dataRow[1].ToString(),
                    dataRow[2].ToString(), dataRow[3].ToString(), dataRow.IsNull(6) ? 0 : Convert.ToDecimal(dataRow[6]), dataRow.IsNull(7) ? 0 : Convert.ToDecimal(dataRow[7]), dataRow.IsNull(8) ? 0 : Convert.ToDecimal(dataRow[8]),
                    dataRow.IsNull(9) ? 0 : Convert.ToDecimal(dataRow[9]), dataRow.IsNull(10) ? 0 : Convert.ToDecimal(dataRow[10]), dataRow.IsNull(6) ? "" : dataRow[11].ToString()));
                
            }
            sqlConexion.Close();
            
            
            
            return listadoRegistros.ToArray();
        }
        /*----------------------MEDINET SMS---------------------*/

        [WebMethod]
        public void prepararSMS()
        {
            DateTime hDesde = Convert.ToDateTime("07:00");
            DateTime hHasta = Convert.ToDateTime("20:00");

            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
                if (hDesde <= DateTime.Now && DateTime.Now <= hHasta)
                {

                    //idCiclo = 2;
                    //idVisitadorHistorial = 1;

                    sqlConexion.Open();
                    sqlComando = new SqlCommand("SP_CRMServ_SMS_EnviadosHoy", sqlConexion);
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@ID_VisitadorHistorial", idVisitadorHistorial);
                    sqlReader = sqlComando.ExecuteReader();
                    int cantidadMensajesEnviadosHoy = 0;
                    if (sqlReader.HasRows && sqlReader.Read())
                    {
                        cantidadMensajesEnviadosHoy = sqlReader.GetInt32(0);
                    }
                    sqlReader.Close();
                    if (cantidadMensajesEnviadosHoy <= 0)
                    {
                        double mix = 0;
                        double porcRev = 0;
                        double pdr = 0;
                        string nombre = "";
                        string telefono = "";

                        sqlComando = new SqlCommand("SP_CRMServ_SMS_ValoresYDatosVisitador", sqlConexion);
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@ID_Ciclo", idCiclo);
                        sqlComando.Parameters.AddWithValue("@ID_VisitadorHistorial", idVisitadorHistorial);
                        sqlReader = sqlComando.ExecuteReader();
                        if (sqlReader.HasRows && sqlReader.Read())
                        {
                            mix = sqlReader.GetDouble(0);
                            porcRev = sqlReader.GetDouble(1);
                            pdr = sqlReader.GetDouble(2);
                            nombre = sqlReader.GetString(3);
                            if (bEnviarSMSNumeroMDN)
                            {
                                telefono = "04141584128";
                            }
                            else
                            {
                                telefono = sqlReader.GetString(4);
                            }
                            //Validacion de Números
                            switch (idVisitadorHistorial)
                            {
                                case 218:
                                case 220:
                                case 222:
                                case 226:
                                    telefono += ",04149896100,04141584128,04241598811";
                                    break;
                            }

                        }
                        sqlReader.Close();

                        string textoSMS = nombre + ", MEDINET te informa que la transmision ha sido satisfactoria. Totalizando un Mix de " + mix + " y un Promedio Diario Real de " + pdr;

                        string sqlInsert = "INSERT INTO SMS_MensajesEnviados (ID_VisitadorHistorial, ID_Ciclo, ID_TipoMensaje, NU_Mix, NU_PorcentajeRevisita, NU_PDR) VALUES (" + idVisitadorHistorial + "," + idCiclo + ",1," + mix.ToString().Replace(",", ".") + "," + porcRev.ToString().Replace(",", ".") + "," + pdr.ToString().Replace(",", ".") + ");SELECT CAST(scope_identity() AS int)";
                        sqlComando = new SqlCommand(sqlInsert, sqlConexion);
                        Int32 idMensaje = 0;

                        try
                        {
                            idMensaje = (Int32)sqlComando.ExecuteScalar();

                            if (!telefono.Equals("") && !telefono.Equals("0"))
                            {
                                string rifEmpresa = ConfigurationManager.AppSettings["rifEmpresa"].ToString();
                                localhost.WSCallCenter mdnService = new localhost.WSCallCenter();
                                bool respuesta = mdnService.enviarSMS(rifEmpresa, idMensaje, telefono, textoSMS);

                                if (respuesta)
                                {
                                    sqlComando = new SqlCommand("UPDATE SMS_MensajesEnviados SET BO_Procesado=" + (respuesta ? 1 : 0) + " WHERE ID_Mensaje=" + idMensaje, sqlConexion);
                                    sqlComando.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                string cuerpo = "Este es un mensaje generado por el servicio receptor de MEDINET " + " <br /> " +
                                                                        "----------------------------------------------------------------------------------------------  " + " <br /> " +
                                                                        "<b>Visitador:</b> " + idVisitadorHistorial + " - " + nombre + " <br /> " +
                                                                        "<b>Error:</b> No tiene teléfono registrado <br /> ";


                                Utilidades.enviarMail("Falla en servicio receptor de MEDINET (SMS)", cuerpo, "soporte@medinetla.com");
                            }
                        }
                        catch (SqlException se)
                        {
                            se.Errors.ToString();
                            string cuerpo = "Este es un mensaje generado por el servicio receptor del archivo de transmisión de MEDINET " + " <br /> " +
                                           "----------------------------------------------------------------------------------------------  " + " <br /> " +
                                           "<br /> " +
                                           "<b>Archivo:</b> " + nombreArchivoPublico + " <br /> " +
                                           "<b>Visitador:</b> " + nombre + " <br /> " +
                                           "<b>Texto:</b> " + textoSMS + " <br /> " +
                                           "<b>Error:</b> " + se.Message + " <br /> " +
                                           "<b>Numero de Error:</b> " + se.Number + " <br /> ";


                            Utilidades.enviarMail("Falla en servicio receptor de MEDINET (SMS)", cuerpo, "soporte@medinetla.com");
                        }
                        catch (TimeoutException te)
                        {
                            te.Message.ToString();
                            string cuerpo = "Este es un mensaje generado por el servicio receptor del archivo de transmisión de MEDINET " + " <br /> " +
                                           "----------------------------------------------------------------------------------------------  " + " <br /> " +
                                           "<br /> " +
                                           "<b>Archivo:</b> " + nombreArchivoPublico + " <br /> " +
                                           "<b>Visitador:</b> " + nombre + " <br /> " +
                                           "<b>Texto:</b> " + textoSMS + " <br /> " +
                                           "<b>Error:</b> " + te.Message + " <br /> ";


                            Utilidades.enviarMail("Falla en servicio receptor de MEDINET (SMS)", cuerpo, "soporte@medinetla.com");
                        }
                        catch (Exception e)
                        {
                            e.Message.ToString();
                            string cuerpo = "Este es un mensaje generado por el servicio receptor del archivo de transmisión de MEDINET " + " <br /> " +
                                           "----------------------------------------------------------------------------------------------  " + " <br /> " +
                                           "<br /> " +
                                           "<b>Archivo:</b> " + nombreArchivoPublico + " <br /> " +
                                           "<b>Visitador:</b> " + nombre + " <br /> " +
                                           "<b>Texto:</b> " + textoSMS + " <br /> " +
                                           "<b>Error:</b> " + e.Message + " <br /> ";


                            Utilidades.enviarMail("Falla en servicio receptor de MEDINET (SMS)", cuerpo, "soporte@medinetla.com");
                        }
                    }
                    sqlConexion.Close();
                }
            }
        }

        /*----------------------MEDINET GERENCIAL---------------------*/

        [WebMethod]
        public TipoGerente[] SolicitarInformacion(string imei)
        {
            string tipo = "NINGUNO";
            int codigo = 0;
            //SqlConnection sqlConexion = new SqlConnection(ConfigurationManager.ConnectionStrings["ZuozBIMobileDatabase"].ConnectionString);

            sqlConexion.Open();

            //string sql = "SELECT 75 as co_gerente";
            string sql = "SELECT ID_Usuario FROM MW_Usuarios WHERE TX_Imei='" + imei + "'";

            SqlCommand sqlComando = new SqlCommand(sql, sqlConexion);

            SqlDataReader reader = sqlComando.ExecuteReader();
            if (reader.Read())
            {
                codigo = reader.GetInt32(0);
            }
            sqlConexion.Close();

            if (codigo != 0)
            {
                tipo = VerificarTipo(codigo);
            }
            List<TipoGerente> datos = new List<TipoGerente>();
            datos.Add(new TipoGerente(tipo, codigo));
            //datos.Add(new TipoGerente(tipo, codigo));
            sqlConexion.Close();
            return datos.ToArray();

        }
        private string VerificarTipo(int codigo)
        {
            string tipo = "NINGUNO";
            //SqlConnection sqlConexion = new SqlConnection(ConfigurationManager.ConnectionStrings["ZuozBIMobileDatabase"].ConnectionString);

            sqlConexion.Open();
            string sql = "SELECT ID_Region,ID_Linea FROM MW_UsuariosResponsabilidades WHERE Id_Usuario=" + codigo + " AND BO_Activo=1";
            sqlComando = new SqlCommand(sql, sqlConexion);
            sqlReader = sqlComando.ExecuteReader();

            bool reg = false;
            bool lin = false;
            while (sqlReader.Read())
            {
                if (!sqlReader.IsDBNull(0))
                {
                    reg = true;
                }
                if (!sqlReader.IsDBNull(1))
                {
                    lin = true;
                }
            }

            if (reg && lin)
            {
                tipo = "REGIONAL";
            }
            else if (reg)
            {
                tipo = "NACIONAL";
            }
            else
            {
                tipo = "NINGUNO";
            }
            sqlConexion.Close();
            return tipo;
        }
        [WebMethod]
        public DataGerenteNacional[] SolicitarDataGerenteNacional(int codigo)
        {
            sqlConexion.Open();
            string sql = "SP_CRMServ_GerentesNacionales";
            SqlCommand sqlComando = new SqlCommand(sql, sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlReader = sqlComando.ExecuteReader();
            List<DataGerenteNacional> lista = new List<DataGerenteNacional>();
            while (sqlReader.Read())
            {
                if (codigo == sqlReader.GetInt32(0))
                {
                    lista.Add(
                        new DataGerenteNacional(sqlReader.GetInt32(0), sqlReader.GetInt16(1), sqlReader.GetString(2),
                                                sqlReader.GetString(3), sqlReader.GetDecimal(4), sqlReader.GetDecimal(5), sqlReader.GetDecimal(6),
                                                sqlReader.GetDecimal(7), sqlReader.GetString(8), sqlReader.GetString(9),
                                                sqlReader.GetString(10), sqlReader.GetString(11), sqlReader.GetString(12)));
                }

            }

            sqlConexion.Close();
            return lista.ToArray();

        }
        [WebMethod]
        public DataGerenteRegional[] SolicitarDataGerenteNacionalHijos(int codigo)
        {
            sqlConexion.Open();
            string sql = "SP_CRMServ_GerentesRegionales";
            SqlCommand sqlComando = new SqlCommand(sql, sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlReader = sqlComando.ExecuteReader();
            List<DataGerenteRegional> lista = new List<DataGerenteRegional>();
            while (sqlReader.Read())
            {
                if (codigo == sqlReader.GetInt32(3))
                {
                    lista.Add(
                        new DataGerenteRegional(sqlReader.GetInt32(3), sqlReader.GetInt16(1), sqlReader.GetString(4),
                                                sqlReader.GetString(5), sqlReader.GetDecimal(6), sqlReader.GetDecimal(7),
                                                sqlReader.GetDecimal(8), sqlReader.GetDecimal(9), sqlReader.GetString(10), sqlReader.GetString(11),
                                                sqlReader.GetString(12), sqlReader.GetString(13), sqlReader.GetString(14),
                                                sqlReader.GetInt32(0), sqlReader.GetInt16(2)));
                }
            }
            sqlConexion.Close();
            return lista.ToArray();
        }
        [WebMethod]
        public DataGerenteRegional[] SolicitarDataGerenteRegional(int codigo)
        {
            sqlConexion.Open();
            string sql = "SP_CRMServ_GerentesRegionales";
            SqlCommand sqlComando = new SqlCommand(sql, sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlReader = sqlComando.ExecuteReader();
            List<DataGerenteRegional> lista = new List<DataGerenteRegional>();
            while (sqlReader.Read())
            {
                if (codigo == sqlReader.GetInt32(0))
                {
                    lista.Add(
                        new DataGerenteRegional(sqlReader.GetInt32(3), sqlReader.GetInt16(1), sqlReader.GetString(4),
                                      sqlReader.GetString(5), sqlReader.GetDecimal(6), sqlReader.GetDecimal(7),
                                      sqlReader.GetDecimal(8), sqlReader.GetDecimal(9), sqlReader.GetString(10), sqlReader.GetString(11),
                                      sqlReader.GetString(12), sqlReader.GetString(13), sqlReader.GetString(14),
                                      sqlReader.GetInt32(0), sqlReader.GetInt16(2)));
                }
            }
            sqlConexion.Close();
            return lista.ToArray();

        }

        public DataVisitadorMedico[] SolicitarDataGerenteRegionalHijos(int codigo)
        {
            sqlConexion.Open();
            string sql = "SP_CRMServ_Visitadores";
            SqlCommand sqlComando = new SqlCommand(sql, sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlReader = sqlComando.ExecuteReader();

            List<DataVisitadorMedico> lista = new List<DataVisitadorMedico>();
            while (sqlReader.Read())
            {
                if (codigo == sqlReader.GetInt32(1))
                {
                    lista.Add(
                        new DataVisitadorMedico(sqlReader.GetInt64(0), sqlReader.GetString(3), sqlReader.GetString(4), sqlReader.GetDouble(5), sqlReader.GetDouble(6), sqlReader.GetDecimal(7),
                                           sqlReader.GetDouble(8), sqlReader.GetString(9), sqlReader.GetString(10), sqlReader.GetString(11), sqlReader.GetString(12), sqlReader.GetString(13),
                                           sqlReader.GetInt32(1), sqlReader.GetInt16(2), sqlReader.GetString(14)));
                }
            }
            sqlConexion.Close();
            return lista.ToArray();

        }
        [WebMethod]
        public DataVisitadorMedicoII[] SolicitarDataGerenteRegionalHijosII(int codigo)
        {
            sqlConexion.Open();
            string sql = "SP_CRMServ_Visitadores";
            SqlCommand sqlComando = new SqlCommand(sql, sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlReader = sqlComando.ExecuteReader();

            List<DataVisitadorMedicoII> lista = new List<DataVisitadorMedicoII>();
            while (sqlReader.Read())
            {
                if (codigo == sqlReader.GetInt32(1))
                {
                    lista.Add(
                        new DataVisitadorMedicoII(sqlReader.GetInt64(0), sqlReader.GetString(4), sqlReader.GetString(5), sqlReader.GetDouble(6), sqlReader.GetDouble(7), sqlReader.GetDecimal(8),
                                           sqlReader.GetDouble(9), sqlReader.GetString(10), sqlReader.GetString(11), sqlReader.GetString(12), sqlReader.GetString(13), sqlReader.GetString(14),
                                           sqlReader.GetInt32(1), sqlReader.GetInt16(3), sqlReader.GetInt16(2), sqlReader.GetString(15)));
                }
            }
            sqlConexion.Close();
            return lista.ToArray();

        }
        [WebMethod]
        public DataVisitadorMedicoFarmacias[] SolicitarDataGerenteRegionalHijosFarmacias(int codigo)
        {
            sqlConexion.Open();
            string sql = "SP_CRMServ_Farmacias";
            SqlCommand sqlComando = new SqlCommand(sql, sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlReader = sqlComando.ExecuteReader();

            List<DataVisitadorMedicoFarmacias> lista = new List<DataVisitadorMedicoFarmacias>();
            while (sqlReader.Read())
            {
                if (codigo == sqlReader.GetInt32(1))
                {
                    lista.Add(
                        new DataVisitadorMedicoFarmacias(
                            sqlReader.GetInt64(0), sqlReader.GetInt32(1),
                            sqlReader.GetInt16(2), sqlReader.GetInt16(3),
                            sqlReader.GetString(4), sqlReader.GetString(5),
                            sqlReader.GetInt32(6), sqlReader.GetInt32(7),
                            sqlReader.GetDecimal(8)));

                }
            }
            sqlConexion.Close();
            return lista.ToArray();

        }
        [WebMethod]
        public DataVisitadorMedicoHospitales[] SolicitarDataGerenteRegionalHijosHospitales(int codigo)
        {
            sqlConexion.Open();
            string sql = "SP_CRMServ_Hospitales";
            SqlCommand sqlComando = new SqlCommand(sql, sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlReader = sqlComando.ExecuteReader();

            List<DataVisitadorMedicoHospitales> lista = new List<DataVisitadorMedicoHospitales>();
            while (sqlReader.Read())
            {
                if (codigo == sqlReader.GetInt32(1))
                {
                    lista.Add(
                        new DataVisitadorMedicoHospitales(
                            sqlReader.GetInt64(0), sqlReader.GetInt32(1),
                            sqlReader.GetInt16(2), sqlReader.GetInt16(3),
                            sqlReader.GetString(4), sqlReader.GetString(5),
                            sqlReader.GetInt32(6), sqlReader.GetInt32(7),
                            sqlReader.GetDecimal(8)));

                }
            }
            sqlConexion.Close();
            return lista.ToArray();

        }
        [WebMethod]
        public DataVisitadorMedicoHojaRuta[] SolicitarDataGerenteRegionalHijosHojaRuta(int codigo)
        {
            sqlConexion.Open();
            string sql = "SP_CRMServ_HojaRuta";
            SqlCommand sqlComando = new SqlCommand(sql, sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlReader = sqlComando.ExecuteReader();

            List<DataVisitadorMedicoHojaRuta> lista = new List<DataVisitadorMedicoHojaRuta>();
            while (sqlReader.Read())
            {
                if (codigo == sqlReader.GetInt32(1))
                {
                    lista.Add(
                        new DataVisitadorMedicoHojaRuta(
                            sqlReader.GetInt64(0), sqlReader.GetInt32(1),
                            sqlReader.GetInt16(2), sqlReader.GetInt16(3),
                            sqlReader.GetString(4), sqlReader.GetString(5),
                            sqlReader.IsDBNull(6) ? "." : sqlReader.GetString(6),
                            sqlReader.IsDBNull(6) ? "." : sqlReader.GetString(7)));

                }
            }
            sqlConexion.Close();
            return lista.ToArray();

        }
        [WebMethod]
        public DataVisitadorMedicoAgenda[] SolicitarDataGerenteRegionalHijosAgenda(int codigo)
        {
            sqlConexion.Open();
            string sql = "SP_CRMServ_Agenda";
            SqlCommand sqlComando = new SqlCommand(sql, sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlReader = sqlComando.ExecuteReader();

            List<DataVisitadorMedicoAgenda> lista = new List<DataVisitadorMedicoAgenda>();
            while (sqlReader.Read())
            {
                if (codigo == sqlReader.GetInt32(1))
                {
                    lista.Add(
                        new DataVisitadorMedicoAgenda(
                            sqlReader.GetInt64(0), sqlReader.GetInt32(1),
                            sqlReader.GetInt16(2), sqlReader.GetInt16(3),
                            sqlReader.GetString(4), sqlReader.GetString(5),
                            sqlReader.GetString(6), sqlReader.GetString(7),
                            sqlReader.GetString(8), sqlReader.GetString(9),
                            sqlReader.GetString(10), sqlReader.GetString(11),
                            sqlReader.GetString(12)));

                }
            }
            sqlConexion.Close();
            return lista.ToArray();

        }
        [WebMethod]
        public Linea[] Lineas()
        {
            //SqlConnection sqlConexion = new SqlConnection(ConfigurationManager.ConnectionStrings["ZuozBIMobileDatabase"].ConnectionString);
            sqlConexion.Open();
            string sql = "SELECT ID_Linea,TX_Linea FROM MW_Lineas WHERE BO_Activo=1 ORDER BY 2";
            SqlCommand sqlComando = new SqlCommand(sql, sqlConexion);
            SqlDataReader reader = sqlComando.ExecuteReader();
            List<Linea> lista = new List<Linea>();
            while (reader.Read())
            {
                lista.Add(new Linea(reader.GetInt16(0), reader.GetString(1)));
            }
            sqlConexion.Close();
            return lista.ToArray();
        }
        [WebMethod]
        public Region[] Regiones()
        {
            //SqlConnection sqlConexion = new SqlConnection(ConfigurationManager.ConnectionStrings["ZuozBIMobileDatabase"].ConnectionString);
            sqlConexion.Open();
            string sql = "SELECT ID_Region,TX_Region FROM MW_Regiones WHERE BO_Activo=1 ORDER BY 2";
            SqlCommand sqlComando = new SqlCommand(sql, sqlConexion);
            SqlDataReader reader = sqlComando.ExecuteReader();
            List<Region> lista = new List<Region>();
            while (reader.Read())
            {
                lista.Add(new Region(reader.GetInt16(0), reader.GetString(1)));
            }
            sqlConexion.Close();
            return lista.ToArray();
        }
        [WebMethod]
        public Umbral[] Umbrales()
        {
            //SqlConnection sqlConexion = new SqlConnection(ConfigurationManager.ConnectionStrings["ZuozBIMobileDatabase"].ConnectionString);
            sqlConexion.Open();
            string sql = "SELECT TOP 1 NU_Percent_Visita,NU_Percent_Revisita,NU_Percent_PDR,NU_Percent_Mix FROM MW_Umbrales WHERE BO_Activo=1";
            SqlCommand sqlComando = new SqlCommand(sql, sqlConexion);
            SqlDataReader reader = sqlComando.ExecuteReader();
            List<Umbral> lista = new List<Umbral>();
            while (reader.Read())
            {
                lista.Add(new Umbral("% de Visita", (double)reader.GetDecimal(0), "% de Revisita", (double)reader.GetDecimal(1), "PDR", (double)reader.GetDecimal(2), "Mix", (double)reader.GetDecimal(3)));
            }
            sqlConexion.Close();
            return lista.ToArray();
        }
        [WebMethod]
        public RegionNacional[] RegionesNacional(int codigo)
        {
            //SqlConnection sqlConexion = new SqlConnection(ConfigurationManager.ConnectionStrings["ZuozBIMobileDatabase"].ConnectionString);
            sqlConexion.Open();
            string sql = "SELECT ID_Usuario,ID_Region FROM MW_UsuariosResponsabilidades WHERE ID_Usuario=" + codigo + " AND BO_Activo=1";
            SqlCommand sqlComando = new SqlCommand(sql, sqlConexion);
            SqlDataReader reader = sqlComando.ExecuteReader();
            List<RegionNacional> lista = new List<RegionNacional>();
            while (reader.Read())
            {
                lista.Add(new RegionNacional(reader.GetInt32(0), reader.GetInt16(1)));
            }
            sqlConexion.Close();
            return lista.ToArray();
        }
        [WebMethod]
        public LineaRegional[] LineasRegional(int codigo)
        {
            //SqlConnection sqlConexion = new SqlConnection(ConfigurationManager.ConnectionStrings["ZuozBIMobileDatabase"].ConnectionString);
            sqlConexion.Open();
            string sql = "SELECT ID_Usuario,ID_Region,ID_Linea FROM MW_UsuariosResponsabilidades WHERE ID_Usuario=" + codigo + " AND BO_Activo=1";
            SqlCommand sqlComando = new SqlCommand(sql, sqlConexion);
            SqlDataReader reader = sqlComando.ExecuteReader();
            List<LineaRegional> lista = new List<LineaRegional>();
            while (reader.Read())
            {
                lista.Add(new LineaRegional(reader.GetInt32(0), reader.GetInt16(1), reader.GetInt16(2)));
            }
            sqlConexion.Close();
            return lista.ToArray();
        }

        private struct datosEnTablas
        {
            public string NombreTabla { get; set; }
            public string Datos { get; set; }
        }

        /*----------------------GOOGLE CLOUD MESSAGING---------------------*/

        [WebMethod]
        public bool RegistroDispositivo(int idVisitadorHistorial, string idRegistroGoogle, string imei)
        {
            try
            {
                sqlConexion.Open();
                sqlComando = new SqlCommand("SP_CRMServ_RegistroGoogle", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@idVisitadorHistorial", idVisitadorHistorial);
                sqlComando.Parameters.AddWithValue("@idRegistroGoogle", idRegistroGoogle);
                sqlComando.Parameters.AddWithValue("@imei", imei);
                sqlComando.ExecuteReader();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        private bool enviarMensajeTEXT()
        {
            string registration_id = "APA91bGtKeNoLdpgWSR_nzioOjAW3hMwTmpI6-Dk4hzpIQi7tCFevZtABi0N-lveO1FNScKicviA4PqQns0zUzsN_7STWXfcmBZwv4GmWcuqs1cAKj-H18DSxG88F47i5hPGLmJxvHtRXJZgWLogElQjpy31c2qYXA";
            string apiKey = "AIzaSyD2HxoRpeO_m0y71YEFw71v_zrywOW0RgM";
            string mensaje = "Hola muchachos...";

            bool flag = false;
            StringBuilder sb = new StringBuilder();

            String GCM_URL = @"https://android.googleapis.com/gcm/send";

            string collapseKey = DateTime.Now.ToString();

            sb.AppendFormat("registration_id={0}&collapse_key={1}",
                            registration_id, collapseKey);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("data.msg",
                HttpUtility.UrlEncode("INSERT INTO Contador VALUES (41543)"));
            //data.Add("data.msg",
            //    HttpUtility.UrlEncode("INSERT INTO Contador VALUES (43);INSERT INTO Contador VALUES (23);UPDATE Contador SET Contador=77 WHERE Contador=43;"));
            //data.Add("data.msg",
            //    HttpUtility.UrlEncode(mensaje + " Timestamp: " + DateTime.Now.ToString()));
            data.Add("data.tipo",
                HttpUtility.UrlEncode("sql"));
            //data.Add("data.tipo",
            //    HttpUtility.UrlEncode("notificacion"));

            foreach (string item in data.Keys)
            {
                if (item.Contains("data."))
                    sb.AppendFormat("&{0}={1}", item, data[item]);
            }

            string msg = sb.ToString();

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(GCM_URL);
            req.Method = "POST";
            req.ContentLength = msg.Length;
            //req.ContentType = "application/json";
            req.ContentType = "application/x-www-form-urlencoded";


            req.Headers.Add("Authorization:key=" + apiKey);

            using (StreamWriter oWriter = new StreamWriter(req.GetRequestStream()))
            {
                oWriter.Write(msg);
            }

            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    string respData = sr.ReadToEnd();

                    if (resp.StatusCode == HttpStatusCode.OK)   // OK = 200
                    {
                        if (respData.StartsWith("id="))
                            flag = true;
                    }
                    else if (resp.StatusCode == HttpStatusCode.InternalServerError)    // 500
                        Console.WriteLine("Error interno del servidor, prueba más tarde.");
                    else if (resp.StatusCode == HttpStatusCode.ServiceUnavailable)    // 503
                        Console.WriteLine("Servidor no disponible temporalmente, prueba más tarde.");
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)          // 401
                        Console.WriteLine("La API Key utilizada no es válida.");
                    else
                        Console.WriteLine("Error: " + resp.StatusCode);
                }
            }

            return flag;


        }

        private bool enviarInstruccionesSqlJson(String[] instrucciones, String[] registrationIds)
        {
            //string registration_id = "APA91bGtKeNoLdpgWSR_nzioOjAW3hMwTmpI6-Dk4hzpIQi7tCFevZtABi0N-lveO1FNScKicviA4PqQns0zUzsN_7STWXfcmBZwv4GmWcuqs1cAKj-H18DSxG88F47i5hPGLmJxvHtRXJZgWLogElQjpy31c2qYXA";
            string apiKey = ConfigurationManager.AppSettings["googleApiKey"].ToString();//"AIzaSyD2HxoRpeO_m0y71YEFw71v_zrywOW0RgM";
            //string mensaje = "Hola muchachos...";

            bool flag = false;
            StringBuilder sb = new StringBuilder();

            String GCM_URL = @"https://android.googleapis.com/gcm/send";

            List<string> listaIds = new List<string>();
            foreach (string id in registrationIds)
            {
                listaIds.Add(id);
            }

            String instruccionesSB = "";

            foreach (String ins in instrucciones)
            {
                instruccionesSB += ins + ";";
            }

            GCMMessage gcmMessage = new GCMMessage(new Data(instruccionesSB.ToString(), "sql"), listaIds);

            string json = new JavaScriptSerializer().Serialize(gcmMessage);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(GCM_URL);
            req.Method = "POST";
            req.ContentLength = json.Length;
            req.ContentType = "application/json";
            req.Headers.Add("Authorization:key=" + apiKey);

            using (StreamWriter oWriter = new StreamWriter(req.GetRequestStream()))
            {
                oWriter.Write(json);
            }

            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    string respData = sr.ReadToEnd();

                    if (resp.StatusCode == HttpStatusCode.OK)   // OK = 200
                    {
                        if (respData.Contains("\"success\":1"))
                            flag = true;
                    }
                    else if (resp.StatusCode == HttpStatusCode.InternalServerError)    // 500
                        Console.WriteLine("Error interno del servidor, prueba más tarde.");
                    else if (resp.StatusCode == HttpStatusCode.ServiceUnavailable)    // 503
                        Console.WriteLine("Servidor no disponible temporalmente, prueba más tarde.");
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)          // 401
                        Console.WriteLine("La API Key utilizada no es válida.");
                    else
                        Console.WriteLine("Error: " + resp.StatusCode);
                }
            }

            return flag;


        }

        private bool enviarMensajeJSON(String titulo, String mensaje, String[] registrationIds)
        {
            //string registration_id = "APA91bGtKeNoLdpgWSR_nzioOjAW3hMwTmpI6-Dk4hzpIQi7tCFevZtABi0N-lveO1FNScKicviA4PqQns0zUzsN_7STWXfcmBZwv4GmWcuqs1cAKj-H18DSxG88F47i5hPGLmJxvHtRXJZgWLogElQjpy31c2qYXA";
            string apiKey = ConfigurationManager.AppSettings["googleApiKey"].ToString();//"AIzaSyD2HxoRpeO_m0y71YEFw71v_zrywOW0RgM";
            //string mensaje = "Hola muchachos...";

            bool flag = false;
            StringBuilder sb = new StringBuilder();

            String GCM_URL = @"https://android.googleapis.com/gcm/send";


            List<string> listaIds = new List<string>();

            foreach (string id in registrationIds)
            {
                listaIds.Add(id);
            }


            GCMMessage gcmMessage = new GCMMessage(new Data(titulo, mensaje, "notificacion"), listaIds);


            string json = new JavaScriptSerializer().Serialize(gcmMessage);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(GCM_URL);
            req.Method = "POST";
            req.ContentLength = Encoding.UTF8.GetByteCount(json);
            //req.ContentLength = json.Length;
            req.ContentType = "application/json";
            req.Headers.Add("Authorization:key=" + apiKey);

            using (StreamWriter oWriter = new StreamWriter(req.GetRequestStream()))
            {
                try
                {
                    oWriter.Write(json);
                }
                catch (Exception ex)
                {

                    flag = false;
                }

            }

            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    string respData = sr.ReadToEnd();

                    if (resp.StatusCode == HttpStatusCode.OK)   // OK = 200
                    {
                        if (respData.Contains("\"success\":1"))
                            flag = true;
                    }
                    else if (resp.StatusCode == HttpStatusCode.InternalServerError)    // 500
                        Console.WriteLine("Error interno del servidor, prueba más tarde.");
                    else if (resp.StatusCode == HttpStatusCode.ServiceUnavailable)    // 503
                        Console.WriteLine("Servidor no disponible temporalmente, prueba más tarde.");
                    else if (resp.StatusCode == HttpStatusCode.Unauthorized)          // 401
                        Console.WriteLine("La API Key utilizada no es válida.");
                    else
                        Console.WriteLine("Error: " + resp.StatusCode);
                }
            }

            return flag;


        }
        [WebMethod]
        public void setInstruccionesSqlPrueba()
        {
            String[] ids = new String[] { "APA91bEE1NJL3A7d1Nk6h5JowzUkAhPhpXMQRCKWk68HUKYP9gysyxWSujYZgl-mEMcl-AvGReRt0XIyPkYWwifaTTpOBAUFzwWmQ3UJ_J6cvZnOJXtDadwSx811OzwmfVcbzKmj-fT5-DMNk6QaF9hpfyw5iKRKWw" };
            String[] sql = new String[] { "DELETE FROM CONTADOR", "INSERT INTO CONTADOR VALUES (43)" };
            setInstruccionesSql(sql, ids);
        }
        [WebMethod]
        public bool setMensajePrueba()
        {
            String[] ids = new String[] { "APA91bEE1NJL3A7d1Nk6h5JowzUkAhPhpXMQRCKWk68HUKYP9gysyxWSujYZgl-mEMcl-AvGReRt0XIyPkYWwifaTTpOBAUFzwWmQ3UJ_J6cvZnOJXtDadwSx811OzwmfVcbzKmj-fT5-DMNk6QaF9hpfyw5iKRKWw" };
            String titulo = "Mensaje del día";
            String mensaje = "Salgan con todas las fuerzas  gggggggggggg gggggggggggggg gggggggggggg gggggggg ggggggg gggg gg ggggg gggggggggg ggggg ggggg gggggggggg gggggggggg ggdddddddddddd dddddddddddddddd ddddddd dddd dd ddd uuuuuuu uuuuuuuuu uuuuuuu uuuuuuuuuu uuuueee eeee eeeee ee eee e ee";
            return setMensajes(titulo, mensaje, ids);
        }
        [WebMethod]
        public bool setInstruccionesSql(String[] instrucciones, String[] registrationIds)
        {
            bool resultado = false;
            try
            {
                resultado = enviarInstruccionesSqlJson(instrucciones, registrationIds);
            }
            catch (Exception)
            {
                resultado = false;
            }
            return resultado;
        }
        [WebMethod]
        public bool setMensajes(String titulo, String mensaje, String[] registrationIds)
        {
            bool resultado = false;

            resultado = enviarMensajeJSON(titulo, mensaje, registrationIds);

            return resultado;
        }

        /*----------------------RECETAS-------------------------------*/

        [WebMethod]
        public MedicoReceta[] getMedicosRecetas(int[] registrosSanitarios)
        //public MedicoReceta[] getMedicosRecetas()
        {

            //registrosSanitarios = new int[] { 89911, 27426, 45020 };

            DataTable tbRegistrosSAnitarios = new DataTable();
            tbRegistrosSAnitarios.Columns.Add("NU_RegistroSanitario", typeof(int));

            foreach (int registro in registrosSanitarios)
            {
                tbRegistrosSAnitarios.Rows.Add(registro);
            }

            sqlConexion.Open();
            sqlComando = new SqlCommand("SP_CRMServ_BuscaRecetas", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@registrosSanitarios", tbRegistrosSAnitarios);
            sqlReader = sqlComando.ExecuteReader();

            listadoMedicosRecetas = new List<MedicoReceta>();
            while (sqlReader.Read())
            {
                listadoMedicosRecetas.Add(
                    new MedicoReceta(
                        sqlReader.IsDBNull(0) ? "." : sqlReader.GetString(0),
                        sqlReader.IsDBNull(1) ? "." : sqlReader.GetString(1),
                        sqlReader.IsDBNull(2) ? "." : sqlReader.GetString(2),
                        sqlReader.IsDBNull(3) ? "." : sqlReader.GetString(3),
                        sqlReader.IsDBNull(4) ? "." : sqlReader.GetString(4),
                        sqlReader.IsDBNull(5) ? "." : sqlReader.GetString(5),
                        sqlReader.IsDBNull(6) ? "." : sqlReader.GetString(6),
                        sqlReader.IsDBNull(7) ? "." : sqlReader.GetString(7),
                        sqlReader.IsDBNull(8) ? "." : sqlReader.GetString(8),
                        sqlReader.IsDBNull(9) ? "." : sqlReader.GetString(9),
                        sqlReader.IsDBNull(10) || sqlReader.GetString(10).Trim().Equals("") || sqlReader.GetString(10).Trim().Length <= 0 ? "." : sqlReader.GetString(10),
                        sqlReader.IsDBNull(11) || sqlReader.GetString(11).Trim().Equals("") || sqlReader.GetString(11).Trim().Length <= 0 ? "." : sqlReader.GetString(11),
                        sqlReader.IsDBNull(12) || sqlReader.GetString(12).Trim().Equals("") || sqlReader.GetString(12).Trim().Length <= 0 ? "." : sqlReader.GetString(12),
                        sqlReader.IsDBNull(13) || sqlReader.GetString(13).Trim().Equals("") || sqlReader.GetString(13).Trim().Length <= 0 ? "." : sqlReader.GetString(13),
                        sqlReader.IsDBNull(14) || sqlReader.GetString(14).Trim().Equals("") || sqlReader.GetString(14).Trim().Length <= 0 ? "." : sqlReader.GetString(14),
                        sqlReader.IsDBNull(15) || sqlReader.GetString(15).Trim().Equals("") || sqlReader.GetString(15).Trim().Length <= 0 ? "." : sqlReader.GetString(15),
                        sqlReader.IsDBNull(16) || sqlReader.GetString(16).Trim().Equals("") || sqlReader.GetString(16).Trim().Length <= 0 ? "." : sqlReader.GetString(16),
                        sqlReader.IsDBNull(17) || sqlReader.GetString(17).Trim().Equals("") || sqlReader.GetString(17).Trim().Length <= 0 ? "." : sqlReader.GetString(17),
                        sqlReader.IsDBNull(18) || sqlReader.GetString(18).Trim().Equals("") || sqlReader.GetString(18).Trim().Length <= 0 ? "." : sqlReader.GetString(18),
                        sqlReader.IsDBNull(19) || sqlReader.GetString(19).Trim().Equals("") || sqlReader.GetString(19).Trim().Length <= 0 ? "." : sqlReader.GetString(19),
                        sqlReader.IsDBNull(20) || sqlReader.GetString(20).Trim().Equals("") || sqlReader.GetString(20).Trim().Length <= 0 ? "." : sqlReader.GetString(20),
                        sqlReader.IsDBNull(21) ? "." : sqlReader.GetString(21),
                        sqlReader.IsDBNull(22) ? 0 : sqlReader.GetDouble(22),
                        sqlReader.IsDBNull(23) ? "." : sqlReader.GetString(23),
                        sqlReader.IsDBNull(24) ? 0 : sqlReader.GetDouble(24),
                        sqlReader.IsDBNull(25) ? "." : sqlReader.GetString(25),
                        sqlReader.IsDBNull(26) ? "." : sqlReader.GetString(26),
                        sqlReader.IsDBNull(27) ? 0 : sqlReader.GetDouble(27),
                        sqlReader.IsDBNull(28) ? "." : sqlReader.GetString(28),
                        sqlReader.IsDBNull(29) ? 0 : sqlReader.GetDouble(29),
                        sqlReader.IsDBNull(30) ? "." : sqlReader.GetString(30),
                        sqlReader.IsDBNull(31) ? 0 : sqlReader.GetDouble(31),
                        sqlReader.IsDBNull(32) ? "." : sqlReader.GetString(32),
                        sqlReader.IsDBNull(33) ? 0 : sqlReader.GetDouble(33),
                        sqlReader.IsDBNull(34) ? "." : sqlReader.GetString(34),
                        sqlReader.IsDBNull(35) ? 0 : sqlReader.GetDouble(35),
                        sqlReader.IsDBNull(36) ? "." : sqlReader.GetString(36),
                        sqlReader.IsDBNull(37) ? 0 : sqlReader.GetDouble(37),
                        sqlReader.IsDBNull(38) ? "." : sqlReader.GetString(38),
                        sqlReader.IsDBNull(39) ? 0 : sqlReader.GetDouble(39),
                        sqlReader.IsDBNull(40) ? "." : sqlReader.GetString(40),
                        sqlReader.IsDBNull(41) ? 0 : sqlReader.GetDouble(41),
                        sqlReader.IsDBNull(42) ? "." : sqlReader.GetString(42),
                        sqlReader.IsDBNull(43) ? 0 : sqlReader.GetDouble(43),
                        sqlReader.IsDBNull(44) ? "." : sqlReader.GetString(44),
                        sqlReader.IsDBNull(45) ? 0 : sqlReader.GetDouble(45),
                        sqlReader.IsDBNull(46) ? "." : sqlReader.GetString(46),
                        sqlReader.IsDBNull(47) ? 0 : sqlReader.GetDouble(47),
                        sqlReader.IsDBNull(48) ? "." : sqlReader.GetString(48),
                        sqlReader.IsDBNull(49) ? 0 : sqlReader.GetDouble(49),
                        sqlReader.IsDBNull(50) ? "." : sqlReader.GetString(50),
                        sqlReader.IsDBNull(51) ? 0 : sqlReader.GetDouble(51),
                        sqlReader.IsDBNull(52) ? "." : sqlReader.GetString(52),
                        sqlReader.IsDBNull(53) ? 0 : sqlReader.GetDouble(53),
                        sqlReader.IsDBNull(54) ? "." : sqlReader.GetString(54),
                        sqlReader.IsDBNull(55) ? 0 : sqlReader.GetDouble(55),
                        sqlReader.IsDBNull(56) ? 0 : sqlReader.GetDouble(56),
                        sqlReader.IsDBNull(57) ? 0 : sqlReader.GetDouble(57),
                        sqlReader.IsDBNull(58) ? "." : sqlReader.GetString(58)));
            }
            sqlConexion.Close();
            return listadoMedicosRecetas.ToArray();
        }

        /*----------------------PEDIDOS FARMACIAS---------------------*/
        [WebMethod]
        public Droguerias[] getDroguerias()
        {
            try
            {
                sqlConexion.Open();
                sqlComando = new SqlCommand("SELECT ID_Drogueria, TX_Drogueria, BO_Activo " +
                    "FROM MW_Droguerias", sqlConexion);
                sqlReader = sqlComando.ExecuteReader();

                List<Droguerias> lista = new List<Droguerias>();
                while (sqlReader.Read())
                {
                    lista.Add(new Droguerias(sqlReader.GetInt16(0), sqlReader.GetString(1), sqlReader.GetBoolean(2)));
                }
                sqlConexion.Close();
                return lista.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [WebMethod]
        public Productos[] getProductos()
        {
            try
            {
                sqlConexion.Open();
                sqlComando = new SqlCommand("SELECT ID_Producto, ID_Marca, TX_Producto, TX_IDProductoCliente, " +
                    "TX_ProductoDesc, BO_Activo FROM MW_Productos", sqlConexion);
                sqlReader = sqlComando.ExecuteReader();

                List<Productos> lista = new List<Productos>();
                while (sqlReader.Read())
                {
                    lista.Add(new Productos(sqlReader.GetInt16(0), sqlReader.GetInt16(1), sqlReader.GetString(2),
                        sqlReader.GetString(3), sqlReader.GetString(4), sqlReader.GetBoolean(5)));
                }
                sqlConexion.Close();
                return lista.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [WebMethod]
        public DrogueriasAlmacenes[] getDrogueriasAlmacenes()
        {
            try
            {
                sqlConexion.Open();
                sqlComando = new SqlCommand("SELECT ID_DrogueriaAlmacen, ID_Drogueria, TX_Almacen, TX_IDAlmacen, BO_Activo " +
                                            "FROM     MW_DrogueriasAlmacenes WHERE BO_Activo = 1", sqlConexion);
                sqlReader = sqlComando.ExecuteReader();

                List<DrogueriasAlmacenes> lista = new List<DrogueriasAlmacenes>();
                while (sqlReader.Read())
                {
                    lista.Add(new DrogueriasAlmacenes(sqlReader.GetInt16(0), sqlReader.GetInt16(1), sqlReader.GetString(2),
                        sqlReader.IsDBNull(3) == true ? string.Empty : sqlReader.GetString(3), sqlReader.GetBoolean(4)));
                }
                sqlConexion.Close();
                return lista.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [WebMethod]
        public DrogueriasProductos[] getDrogueriasProductos()
        {
            try
            {
                sqlConexion.Open();
                sqlComando = new SqlCommand("SELECT ID_DrogueriaProducto, ID_DrogueriaAlmacen, ID_Producto, " +
                    "TX_IDProductoDrogueria, TX_ProductoDrogueria, NU_PrecioProducto, NU_InvProducto, TX_DrogueriaRef1, " +
                    "TX_DrogueriaRef2, BO_Activo " +
                    "FROM     MW_DrogueriasProductos " +
                    "WHERE ID_Producto IS NOT NULL", sqlConexion);
                sqlReader = sqlComando.ExecuteReader();

                List<DrogueriasProductos> lista = new List<DrogueriasProductos>();
                while (sqlReader.Read())
                {
                    lista.Add(new DrogueriasProductos(sqlReader.GetInt32(0), sqlReader.GetInt16(1),
                        sqlReader.IsDBNull(2) == true ? Convert.ToInt16(0) : sqlReader.GetInt16(2),
                        sqlReader.GetString(3), sqlReader.GetString(4), sqlReader.GetDecimal(5), sqlReader.GetInt32(6),
                        sqlReader.IsDBNull(7) == true ? string.Empty : sqlReader.GetString(7),
                        sqlReader.IsDBNull(8) == true ? string.Empty : sqlReader.GetString(8), sqlReader.GetBoolean(9)));
                }
                sqlConexion.Close();
                return lista.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [WebMethod]
        public bool getPedidos(Byte[] zipPedidos)
        {
            //File.WriteAllBytes(@"C:\Users\Javier Contreras\Desktop\Pedidos.zip", zipPedidos);
            try
            {
                string pedidosCAB = "";
                string pedidosDET = "";
                Stream zipStream = new MemoryStream(zipPedidos);
                using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(zipStream))
                {
                    ICollection<ZipEntry> obFileEntries = zip.Entries;
                    foreach (ZipEntry obZipEntry in obFileEntries)
                    {
                        using (System.IO.MemoryStream obMemoryStream = new MemoryStream())
                        {
                            //obZipEntry.ExtractWithPassword(obMemoryStream, "InterS_Issa");
                            obZipEntry.Extract(obMemoryStream);
                            obMemoryStream.Seek(0, System.IO.SeekOrigin.Begin);

                            switch (obZipEntry.FileName)
                            {
                                case "PedidosCabecera.txt":
                                    pedidosCAB = Encoding.ASCII.GetString(obMemoryStream.ToArray());
                                    break;
                                case "PedidosDetalle.txt":
                                    pedidosDET = Encoding.ASCII.GetString(obMemoryStream.ToArray());
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                return savePedidos(pedidosCAB, pedidosDET);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private bool savePedidos(string datosCAB, string datosDET)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MedinetWeb"].ConnectionString))
                {
                    sqlConn.Open();
                    // bulk send data to database
                    var cmd = new SqlCommand("SP_MD_BulkInsertPedidos", sqlConn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    SqlParameter paramCAB = cmd.Parameters.AddWithValue("@pedidosCAB", DatosPedidosCAB(datosCAB));
                    paramCAB.SqlDbType = SqlDbType.Structured;
                    SqlParameter paramDET = cmd.Parameters.AddWithValue("@pedidosDET", DatosPedidosDET(datosDET));
                    paramDET.SqlDbType = SqlDbType.Structured;
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }

        private DataTable DatosPedidosCAB(string datos)
        {
            DataTable dt = new DataTable();
            dt = ModelDataTable("MD_Pedidos");
            DataRow dr;
            try
            {
                using (StringReader reader = new StringReader(datos))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] s = line.Split(new char[] { '|' });
                        dr = dt.NewRow();

                        dr[0] = Int64.Parse(s[0]);
                        dr[1] = Int64.Parse(s[1]);
                        dr[2] = Int16.Parse(s[2]);
                        dr[3] = Int16.Parse(s[3]);
                        dr[4] = Decimal.Parse(s[4].Replace(".", ","));
                        dr[5] = Decimal.Parse(s[5].Replace(".", ","));
                        dr[6] = Decimal.Parse(s[6].Replace(".", ","));
                        dr[7] = Decimal.Parse(s[7].Replace(".", ","));
                        dr[8] = s[8];
                        dr[9] = s[9];
                        dr[10] = s[10];
                        dr[11] = s[11];
                        dr[12] = s[12] == "0" ? false : true;
                        dr[13] = Int16.Parse(s[13]);

                        dt.Rows.Add(dr);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private DataTable DatosPedidosDET(string datos)
        {
            DataTable dt = new DataTable();
            dt = ModelDataTable("MD_PedidosDetalle");
            DataRow dr;
            try
            {
                using (StringReader reader = new StringReader(datos))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] s = line.Split(new char[] { '|' });
                        dr = dt.NewRow();

                        //for (int i = 0; i <= dt.Columns.Count - 1; i++)
                        //{
                        dr[0] = Int64.Parse(s[0]);
                        dr[1] = Int64.Parse(s[1]);
                        dr[2] = Int64.Parse(s[2]);
                        dr[3] = Int16.Parse(s[3]);
                        dr[4] = s[4];
                        dr[5] = Int16.Parse(s[5]);
                        dr[6] = Int32.Parse(s[6]);
                        dr[7] = Decimal.Parse(s[7].Replace(".", ","));
                        dr[8] = Int32.Parse(s[8]);
                        dr[9] = Decimal.Parse(s[9].Replace(".", ","));
                        dr[10] = s[10] == "0" ? false : true;
                        //}


                        dt.Rows.Add(dr);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private DataTable ModelDataTable(string table)
        {
            try
            {
                string query;
                switch (table)
                {
                    case "MD_Pedidos":
                        query = "SET FMTONLY ON; SELECT ID_Visitador, ID_Pedido, ID_Farmacia, ID_TipoDescuento, "
                                + "NU_CantTotalPedido, NU_CostoTotalPedido, NU_CantTotalBonif, NU_Descuento, TX_Contacto, TX_TelefonoCont, TX_Observacion, "
                                + "FE_Registro, BO_Procesado, NU_Estatus FROM " + table + "; SET FMTONLY OFF;";
                        break;
                    default:
                        query = "SET FMTONLY ON; SELECT ID_Pedido, ID_Visitador, ID_Farmacia, ID_DrogueriaAlmacen, TX_CodFarmacia, ID_Producto, NU_CantPedido, "
                                + "NU_PrecioProducto, NU_CantBonif, NU_DescuentoProducto, BO_Estatus "
                                + "FROM  " + table + "; SET FMTONLY OFF;";
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        [WebMethod(Description = "Devuelve los pedidos actualizados")]
        public PedidoActualizado[] getPedidosActualizados(int idVisitadorHistorial)
        {
            List<PedidoActualizado> listaPedidos = new List<PedidoActualizado>();
            try
            {
                sqlConexion.Open();
                sqlComando = new SqlCommand("SELECT ID_PedidoMedinet,ID_Visitador,ID_Pedido,NU_Estatus,NU_EstatusProcesado " +
                    "FROM MD_Pedidos  WHERE (ID_Visitador = " + idVisitadorHistorial + ")", sqlConexion);
                sqlReader = sqlComando.ExecuteReader();


                while (sqlReader.Read())
                {
                    PedidoActualizado miPedido = new PedidoActualizado();

                    miPedido.ID_PedidoMedinet = sqlReader.GetInt64(0);
                    miPedido.ID_Visitador = sqlReader.GetInt64(1);
                    miPedido.ID_Pedido = sqlReader.GetInt64(2);
                    miPedido.NU_Estatus = sqlReader.GetInt16(3);
                    miPedido.NU_EstatusProcesado = sqlReader.GetInt16(4);


                    listaPedidos.Add(miPedido);
                }
                sqlConexion.Close();
                return listaPedidos.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        [WebMethod(Description = "Devuelve las cabeceras de las facturas de los pedidos procesados por las droguerias")]
        public PedidoFacturaCabecera[] getPedidosFacturaCabecera(int idVisitadorHistorial)
        {
            List<PedidoFacturaCabecera> listaFacturasCabeceras = new List<PedidoFacturaCabecera>();
            try
            {
                sqlConexion.Open();
                sqlComando = new SqlCommand("SELECT PFC.ID_FacturaMedinet, PFC.ID_PedidoMedinet, " +
                    "PFC.ID_Drogueria, PFC.NU_FacturaDrogueria, PFC.NU_PedidoDrogueria, " +
                    "PFC.FE_FacturaDrogueria, PFC.NU_TotalUnidades, PFC.NU_CostoTotalFactura, " +
                         "PFC.FE_Recibido, PFC.FE_Modificado FROM MW_PedidosFacturasCabeceras AS PFC INNER JOIN " +
                         "MD_Pedidos AS P ON PFC.ID_PedidoMedinet = P.ID_PedidoMedinet " +
                         "WHERE ID_Visitador = (" + idVisitadorHistorial + ") AND (P.NU_Estatus = 3)", sqlConexion);
                sqlReader = sqlComando.ExecuteReader();

                while (sqlReader.Read())
                {
                    PedidoFacturaCabecera pfc = new PedidoFacturaCabecera();
                    pfc.ID_FacturaMedinet = sqlReader.GetInt64(0);
                    pfc.ID_PedidoMedinet = sqlReader.GetInt64(1);
                    pfc.ID_Drogueria = sqlReader.GetInt16(2);
                    pfc.NU_FacturaDrogueria = sqlReader.GetInt64(3);
                    pfc.NU_PedidoDrogueria = sqlReader.IsDBNull(4) ? 0 : sqlReader.GetInt64(4);
                    pfc.FE_FacturaDrogueria = sqlReader.GetDateTime(5);
                    pfc.NU_TotalUnidades = sqlReader.IsDBNull(6) ? 0 : sqlReader.GetInt32(6);
                    pfc.NU_CostoTotalFactura = sqlReader.GetDecimal(7);
                    pfc.FE_Recibido = sqlReader.GetDateTime(8);
                    pfc.FE_Modificado = sqlReader.GetDateTime(9);

                    listaFacturasCabeceras.Add(pfc);
                }
                sqlConexion.Close();
                return listaFacturasCabeceras.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        [WebMethod(Description = "Devuelve los detalles de las facturas de los pedidos procesados por las droguerias")]
        public PedidoFacturaDetalle[] getPedidosFacturaDetalle(int idVisitadorHistorial)
        {
            List<PedidoFacturaDetalle> listaFacturasDetalles = new List<PedidoFacturaDetalle>();
            try
            {
                sqlConexion.Open();
                sqlComando = new SqlCommand("SELECT PFD.ID_Detalle, PFD.ID_FacturaMedinet, PFD.ID_Producto, " +
                                            "PFD.TX_IDProductoDrogueria, PFD.TX_Lote, PFD.NU_CantidadFacturada, " +
                                            "PFD.FE_Recibido, PFD.FE_Modificado FROM MW_PedidosFacturasCabeceras " +
                                            "AS PFC INNER JOIN MD_Pedidos AS P ON PFC.ID_PedidoMedinet = P.ID_PedidoMedinet " +
                                            "INNER JOIN MW_PedidosFacturasDetalles AS PFD " +
                                            "ON PFC.ID_FacturaMedinet = PFD.ID_FacturaMedinet " +
                                            "WHERE ID_Visitador = (" + idVisitadorHistorial + ") AND (P.NU_Estatus = 3)", sqlConexion);
                sqlReader = sqlComando.ExecuteReader();

                while (sqlReader.Read())
                {
                    PedidoFacturaDetalle pfd = new PedidoFacturaDetalle();
                    pfd.ID_Detalle = sqlReader.GetInt64(0);
                    pfd.ID_FacturaMedinet = sqlReader.GetInt64(1);
                    pfd.ID_Producto = sqlReader.GetInt16(2);
                    pfd.TX_IDProductoDrogueria = sqlReader.GetString(3);
                    pfd.TX_Lote = sqlReader.IsDBNull(4) ? "S/L" : sqlReader.GetString(4);
                    pfd.NU_CantidadFacturada = sqlReader.GetInt64(5);
                    pfd.FE_Recibido = sqlReader.GetDateTime(6);
                    pfd.FE_Modificado = sqlReader.GetDateTime(7);


                    listaFacturasDetalles.Add(pfd);
                }
                sqlConexion.Close();
                return listaFacturasDetalles.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


    }
}