using Busticket.Data;
using Busticket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Busticket.Controllers
{
    public class PagoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Pago/Index
        public IActionResult Index()
        {
            // Obtener carrito y total desde la sesión
            var carritoJson = HttpContext.Session.GetString("Carrito");
            var totalString = HttpContext.Session.GetString("Total");

            var asientos = string.IsNullOrEmpty(carritoJson)
                ? new List<string>()
                : JsonConvert.DeserializeObject<List<string>>(carritoJson);

            var total = decimal.TryParse(totalString, out var t) ? t : 0;

            // Si hay asientos, obtener datos de la ruta (ejemplo usando el primer asiento)
            string origen = "", destino = "", empresa = "", duracion = "", fecha = "", hora = "";
            int rutaId = 0;

            if (asientos.Count > 0)
            {
                // Obtener el primer asiento para determinar la ruta
                var primerAsiento = _context.Asientos.FirstOrDefault(a => a.Codigo == asientos[0]);
                if (primerAsiento != null)
                {
                    var ruta = _context.Rutas.FirstOrDefault(r => r.RutaId == primerAsiento.RutaId);
                    if (ruta != null)
                    {
                        rutaId = ruta.RutaId;
                        origen = ruta.Origen;
                        destino = ruta.Destino;
                        empresa = ruta.Empresa;
                        duracion = ruta.Duracion + " min";
                        fecha = DateTime.Now.ToString("dd/MM/yyyy"); // o tu fecha real de viaje
                        hora = "00:00"; // ejemplo, reemplaza con hora real si tienes
                    }
                }
            }

            var model = new PagoViewModel
            {
                RutaId = rutaId,
                Asientos = asientos,
                Total = total,
                Origen = origen,
                Destino = destino,
                Empresa = empresa,
                Duracion = duracion,
                Fecha = fecha,
                Hora = hora
            };

            return View(model);
        }

        // POST: /Pago/FinalizarPago
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FinalizarPago(PagoViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            // Marcar los asientos como ocupados en la base de datos
            var asientosDb = _context.Asientos
                .Where(a => model.Asientos.Contains(a.Codigo) && a.RutaId == model.RutaId)
                .ToList();

            foreach (var asiento in asientosDb)
            {
                asiento.Disponible = false;
            }

            _context.SaveChanges();

            // Limpiar sesión
            HttpContext.Session.Remove("Carrito");
            HttpContext.Session.Remove("Total");

            // Redirigir a página de confirmación
            return RedirectToAction("ConfirmacionPago", new { rutaId = model.RutaId });
        }

        // GET: /Pago/ConfirmacionPago
        public IActionResult ConfirmacionPago(int rutaId)
        {
            // Aquí puedes mostrar detalles del pago y ruta
            var ruta = _context.Rutas.FirstOrDefault(r => r.RutaId == rutaId);
            var model = new PagoViewModel();

            if (ruta != null)
            {
                model.RutaId = ruta.RutaId;
                model.Origen = ruta.Origen;
                model.Destino = ruta.Destino;
                model.Empresa = ruta.Empresa;
                model.Duracion = ruta.Duracion + " min";
                model.Fecha = DateTime.Now.ToString("dd/MM/yyyy");
                model.Hora = "00:00"; // reemplazar si tienes hora real
            }

            return View(model);
        }
    }
}
