using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_Publicaciones
    {
        [Editable(false)]
        public short ID_Publicacion { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(100, ErrorMessage = "El campo no debe exceder los 100 caracteres.")]
        public string TX_Publicacion { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(50, ErrorMessage = "El campo no debe exceder los 50 caracteres.")]
        [UIHint("ArchivoPublicaciones")]
        public string TX_Archivo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Marca { get; set; }

        [Editable(false)]
        public System.DateTime FE_Registro { get; set; }
        public bool BO_Activo { get; set; }
    }
}