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
    
    public partial class MWS_CierreDistribucion
    {
        public int ID_CierreDistribucion { get; set; }
        public short ID_Ciclo { get; set; }
        public int ID_Usuario { get; set; }
        public System.DateTime FE_Registro { get; set; }
    
        public virtual MW_Ciclos MW_Ciclos { get; set; }
        public virtual MW_Usuarios MW_Usuarios { get; set; }
    }
}
