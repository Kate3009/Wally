using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WALLY_PROJECT.Models
{
    public class Cuenta
    {
        public int ID_CUENTA { get; set; }
        public int ID_USUARIO { get; set; }
        public string C_NUMERO_CUENTA { get; set; }
        public decimal C_SALDO { get; set; }
        public char C_ESTADO_CUENTA { get; set; }

    }
}