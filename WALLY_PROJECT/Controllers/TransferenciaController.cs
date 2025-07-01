using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WALLY_PROJECT.Models;

namespace WALLY_PROJECT.Controllers
{
    public class TransferenciaController : Controller
    {
        private readonly string apiUrlBase = "http://127.0.0.1:5000/transacciones";

        // GET: Transferencia/Create
        public ActionResult Create()
        {
            var transaccion = new Transaccion();
            transaccion.T_CUENTA_ORIGEN = (int)Session["NCuenta"];
            ViewBag.NumeroCuenta = Session["NumeroCuenta"]; // Para mostrar

            return View(transaccion);
        }


        // POST: Transferencia/Create
        [HttpPost]
        public async Task<ActionResult> Create(Transaccion model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Todos los campos son obligatorios.";
                return View(model);
            }

            if (model.T_CUENTA_ORIGEN == model.T_CUENTA_DESTINO)
            {
                ViewBag.Error = "Las cuentas no pueden ser iguales.";
                return View(model);
            }

            if (model.T_MONTO <= 0)
            {
                ViewBag.Error = "El monto debe ser mayor que cero.";
                return View(model);
            }

            string url = $"{apiUrlBase}/{model.T_CUENTA_ORIGEN}";

            var payload = new
            {
                cuenta_origen = model.T_CUENTA_ORIGEN,
                cuenta_destino = model.T_CUENTA_DESTINO,
                monto = model.T_MONTO,
                detalle = model.T_DETALLE
            };

            try
            {
                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync(url, content);
                    var result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Success = "Transferencia exitosa.";
                        return View();
                    }
                    else
                    {
                        ViewBag.Error = "Transferencia errónea.";
                        return View(model);
                    }

                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al contactar la API: " + ex.Message;
                return View(model);
            }
        }

    }
}
