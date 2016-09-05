using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_MarcasLineas
    {
        [Editable(false)]
        public short ID_MarcasLineas { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Marca { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Linea { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_CicloIni { get; set; }
        public Nullable<short> ID_CicloFin { get; set; }
        public bool BO_Activo { get; set; }
    }
}