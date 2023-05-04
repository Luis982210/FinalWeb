using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using System.Web;
using System.Drawing;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Front.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class BicicletasController : Controller
    {
        private readonly BicicletasContext _context;

        public BicicletasController(BicicletasContext context)
        {
            _context = context;
        }
        static string url = "https://localhost:7284";
        // GET: Bicicletas
        public async Task<IActionResult> Index()
        {
            return _context.Bicicletas != null ? 
                          View(await _context.Bicicletas.ToListAsync()) :
                          Problem("Entity set 'BicicletasContext.Bicicletas'  is null.");
        }

        public ActionResult convertirImagen(int codigo)
        {

            using (var context = new BicicletasContext())
            {
                var imagen = (from Bicicleta in context.Bicicletas
                              where Bicicleta.Id == codigo
                              select Bicicleta).FirstOrDefault();

                return File(imagen.Imagen, "image/jpeg");

            }

        }

        // GET: Bicicletas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            HttpClient client = new HttpClient();
            var salas = await client.GetFromJsonAsync<IEnumerable<Models.Models.Bicicleta>>(url + "/api/Bicicletas");
            if (id == null || _context.Bicicletas == null)
            {
                return NotFound();
            }

            var bicicleta = await _context.Bicicletas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bicicleta == null)
            {
                return NotFound();
            }

            return View(bicicleta);
        }

        // GET: Bicicletas/Create
        public IActionResult Create()
        {
            HttpClient client = new HttpClient();
            var res = client.GetAsync(url + "/api/Create");
            return View();
        }

        // POST: Bicicletas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Tipo,Marca,Tamano,Cantidadplatos,Cantidadpinones,Imagen")] Bicicleta bicicleta)
        {
            HttpClient client = new HttpClient();

        
            var response = await client.PostAsJsonAsync<Bicicleta>(url + "/api/Bicicletas", bicicleta);
            Console.WriteLine("todo bien conectando con API" + url);
           


            return RedirectToAction(nameof(Index));

        }

        // GET: Bicicletas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            HttpClient client = new HttpClient();
            var salas = await client.GetFromJsonAsync<IEnumerable<Models.Models.Bicicleta>>(url + "/api/Bicicletas");
            if (id == null || _context.Bicicletas == null)
            {
                return NotFound();
            }

            var bicicleta = await _context.Bicicletas.FindAsync(id);
            if (bicicleta == null)
            {
                return NotFound();
            }
            return View(bicicleta);
        }

        // POST: Bicicletas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Tipo,Marca,Tamano,Cantidadplatos,Cantidadpinones,Imagen")] Bicicleta bicicleta)
        {
            HttpClient client = new HttpClient();
            if (id != bicicleta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var response = await client.PutAsJsonAsync(url + "/api/Bicicletas/" + bicicleta.Id.ToString(), bicicleta);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdMobiliario"] = await client.GetFromJsonAsync<List<SelectListItem>>(url + "/api/Bicicletas");
            return View(bicicleta);
        }


        // GET: Bicicletas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            HttpClient client = new HttpClient();
            if (id == null || _context.Bicicletas == null)
            {
                return NotFound();
            }

            var sala = await client.GetFromJsonAsync<Bicicleta>(url + "/api/Bicicletas/" + id.ToString());
            if (sala == null)
            {
                return NotFound();
            }

            return View(sala);
        }

        // POST: Bicicletas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient client = new HttpClient();
            if (_context.Bicicletas == null)
            {
                return Problem("Entity set is null.");
            }
            Console.WriteLine("hola");
            var response = await client.DeleteFromJsonAsync<Bicicleta>(url + "/api/Bicicletas/" + id.ToString());

            return RedirectToAction(nameof(Index));
        }

        private bool BicicletaExists(int id)
        {
          return (_context.Bicicletas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
