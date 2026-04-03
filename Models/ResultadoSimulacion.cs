
// code para resultado xxxxx

using PROYECTO_2.Estructuras;

namespace PROYECTO_2.Models
{
    
    public class DronEstado
    {
        public string Nombre { get; set; }
        public int AlturaActual { get; set; }

        public DronEstado(string nombre)
        {
            Nombre = nombre;
            AlturaActual = 0; 
        }
    }

    
    public class Accion
    {
        public string NombreDron { get; set; }
        public string Movimiento { get; set; }

        public Accion(string nombreDron, string movimiento)
        {
            NombreDron = nombreDron;
            Movimiento = movimiento;
        }
    }

    
    public class PasoTiempo
    {
        public int Segundo { get; set; }
        public ListaEnlazada<Accion> Acciones { get; set; }

        public PasoTiempo(int segundo)
        {
            Segundo = segundo;
            Acciones = new ListaEnlazada<Accion>();
        }
    }

    
    public class ResultadoSimulacion
    {
        public string NombreMensaje { get; set; }
        public string SistemaDrones { get; set; }
        public int TiempoOptimo { get; set; }
        public string MensajeRecibido { get; set; } = ""; 
        public ListaEnlazada<PasoTiempo> Pasos { get; set; }

        public ResultadoSimulacion()
        {
            Pasos = new ListaEnlazada<PasoTiempo>();
        }
    }
}