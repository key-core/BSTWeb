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
    
    public partial class MW_FichasAcceso
    {
        public short ID_FichaAcceso { get; set; }
        public int ID_Usuario { get; set; }
        public short NU_M1 { get; set; }
        public short NU_M2 { get; set; }
        public short NU_M3 { get; set; }
        public short NU_D1 { get; set; }
        public short NU_D2 { get; set; }
        public short NU_D3 { get; set; }
        public short NU_N1 { get; set; }
        public short NU_N2 { get; set; }
        public short NU_N3 { get; set; }
        public System.DateTime FE_Registro { get; set; }
        public bool BO_Activo { get; set; }
    
        public virtual MW_Usuarios MW_Usuarios { get; set; }
    }
}
