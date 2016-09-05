using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_DeporteMedicos
    {
        [Editable(false)]
        public short ID_Deporte { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(25, ErrorMessage = "El campo no debe exceder los 25 caracteres.")]
        public string TX_Deporte { get; set; }
        public bool BO_Activo { get; set; }
    }
}