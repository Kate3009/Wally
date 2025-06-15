using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WALLY_PROJECT.Models
{
    public class Transaccion
    {
        public int IdTransaccion { get; set; }
        public decimal T_Monto { get; set; }
        public DateTime T_FecTransaccion { get; set; }
        public int T_CuentaOrigen { get; set; }
        public int T_CuentaDestino { get; set; }
        public char T_EstadoTransaccion { get; set; }
        public string T_Detalle { get; set; }
    }
}