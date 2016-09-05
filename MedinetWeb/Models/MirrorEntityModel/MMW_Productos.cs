using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_Productos
    {
        [Editable(false)]
        public short ID_Producto { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Marca { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(15, ErrorMessage = "El campo no debe exceder los 15 caracteres.")]
        public string TX_IDProductoCliente { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(150, ErrorMessage = "El campo no debe exceder los 150 caracteres.")]
        public string TX_Producto { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(250, ErrorMessage = "El campo no debe exceder los 250 caracteres.")]
        public string TX_ProductoDesc { get; set; }
        public bool BO_Activo { get; set; }
    }
}