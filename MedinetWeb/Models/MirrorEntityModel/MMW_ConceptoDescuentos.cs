using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_ConceptoDescuentos
    {
        [Editable(false)]
        public short ID_ConceptoDescuento { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(30, ErrorMessage = "El campo no debe exceder los 30 caracteres.")]
        public string TX_ConceptoDescuento { get; set; }
        public bool BO_AplicaVehiculo { get; set; }
        public bool BO_AplicaCobertura { get; set; }
        public bool BO_Activo { get; set; }
    }
}