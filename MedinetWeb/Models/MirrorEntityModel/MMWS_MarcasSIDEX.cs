using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMWS_MarcasSIDEX
    {
        [Editable(false)]
        public short ID_MarcaSIDEX { get; set; }
        public Nullable<short> ID_MarcaMedinet { get; set; }

        [Editable(false)]
        public string TX_IDMarcaCliente { get; set; }

        [Editable(false)]
        public string TX_Marca { get; set; }

        [Editable(false)]
        public bool BO_Activo { get; set; }
    }
}