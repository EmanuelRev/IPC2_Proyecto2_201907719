// para las graficas code xxxx

using System;
using System.Diagnostics;
using System.IO;
using PROYECTO_2.Estructuras;
using PROYECTO_2.Models;

namespace PROYECTO_2.Utilidades
{
    public class GeneradorGraphviz
    {
        
        public void GraficarSistemasDrones(ListaEnlazada<SistemaDrones> sistemas, string rutaSalidaBase)
        {
            string dot = "digraph G {\n";
            dot += "  node [shape=box, style=filled, fillcolor=lightblue, fontname=\"Arial\"];\n";
            dot += "  rankdir=TB;\n"; 
            dot += "  Titulo [label=\"Sistemas de Drones\", shape=plaintext, fillcolor=white, fontsize=18];\n";

            Nodo<SistemaDrones> nodo = sistemas.Primero;
            while (nodo != null)
            {
                SistemaDrones sist = nodo.Valor;
                
                string idSist = sist.Nombre.Replace(" ", "_"); 
                
                dot += $"  {idSist} [label=\"Sistema: {sist.Nombre}\\nAltura Max: {sist.AlturaMaxima}\\nCant. Drones: {sist.CantidadDrones}\"];\n";
                
                if (nodo.Siguiente != null)
                {
                    string idSistSig = nodo.Siguiente.Valor.Nombre.Replace(" ", "_");
                    dot += $"  {idSist} -> {idSistSig};\n"; 
                }

                nodo = nodo.Siguiente;
            }

            dot += "}\n";
            CompilarImagen(dot, rutaSalidaBase);
        }

        
        public void GraficarInstrucciones(ResultadoSimulacion resultado, string rutaSalidaBase)
        {
            string dot = "digraph G {\n";
            dot += "  node [shape=record, style=filled, fillcolor=lightyellow, fontname=\"Arial\"];\n";
            dot += "  rankdir=LR;\n"; 
            dot += $"  Titulo [label=\"Mensaje: {resultado.NombreMensaje}\\nTiempo: {resultado.TiempoOptimo}s\", shape=plaintext, fillcolor=white, fontsize=16];\n";

            Nodo<PasoTiempo> nodoPaso = resultado.Pasos.Primero;
            while (nodoPaso != null)
            {
                PasoTiempo paso = nodoPaso.Valor;
                string idPaso = "t" + paso.Segundo;
                
                
                string labelAcciones = $"{{ Segundo {paso.Segundo} ";
                
                Nodo<Accion> nodoAccion = paso.Acciones.Primero;
                while (nodoAccion != null)
                {
                    labelAcciones += $"| {nodoAccion.Valor.NombreDron}: {nodoAccion.Valor.Movimiento} ";
                    nodoAccion = nodoAccion.Siguiente;
                }
                labelAcciones += "}";

                dot += $"  {idPaso} [label=\"{labelAcciones}\"];\n";

                if (nodoPaso.Siguiente != null)
                {
                    dot += $"  {idPaso} -> t{nodoPaso.Siguiente.Valor.Segundo};\n";
                }

                nodoPaso = nodoPaso.Siguiente;
            }

            dot += "}\n";
            CompilarImagen(dot, rutaSalidaBase);
        }

        
        private void CompilarImagen(string contenidoDot, string rutaSalidaBase)
        {
            try
            {
                string archivoDot = $"{rutaSalidaBase}.dot";
                string archivoImg = $"{rutaSalidaBase}.png";

                File.WriteAllText(archivoDot, contenidoDot);

                
                ProcessStartInfo startInfo = new ProcessStartInfo("dot");
                startInfo.Arguments = $"-Tpng \"{archivoDot}\" -o \"{archivoImg}\"";
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;

                using (Process proceso = Process.Start(startInfo))
                {
                    proceso.WaitForExit();
                }
                Console.WriteLine($"Imagen generada: {archivoImg}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en Graphviz (Verifica que esté instalado y en el PATH): " + ex.Message);
            }
        }
    }
}