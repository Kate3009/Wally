using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using WALLY_PROJECT.Models;

namespace WALLY_PROJECT.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly string apiUrl = "http://127.0.0.1:5000/usuarios";

        // GET: Usuario
        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
                    return View(usuarios);
                }
                return View(new List<Usuario>());
            }
        }

        // GET: Usuario/Details/5
        public async Task<ActionResult> Details(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{apiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(json);
                    var usuario = JsonConvert.DeserializeObject<Usuario>(JsonConvert.SerializeObject(result.cursor));
                    return View(usuario);
                }
                return HttpNotFound();
            }
        }
        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Usuario usuario)
        {
            string confirmPassword = Request.Form["ConfirmPassword"];

            if (usuario.U_TXT_ACCESO != confirmPassword)
            {
                ViewBag.PasswordMismatch = "Las contraseñas no coinciden.";
                return View(usuario);
            }

            using (var client = new HttpClient())
            {
                var jsonContent = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Mensaje"] = "Usuario registrado correctamente.";

                    if (usuario.U_PERFIL != null && usuario.U_PERFIL.ToUpper() == "ADMIN")
                    {
                        // Redirige a Index si es ADMIN
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Redirige a inicio de sesión si es CLIENTE u otro perfil
                        return RedirectToAction("IniciarSesion", "Login");
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ViewBag.Error = " Error al registrar el usuario: " + error;
                    return View(usuario);
                }
            }
        }




        // GET: Usuario/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return await Details(id); // Reutiliza lógica de Details
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Usuario usuario)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(usuario), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{apiUrl}/{usuario.ID_USUARIO}", content);

                if (response.IsSuccessStatusCode)
                {
                    string rol = Session["Rol"] != null ? Session["Rol"].ToString().ToUpper() : "";

                    if (rol == "ADMIN")
                    {
                        // Redirige a la lista de usuarios
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Muestra el mismo formulario con mensaje de éxito
                        ViewBag.Success = "La información fue actualizada correctamente.";
                        return View(usuario);
                    }
                }

                ViewBag.Error = "Error al actualizar el usuario.";
                return View(usuario);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Inactivar(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PutAsync($"{apiUrl}/{id}/inactivar", null);
                return Json(new { success = response.IsSuccessStatusCode });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Activar(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PutAsync($"{apiUrl}/{id}/activar", null);
                return Json(new { success = response.IsSuccessStatusCode });
            }
        }
        private readonly string apiUrlBuscar = "http://127.0.0.1:5000/usuarios/buscar";

        [HttpGet]
        public async Task<ActionResult> Buscar(string q)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrlBuscar + "?q=" + q);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var usuarios = JsonConvert.DeserializeObject<List<dynamic>>(json);

                    var resultados = new List<object>();
                    foreach (var u in usuarios)
                    {
                        resultados.Add(new
                        {
                            id = (int)u.id,  // ID de cuenta destino
                            nombre_completo = (string)u.nombre_completo,
                            usuario = (string)u.usuario,
                            numero_cuenta = (string)u.numero_cuenta  // <- necesario si quieres mostrarla también
                        });
                    }

                    return Json(resultados, JsonRequestBehavior.AllowGet);
                }
                return new HttpStatusCodeResult(500, "Error al consultar usuarios");
            }
        }


    }
}