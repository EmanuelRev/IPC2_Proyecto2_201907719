//para guardar los datos creando memoria--------

using PROYECTO_2.Estructuras;
using PROYECTO_2.Models;

namespace PROYECTO_2.Utilidades
{
    public static class DatosGlobales
    {
        
        public static ListaEnlazada<Dron> DronesGlobales = new ListaEnlazada<Dron>();
        public static ListaEnlazada<SistemaDrones> SistemasDeDrones = new ListaEnlazada<SistemaDrones>();
        public static ListaEnlazada<Mensaje> Mensajes = new ListaEnlazada<Mensaje>();
        public static ListaEnlazada<ResultadoSimulacion> Resultados = new ListaEnlazada<ResultadoSimulacion>();

        
        public static void Inicializar()
        {
            DronesGlobales = new ListaEnlazada<Dron>();
            SistemasDeDrones = new ListaEnlazada<SistemaDrones>();
            Mensajes = new ListaEnlazada<Mensaje>();
            Resultados = new ListaEnlazada<ResultadoSimulacion>();
        }
    }
}