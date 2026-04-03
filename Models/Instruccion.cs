
// instrucciones aqui xxx

namespace PROYECTO_2.Models
{
    public class Instruccion
    {
        public string NombreDron { get; set; }
        public int Altura { get; set; }

        public Instruccion(string nombreDron, int altura)
        {
            NombreDron = nombreDron;
            Altura = altura;
        }
    }
}