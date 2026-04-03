using PROYECTO_2.Estructuras;
using PROYECTO_2.Models;

namespace PROYECTO_2.Utilidades
{
    public class Optimizador
    {
        public ResultadoSimulacion ProcesarMensaje(Mensaje mensaje, ListaEnlazada<SistemaDrones> listaSistemas)
        {
            ResultadoSimulacion resultado = new ResultadoSimulacion();
            resultado.NombreMensaje = mensaje.Nombre;
            resultado.SistemaDrones = mensaje.SistemaDrones;
            resultado.MensajeRecibido = ""; 

            ListaEnlazada<DronEstado> estadosDrones = ObtenerDronesDelMensaje(mensaje.Instrucciones);
            
            int tiempoActual = 1;

            while (!mensaje.Instrucciones.EstaVacia())
            {
                PasoTiempo pasoActual = new PasoTiempo(tiempoActual);
                
                Instruccion instruccionTurno = mensaje.Instrucciones.Primero.Valor;
                bool luzEmitidaEnEsteSegundo = false;

                Nodo<DronEstado> nodoEstado = estadosDrones.Primero;
                
                while (nodoEstado != null)
                {
                    DronEstado dron = nodoEstado.Valor;
                    string accionDron = "Esperar";

                    if (dron.Nombre == instruccionTurno.NombreDron && !luzEmitidaEnEsteSegundo)
                    {
                        if (dron.AlturaActual == instruccionTurno.Altura)
                        {
                            accionDron = "Emitir luz";
                            luzEmitidaEnEsteSegundo = true; 
                            
                            
                            string letraDescubierta = ObtenerLetra(listaSistemas, mensaje.SistemaDrones, dron.Nombre, dron.AlturaActual);
                            resultado.MensajeRecibido += letraDescubierta;
                        }
                        else if (dron.AlturaActual < instruccionTurno.Altura)
                        {
                            dron.AlturaActual++;
                            accionDron = "Subir";
                        }
                        else
                        {
                            dron.AlturaActual--;
                            accionDron = "Bajar";
                        }
                    }
                    else
                    {
                        int alturaFutura = ObtenerAlturaFutura(mensaje.Instrucciones, dron.Nombre);

                        if (alturaFutura != -1) 
                        {
                            if (dron.AlturaActual < alturaFutura)
                            {
                                dron.AlturaActual++;
                                accionDron = "Subir";
                            }
                            else if (dron.AlturaActual > alturaFutura)
                            {
                                dron.AlturaActual--;
                                accionDron = "Bajar";
                            }
                        }
                    }

                    pasoActual.Acciones.Agregar(new Accion(dron.Nombre, accionDron));
                    nodoEstado = nodoEstado.Siguiente;
                }

                resultado.Pasos.Agregar(pasoActual);

                if (luzEmitidaEnEsteSegundo)
                {
                    mensaje.Instrucciones.Desencolar();
                }

                tiempoActual++;
            }

            resultado.TiempoOptimo = tiempoActual - 1;
            return resultado;
        }

        private ListaEnlazada<DronEstado> ObtenerDronesDelMensaje(Cola<Instruccion> instrucciones)
        {
            ListaEnlazada<DronEstado> dronesUnicos = new ListaEnlazada<DronEstado>();
            Nodo<Instruccion> actual = instrucciones.Primero;

            while (actual != null)
            {
                if (!ExisteDron(dronesUnicos, actual.Valor.NombreDron))
                {
                    dronesUnicos.Agregar(new DronEstado(actual.Valor.NombreDron));
                }
                actual = actual.Siguiente;
            }
            return dronesUnicos;
        }

        private bool ExisteDron(ListaEnlazada<DronEstado> lista, string nombre)
        {
            Nodo<DronEstado> actual = lista.Primero;
            while (actual != null)
            {
                if (actual.Valor.Nombre == nombre) return true;
                actual = actual.Siguiente;
            }
            return false;
        }

        private int ObtenerAlturaFutura(Cola<Instruccion> cola, string nombreDron)
        {
            Nodo<Instruccion> actual = cola.Primero;
            while (actual != null)
            {
                if (actual.Valor.NombreDron == nombreDron)
                {
                    return actual.Valor.Altura;
                }
                actual = actual.Siguiente;
            }
            return -1; 
        }

        
        private string ObtenerLetra(ListaEnlazada<SistemaDrones> listaSistemas, string nombreSistema, string nombreDron, int altura)
        {
            Nodo<SistemaDrones> nodoSist = listaSistemas.Primero;
            while (nodoSist != null)
            {
                if (nodoSist.Valor.Nombre == nombreSistema)
                {
                    Nodo<LetraDron> nodoLetra = nodoSist.Valor.ContenidoLetras.Primero;
                    while (nodoLetra != null)
                    {
                        if (nodoLetra.Valor.NombreDron == nombreDron && nodoLetra.Valor.Altura == altura)
                        {
                            return nodoLetra.Valor.Letra;
                        }
                        nodoLetra = nodoLetra.Siguiente;
                    }
                }
                nodoSist = nodoSist.Siguiente;
            }
            return ""; 
        }
    }
}