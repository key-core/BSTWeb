using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_Marcas
    {
        [Editable(false)]
        public short ID_Marca { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(40, ErrorMessage = "El campo no debe exceder los 40 caracteres.")]
        public string TX_Marca { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Laboratorio { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(200, ErrorMessage = "El campo no debe exceder los 200 caracteres.")]
        public string TX_Posicionamiento { get; set; }

        [Editable(false)]
        public System.DateTime FE_Registro { get; set; }
        public bool BO_Activo { get; set; }
    }
}