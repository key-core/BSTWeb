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
    
    public partial class MW_Umbrales
    {
        public short ID_Umbral { get; set; }
        public short NU_PDR { get; set; }
        public short NU_Farmacias { get; set; }
        public short NU_Hospitales { get; set; }
        public decimal NU_Percent_PDR { get; set; }
        public decimal NU_Percent_Visita { get; set; }
        public decimal NU_Percent_Revisita { get; set; }
        public decimal NU_Percent_Mix { get; set; }
        public short ID_CicloIni { get; set; }
        public Nullable<short> ID_CicloFin { get; set; }
        public bool BO_Activo { get; set; }
    
        public virtual MW_Ciclos MW_Ciclos { get; set; }
        public virtual MW_Ciclos MW_Ciclos1 { get; set; }
    }
}