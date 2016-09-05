using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_DrogueriasAlmacenes
    {
        [Editable(false)]
        public short ID_DrogueriaAlmacen { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Drogueria { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(30, ErrorMessage = "El campo no debe exceder los 30 caracteres.")]
        public string TX_Almacen { get; set; }

        [StringLength(10, ErrorMessage = "El campo no debe exceder los 10 caracteres.")]
        public string TX_IDAlmacen { get; set; }
        public bool BO_Activo { get; set; }
    }
}