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
    
    public partial class MWS_NoContFichero
    {
        public short ID_CicloDistribucion { get; set; }
        public int ID_Articulo { get; set; }
        public int NU_Cantidad { get; set; }
    
        public virtual MW_Ciclos MW_Ciclos { get; set; }
        public virtual MWS_Articulos MWS_Articulos { get; set; }
    }
}
