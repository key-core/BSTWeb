using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_DescuentosUsuarios
    {
        [Editable(false)]
        public int ID_DescuentosUsuarios { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Ciclo { get; set; }

        [Editable(false)]
        public int ID_Usuario { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_ConceptoDescuento { get; set; }

        [DataType(DataType.Date)]
        public System.DateTime FE_DescuentoIni { get; set; }
        
        [DataType(DataType.Date)]
        public System.DateTime FE_DescuentoFin { get; set; }
        public bool BO_MedioDia { get; set; }
        public decimal NU_TotalDescuento { get; set; }

        [StringLength(50, ErrorMessage = "El campo no debe exceder los 50 caracteres.")]
        public string TX_Comentario { get; set; }
    }
}