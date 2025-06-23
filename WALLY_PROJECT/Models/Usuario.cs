using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WALLY_PROJECT.Models
{
    public class Usuario
    {
        [Display(Name = "Código")]
        public int ID_USUARIO { get; set; }
        [Display(Name = "Usuario")]
        public string U_TXT_USUARIO { get; set; }
        [Display(Name = "Contraseña")]
        public string U_TXT_ACCESO { get; set; }
        [Display(Name = "Fecha de Creación")]
        public DateTime? U_FEC_CREACION { get; set; }
        [Display(Name = "Fecha de Modificación")]
        public DateTime? U_FEC_MODIFICACION { get; set; }
        [Display(Name = "Nombres")]
        public string U_NOMBRES { get; set; }
        [Display(Name = "Apellido Paterno")]
        public string U_APE_PATERNO { get; set; }
        [Display(Name = "Aepllido Materno")]
        public string U_APE_MATERNO { get; set; }
        [Display(Name = "Email")]
        public string U_EMAIL { get; set; }
        [Display(Name = "DNI")]
        public string U_NUM_IDENTIDAD { get; set; }
        [Display(Name = "Perfil")]
        public string U_PERFIL { get; set; } // ADMIN, CLIENTE
        [Display(Name = "Estado")]
        public string U_ESTADO { get; set; }

        

    }
}