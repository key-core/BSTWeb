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
    
    public partial class MW_MarcasLineas
    {
        public short ID_MarcasLineas { get; set; }
        public short ID_Marca { get; set; }
        public short ID_Linea { get; set; }
        public short ID_CicloIni { get; set; }
        public Nullable<short> ID_CicloFin { get; set; }
        public bool BO_Activo { get; set; }
    
        public virtual MW_Lineas MW_Lineas { get; set; }
        public virtual MW_Marcas MW_Marcas { get; set; }
        public virtual MW_Ciclos MW_Ciclos { get; set; }
        public virtual MW_Ciclos MW_Ciclos1 { get; set; }
    }
}
