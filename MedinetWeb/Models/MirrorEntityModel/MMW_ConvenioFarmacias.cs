using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_ConvenioFarmacias
    {
        [Editable(false)]
        public short ID_Convenio { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(25, ErrorMessage = "El campo no debe exceder los 25 caracteres.")]
        public string TX_Convenio { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Region { get; set; }
        public bool BO_Activo { get; set; }
    }
}