using System;
using System.Xml;
using PROYECTO_2.Estructuras;
using PROYECTO_2.Models;

namespace PROYECTO_2.Utilidades
{
    public class LectorXML
    {
        public ListaEnlazada<Dron> DronesGlobales { get; set; }
        public ListaEnlazada<SistemaDrones> SistemasDeDrones { get; set; }
        public ListaEnlazada<Mensaje> Mensajes { get; set; }

        public LectorXML()
        {
            DronesGlobales = new ListaEnlazada<Dron>();
            SistemasDeDrones = new ListaEnlazada<SistemaDrones>();
            Mensajes = new ListaEnlazada<Mensaje>();
        }

        public void CargarArchivo(string rutaArchivo)
        {
            try
            {
                XmlDocument documento = new XmlDocument();
                documento.Load(rutaArchivo);

                
                XmlNodeList listaDrones = documento.SelectNodes("//config/listaDrones/dron");
                if (listaDrones != null)
                {
                    foreach (XmlNode nodo in listaDrones)
                    {
                        Dron nuevoDron = new Dron(nodo.InnerText.Trim());
                        DronesGlobales.Agregar(nuevoDron);
                    }
                }

                
                XmlNodeList listaSistemas = documento.SelectNodes("//config/listaSistemasDrones/sistemaDrones");
                if (listaSistemas != null)
                {
                    foreach (XmlNode nodo in listaSistemas)
                    {
                        string nombre = nodo.Attributes["nombre"].Value;
                        int alturaMax = int.Parse(nodo.SelectSingleNode("alturaMaxima").InnerText);
                        int cantDrones = int.Parse(nodo.SelectSingleNode("cantidadDrones").InnerText);

                        SistemaDrones nuevoSistema = new SistemaDrones(nombre, alturaMax, cantDrones);
                        
                        
                        XmlNode contenidoNode = nodo.SelectSingleNode("contenido");
                        if (contenidoNode != null)
                        {
                            string dronActual = "";
                            
                            foreach (XmlNode hijo in contenidoNode.ChildNodes)
                            {
                                if (hijo.Name == "dron")
                                {
                                    dronActual = hijo.InnerText.Trim();
                                }
                                else if (hijo.Name == "alturas")
                                {
                                    foreach (XmlNode alturaNode in hijo.SelectNodes("altura"))
                                    {
                                        int valAltura = int.Parse(alturaNode.Attributes["valor"].Value);
                                        string letra = alturaNode.InnerText; 
                                        
                                        nuevoSistema.ContenidoLetras.Agregar(new LetraDron(dronActual, valAltura, letra));
                                    }
                                }
                            }
                        }
                        
                        SistemasDeDrones.Agregar(nuevoSistema);
                    }
                }

            
                XmlNodeList listaMensajes = documento.SelectNodes("//config/listaMensajes/Mensaje");
                if (listaMensajes != null)
                {
                    foreach (XmlNode nodo in listaMensajes)
                    {
                        string nombreMensaje = nodo.Attributes["nombre"].Value;
                        string nombreSistema = nodo.SelectSingleNode("sistemaDrones").InnerText;

                        Mensaje nuevoMensaje = new Mensaje(nombreMensaje, nombreSistema);

                        XmlNodeList instrucciones = nodo.SelectNodes("instrucciones/instruccion");
                        if (instrucciones != null)
                        {
                            foreach (XmlNode inst in instrucciones)
                            {
                                string dronInst = inst.Attributes["dron"].Value;
                                int alturaInst = int.Parse(inst.InnerText);
                                
                                nuevoMensaje.Instrucciones.Encolar(new Instruccion(dronInst, alturaInst));
                            }
                        }
                        Mensajes.Agregar(nuevoMensaje);
                    }
                }

                Console.WriteLine("¡Archivo XML cargado en estructuras dinámicas exitosamente!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer el archivo XML: " + ex.Message);
            }
        }
    }
}