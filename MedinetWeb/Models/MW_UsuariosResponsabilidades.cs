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
    
    public partial class MW_UsuariosResponsabilidades
    {
        public int ID_UsuariosResponsabilidades { get; set; }
        public int ID_Usuario { get; set; }
        public Nullable<short> ID_Region { get; set; }
        public Nullable<short> ID_Marca { get; set; }
        public Nullable<short> ID_Linea { get; set; }
        public short ID_CicloIni { get; set; }
        public Nullable<short> ID_CicloFin { get; set; }
        public System.DateTime FE_Registro { get; set; }
        public bool BO_Activo { get; set; }
    
        public virtual MW_Ciclos MW_Ciclos { get; set; }
        public virtual MW_Ciclos MW_Ciclos1 { get; set; }
        public virtual MW_Lineas MW_Lineas { get; set; }
        public virtual MW_Marcas MW_Marcas { get; set; }
        public virtual MW_Regiones MW_Regiones { get; set; }
        public virtual MW_Usuarios MW_Usuarios { get; set; }
    }
}
