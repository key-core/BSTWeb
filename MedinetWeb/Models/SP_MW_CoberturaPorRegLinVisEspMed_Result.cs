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
    
    public partial class SP_MW_CoberturaPorRegLinVisEspMed_Result
    {
        public short ID_Ciclo { get; set; }
        public short ID_Region { get; set; }
        public short ID_Linea { get; set; }
        public int ID_Usuario { get; set; }
        public short ID_Especialidad { get; set; }
        public string Medico { get; set; }
        public Nullable<int> Fichados { get; set; }
        public Nullable<int> Visita { get; set; }
        public Nullable<decimal> PCJVisita { get; set; }
        public Nullable<int> REVisita { get; set; }
        public Nullable<int> REVisitado { get; set; }
        public Nullable<decimal> PCJREVisita { get; set; }
        public Nullable<int> FmasR { get; set; }
        public Nullable<int> VmasR { get; set; }
        public Nullable<decimal> PCJFVRR { get; set; }
        public Nullable<decimal> Proyeccion { get; set; }
    }
}
