using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_Umbrales
    {
        [Editable(false)]
        public short ID_Umbral { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_PDR { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_Farmacias { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_Hospitales { get; set; }

        [Range(1, 100, ErrorMessage = "Valor Fuera de rango.")]
        public decimal NU_Percent_PDR { get; set; }

        [Range(1, 100, ErrorMessage = "Valor Fuera de rango.")]
        public decimal NU_Percent_Visita { get; set; }

        [Range(1, 100, ErrorMessage = "Valor Fuera de rango.")]
        public decimal NU_Percent_Revisita { get; set; }

        [Range(1, 100, ErrorMessage = "Valor Fuera de rango.")]
        public decimal NU_Percent_Mix { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_CicloIni { get; set; }
        public Nullable<short> ID_CicloFin { get; set; }
        public bool BO_Activo { get; set; }
    }
}