using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WALLY_PROJECT.Models
{
    public class Cuenta
    {
        [Display(Name = "Código")]
        public int ID_CUENTA { get; set; }
        [Display(Name = "Usuario")]
        public int ID_USUARIO { get; set; }
        [Display(Name = "Número de Cuenta")]
        public string C_NUMERO_CUENTA { get; set; }
        [Display(Name = "Saldo")]
        public decimal C_SALDO { get; set; }
        [Display(Name = "Estado")]
        public char C_ESTADO_CUENTA { get; set; }

    }
}