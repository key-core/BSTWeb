using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_LocacionBricks
    {
        [Editable(false)]
        public short ID_Brick { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_Brick { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(50, ErrorMessage = "El campo no debe exceder los 50 caracteres.")]
        public string TX_Brick { get; set; }
        public bool BO_Activo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Estado { get; set; }
    }
}