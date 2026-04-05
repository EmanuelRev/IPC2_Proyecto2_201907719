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

                Nodo<Dron> nDron = lector.DronesGlobales.Primero;
                while(nDron != null) {
                    bool existe = false;
                    Nodo<Dron> actualGlobal = DatosGlobales.DronesGlobales.Primero;
                    
                    while (actualGlobal != null)
                    {
                        if (actualGlobal.Valor.Nombre == nDron.Valor.Nombre)
                        {
                            existe = true;
                            break;
                        }
                        actualGlobal = actualGlobal.Siguiente;
                    }

                    if (!existe)
                    {
                        DatosGlobales.DronesGlobales.Agregar(nDron.Valor);
                    }
                    nDron = nDron.Siguiente;
                }

                Nodo<SistemaDrones> nSist = lector.SistemasDeDrones.Primero;
                while(nSist != null) {
                    DatosGlobales.SistemasDeDrones.Agregar(nSist.Valor);
                    nSist = nSist.Siguiente;
                }

                Optimizador optimizador = new Optimizador();
                Nodo<Mensaje> nMsj = lector.Mensajes.Primero;
                while(nMsj != null) {
                    DatosGlobales.Mensajes.Agregar(nMsj.Valor);
                    
                    // Parte corregida: Procesamiento y guardado de resultados
                    ResultadoSimulacion resultado = optimizador.ProcesarMensaje(nMsj.Valor, DatosGlobales.SistemasDeDrones);
                    DatosGlobales.Resultados.Agregar(resultado);
                    
                    nMsj = nMsj.Siguiente;
                }

                TempData["Mensaje"] = "Archivo XML cargado de forma incremental y procesado exitosamente.";
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

        [HttpPost]
        public IActionResult AgregarDron(string nombreDron)
        {
            if (string.IsNullOrWhiteSpace(nombreDron))
            {
                TempData["Error"] = "El nombre del dron no puede estar vacío.";
                return RedirectToAction("Gestion");
            }

            nombreDron = nombreDron.Trim();

            Nodo<Dron> actual = DatosGlobales.DronesGlobales.Primero;
            while (actual != null)
            {
                if (actual.Valor.Nombre.Equals(nombreDron, System.StringComparison.OrdinalIgnoreCase))
                {
                    TempData["Error"] = $"Error: El dron '{nombreDron}' ya existe en el sistema.";
                    return RedirectToAction("Gestion");
                }
                actual = actual.Siguiente;
            }

            DatosGlobales.DronesGlobales.Agregar(new Dron(nombreDron));
            TempData["Mensaje"] = $"Dron '{nombreDron}' agregado exitosamente.";
            
            return RedirectToAction("Gestion");
        }

        
        public IActionResult GraficarSistema()
        {
            if (DatosGlobales.SistemasDeDrones.Tamano == 0)
            {
                TempData["Error"] = "No hay sistemas cargados para graficar.";
                return RedirectToAction("Gestion");
            }

            string rutaCarpeta = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "graficas");
            if (!Directory.Exists(rutaCarpeta)) Directory.CreateDirectory(rutaCarpeta);

            string rutaBase = Path.Combine(rutaCarpeta, "ListadoSistemas"); 
            string rutaPng = rutaBase + ".png";

            GeneradorGraphviz generador = new GeneradorGraphviz();
            generador.GraficarSistemasDrones(DatosGlobales.SistemasDeDrones, rutaBase); 

            if (System.IO.File.Exists(rutaPng))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(rutaPng);
                return File(fileBytes, "image/png");
            }
            else
            {
                TempData["Error"] = "Error al generar la imagen. Verifica que Graphviz esté instalado.";
                return RedirectToAction("Gestion");
            }
        }

        
        public IActionResult GraficarMensaje(string nombreMensaje)
        {
            Nodo<ResultadoSimulacion> actual = DatosGlobales.Resultados.Primero;
            ResultadoSimulacion resultadoEncontrado = null;
            
            while (actual != null)
            {
                if (actual.Valor.NombreMensaje == nombreMensaje)
                {
                    resultadoEncontrado = actual.Valor;
                    break;
                }
                actual = actual.Siguiente;
            }

            if (resultadoEncontrado == null)
            {
                TempData["Error"] = "Mensaje no encontrado.";
                return RedirectToAction("Gestion");
            }

            string rutaCarpeta = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "graficas");
            if (!Directory.Exists(rutaCarpeta)) Directory.CreateDirectory(rutaCarpeta);

            string nombreLimpio = nombreMensaje.Replace(" ", "_");
            string rutaBase = Path.Combine(rutaCarpeta, $"Instrucciones_{nombreLimpio}"); 
            string rutaPng = rutaBase + ".png";

            GeneradorGraphviz generador = new GeneradorGraphviz();
            generador.GraficarInstrucciones(resultadoEncontrado, rutaBase); 

            if (System.IO.File.Exists(rutaPng))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(rutaPng);
                return File(fileBytes, "image/png");
            }
            else
            {
                TempData["Error"] = "Error al generar la imagen. Verifica que Graphviz esté instalado.";
                return RedirectToAction("Gestion");
            }
        }
    }
}