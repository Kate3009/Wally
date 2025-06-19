using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WALLY_PROJECT.Models
{
    public class Usuario
    {
            public int IdUsuario { get; set; }
            public string U_TxtUsuario { get; set; }
            public string U_TxtAcceso { get; set; } 
            public DateTime? U_FecCreacion { get; set; }
            public DateTime? U_FecModificacion { get; set; }
            public string U_Nombres { get; set; }
            public string U_ApePaterno { get; set; } 
            public string U_ApeMaterno { get; set; } 
            public string U_Email { get; set; }
            public string U_NumIdentidad { get; set; } 
            public string U_Perfil { get; set; } // ADMIN, CLIENTE
            public string U_Estado { get; set; }

        

    }
}