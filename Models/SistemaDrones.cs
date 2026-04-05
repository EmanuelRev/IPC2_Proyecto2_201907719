
// para el sistema de drones aqui xxxxx

using PROYECTO_2.Estructuras;

namespace PROYECTO_2.Models
{
    public class SistemaDrones : INombrable
    {
        public string Nombre { get; set; }
        public string NombreObj => Nombre;
        public int AlturaMaxima { get; set; }
        public int CantidadDrones { get; set; }
        public ListaEnlazada<Dron> Drones { get; set; }
        
        
        public ListaEnlazada<LetraDron> ContenidoLetras { get; set; }

        public SistemaDrones(string nombre, int alturaMaxima, int cantidadDrones)
        {
            Nombre = nombre;
            AlturaMaxima = alturaMaxima;
            CantidadDrones = cantidadDrones;
            Drones = new ListaEnlazada<Dron>();
            ContenidoLetras = new ListaEnlazada<LetraDron>(); 
        }
    }
}