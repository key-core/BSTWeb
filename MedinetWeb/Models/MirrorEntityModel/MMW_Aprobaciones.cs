using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models.MirrorEntityModel
{
    public class MMW_Aprobaciones
    {
        [Editable(false)]
        public long ID_Aprobacion { get; set; }

        [Editable(false)]
        public short ID_Ciclo { get; set; }

        [Editable(false)]
        public short ID_Region { get; set; }

        [Editable(false)]
        public short ID_Linea { get; set; }

        [Editable(false)]
        public bool BO_CierreCiclo { get; set; }

        [Editable(false)]
        public long ID_VisitadoresHistorial { get; set; }

        [Editable(false)]
        public System.DateTime FE_Transmision { get; set; }

        [Editable(false)]
        public decimal NU_PorcentajeVisita { get; set; }

        [Editable(false)]
        public decimal NU_PorcentajeRevisita { get; set; }

        [Editable(false)]
        public decimal NU_MIX { get; set; }

        [Editable(false)]
        public decimal NU_PDR { get; set; }

        [Editable(false)]
        public decimal NU_PP { get; set; }

        [Editable(false)]
        public int NU_AltasAprob { get; set; }

        [Editable(false)]
        public int NU_BajasAprob { get; set; }

        [Editable(false)]
        public decimal NU_PorcentajeVisitaFarmacia { get; set; }

        [Editable(false)]
        public decimal NU_PorcentajeVisitaHospital { get; set; }

        [Editable(false)]
        public Nullable<double> NU_DCS { get; set; }

        [Editable(false)]
        public Nullable<double> NU_DCN { get; set; }

        [Editable(false)]
        public Nullable<double> NU_DVN { get; set; }
        
        //[Range(0, double.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public Nullable<double> NU_Total { get; set; }

        //[Range(0, double.MaxValue, ErrorMessage = "Valor Fuera de rango.")]
        public Nullable<decimal> NU_Ajuste { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(30, ErrorMessage = "El campo no debe exceder los 30 caracteres.")]
        public string TX_Observacion { get; set; }
    }
}