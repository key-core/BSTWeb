using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMWS_Configuraciones
    {
        [Editable(false)]
        public short ID_Configuracion { get; set; }

        //[Range(0, 1, ErrorMessage = "Valor Fuera de rango.")]
        //public decimal NU_UnidadRedondeo { get; set; }
        public double NU_UnidadRedondeo { get; set; }
        
        public bool BO_PorUnidadManejo { get; set; }
        public bool BO_ForzarUnidad { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_DiasVigencia { get; set; }
    }
}