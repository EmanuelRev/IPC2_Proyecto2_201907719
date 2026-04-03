
// primer nodo para iniciar y pasa al otro.......
namespace PROYECTO_2.Estructuras
{
    public class Nodo<T>
    {
        public T Valor { get; set; }
        public Nodo<T> Siguiente { get; set; }

        public Nodo(T valor)
        {
            Valor = valor;
            Siguiente = null; // Por defecto no apunta a nada
        }
    }
}