using System;
using System.Collections.Generic;

namespace MedinetLibrary
{

    /// <summary>
    /// Estas clases son utilizadas para definir estructuras de comunicaciòn entre el dispositivo y el Web Service que lo alimenta
    /// </summary>
    public class Tabla
    {
        string nombre;
        string estructura;

        public Tabla()
        {
            Nombre = "";
            Estructura = "";
        }
        public Tabla(String nombre, String estructura)
        {
            Nombre = nombre;
            Estructura = estructura;
        }


        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        public string Estructura
        {
            get { return estructura; }
            set { estructura = value; }
        }
    }
    public class Medico
    {
        public int nroSanitario;
        public string doctor;

        public Medico()
        {
            this.nroSanitario = 0;
            this.doctor = "";
        }
        public Medico(int nroSanitario, string doctor)
        {
            this.nroSanitario = nroSanitario;
            this.doctor = doctor;
        }
    }
    public class Farmacia
    {
        public int numero;
        public int brick;
        public string ruta;
        public string farmacia;
        public string direccion;
        public string ciudad;
        public string estado;
        public string cadena;
        public string telefono1;
        public string telefono2;
        public string rif;

        public Farmacia(int numero, int brick, string ruta, string farmacia,
                string direccion, string ciudad, string estado, string cadena,
                string telefono1, string telefono2, string rif)
        {

            this.numero = numero;
            this.brick = brick;
            this.ruta = ruta;
            this.farmacia = farmacia;
            this.direccion = direccion;
            this.ciudad = ciudad;
            this.estado = estado;
            this.cadena = cadena;
            this.telefono1 = telefono1;
            this.telefono2 = telefono2;
            this.rif = rif;
        }

        public Farmacia()
        {
            this.numero = 0;
            this.brick = 0;
            this.ruta = "";
            this.farmacia = "";
            this.direccion = "";
            this.ciudad = "";
            this.estado = "";
            this.cadena = "";
            this.telefono1 = "";
            this.telefono2 = "";
            this.rif = "";

        }

    }
    public class Hospital
    {
        public int numero;
        public int brick;
        public string ruta;
        public string hospital;
        public string institucion;
        public string direccion;
        public Boolean cliente;
        public Boolean docente;

        public Hospital(int numero, int brick, string ruta, string hospital,
                string institucion, string direccion, Boolean cliente, Boolean docente)
        {

            this.numero = numero;
            this.brick = brick;
            this.ruta = ruta;
            this.hospital = hospital;
            this.institucion = institucion;
            this.direccion = direccion;
            this.cliente = cliente;
            this.docente = docente;

        }

        public Hospital()
        {
            this.numero = 0;
            this.brick = 0;
            this.ruta = "";
            this.hospital = "";
            this.institucion = "";
            this.direccion = "";
            this.cliente = false;
            this.docente = false;

        }

    }
    public class EsquemaPromocional
    {
        public int posicion;
        public string especialidad;
        public string marca;
        public string posicionamiento;

        public EsquemaPromocional(int posicion, string especialidad, string marca, string posicionamiento)
        {
            this.posicion = posicion;
            this.especialidad = especialidad;
            this.marca = marca;
            this.posicionamiento = posicionamiento;
        }
        public EsquemaPromocional()
        {
            this.posicion = 0;
            this.especialidad = "";
            this.marca = "";
            this.posicionamiento = "";
        }
    }
    public class Ciclo
    {
        public string fechaICiclo;
        public string fechaFCiclo;
        public int nroCiclo;
        public string cicloCerrado;
        public int diasHabiles;
        public string estatus;

        public Ciclo()
        {
            this.nroCiclo = 0;
            this.fechaICiclo = "";
            this.fechaFCiclo = "";
            this.estatus = "";
            this.diasHabiles = 0;
            this.cicloCerrado = "";
        }
        public Ciclo(string fechaICiclo, string fechaFCiclo, int nroCiclo, string cicloCerrado, int diasHabiles, string estatus)
        {
            this.nroCiclo = nroCiclo;
            this.fechaICiclo = fechaICiclo;
            this.fechaFCiclo = fechaFCiclo;
            this.estatus = estatus;
            this.diasHabiles = diasHabiles;
            this.cicloCerrado = cicloCerrado;
        }
    }
    public class CicloVehiculo
    {
        public int id;
        public string fechaICiclo;
        public string fechaFCiclo;
        public int nroCiclo;
        public string cicloCerrado;
        public int diasHabiles;
        public string estatus;

        public CicloVehiculo()
        {
            this.id = 0;
            this.nroCiclo = 0;
            this.fechaICiclo = "";
            this.fechaFCiclo = "";
            this.estatus = "";
            this.diasHabiles = 0;
            this.cicloCerrado = "";
        }
        public CicloVehiculo(string fechaICiclo, string fechaFCiclo, int nroCiclo, string cicloCerrado, int diasHabiles, string estatus, int id)
        {
            this.nroCiclo = nroCiclo;
            this.fechaICiclo = fechaICiclo;
            this.fechaFCiclo = fechaFCiclo;
            this.estatus = estatus;
            this.diasHabiles = diasHabiles;
            this.cicloCerrado = cicloCerrado;
            this.id = id;
        }
    }
    public class DataGerenteNacional
    {
        public Int32 co_gerente_nac;
        public Int16 co_region;
        public string nombre;
        public string apellido;

        public decimal nu_vm;
        public decimal nu_rm;
        public decimal nu_pdr;
        public decimal nu_mix;

        public string nu_ranking;

        public string nu_vm_total;
        public string nu_rm_total;
        public string nu_pdr_total;
        public string nu_mix_total;

        public DataGerenteNacional(Int32 co_gerente_nac, Int16 co_region,
                                    string nombre, string apellido,
                                    decimal nu_vm, decimal nu_rm, decimal nu_pdr,
                                    decimal nu_mix, string nu_ranking, string nu_vm_total,
                                    string nu_rm_total, string nu_pdr_total, string nu_mix_total)
        {

            this.co_gerente_nac = co_gerente_nac;
            this.co_region = co_region;
            this.nombre = nombre;
            this.apellido = apellido;
            this.nu_vm = nu_vm;
            this.nu_rm = nu_rm;
            this.nu_pdr = nu_pdr;
            this.nu_mix = nu_mix;
            this.nu_ranking = nu_ranking;
            this.nu_vm_total = nu_vm_total;
            this.nu_rm_total = nu_rm_total;
            this.nu_pdr_total = nu_pdr_total;
            this.nu_mix_total = nu_mix_total;
        }
        public DataGerenteNacional()
        {
            this.co_gerente_nac = 0;
            this.co_region = 0;
            this.nombre = "";
            this.apellido = "";
            this.nu_vm = 0;
            this.nu_rm = 0;
            this.nu_pdr = 0;
            this.nu_mix = 0;
            this.nu_ranking = "";
            this.nu_vm_total = "";
            this.nu_rm_total = "";
            this.nu_pdr_total = "";
            this.nu_mix_total = "";
        }
    }
    public class DataGerenteRegional
    {
        public Int32 co_gerente_nac;
        public Int32 co_gerente_reg;
        public Int16 co_linea;
        public Int16 co_region;
        public string nombre;
        public string apellido;

        public decimal nu_vm;
        public decimal nu_rm;
        public decimal nu_pdr;
        public decimal nu_mix;

        public string nu_ranking;

        public string nu_vm_total;
        public string nu_rm_total;
        public string nu_pdr_total;
        public string nu_mix_total;

        public DataGerenteRegional(Int32 co_gerente_nac, Int16 co_region,
                                    string nombre, string apellido,
                                    decimal nu_vm, decimal nu_rm, decimal nu_pdr,
                                    decimal nu_mix, string nu_ranking, string nu_vm_total,
                                    string nu_rm_total, string nu_pdr_total, string nu_mix_total, Int32 co_gerente_reg, Int16 co_linea)
        {

            this.co_gerente_nac = co_gerente_nac;
            this.co_region = co_region;
            this.nombre = nombre;
            this.apellido = apellido;
            this.nu_vm = nu_vm;
            this.nu_rm = nu_rm;
            this.nu_pdr = nu_pdr;
            this.nu_mix = nu_mix;
            this.nu_ranking = nu_ranking;
            this.nu_vm_total = nu_vm_total;
            this.nu_rm_total = nu_rm_total;
            this.nu_pdr_total = nu_pdr_total;
            this.nu_mix_total = nu_mix_total;
            this.co_gerente_reg = co_gerente_reg;
            this.co_linea = co_linea;
        }
        public DataGerenteRegional()
        {
            this.co_gerente_nac = 0;
            this.co_region = 0;
            this.nombre = "";
            this.apellido = "";
            this.nu_vm = 0;
            this.nu_rm = 0;
            this.nu_pdr = 0;
            this.nu_mix = 0;
            this.nu_ranking = "";
            this.nu_vm_total = "";
            this.nu_rm_total = "";
            this.nu_pdr_total = "";
            this.nu_mix_total = "";
            this.co_gerente_reg = 0;
            this.co_linea = 0;
        }
    }
    public class DataVisitadorMedico
    {

        public Int64 co_visitador;
        public Int32 co_gerente_reg;
        public Int32 co_linea;
        public string nombre;
        public string apellido;

        public double nu_vm;
        public double nu_rm;
        public decimal nu_pdr;
        public double nu_mix;

        public string nu_ranking;

        public string nu_vm_total;
        public string nu_rm_total;
        public string nu_pdr_total;
        public string nu_mix_total;

        public string da_transmision;

        public DataVisitadorMedico(Int64 co_visitador,
                                    string nombre, string apellido,
                                    double nu_vm, double nu_rm, decimal nu_pdr,
                                    double nu_mix, string nu_ranking, string nu_vm_total,
                                    string nu_rm_total, string nu_pdr_total, string nu_mix_total, Int32 co_gerente_reg, Int32 co_linea, string da_transmision)
        {

            this.co_visitador = co_visitador;
            this.nombre = nombre;
            this.apellido = apellido;
            this.nu_vm = nu_vm;
            this.nu_rm = nu_rm;
            this.nu_pdr = nu_pdr;
            this.nu_mix = nu_mix;
            this.nu_ranking = nu_ranking;
            this.nu_vm_total = nu_vm_total;
            this.nu_rm_total = nu_rm_total;
            this.nu_pdr_total = nu_pdr_total;
            this.nu_mix_total = nu_mix_total;
            this.co_gerente_reg = co_gerente_reg;
            this.co_linea = co_linea;
            this.da_transmision = da_transmision;
        }
        public DataVisitadorMedico()
        {
            this.co_visitador = 0;
            this.nombre = "";
            this.apellido = "";
            this.nu_vm = 0;
            this.nu_rm = 0;
            this.nu_pdr = 0;
            this.nu_mix = 0;
            this.nu_ranking = "";
            this.nu_vm_total = "";
            this.nu_rm_total = "";
            this.nu_pdr_total = "";
            this.nu_mix_total = "";
            this.co_gerente_reg = 0;
            this.co_linea = 0;
            this.da_transmision = "";
        }
    }
    public class DataVisitadorMedicoII
    {

        public Int64 co_visitador;
        public Int32 co_gerente_reg;
        public Int32 co_linea;
        public Int32 co_region;
        public string nombre;
        public string apellido;

        public double nu_vm;
        public double nu_rm;
        public decimal nu_pdr;
        public double nu_mix;

        public string nu_ranking;

        public string nu_vm_total;
        public string nu_rm_total;
        public string nu_pdr_total;
        public string nu_mix_total;

        public string da_transmision;

        public DataVisitadorMedicoII(Int64 co_visitador,
                                    string nombre, string apellido,
                                    double nu_vm, double nu_rm, decimal nu_pdr,
                                    double nu_mix, string nu_ranking, string nu_vm_total,
                                    string nu_rm_total, string nu_pdr_total, string nu_mix_total, Int32 co_gerente_reg, Int32 co_linea, Int32 co_region, string da_transmision)
        {

            this.co_visitador = co_visitador;
            this.nombre = nombre;
            this.apellido = apellido;
            this.nu_vm = nu_vm;
            this.nu_rm = nu_rm;
            this.nu_pdr = nu_pdr;
            this.nu_mix = nu_mix;
            this.nu_ranking = nu_ranking;
            this.nu_vm_total = nu_vm_total;
            this.nu_rm_total = nu_rm_total;
            this.nu_pdr_total = nu_pdr_total;
            this.nu_mix_total = nu_mix_total;
            this.co_gerente_reg = co_gerente_reg;
            this.co_linea = co_linea;
            this.co_region = co_region;
            this.da_transmision = da_transmision;
        }
        public DataVisitadorMedicoII()
        {
            this.co_visitador = 0;
            this.nombre = "";
            this.apellido = "";
            this.nu_vm = 0;
            this.nu_rm = 0;
            this.nu_pdr = 0;
            this.nu_mix = 0;
            this.nu_ranking = "";
            this.nu_vm_total = "";
            this.nu_rm_total = "";
            this.nu_pdr_total = "";
            this.nu_mix_total = "";
            this.co_gerente_reg = 0;
            this.co_linea = 0;
            this.co_region = 0;
            this.da_transmision = "";
        }
    }
    public class DataVisitadorMedicoFarmacias
    {

        public Int64 co_visitador;
        public Int32 co_gerente_reg;
        public Int32 co_region;
        public Int32 co_linea;

        public string nombre;
        public string apellido;

        public Int32 nu_fichados;
        public Int32 nu_visitados;
        public decimal nu_cobertura;


        public DataVisitadorMedicoFarmacias(Int64 co_visitador,
        Int32 co_gerente_reg,
        Int32 co_region,
        Int32 co_linea,

        string nombre,
        string apellido,

        Int32 nu_fichados,
        Int32 nu_visitados,
        decimal nu_cobertura)
        {

            this.co_visitador = co_visitador;
            this.nombre = nombre;
            this.apellido = apellido;
            this.co_gerente_reg = co_gerente_reg;
            this.co_linea = co_linea;
            this.co_region = co_region;
            this.nu_fichados = nu_fichados;
            this.nu_visitados = nu_visitados;
            this.nu_cobertura = nu_cobertura;

        }
        public DataVisitadorMedicoFarmacias()
        {
            this.co_visitador = 0;
            this.nombre = "";
            this.apellido = "";
            this.co_gerente_reg = 0;
            this.co_linea = 0;
            this.co_region = 0;
            this.nu_fichados = 0;
            this.nu_visitados = 0;
            this.nu_cobertura = 0;
        }
    }
    public class DataVisitadorMedicoHospitales
    {

        public Int64 co_visitador;
        public Int32 co_gerente_reg;
        public Int32 co_region;
        public Int32 co_linea;

        public string nombre;
        public string apellido;

        public Int32 nu_fichados;
        public Int32 nu_visitados;
        public decimal nu_cobertura;


        public DataVisitadorMedicoHospitales(Int64 co_visitador,
        Int32 co_gerente_reg,
        Int32 co_region,
        Int32 co_linea,

        string nombre,
        string apellido,

        Int32 nu_fichados,
        Int32 nu_visitados,
        decimal nu_cobertura)
        {

            this.co_visitador = co_visitador;
            this.nombre = nombre;
            this.apellido = apellido;
            this.co_gerente_reg = co_gerente_reg;
            this.co_linea = co_linea;
            this.co_region = co_region;
            this.nu_fichados = nu_fichados;
            this.nu_visitados = nu_visitados;
            this.nu_cobertura = nu_cobertura;

        }
        public DataVisitadorMedicoHospitales()
        {
            this.co_visitador = 0;
            this.nombre = "";
            this.apellido = "";
            this.co_gerente_reg = 0;
            this.co_linea = 0;
            this.co_region = 0;
            this.nu_fichados = 0;
            this.nu_visitados = 0;
            this.nu_cobertura = 0;
        }
    }
    public class DataVisitadorMedicoHojaRuta
    {

        public Int64 co_visitador;
        public Int32 co_gerente_reg;
        public Int32 co_region;
        public Int32 co_linea;

        public string nombre;
        public string apellido;

        public string AM;
        public string PM;


        public DataVisitadorMedicoHojaRuta(Int64 co_visitador,
        Int32 co_gerente_reg,
        Int32 co_region,
        Int32 co_linea,

        string nombre,
        string apellido,

        string AM, string PM)
        {

            this.co_visitador = co_visitador;
            this.nombre = nombre;
            this.apellido = apellido;
            this.co_gerente_reg = co_gerente_reg;
            this.co_linea = co_linea;
            this.co_region = co_region;
            this.AM = AM;
            this.PM = PM;

        }
        public DataVisitadorMedicoHojaRuta()
        {
            this.co_visitador = 0;
            this.nombre = "";
            this.apellido = "";
            this.co_gerente_reg = 0;
            this.co_linea = 0;
            this.co_region = 0;
            this.AM = "";
            this.PM = "";
        }

    }
    public class DataVisitadorMedicoAgenda
    {

        public Int64 co_visitador;
        public Int32 co_gerente_reg;
        public Int32 co_region;
        public Int32 co_linea;

        public string nombre;
        public string apellido;

        public string doctor;
        public string especialidad;
        public string clinica;
        public string clasificacion;
        public string hora;
        public string ampm;
        public string tipo;


        public DataVisitadorMedicoAgenda(Int64 co_visitador,
        Int32 co_gerente_reg,
        Int32 co_region,
        Int32 co_linea,

        string nombre,
        string apellido,

         string doctor,
         string especialidad,
         string clinica,
         string clasificacion,
         string hora,
         string ampm, string tipo)
        {

            this.co_visitador = co_visitador;
            this.nombre = nombre;
            this.apellido = apellido;
            this.co_gerente_reg = co_gerente_reg;
            this.co_linea = co_linea;
            this.co_region = co_region;
            this.doctor = doctor;
            this.especialidad = especialidad;
            this.clinica = clinica;
            this.clasificacion = clasificacion;
            this.hora = hora;
            this.ampm = ampm;
            this.tipo = tipo;

        }
        public DataVisitadorMedicoAgenda()
        {
            this.co_visitador = 0;
            this.nombre = "";
            this.apellido = "";
            this.co_gerente_reg = 0;
            this.co_linea = 0;
            this.co_region = 0;
            this.doctor = "";
            this.especialidad = "";
            this.clinica = "";
            this.clasificacion = "";
            this.hora = "";
            this.ampm = "";
            this.tipo = "";
        }

    }
    public class TipoGerente
    {
        public string tipo;
        public int codigo;

        public TipoGerente()
        {
            this.tipo = "";
            this.codigo = 0;
        }
        public TipoGerente(string tipo, int codigo)
        {
            this.codigo = codigo;
            this.tipo = tipo;
        }

    }
    public class Linea
    {

        public Int16 co_linea;
        public string linea;

        public Linea(Int16 co_linea, string linea)
        {
            this.co_linea = co_linea;
            this.linea = linea;
        }
        public Linea()
        {
            this.co_linea = 0;
            this.linea = "";
        }
    }
    public class Region
    {

        public Int16 co_region;
        public string region;

        public Region(Int16 co_region, string region)
        {
            this.co_region = co_region;
            this.region = region;
        }
        public Region()
        {
            this.co_region = 0;
            this.region = "";
        }
    }
    public class Umbral
    {
        public string vm;
        public double vm_valor_umbral;
        public string rm;
        public double rm_valor_umbral;
        public string pdr;
        public double pdr_valor_umbral;
        public string mix;
        public double mix_valor_umbral;
        public Umbral(string vm, double vm_valor_umbral, string rm, double rm_valor_umbral, string pdr, double pdr_valor_umbral, string mix, double mix_valor_umbral)
        {
            this.vm = vm;
            this.vm_valor_umbral = vm_valor_umbral;
            this.rm = rm;
            this.rm_valor_umbral = rm_valor_umbral;
            this.pdr = pdr;
            this.pdr_valor_umbral = pdr_valor_umbral;
            this.mix = mix;
            this.mix_valor_umbral = mix_valor_umbral;
        }
        public Umbral()
        {
            this.vm = "";
            this.vm_valor_umbral = 0.00;
            this.rm = "";
            this.rm_valor_umbral = 0.00;
            this.pdr = "";
            this.pdr_valor_umbral = 0.00;
            this.mix = "";
            this.mix_valor_umbral = 0.00;
        }
    }
    public class RegionNacional
    {
        public Int32 co_gerente_nac;
        public Int16 co_region;

        public RegionNacional(Int32 co_gerente_nac, Int16 co_region)
        {
            this.co_gerente_nac = co_gerente_nac;
            this.co_region = co_region;
        }
        public RegionNacional()
        {
            this.co_gerente_nac = 0;
            this.co_region = 0;
        }
    }
    public class LineaRegional
    {
        public Int32 co_gerente_reg;
        public Int16 co_region;
        public Int16 co_linea;

        public LineaRegional(Int32 co_gerente_reg, Int16 co_region, Int16 co_linea)
        {
            this.co_gerente_reg = co_gerente_reg;
            this.co_region = co_region;
            this.co_linea = co_linea;
        }
        public LineaRegional()
        {
            this.co_gerente_reg = 0;
            this.co_region = 0;
            this.co_linea = 0;
        }
    }
    public class Ficha
    {
        public string codigo;
        public int v1;
        public int v2;
        public int v3;
        public int e1;
        public int e2;
        public int e3;
        public int r1;
        public int r2;
        public int r3;

        public Ficha(string codigo, int v1, int v2, int v3, int e1, int e2, int e3, int r1, int r2, int r3)
        {
            this.codigo = codigo;
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.e1 = e1;
            this.e2 = e2;
            this.e3 = e3;
            this.r1 = r1;
            this.r2 = r2;
            this.r3 = r3;
        }
        public Ficha()
        {
            this.codigo = "";
            this.v1 = 0;
            this.v2 = 0;
            this.v3 = 0;
            this.e1 = 0;
            this.e2 = 0;
            this.e3 = 0;
            this.r1 = 0;
            this.r2 = 0;
            this.r3 = 0;
        }
    }
    public class Data
    {
        public String msg;
        public String tipo;
        public String titulo;


        public Data(String mensaje, String tipo)
        {
            this.msg = mensaje;
            this.tipo = tipo;
        }
        public Data(String titulo, String mensaje, String tipo)
        {
            this.titulo = titulo;
            this.msg = mensaje;
            this.tipo = tipo;
        }
    }
    public class GCMMessage
    {
        public Data data;

        public List<String> registration_ids;

        public String collapse_key = "";

        public int time_to_live = 2419200;

        public bool delay_while_idle = false;

        public GCMMessage(Data data, List<String> listaRegistrationIds, String collapseKey, int timeToLive, bool delayWhileIdle)
        {
            this.data = data;
            this.registration_ids = listaRegistrationIds;
            if (!collapseKey.Equals(""))
            {
                this.collapse_key = collapseKey;
            }
            if (timeToLive > 0)
            {
                this.time_to_live = timeToLive;
            }
            if (delayWhileIdle)
            {
                this.delay_while_idle = delayWhileIdle;
            }
        }
        public GCMMessage(Data data, List<String> listaRegistrationIds)
        {
            this.data = data;
            this.registration_ids = listaRegistrationIds;

        }
        public GCMMessage(Data data, List<String> listaRegistrationIds, String collapseKey)
        {
            this.data = data;
            this.registration_ids = listaRegistrationIds;
            if (!collapseKey.Equals(""))
            {
                this.collapse_key = collapseKey;
            }

        }
    }

    /// 
    /// CLASES PEDIDOS A DROGUERIAS
    /// 
    public class Droguerias
    {
        public short ID_Drogueria { get; set; }
        public string TX_Drogueria { get; set; }
        public bool BO_Activo { get; set; }

        public Droguerias(short _ID_Drogueria, string _TX_Drogueria, bool _BO_Activo)
        {
            this.ID_Drogueria = _ID_Drogueria;
            this.TX_Drogueria = _TX_Drogueria;
            this.BO_Activo = _BO_Activo;
        }

        public Droguerias()
        {
            this.ID_Drogueria = 0;
            this.TX_Drogueria = "";
            this.BO_Activo = true;
        }
    }

    public class Productos
    {
        public short ID_Producto { get; set; }
        public short ID_Marca { get; set; }
        public string TX_Producto { get; set; }
        public string TX_IDProductoCliente { get; set; }
        public string TX_ProductoDesc { get; set; }
        public bool BO_Activo { get; set; }

        public Productos(short _ID_Producto, short _ID_Marca, string _TX_Producto, string _TX_IDProductoCliente,
            string _TX_ProductoDesc, bool _BO_Activo)
        {
            this.ID_Producto = _ID_Producto;
            this.ID_Marca = _ID_Marca;
            this.TX_Producto = _TX_Producto;
            this.TX_IDProductoCliente = _TX_IDProductoCliente;
            this.TX_ProductoDesc = _TX_ProductoDesc;
            this.BO_Activo = _BO_Activo;
        }

        public Productos()
        {
            this.ID_Producto = 0;
            this.ID_Marca = 0;
            this.TX_Producto = "";
            this.TX_IDProductoCliente = "";
            this.TX_ProductoDesc = "";
            this.BO_Activo = true;
        }
    }

    public class DrogueriasAlmacenes
    {
        public short ID_DrogueriaAlmacen { get; set; }
        public short ID_Drogueria { get; set; }
        public string TX_Almacen { get; set; }
        public string TX_IDAlmacen { get; set; }
        public bool BO_Activo { get; set; }

        public DrogueriasAlmacenes(short _ID_DrogueriaAlmacen, short _ID_Drogueria, string _TX_Almacen, string _TX_IDAlmacen,
            bool _BO_Activo)
        {
            this.ID_DrogueriaAlmacen = _ID_DrogueriaAlmacen;
            this.ID_Drogueria = _ID_Drogueria;
            this.TX_Almacen = _TX_Almacen;
            this.TX_IDAlmacen = _TX_IDAlmacen;
            this.BO_Activo = _BO_Activo;
        }

        public DrogueriasAlmacenes()
        {
            this.ID_DrogueriaAlmacen = 0;
            this.ID_Drogueria = 0;
            this.TX_Almacen = "";
            this.TX_IDAlmacen = "";
            this.BO_Activo = true;
        }
    }

    public class DrogueriasProductos
    {
        public int ID_DrogueriaProducto { get; set; }
        public short ID_DrogueriaAlmacen { get; set; }
        public short ID_Producto { get; set; }
        public string TX_IDProductoDrogueria { get; set; }
        public string TX_ProductoDrogueria { get; set; }
        public decimal NU_PrecioProducto { get; set; }
        public int NU_InvProducto { get; set; }
        public string TX_DrogueriaRef1 { get; set; }
        public string TX_DrogueriaRef2 { get; set; }
        public bool BO_Activo { get; set; }

        public DrogueriasProductos(int _ID_DrogueriaProducto, short _ID_DrogueriaAlmacen, short _ID_Producto,
            string _TX_IDProductoDrogueria, string _TX_ProductoDrogueria, decimal _NU_PrecioProducto,
            int _NU_InvProducto, string _TX_DrogueriaRef1, string _TX_DrogueriaRef2, bool _BO_Activo)
        {
            this.ID_DrogueriaProducto = _ID_DrogueriaProducto;
            this.ID_DrogueriaAlmacen = _ID_DrogueriaAlmacen;
            this.ID_Producto = _ID_Producto;
            this.TX_IDProductoDrogueria = _TX_IDProductoDrogueria;
            this.TX_ProductoDrogueria = _TX_ProductoDrogueria;
            this.NU_PrecioProducto = _NU_PrecioProducto;
            this.NU_InvProducto = _NU_InvProducto;
            this.TX_DrogueriaRef1 = _TX_DrogueriaRef1;
            this.TX_DrogueriaRef2 = _TX_DrogueriaRef2;
            this.BO_Activo = _BO_Activo;
        }

        public DrogueriasProductos()
        {
            this.ID_DrogueriaProducto = 0;
            this.ID_DrogueriaAlmacen = 0;
            this.ID_Producto = 0;
            this.TX_IDProductoDrogueria = "";
            this.TX_ProductoDrogueria = "";
            this.NU_PrecioProducto = 0;
            this.NU_InvProducto = 0;
            this.TX_DrogueriaRef1 = "";
            this.TX_DrogueriaRef2 = "";
            this.BO_Activo = true;
        }
    }
    public class PedidoActualizado
    {
        public long ID_PedidoMedinet { get; set; }
        public long ID_Visitador { get; set; }
        public long ID_Pedido { get; set; }
        public int NU_Estatus { get; set; }
        public int NU_EstatusProcesado { get; set; }

        public PedidoActualizado()
        {
            this.ID_PedidoMedinet = 0;
            this.ID_Visitador = 0;
            this.ID_Pedido = 0;
            this.NU_Estatus = 0;
            this.NU_EstatusProcesado = 0;
        }

        public PedidoActualizado(long ID_PedidoMedinet, long ID_Visitador, long ID_Pedido, int NU_Estatus,
                                    int NU_EstatusProcesado)
        {
            this.ID_PedidoMedinet = ID_PedidoMedinet;
            this.ID_Visitador = ID_Visitador;
            this.ID_Pedido = ID_Pedido;
            this.NU_Estatus = NU_Estatus;
            this.NU_EstatusProcesado = NU_EstatusProcesado;
        }

    }

    public class PedidoFacturaCabecera
    {
        public long ID_FacturaMedinet { get; set; }
        public long ID_PedidoMedinet { get; set; }
        public int ID_Drogueria { get; set; }
        public long NU_FacturaDrogueria { get; set; }
        public long NU_PedidoDrogueria { get; set; }
        public DateTime FE_FacturaDrogueria { get; set; }
        public int NU_TotalUnidades { get; set; }
        public decimal NU_CostoTotalFactura { get; set; }
        public DateTime FE_Recibido { get; set; }
        public DateTime FE_Modificado { get; set; }

        public PedidoFacturaCabecera()
        {
            this.ID_FacturaMedinet = 0;
            this.ID_PedidoMedinet = 0;
            this.ID_Drogueria = 0;
            this.NU_FacturaDrogueria = 0;
            this.NU_PedidoDrogueria = 0;
            this.FE_FacturaDrogueria = DateTime.Now;
            this.NU_TotalUnidades = 0;
            this.NU_CostoTotalFactura = 0;
            this.FE_Recibido = DateTime.Now;
            this.FE_Modificado = DateTime.Now;
        }
        public PedidoFacturaCabecera(long ID_FacturaMedinet, long ID_PedidoMedinet, int ID_Drogueria, long NU_FacturaDrogueria,
                                        long NU_PedidoDrogueria, DateTime FE_FacturaDrogueria, int NU_TotalUnidades,
                                        decimal NU_CostoTotalFactura, DateTime FE_Recibido, DateTime FE_Modificado)
        {
            this.ID_FacturaMedinet = ID_FacturaMedinet;
            this.ID_PedidoMedinet = ID_PedidoMedinet;
            this.ID_Drogueria = ID_Drogueria;
            this.NU_FacturaDrogueria = NU_FacturaDrogueria;
            this.NU_PedidoDrogueria = NU_PedidoDrogueria;
            this.FE_FacturaDrogueria = FE_FacturaDrogueria;
            this.NU_TotalUnidades = NU_TotalUnidades;
            this.NU_CostoTotalFactura = NU_CostoTotalFactura;
            this.FE_Recibido = FE_Recibido;
            this.FE_Modificado = FE_Modificado;
        }
    }

    public class PedidoFacturaDetalle
    {
        public long ID_Detalle { get; set; }
        public long ID_FacturaMedinet { get; set; }
        public int ID_Producto { get; set; }
        public string TX_IDProductoDrogueria { get; set; }
        public string TX_Lote { get; set; }
        public long NU_CantidadFacturada { get; set; }
        public DateTime FE_Recibido { get; set; }
        public DateTime FE_Modificado { get; set; }

        public PedidoFacturaDetalle()
        {
            this.ID_Detalle = 0;
            this.ID_FacturaMedinet = 0;
            this.ID_Producto = 0;
            this.TX_IDProductoDrogueria = "";
            this.TX_Lote = "";
            this.NU_CantidadFacturada = 0;
            this.FE_Recibido = DateTime.Now;
            this.FE_Modificado = DateTime.Now;
        }
        public PedidoFacturaDetalle(long ID_Detalle, long ID_FacturaMedinet, int ID_Producto, string TX_IDProductoDrogueria,
                                    string TX_Lote, long NU_CantidadFacturada, DateTime FE_Recibido, DateTime FE_Modificado)
        {
            this.ID_Detalle = ID_Detalle;
            this.ID_FacturaMedinet = ID_FacturaMedinet;
            this.ID_Producto = ID_Producto;
            this.TX_IDProductoDrogueria = TX_IDProductoDrogueria;
            this.TX_Lote = TX_Lote;
            this.NU_CantidadFacturada = NU_CantidadFacturada;
            this.FE_Recibido = FE_Recibido;
            this.FE_Modificado = FE_Modificado;
        }
    }

    public class MedicoReceta
    {
        public string nombre;
        public string domicilio;
        public string localidad;
        public string ciudad;
        public string cdg_region;
        public string region;
        public string cdg_postal;
        public string matricula;
        public string cdg_esp1;
        public string cdg_esp2;
        public string rep;
        public string rep1;
        public string rep2;
        public string rep3;
        public string rep4;
        public string rep5;
        public string rep6;
        public string rep7;
        public string rep8;
        public string rep9;
        public string rep10;
        public string cdg_zonapostal;
        public double categoria;
        public string descripcion;
        public double mercado;
        public string periodo;
        public string prod1;
        public double val1;
        public string prod2;
        public double val2;
        public string prod3;
        public double val3;
        public string prod4;
        public double val4;
        public string prod5;
        public double val5;
        public string prod6;
        public double val6;
        public string prod7;
        public double val7;
        public string prod8;
        public double val8;
        public string prod9;
        public double val9;
        public string prod10;
        public double val10;
        public string prod11;
        public double val11;
        public string prod12;
        public double val12;
        public string prod13;
        public double val13;
        public string prod14;
        public double val14;
        public string prod15;
        public double val15;
        public double cdg_medico;
        public double cdg_lider;
        public string orden;

        public MedicoReceta()
        {
            this.nombre = "";
            this.domicilio = "";
            this.localidad = "";
            this.ciudad = "";
            this.cdg_region = "";
            this.region = "";
            this.cdg_postal = "";
            this.matricula = "";
            this.cdg_esp1 = "";
            this.cdg_esp2 = "";
            this.rep = "";
            this.rep1 = "";
            this.rep2 = "";
            this.rep3 = "";
            this.rep4 = "";
            this.rep5 = "";
            this.rep6 = "";
            this.rep7 = "";
            this.rep8 = "";
            this.rep9 = "";
            this.rep10 = "";
            this.cdg_zonapostal = "";
            this.categoria = 0;
            this.descripcion = "";
            this.mercado = 0;
            this.periodo = "";
            this.prod1 = "";
            this.val1 = 0;
            this.prod2 = "";
            this.val2 = 0;
            this.prod3 = "";
            this.val3 = 0;
            this.prod4 = "";
            this.val4 = 0;
            this.prod5 = "";
            this.val5 = 0;
            this.prod6 = "";
            this.val6 = 0;
            this.prod7 = "";
            this.val7 = 0;
            this.prod8 = "";
            this.val8 = 0;
            this.prod9 = "";
            this.val9 = 0;
            this.prod10 = "";
            this.val10 = 0;
            this.prod11 = "";
            this.val11 = 0;
            this.prod12 = "";
            this.val12 = 0;
            this.prod13 = "";
            this.val13 = 0;
            this.prod14 = "";
            this.val14 = 0;
            this.prod15 = "";
            this.val15 = 0;
            this.cdg_medico = 0;
            this.cdg_lider = 0;
            this.orden = ""; ;
        }

        public MedicoReceta(string nombre,
         string domicilio,
         string localidad,
         string ciudad,
         string cdg_region,
         string region,
         string cdg_postal,
         string matricula,
         string cdg_esp1,
         string cdg_esp2,
         string rep,
         string rep1,
         string rep2,
         string rep3,
         string rep4,
         string rep5,
         string rep6,
         string rep7,
         string rep8,
         string rep9,
         string rep10,
         string cdg_zonapostal,
         double categoria,
         string descripcion,
         double mercado,
         string periodo,
         string prod1,
         double val1,
         string prod2,
         double val2,
         string prod3,
         double val3,
         string prod4,
         double val4,
         string prod5,
         double val5,
         string prod6,
         double val6,
         string prod7,
         double val7,
         string prod8,
         double val8,
         string prod9,
         double val9,
         string prod10,
         double val10,
         string prod11,
         double val11,
         string prod12,
         double val12,
         string prod13,
         double val13,
         string prod14,
         double val14,
         string prod15,
         double val15,
         double cdg_medico,
         double cdg_lider,
         string orden)
        {
            this.nombre = nombre;
            this.domicilio = domicilio;
            this.localidad = localidad;
            this.ciudad = ciudad;
            this.cdg_region = cdg_region;
            this.region = region;
            this.cdg_postal = cdg_postal;
            this.matricula = matricula;
            this.cdg_esp1 = cdg_esp1;
            this.cdg_esp2 = cdg_esp2;
            this.rep = rep;
            this.rep1 = rep1;
            this.rep2 = rep2;
            this.rep3 = rep3;
            this.rep4 = rep4;
            this.rep5 = rep5;
            this.rep6 = rep6;
            this.rep7 = rep7;
            this.rep8 = rep8;
            this.rep9 = rep9;
            this.rep10 = rep10;
            this.cdg_zonapostal = cdg_zonapostal;
            this.categoria = categoria;
            this.descripcion = descripcion;
            this.mercado = mercado;
            this.periodo = periodo;
            this.prod1 = prod1;
            this.val1 = val1;
            this.prod2 = prod2;
            this.val2 = val2;
            this.prod3 = prod3;
            this.val3 = val3;
            this.prod4 = prod4;
            this.val4 = val4;
            this.prod5 = prod5;
            this.val5 = val5;
            this.prod6 = prod6;
            this.val6 = val6;
            this.prod7 = prod7;
            this.val7 = val7;
            this.prod8 = prod8;
            this.val8 = val8;
            this.prod9 = prod9;
            this.val9 = val9;
            this.prod10 = prod10;
            this.val10 = val10;
            this.prod11 = prod11;
            this.val11 = val11;
            this.prod12 = prod12;
            this.val12 = val12;
            this.prod13 = prod13;
            this.val13 = val13;
            this.prod14 = prod14;
            this.val14 = val14;
            this.prod15 = prod15;
            this.val15 = val15;
            this.cdg_medico = cdg_medico;
            this.cdg_lider = cdg_lider;
            this.orden = orden;
        }
    }
    public class VehiculoRegistro
    {
        public string region;
        public string linea;
        public string nombreApellido;
        public string cedula;
        public decimal descontados;
        public decimal ajuste;
        public decimal total;
        public decimal monto;
        public decimal cancelar;
        public string observacion;

        public VehiculoRegistro()
        {
            this.region = "";
            this.linea = "";
            this.nombreApellido = "";
            this.cedula = "";
            this.descontados = 0;
            this.ajuste = 0;
            this.total = 0;
            this.monto = 0;
            this.cancelar = 0;
            this.observacion = "";
        }
        public VehiculoRegistro(string region, string linea, string nombreApellido, string cedula,
         decimal descontados, decimal ajuste, decimal total, decimal monto, decimal cancelar, string observacion)
        {
            this.region = region;
            this.linea = linea;
            this.nombreApellido = nombreApellido;
            this.cedula = cedula;
            this.descontados = descontados;
            this.ajuste = ajuste;
            this.total = total;
            this.monto = monto;
            this.cancelar = cancelar;
            this.observacion = observacion;
        }

    }
}
