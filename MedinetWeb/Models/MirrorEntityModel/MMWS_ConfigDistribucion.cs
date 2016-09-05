using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMWS_ConfigDistribucion
    {
        [Editable(false)]
        public long ID_ConfigDistribucion { get; set; }

        [Editable(false)]
        public short ID_Ciclo { get; set; }

        [Editable(false)]
        public int ID_Articulo { get; set; }

        [Editable(false)]
        public short ID_Linea { get; set; }

        [Editable(false)]
        public short ID_Especialidad { get; set; }

        [Editable(false)]
        public short ID_Clasificacion { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [Range(0, Int16.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public short NU_CantidadEntregar { get; set; }

        [Editable(false)]
        public int NU_Fichados { get; set; }

        [Editable(false)]
        public int NU_Revisita { get; set; }

        [Editable(false)]
        public int NU_Contactos { get; set; }
    }
}