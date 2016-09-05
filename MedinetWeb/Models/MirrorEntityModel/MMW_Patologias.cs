using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_Patologias
    {
        [Editable(false)]
        public short ID_Patologia { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(40, ErrorMessage = "El campo no debe exceder los 40 caracteres.")]
        public string TX_Patologia { get; set; }
        public bool BO_Activo { get; set; }
    }
}