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
    
    public partial class MW_LocacionEstados
    {
        public MW_LocacionEstados()
        {
            this.MW_Farmacias = new HashSet<MW_Farmacias>();
            this.MW_Hospitales = new HashSet<MW_Hospitales>();
            this.MW_LocacionBricks = new HashSet<MW_LocacionBricks>();
            this.MW_RegionesEstados = new HashSet<MW_RegionesEstados>();
            this.MW_Usuarios = new HashSet<MW_Usuarios>();
        }
    
        public short ID_Estado { get; set; }
        public string TX_Estado { get; set; }
        public bool BO_Activo { get; set; }
        public short ID_Pais { get; set; }
    
        public virtual ICollection<MW_Farmacias> MW_Farmacias { get; set; }
        public virtual ICollection<MW_Hospitales> MW_Hospitales { get; set; }
        public virtual ICollection<MW_LocacionBricks> MW_LocacionBricks { get; set; }
        public virtual ICollection<MW_RegionesEstados> MW_RegionesEstados { get; set; }
        public virtual MW_LocacionPaises MW_LocacionPaises { get; set; }
        public virtual ICollection<MW_Usuarios> MW_Usuarios { get; set; }
    }
}