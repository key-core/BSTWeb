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
    
    public partial class SP_MWS_ResumenCierreDistribucion_CieDis_Result
    {
        public short ID_Marca { get; set; }
        public int ID_Articulo { get; set; }
        public short ID_TipoArticulo { get; set; }
        public Nullable<int> StockInicial { get; set; }
        public Nullable<int> Dispensado { get; set; }
        public Nullable<int> StockFinal { get; set; }
    }
}
