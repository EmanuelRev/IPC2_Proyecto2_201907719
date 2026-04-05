using System;
using System.Xml;
using PROYECTO_2.Estructuras;
using PROYECTO_2.Models;

namespace PROYECTO_2.Utilidades
{
    public class EscritorXML
    {
        public void GenerarArchivoSalida(ListaEnlazada<ResultadoSimulacion> listaResultados, string rutaDestino)
        {
            try
            {
                
                XmlWriterSettings configuracion = new XmlWriterSettings();
                configuracion.Indent = true;

                using (XmlWriter escritor = XmlWriter.Create(rutaDestino, configuracion))
                {
                    escritor.WriteStartDocument();
                    escritor.WriteStartElement("respuesta");
                    escritor.WriteStartElement("listaMensajes");

                    Nodo<ResultadoSimulacion> nodoRes = listaResultados.Primero;
                    while (nodoRes != null)
                    {
                        ResultadoSimulacion resultado = nodoRes.Valor;
                        
                        escritor.WriteStartElement("mensaje");
                        escritor.WriteAttributeString("nombre", resultado.NombreMensaje);

                        escritor.WriteElementString("sistemaDrones", resultado.SistemaDrones);
                        escritor.WriteElementString("tiempoOptimo", resultado.TiempoOptimo.ToString());
                        escritor.WriteElementString("mensajeRecibido", resultado.MensajeRecibido);

                        escritor.WriteStartElement("instrucciones");

                        Nodo<PasoTiempo> nodoPaso = resultado.Pasos.Primero;
                        while (nodoPaso != null)
                        {
                            PasoTiempo paso = nodoPaso.Valor;
                            escritor.WriteStartElement("tiempo");
                            escritor.WriteAttributeString("valor", paso.Segundo.ToString());
                            escritor.WriteStartElement("acciones");

                            Nodo<Accion> nodoAccion = paso.Acciones.Primero;
                            while (nodoAccion != null)
                            {
                                Accion accion = nodoAccion.Valor;
                                escritor.WriteStartElement("dron");
                                escritor.WriteAttributeString("nombre", accion.NombreDron);
                                escritor.WriteString(accion.Movimiento);
                                escritor.WriteEndElement(); 

                                nodoAccion = nodoAccion.Siguiente;
                            }

                            escritor.WriteEndElement(); 
                            escritor.WriteEndElement();

                            nodoPaso = nodoPaso.Siguiente;
                        }

                        escritor.WriteEndElement(); 
                        escritor.WriteEndElement(); 

                        nodoRes = nodoRes.Siguiente;
                    }

                    escritor.WriteEndElement(); 
                    escritor.WriteEndElement(); 
                    escritor.WriteEndDocument();
                }
                Console.WriteLine("¡Archivo Salida.xml generado exitosamente!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al generar el XML de salida: " + ex.Message);
            }
        }
    }
}