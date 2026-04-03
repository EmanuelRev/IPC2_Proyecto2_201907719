
// lista 

namespace PROYECTO_2.Estructuras
{
    public class ListaEnlazada<T>
    {
        public Nodo<T> Primero { get; set; }
        public int Tamano { get; set; }

        public ListaEnlazada()
        {
            Primero = null;
            Tamano = 0;
        }

        
        public void Agregar(T valor)
        {
            Nodo<T> nuevoNodo = new Nodo<T>(valor);

            if (Primero == null)
            {
                Primero = nuevoNodo;
            }
            else
            {
                Nodo<T> actual = Primero;
                
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevoNodo;
            }
            Tamano++;
        }
    }
}