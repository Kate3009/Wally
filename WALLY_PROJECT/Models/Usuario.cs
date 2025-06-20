using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WALLY_PROJECT.Models
{
    public class Usuario
    {
        public int ID_USUARIO { get; set; }
        public string U_TXT_USUARIO { get; set; }
            public string U_TXT_ACCESO { get; set; }
        public DateTime? U_FEC_CREACION { get; set; }
            public DateTime? U_FEC_MODIFICACION { get; set; }
        public string U_NOMBRES { get; set; }
        public string U_APE_PATERNO { get; set; }
        public string U_APE_MATERNO { get; set; }
        public string U_EMAIL { get; set; }
            public string U_NUM_IDENTIDAD { get; set; } 
            public string U_PERFIL { get; set; } // ADMIN, CLIENTE
            public string U_ESTADO { get; set; }

        

    }
}