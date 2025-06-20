using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WALLY_PROJECT.Models
{
    public class Transaccion
    {
        public int ID_TRANSACCION { get; set; }
        public decimal T_MONTO { get; set; }
        public DateTime T_FEC_TRANSACCION { get; set; }
        public int T_CUENTA_ORIGEN { get; set; }
        public int T_CUENTA_DESTINO { get; set; }
        public char T_ESTADO_TRANSACCION { get; set; }
        public string T_DETALLE { get; set; }
    }
}