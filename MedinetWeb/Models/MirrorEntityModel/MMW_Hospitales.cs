using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_Hospitales
    {
        [Editable(false)]
        public short ID_Hospital { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(80, ErrorMessage = "El campo no debe exceder los 80 caracteres.")]
        public string TX_Hospital { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Estado { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Brick { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Institucion { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(50, ErrorMessage = "El campo no debe exceder los 50 caracteres.")]
        public string TX_Ruta { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(70, ErrorMessage = "El campo no debe exceder los 70 caracteres.")]
        public string TX_Direccion { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(20, ErrorMessage = "El campo no debe exceder los 20 caracteres.")]
        public string TX_Telefono1 { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(20, ErrorMessage = "El campo no debe exceder los 20 caracteres.")]
        public string TX_Telefono2 { get; set; }
        public bool BO_Cliente { get; set; }
        public bool BO_Docente { get; set; }

        [Editable(false)]
        public System.DateTime FE_Registro { get; set; }
        public bool BO_Activo { get; set; }
    }
}