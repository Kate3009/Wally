using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WALLY_PROJECT.Models
{
    public class Transaccion
    {
        [Display(Name = "Código")]
        public int ID_TRANSACCION { get; set; }
        [Display(Name = "Monto")]
        public decimal T_MONTO { get; set; }
        [Display(Name = "Fecha")]
        public DateTime T_FEC_TRANSACCION { get; set; }
        [Display(Name = "Cuenta Origen")]
        public string T_CUENTA_ORIGEN { get; set; }
        [Display(Name = "Cuenta Destino")]
        public string T_CUENTA_DESTINO { get; set; }
        [Display(Name = "Estado")]
        public string T_ESTADO_TRANSACCION { get; set; }
        [Display(Name = "Detalle")]
        public string T_DETALLE { get; set; }
    }
}