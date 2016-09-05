using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_MarcasCompetencias
    {
        [Editable(false)]
        public short ID_MarcasCompentencia { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Marca { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe seleccionar un item.")]
        public short ID_Competencia { get; set; }
    }
}