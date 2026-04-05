//cola FIFO xxxxxxx

namespace PROYECTO_2.Estructuras
{
    public class Cola<T>
    {
        public Nodo<T> Primero { get; set; }
        public Nodo<T> Ultimo { get; set; }
        public int Tamano { get; set; }

        public Cola()
        {
            Primero = null;
            Ultimo = null;
            Tamano = 0;
        }

        public void Encolar(T valor)
        {
            Nodo<T> nuevoNodo = new Nodo<T>(valor);

            if (Primero == null)
            {
                Primero = nuevoNodo;
                Ultimo = nuevoNodo;
            }
            else
            {
                Ultimo.Siguiente = nuevoNodo;
                Ultimo = nuevoNodo;
            }
            Tamano++;
        }

        public T Desencolar()
        {
            if (Primero == null)
            {
                return default; 
            }

            T valor = Primero.Valor;
            Primero = Primero.Siguiente;
            Tamano--;

            if (Primero == null)
            {
                Ultimo = null;
            }

            return valor;
        }

        public bool EstaVacia()
        {
            return Primero == null;
        }

        public Cola<T> Copiar()
        {
            Cola<T> nuevaCola = new Cola<T>();
            Nodo<T> actual = Primero;
            while (actual != null)
            {
                nuevaCola.Encolar(actual.Valor);
                actual = actual.Siguiente;
            }
            return nuevaCola;
        }
    }
}