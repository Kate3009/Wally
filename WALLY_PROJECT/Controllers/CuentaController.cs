using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using WALLY_PROJECT.Models;

public class CuentaController : Controller
{
    private readonly string apiUrlBase = "http://127.0.0.1:5000/cuentas";

    public async Task<ActionResult> Detalle()
    {
        if (Session["UsuarioId"] == null)
            return RedirectToAction("IniciarSesion", "Login");

        int idUsuario = (int)Session["UsuarioId"];
        string url = $"{apiUrlBase}/{idUsuario}";

        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<CuentaUsuario>(json);
                return View(model);
            }

            ViewBag.Error = "No se pudo obtener la información.";
            return View();
        }
    }
}

