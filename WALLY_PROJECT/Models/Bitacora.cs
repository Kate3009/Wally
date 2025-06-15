using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WALLY_PROJECT.Models
{
    public class Bitacora
    { 


    public int IdBitacora { get; set; }
    public DateTime? W_FecEvento { get; set; }
    public string W_DescripcionEvento { get; set; }
    public string W_TipoEvento { get; set; }
    public int W_UsuarioAfectado { get; set; }
    public int IdTransaccion { get; set; }

}
}