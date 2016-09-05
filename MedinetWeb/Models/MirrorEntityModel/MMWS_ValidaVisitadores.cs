using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMWS_ValidaVisitadores
    {
        [Editable(false)]
        public long ID_ValidaVisitadores { get; set; }

        [Editable(false)]
        public short ID_CicloDistribucion { get; set; }

        [Editable(false)]
        public long ID_VisitadoresHistorial { get; set; }

        [Editable(false)]
        public short ID_Visitador { get; set; }
        
        [Editable(false)]
        public short ID_Region { get; set; }
        
        [Editable(false)]
        public short ID_Linea { get; set; }
        public bool BO_Activo { get; set; }
    }
}