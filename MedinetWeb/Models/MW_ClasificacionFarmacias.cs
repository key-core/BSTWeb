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
    
    public partial class MW_ClasificacionFarmacias
    {
        public MW_ClasificacionFarmacias()
        {
            this.MW_Farmacias = new HashSet<MW_Farmacias>();
        }
    
        public short ID_Clasificacion { get; set; }
        public string TX_Clasificacion { get; set; }
        public bool BO_Activo { get; set; }
    
        public virtual ICollection<MW_Farmacias> MW_Farmacias { get; set; }
    }
}
