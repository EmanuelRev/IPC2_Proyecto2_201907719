// para gestionar la interfzzz botones ---------

using Microsoft.AspNetCore.Mvc;
using System.IO;
using PROYECTO_2.Utilidades;
using PROYECTO_2.Models;
using PROYECTO_2.Estructuras;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace PROYECTO_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public HomeController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Inicializar()
        {
            DatosGlobales.Inicializar();
            TempData["Mensaje"] = "Sistema inicializado correctamente. Toda la memoria ha sido borrada.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CargarXML(IFormFile archivoXml)
        {
            if (archivoXml != null && archivoXml.Length > 0)
            {
                string rutaUploads = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");
                if (!Directory.Exists(rutaUploads)) Directory.CreateDirectory(rutaUploads);
                
                string rutaArchivo = Path.Combine(rutaUploads, archivoXml.FileName);
                
                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    archivoXml.CopyTo(stream);
                }

                LectorXML lector = new LectorXML();
                lector.CargarArchivo(rutaArchivo);

                DatosGlobales.DronesGlobales = lector.DronesGlobales;
                DatosGlobales.SistemasDeDrones = lector.SistemasDeDrones;
                DatosGlobales.Mensajes = lector.Mensajes;

                Optimizador optimizador = new Optimizador();
                Nodo<Mensaje> nodoMensaje = DatosGlobales.Mensajes.Primero;
                while(nodoMensaje != null)
                {
                    ResultadoSimulacion resultado = optimizador.ProcesarMensaje(nodoMensaje.Valor, DatosGlobales.SistemasDeDrones);
                    DatosGlobales.Resultados.Agregar(resultado);
                    nodoMensaje = nodoMensaje.Siguiente;
                }

                TempData["Mensaje"] = "Archivo XML cargado y mensajes procesados exitosamente.";
            }
            return RedirectToAction("Index");
        }

        public IActionResult GenerarSalidaXML()
        {
            if (DatosGlobales.Resultados.Primero == null)
            {
                TempData["Error"] = "No hay resultados para generar el archivo de salida.";
                return RedirectToAction("Index");
            }

            string rutaSalida = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "Salida.xml");
            
            EscritorXML escritor = new EscritorXML();
            escritor.GenerarArchivoSalida(DatosGlobales.Resultados, rutaSalida);

            byte[] fileBytes = System.IO.File.ReadAllBytes(rutaSalida);
            return File(fileBytes, "application/xml", "Salida.xml");
        }

        public IActionResult Ayuda()
        {
            return View();
        }

    
        public IActionResult Gestion()
        {
            Ordenador ordenador = new Ordenador();
            
           
            ordenador.OrdenarAlfabeticamente(DatosGlobales.DronesGlobales);
            ordenador.OrdenarAlfabeticamente(DatosGlobales.SistemasDeDrones);
            ordenador.OrdenarAlfabeticamente(DatosGlobales.Mensajes);

            return View();
        }
    }
}
