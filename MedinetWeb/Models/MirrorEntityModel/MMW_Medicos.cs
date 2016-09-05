using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_Medicos
    {
        [Editable(false)]
        public int ID_Medico { get; set; }

        [Range(1, Int32.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public int NU_RegistroSanitario { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(20, ErrorMessage = "El campo no debe exceder los 20 caracteres.")]
        public string TX_Nombre1 { get; set; }

        [StringLength(20, ErrorMessage = "El campo no debe exceder los 20 caracteres.")]
        public string TX_Nombre2 { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(20, ErrorMessage = "El campo no debe exceder los 20 caracteres.")]
        public string TX_Apellido1 { get; set; }

        [StringLength(20, ErrorMessage = "El campo no debe exceder los 20 caracteres.")]
        public string TX_Apellido2 { get; set; }

        [StringLength(50, ErrorMessage = "El campo no debe exceder los 50 caracteres.")]
        [UIHint("ImageSelloMedicos")]
        public string TX_Sello { get; set; }
        public bool BO_Activo { get; set; }
    }
}