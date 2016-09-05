using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_DrogueriasProductos
    {
        [Editable(false)]
        public int ID_DrogueriaProducto { get; set; }

        [Editable(false)]
        public short ID_DrogueriaAlmacen { get; set; }
        public Nullable<short> ID_Producto { get; set; }

        [Editable(false)]
        public string TX_IDProductoDrogueria { get; set; }

        [Editable(false)]
        public string TX_ProductoDrogueria { get; set; }
        
        [Editable(false)]
        public decimal NU_PrecioProducto { get; set; }

        [Editable(false)]
        public int NU_InvProducto { get; set; }

        [Editable(false)]
        public string TX_DrogueriaRef1 { get; set; }

        [Editable(false)]
        public string TX_DrogueriaRef2 { get; set; }

        [Editable(false)]
        public bool BO_Activo { get; set; }
    }
}