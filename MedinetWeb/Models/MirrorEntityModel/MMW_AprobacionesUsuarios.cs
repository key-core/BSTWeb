using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_AprobacionesUsuarios
    {
        [Editable(false)]
        public int ID_AprobUsuarios { get; set; }

        [Editable(false)]
        public short ID_Ciclo { get; set; }

        [Editable(false)]
        public int ID_Usuario { get; set; }

        [Editable(false)]
        public System.DateTime FE_Registro { get; set; }
    }
}