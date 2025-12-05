using Microsoft.AspNetCore.Mvc;
using Busticket.Models;  // Para LoginViewModel, RegisterViewModel y Usuario
using Busticket.Data;    // Para ApplicationDbContext
using Microsoft.AspNetCore.Http; // Para HttpContext.Session

namespace Busticket.Controllers
{
    public class AuthController : Controller
    {
        private readonly Data.ApplicationDbContext _context;

        // Inyectamos DbContext
        public AuthController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Debe existir Views/Auth/Login.cshtml
        }

        // POST: /Auth/Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Autenticación básica: buscar usuario en la DB
            var usuario = _context.Usuarios
                                  .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (usuario != null)
            {
                HttpContext.Session.SetString("Usuario", usuario.Email);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View(model);
        }

        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(); // Debe existir Views/Auth/Register.cshtml
        }

        // POST: /Auth/Register
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Verificar si ya existe el usuario
            var existe = _context.Usuarios.Any(u => u.Email == model.Email);
            if (existe)
            {
                ModelState.AddModelError("Email", "Este correo ya está registrado");
                return View(model);
            }

            // Crear usuario y guardar en la DB
            var usuario = new Usuario
            {
                Nombre = model.Nombre,
                Email = model.Email,
                Password = model.Password // ⚠️ En producción, siempre hashea la contraseña
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            // Iniciar sesión automáticamente
            HttpContext.Session.SetString("Usuario", usuario.Email);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Auth/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
    
}

