using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_Usuarios
    {
        [Editable(false)]
        public int ID_Usuario { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(15, ErrorMessage = "El campo no debe exceder los 15 caracteres.")]
        public string TX_Usuario { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(50, ErrorMessage = "El campo no debe exceder los 50 caracteres.")]
        public string TX_Clave { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public int NU_Cedula { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(20, ErrorMessage = "El campo no debe exceder los 20 caracteres.")]
        public string TX_Nombre { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(20, ErrorMessage = "El campo no debe exceder los 20 caracteres.")]
        public string TX_Apellido { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Cargo { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(50, ErrorMessage = "El campo no debe exceder los 50 caracteres.")]
        public string TX_Email1 { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "El campo no debe exceder los 50 caracteres.")]
        public string TX_Email2 { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(20, ErrorMessage = "El campo no debe exceder los 20 caracteres.")]
        public string TX_Telefono1 { get; set; }

        [StringLength(20, ErrorMessage = "El campo no debe exceder los 20 caracteres.")]
        public string TX_Telefono2 { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(150, ErrorMessage = "El campo no debe exceder los 150 caracteres.")]
        public string TX_Direccion { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Estado { get; set; }

        [StringLength(15, ErrorMessage = "El campo no debe exceder los 15 caracteres.")]
        public string TX_Imei { get; set; }

        public bool BO_Expiro { get; set; }

        [Editable(false)]
        public System.DateTime FE_Registro { get; set; }
        public bool BO_Activo { get; set; }
    }
}