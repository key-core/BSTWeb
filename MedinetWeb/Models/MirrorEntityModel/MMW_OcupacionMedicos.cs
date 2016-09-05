using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_OcupacionMedicos
    {
        [Editable(false)]
        public short ID_Ocupacion { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(100, ErrorMessage = "El campo no debe exceder los 100 caracteres.")]
        public string TX_Ocupacion { get; set; }
        public bool BO_Activo { get; set; }
    }
}