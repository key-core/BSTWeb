using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_ClasificacionMedicos
    {
        [Editable(false)]
        public short ID_Clasificacion { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(2, ErrorMessage = "El campo no debe exceder los 2 caracteres.")]
        public string TX_Clasificacion { get; set; }

        [Range(1, Int16.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_Puntaje { get; set; }
        public bool BO_Activo { get; set; }
    }
}