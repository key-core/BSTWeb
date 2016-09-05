using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_UsuariosResponsabilidades
    {
        [Editable(false)]
        public int ID_UsuariosResponsabilidades { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public int ID_Usuario { get; set; }
        public Nullable<short> ID_Region { get; set; }
        public Nullable<short> ID_Marca { get; set; }
        public Nullable<short> ID_Linea { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_CicloIni { get; set; }
        public Nullable<short> ID_CicloFin { get; set; }

        [Editable(false)]
        public System.DateTime FE_Registro { get; set; }
        public bool BO_Activo { get; set; }
    }
}