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
    
    public partial class SP_MWS_NotaEntregaDetalle_NotEnt_Result
    {
        public long ID_NotaEntrega { get; set; }
        public int ID_Articulo { get; set; }
        public string TX_Articulo { get; set; }
        public string TX_Lote { get; set; }
        public string TX_Ubicacion { get; set; }
        public short NU_UnidadEntregar { get; set; }
        public decimal NU_UMEntregar { get; set; }
        public decimal NU_Costo { get; set; }
        public Nullable<decimal> NU_Total { get; set; }
    }
}
