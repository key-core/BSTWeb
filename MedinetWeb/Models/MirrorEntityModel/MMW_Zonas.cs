using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_Zonas
    {
        [Editable(false)]
        public short ID_Zona { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Region { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Linea { get; set; }

        [StringLength(10, ErrorMessage = "El campo no debe exceder los 10 caracteres.")]
        public string TX_Zona { get; set; }
        public bool BO_Activo { get; set; }
    }
}