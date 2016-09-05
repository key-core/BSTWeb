using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_LaboratoriosDroguerias
    {
        [Editable(false)]
        public byte ID_LaboratorioDrogueria { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Laboratorio { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Drogueria { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(250, ErrorMessage = "El campo no debe exceder los 250 caracteres.")]
        public string TX_Ruta { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(30, ErrorMessage = "El campo no debe exceder los 30 caracteres.")]
        public string TX_Usuario { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(30, ErrorMessage = "El campo no debe exceder los 30 caracteres.")]
        public string TX_Password { get; set; }
        public bool BO_Activo { get; set; }
    }
}