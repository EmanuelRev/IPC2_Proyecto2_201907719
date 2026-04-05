
// todo el code del dron aqui ---

namespace PROYECTO_2.Models
{
    public class Dron : INombrable
    {
        public string Nombre { get; set; }
        public string NombreObj => Nombre;

        public Dron(string nombre)
        {
            Nombre = nombre;
        }
    }
}