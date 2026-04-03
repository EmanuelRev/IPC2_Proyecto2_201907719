// usando metto bublbee

using PROYECTO_2.Estructuras;
using PROYECTO_2.Models;

namespace PROYECTO_2.Utilidades
{
    public class Ordenador
    {
        
        public void OrdenarAlfabeticamente<T>(ListaEnlazada<T> lista) where T : INombrable
        {
            if (lista.Primero == null || lista.Primero.Siguiente == null) return;

            bool huboIntercambio;
            do
            {
                huboIntercambio = false;
                Nodo<T> actual = lista.Primero;

                while (actual.Siguiente != null)
                {
                
                    if (string.Compare(actual.Valor.NombreObj, actual.Siguiente.Valor.NombreObj) > 0)
                    {
                        
                        T temp = actual.Valor;
                        actual.Valor = actual.Siguiente.Valor;
                        actual.Siguiente.Valor = temp;
                        huboIntercambio = true;
                    }
                    actual = actual.Siguiente;
                }
            } while (huboIntercambio);
        }
    }
}