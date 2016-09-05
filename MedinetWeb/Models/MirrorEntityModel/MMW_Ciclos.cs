using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_Ciclos
    {
        [Editable(false)]
        public short ID_Ciclo { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_Ano { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_Ciclo { get; set; }

        [DataType(DataType.Date)]
        public System.DateTime FE_CicloIni { get; set; }

        [DataType(DataType.Date)]
        public System.DateTime FE_CicloFin { get; set; }

        [DataType(DataType.Date)]
        public System.DateTime FE_CicloProrroga { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_DiasCancelarVehiculo { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_DiasHabiles { get; set; }

        [DataType(DataType.Date)]
        public System.DateTime FE_CargaInv { get; set; }

        [DataType(DataType.Date)]
        public System.DateTime FE_IniDistribucion { get; set; }

        [DataType(DataType.Date)]
        public System.DateTime FE_FinDistribucion { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_Estatus { get; set; }
        public bool BO_Activo { get; set; }
    }
}