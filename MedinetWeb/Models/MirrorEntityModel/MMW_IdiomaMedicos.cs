﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_IdiomaMedicos
    {
        [Editable(false)]
        public short ID_Idioma { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(15, ErrorMessage = "El campo no debe exceder los 15 caracteres.")]
        public string TX_Idioma { get; set; }
        public bool BO_Activo { get; set; }
    }
}