using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_Especialidades
    {
        [Editable(false)]
        public short ID_Especialidad { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(25, ErrorMessage = "El campo no debe exceder los 25 caracteres.")]
        public string TX_Especialidad { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(6, ErrorMessage = "El campo no debe exceder los 6 caracteres.")]
        public string TX_EspecialidadAbr { get; set; }
        public bool BO_Activo { get; set; }
    }
}