
// metodo burbuja para letrasxxxxx

namespace PROYECTO_2.Models
{
    public class LetraDron
    {
        public string NombreDron { get; set; }
        public int Altura { get; set; }
        public string Letra { get; set; }

        public LetraDron(string nombreDron, int altura, string letra)
        {
            NombreDron = nombreDron;
            Altura = altura;
            Letra = letra;
        }
    }
}

