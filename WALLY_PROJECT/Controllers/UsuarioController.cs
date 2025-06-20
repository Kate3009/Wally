using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using WALLY_PROJECT.Models;

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

    // POST: Usuario/Create
    [HttpPost]
    public async Task<ActionResult> Create(Usuario usuario)
    {
        using (var client = new HttpClient())
        {
            var content = new StringContent(JsonConvert.SerializeObject(usuario), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Error = "Error al registrar el usuario.";
            return View(usuario);
        }
    }

    // GET: Usuario/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        return await Details(id); // Reutiliza lógica de Details
    }

    // POST: Usuario/Edit/5
    [HttpPost]
    public async Task<ActionResult> Edit(Usuario usuario)
    {
        using (var client = new HttpClient())
        {
            var content = new StringContent(JsonConvert.SerializeObject(usuario), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{apiUrl}/{usuario.ID_USUARIO}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Error = "Error al actualizar el usuario.";
            return View(usuario);
        }
    }

    // PUT: Usuario/Inactivar/5
    public async Task<ActionResult> Inactivar(int id)
    {
        using (var client = new HttpClient())
        {
            var response = await client.PutAsync($"{apiUrl}/{id}/inactivar", null);
            return RedirectToAction("Index");
        }
    }

    // PUT: Usuario/Activar/5
    public async Task<ActionResult> Activar(int id)
    {
        using (var client = new HttpClient())
        {
            var response = await client.PutAsync($"{apiUrl}/{id}/activar", null);
            return RedirectToAction("Index");
        }
    }
}
