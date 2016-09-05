using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMWS_CierreDistribucion
    {
        [Editable(false)]
        public int ID_CierreDistribucion { get; set; }

        [Editable(false)]
        public short ID_Ciclo { get; set; }

        [Editable(false)]
        public int ID_Usuario { get; set; }

        [Editable(false)]
        public System.DateTime FE_Registro { get; set; }

        public virtual MW_Ciclos MW_Ciclos { get; set; }
        public virtual MW_Usuarios MW_Usuarios { get; set; }
    }
}