//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedinetWeb.Models
{
    using System;
    
    public partial class SP_MWS_Existencias_ConDis_Result
    {
        public int ID_Articulo { get; set; }
        public int NU_UnidadManejo { get; set; }
        public short ID_Ciclo { get; set; }
        public string TX_Lote { get; set; }
        public string TX_Ubicacion { get; set; }
        public Nullable<System.DateTime> FE_VencimientoLote { get; set; }
        public int NU_CantidadPrimaria { get; set; }
        public int NU_CantidadSecundaria { get; set; }
        public decimal NU_UltimoCosto { get; set; }
    }
}
