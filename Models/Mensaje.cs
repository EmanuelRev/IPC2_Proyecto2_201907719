
// mensajes jaajaja

using PROYECTO_2.Estructuras;

namespace PROYECTO_2.Models
{
    public class Mensaje
    {
        public string Nombre { get; set; }
        public string NombreObj => Nombre;
        public string SistemaDrones { get; set; }
        
        
        public Cola<Instruccion> Instrucciones { get; set; }

        public Mensaje(string nombre, string sistemaDrones)
        {
            Nombre = nombre;
            SistemaDrones = sistemaDrones;
            Instrucciones = new Cola<Instruccion>(); // Inicializamos tu cola
        }
    }
}