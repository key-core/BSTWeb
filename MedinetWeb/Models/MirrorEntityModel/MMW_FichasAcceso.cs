using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_FichasAcceso
    {
        [Editable(false)]
        public short ID_FichaAcceso { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public int ID_Usuario { get; set; }

        [Range(10, 99, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_M1 { get; set; }

        [Range(10, 99, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_M2 { get; set; }

        [Range(10, 99, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_M3 { get; set; }

        [Range(10, 99, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_D1 { get; set; }

        [Range(10, 99, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_D2 { get; set; }

        [Range(10, 99, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_D3 { get; set; }

        [Range(10, 99, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_N1 { get; set; }

        [Range(10, 99, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_N2 { get; set; }

        [Range(10, 99, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_N3 { get; set; }

        [Editable(false)]
        public System.DateTime FE_Registro { get; set; }
        public bool BO_Activo { get; set; }
    }
}