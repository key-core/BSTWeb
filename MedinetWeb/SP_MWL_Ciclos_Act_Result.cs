//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedinetWeb
{
    using System;
    
    public partial class SP_MWL_Ciclos_Act_Result
    {
        public short ID_Ciclo { get; set; }
        public string TX_Ciclo { get; set; }
        public short NU_Ano { get; set; }
        public short NU_Ciclo { get; set; }
        public System.DateTime FE_CicloIni { get; set; }
        public System.DateTime FE_CicloFin { get; set; }
        public System.DateTime FE_CicloProrroga { get; set; }
        public short NU_DiasCancelarVehiculo { get; set; }
        public short NU_DiasHabiles { get; set; }
        public Nullable<System.DateTime> FE_CargaInv { get; set; }
        public Nullable<System.DateTime> FE_IniDistribucion { get; set; }
        public Nullable<System.DateTime> FE_FinDistribucion { get; set; }
        public short NU_Estatus { get; set; }
        public bool BO_Activo { get; set; }
    }
}
