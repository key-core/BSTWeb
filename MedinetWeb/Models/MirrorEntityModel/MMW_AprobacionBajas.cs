using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_AprobacionBajas
    {
        [Editable(false)]
        public int Registro { get; set; }

        [Editable(false)]
        public string NroSanitario { get; set; }

        [Editable(false)]
        public string Medico { get; set; }

        [Editable(false)]
        public string Clinica { get; set; }

        [Editable(false)]
        public short ID_Clasificacion { get; set; }

        [Editable(false)]
        public short ID_Especialidad { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public int Estatus { get; set; }

        [Editable(false)]
        public Nullable<bool> BO_GoogleID { get; set; }
    }
}