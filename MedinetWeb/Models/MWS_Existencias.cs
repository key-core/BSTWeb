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
    using System.Collections.Generic;
    
    public partial class MWS_Existencias
    {
        public int ID_Articulo { get; set; }
        public short ID_Ciclo { get; set; }
        public string TX_Lote { get; set; }
        public string TX_Ubicacion { get; set; }
        public Nullable<System.DateTime> FE_VencimientoLote { get; set; }
        public int NU_CantidadPrimaria { get; set; }
        public int NU_CantidadSecundaria { get; set; }
        public decimal NU_UltimoCosto { get; set; }
    
        public virtual MW_Ciclos MW_Ciclos { get; set; }
        public virtual MWS_Articulos MWS_Articulos { get; set; }
    }
}