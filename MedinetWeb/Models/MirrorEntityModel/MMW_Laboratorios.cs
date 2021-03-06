﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_Laboratorios
    {
        [Editable(false)]
        public short ID_Laboratorio { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(40, ErrorMessage = "El campo no debe exceder los 40 caracteres.")]
        public string TX_Laboratorio { get; set; }
        public bool BO_LaboratorioMedinet { get; set; }
        public bool BO_Activo { get; set; }
    }
}