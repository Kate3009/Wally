using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using WALLY_PROJECT.Models;

public class LoginController : Controller
{
    [HttpGet]
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
                SELECT id_usuario, U_PERFIL, U_NOMBRES, U_APE_PATERNO, U_APE_MATERNO
                FROM W_USUARIO
                WHERE U_TXT_USUARIO = @usuario 
                  AND U_TXT_ACCESO = @contrasena 
                  AND U_ESTADO = 'A'";

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@usuario", usu.U_TXT_USUARIO);
            cmd.Parameters.AddWithValue("@contrasena", usu.U_TXT_ACCESO);

            cn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                int idUsuario = Convert.ToInt32(reader["id_usuario"]);
                string perfil = reader["U_PERFIL"].ToString();
                string nombreCompleto = $"{reader["U_NOMBRES"]} {reader["U_APE_PATERNO"]} {reader["U_APE_MATERNO"]}";

                reader.Close();

                // Guardar datos comunes en sesión
                Session["UsuarioId"] = idUsuario;
                Session["Perfil"] = perfil;
                Session["NombreUsuario"] = nombreCompleto;

                // Si es ADMIN, no validamos cuenta
                if (perfil == "ADMIN")
                {
                    return RedirectToAction("Index", "Home");
                }

                // Si no es ADMIN, verificar que tenga una cuenta asociada
                string cuentaQuery = "SELECT TOP 1 ID_CUENTA, C_NUMERO_CUENTA FROM W_CUENTA WHERE ID_USUARIO = @idUsuario";
                SqlCommand cuentaCmd = new SqlCommand(cuentaQuery, cn);
                cuentaCmd.Parameters.AddWithValue("@idUsuario", idUsuario);

                SqlDataReader cuentaReader = cuentaCmd.ExecuteReader();

                if (cuentaReader.Read())
                {
                    Session["NCuenta"] = cuentaReader["ID_CUENTA"].ToString();
                    Session["NumeroCuenta"] = cuentaReader["C_NUMERO_CUENTA"].ToString();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Mensaje = "No se encontró una cuenta asociada.";
                    return View();
                }
            }
            else
            {
                ViewBag.Mensaje = "Usuario o contraseña incorrectos o inactivo.";
                return View();
            }
        }
    }
}
