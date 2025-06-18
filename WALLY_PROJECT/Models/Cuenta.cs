using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WALLY_PROJECT.Models
{
    public class Cuenta
    {
        public int IdCuenta { get; set; }
        public int IdUsuario { get; set; }
        public string C_NUmeroCuenta { get; set; }
        public decimal C_Saldo { get; set; }
        public char C_EstadoCuenta { get; set; }

    }
}