using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WALLY_PROJECT.Models
{
    public class Bitacora
    { 


    public int ID_BITACORA { get; set; }
    public DateTime? W_FEC_EVENTO { get; set; }
    public string W_DESCRIPCION_EVENTO { get; set; }
    public string W_TIPO_EVENTO { get; set; }
    public int W_USUARIO_AFECTADO { get; set; }
    public int ID_TRANSACCION { get; set; }

}
}