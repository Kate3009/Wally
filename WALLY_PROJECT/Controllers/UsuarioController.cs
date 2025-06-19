using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WALLY_PROJECT.Models;
using System.Configuration;
using System.Threading.Tasks;

namespace WALLY_PROJECT.Controllers
{
    public class UsuarioController : Controller
    {
        IEnumerable<Usuario> getUsuario()
        {
            List<Usuario> lista = new List<Usuario>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["wally"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("USP_LISTAR_USUARIOS", cn);

                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    lista.Add(new Usuario
                    {
                        IdUsuario = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0),
                        U_TxtUsuario = dataReader.IsDBNull(1) ? "" : dataReader.GetString(1),
                        U_Nombres = dataReader.IsDBNull(2) ? "" : dataReader.GetString(2),
                        U_ApePaterno = dataReader.IsDBNull(3) ? "" : dataReader.GetString(3),
                        U_ApeMaterno = dataReader.IsDBNull(4) ? "" : dataReader.GetString(4),
                        U_Email = dataReader.IsDBNull(5) ? "" : dataReader.GetString(5),
                        U_NumIdentidad = dataReader.IsDBNull(6) ? "" : dataReader.GetString(6),
                        U_Estado = dataReader.IsDBNull(7) ? "" : dataReader.GetString(7)
                    });
                }
                cn.Close();
            }
            return lista;
        }

       
        // GET: Usuario
        public async Task<ActionResult> Index(int pa = 0)
        {
         
            int filas = 10;
            int n = getUsuario().Count();
            int pags = n % filas > 0 ? n / filas + 1 : n / filas;
            ViewBag.pags = pags;
            ViewBag.pa = pa;
            //return View(await Task.Run(() => getLibroConsulta(nomlibro).ToList()));
            return View(await Task.Run(() => getUsuario().ToList().Skip(pa * filas).Take(filas)));
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
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

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Usuario/Edit/5
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

        // GET: Usuario/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Usuario/Delete/5
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
