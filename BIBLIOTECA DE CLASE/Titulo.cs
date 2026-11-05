using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIBLIOTECA_DE_CLASE
{
    public class Titulo
    {
        public string Nombre { get; set; }
        public string ISBN { get; set; }
        public int PrestamosHistoricos { get; set; }

        // Arreglo unidimensional que define la cantidad exactas de copias para este título (sin celdas vacías).
        // Cada posición indica si el ejemplar está disponible (true) o prestado (false).
        public bool[] Ejemplares { get; set; }

        public Titulo(string nombre, string isbn, int prestamosHistoricos, int cantidadEjemplares)
        {
            Nombre = nombre;
            ISBN = isbn;
            PrestamosHistoricos = prestamosHistoricos;

            // Se inicializa el arreglo con el tamaño exacto solicitado para este título
            Ejemplares = new bool[cantidadEjemplares];
            for (int i = 0; i < cantidadEjemplares; i++)
            {
                Ejemplares[i] = true; // Por defecto, todos los ejemplares inician disponibles
            }
        }
    }
}