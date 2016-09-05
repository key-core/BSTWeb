using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_Instituciones
    {
        [Editable(false)]
        public short ID_Institucion { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(20, ErrorMessage = "El campo no debe exceder los 20 caracteres.")]
        public string TX_Institucion { get; set; }
        public bool BO_Activo { get; set; }
    }
}