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
    
    public partial class MW_UsuariosRoles
    {
        public int ID_UsuariosRoles { get; set; }
        public int ID_Usuario { get; set; }
        public short ID_Rol { get; set; }
    
        public virtual MW_Roles MW_Roles { get; set; }
        public virtual MW_Usuarios MW_Usuarios { get; set; }
    }
}