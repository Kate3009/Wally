using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WALLY_PROJECT.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Xml.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace WALLY_PROJECT.Controllers
{
    public class TransferenciaController : Controller
    {
        private readonly string apiUrlBase = "http://127.0.0.1:5000/transacciones";

        public ActionResult Create()
        {
            var transaccion = new Transaccion
            {
                T_CUENTA_ORIGEN = Session["NCuenta"]?.ToString()
            };

            return View(transaccion);
        }

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
                id_cuenta_destino = model.T_CUENTA_DESTINO,
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

                        // Guardamos en TempData para generar el PDF
                        TempData["CuentaDestino"] = model.T_CUENTA_DESTINO.ToString();
                        TempData["Monto"] = model.T_MONTO.ToString("F2");
                        TempData["Detalle"] = model.T_DETALLE;

                        ModelState.Clear();
                        return View(new Transaccion { T_CUENTA_ORIGEN = model.T_CUENTA_ORIGEN });
                    }
                    else
                    {
                        ViewBag.Error = "Error en la transferencia: " + result;
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
        public ActionResult ExportarComprobante()
        {
            // Simulamos datos para el comprobante, en un caso real los tomas de la BD o Session
            var cuentaOrigen = Session["NCuenta"]?.ToString() ?? "N/D";
            var cuentaDestino = TempData["CuentaDestino"]?.ToString() ?? "N/D";
            var monto = TempData["Monto"]?.ToString() ?? "0.00";
            var detalle = TempData["Detalle"]?.ToString() ?? "Sin detalle";
            var fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            using (var ms = new MemoryStream())
            {
                var doc = new Document();
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                doc.Add(new Paragraph("Comprobante de Transferencia", titleFont));
                doc.Add(new Paragraph(" ")); // espacio

                doc.Add(new Paragraph($"Fecha: {fecha}", normalFont));
                doc.Add(new Paragraph($"Cuenta Origen: {cuentaOrigen}", normalFont));
                doc.Add(new Paragraph($"Cuenta Destino: {cuentaDestino}", normalFont));
                doc.Add(new Paragraph($"Monto: ${monto}", normalFont));
                doc.Add(new Paragraph($"Detalle: {detalle}", normalFont));

                doc.Close();

                return File(ms.ToArray(), "application/pdf", "comprobante_transferencia.pdf");
            }
        }
        private string ObtenerNumeroCuentaDestino(int idCuentaDestino)
        {
            string numeroCuenta = idCuentaDestino.ToString(); // fallback por si no se encuentra

            string connectionString = ConfigurationManager.ConnectionStrings["wally"].ConnectionString;

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = "SELECT C_NUMERO_CUENTA FROM W_CUENTA WHERE ID_CUENTA = @idCuentaDestino";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@idCuentaDestino", idCuentaDestino);

                cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    numeroCuenta = reader["C_NUMERO_CUENTA"].ToString();
                }
            }

            return numeroCuenta;
        }

        public async Task<ActionResult> Historial(int id)
        {
            List<Transaccion> lista = new List<Transaccion>();
            string cuentaDelUsuario = "";  // Se usará para marcar si la transferencia fue recibida o enviada

            string apiUrl = $"http://127.0.0.1:5000/transferencias/usuario/{id}";

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(apiUrl);
                    if (!response.IsSuccessStatusCode)
                    {
                        ViewBag.Error = "No se pudo obtener el historial.";
                        return View(lista);
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    dynamic resultado = JsonConvert.DeserializeObject(json);

                    foreach (var item in resultado.transferencias)
                    {
                        var transaccion = new Transaccion
                        {
                            ID_TRANSACCION = (int)item.ID_TRANSACCION,
                            T_CUENTA_ORIGEN = (string)item.T_CUENTA_ORIGEN,
                            T_CUENTA_DESTINO = (string)item.T_CUENTA_DESTINO,
                            T_FEC_TRANSACCION = DateTime.Parse((string)item.T_FEC_TRANSACCION),
                            T_MONTO = (decimal)item.T_MONTO,
                            T_ESTADO_TRANSACCION = (string)item.T_ESTADO_TRANSACCION,
                            T_DETALLE = (string)item.T_DETALLE
                        };

                        lista.Add(transaccion);

                        // Extraer cuenta origen solo una vez
                        if (string.IsNullOrEmpty(cuentaDelUsuario))
                        {
                            cuentaDelUsuario = transaccion.T_CUENTA_ORIGEN;
                        }
                    }

                    ViewBag.CuentaDelUsuario = cuentaDelUsuario;
                }

                return View(lista);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al contactar la API: " + ex.Message;
                return View(lista);
            }
        }

    }

}
