

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WALLY_PROJECT.Models;
namespace WALLY_PROJECT.Controllers
{
    public class LoginController : Controller
    {

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IniciarSesion()
        {
            return View();
        }
        [HttpPost]
        public ActionResult IniciarSesion(Usuario usu)
        {
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["wally"].ConnectionString))
            {
                string query = @"
            SELECT id_usuario, U_PERFIL
            FROM W_USUARIO
            WHERE U_TXT_USUARIO = @usuario 
              AND U_TXT_ACCESO = @contrasena 
              AND U_ESTADO = 'A'
        ";

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@usuario", usu.U_TxtUsuario);
                cmd.Parameters.AddWithValue("@contrasena", usu.U_TxtAcceso);

                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string perfil = reader["U_PERFIL"].ToString();
                    string idUsuario = reader["IdUsuario"].ToString();

                    // Guardar en ViewBag (para usar en la vista actual)
                    ViewBag.perfil = perfil;
                    ViewBag.idUsuario = idUsuario;

                    // También puedes usar ViewData si lo prefieres
                    ViewData["perfil_usu"] = perfil;
                    ViewData["IdUsuario"] = idUsuario;

                    // Si quieres usarlo después de redirigir (otra vista), usa Session
                    Session["perfil"] = perfil;
                    Session["IdUsuario"] = idUsuario;

                    // Redirección basada en perfil
                    if (perfil == "CLIENTE")
                    {
                        return RedirectToAction("IndexCliente", "Principal");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Principal");
                    }
                }
                else
                {
                    ViewData["Mensaje"] = "El usuario o la contraseña son incorrectos o el usuario está inactivo.";
                    return View();
                }
            }
        }



        // GET: Login/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Login/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Login/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}