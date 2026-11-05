using System;

namespace BibliotecaEscolar
{
    public class Titulo
    {
        public string Nombre { get; set; }
        public string ISBN { get; set; }
        public int PrestamosHistoricos { get; set; }
        public bool[] Ejemplares { get; set; }

        public Titulo(string nombre, string isbn, int prestamosHistoricos, int cantidadEjemplares)
        {
            Nombre = nombre;
            ISBN = isbn;
            PrestamosHistoricos = prestamosHistoricos;
            Ejemplares = new bool[cantidadEjemplares];
            for (int i = 0; i < cantidadEjemplares; i++)
            {
                Ejemplares[i] = true;
            }
        }

        public int ObtenerDisponibilidad()
        {
            int disponibles = 0;
            for (int i = 0; i < Ejemplares.Length; i++)
            {
                if (Ejemplares[i]) disponibles++;
            }
            return disponibles;
        }

        public bool RegistrarPrestamo()
        {
            for (int i = 0; i < Ejemplares.Length; i++)
            {
                if (Ejemplares[i])
                {
                    Ejemplares[i] = false;
                    PrestamosHistoricos++;
                    return true;
                }
            }
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Carga de los 8 títulos con datos de prueba
            // Nota: "Don Quijote de la Mancha" y "El Principito" inician empatados en 20 préstamos
            Titulo[] catalogo = new Titulo[8]
            {
                new Titulo("Cien Años de Soledad", "9780307474728", 15, 5),
                new Titulo("Don Quijote de la Mancha", "9788424116484", 20, 3),
                new Titulo("El Principito", "9780156013987", 20, 4),
                new Titulo("Pedro Páramo", "9788437604183", 8, 1),
                new Titulo("Rayuela", "9788437604572", 12, 2),
                new Titulo("Ficciones", "9788420633121", 5, 2),
                new Titulo("La Ciudad y los Perros", "9788466332149", 10, 3),
                new Titulo("La Casa de los Espíritus", "9780307474803", 18, 4)
            };

            Console.WriteLine("=========================================================");
            Console.WriteLine("        CATÁLOGO ORIGINAL (DESORDENADO)");
            Console.WriteLine("=========================================================");
            MostrarCatalogo(catalogo);

            // ==========================================
            // DÍA 4: ORDENAMIENTO POR INSERCIÓN MANUAL
            // ==========================================
            OrdenarPorInsercion(catalogo);

            Console.WriteLine("\n=========================================================");
            Console.WriteLine("  CATÁLOGO ORDENADO POR PRÉSTAMOS (MAYOR A MENOR) - DÍA 4");
            Console.WriteLine("=========================================================");
            MostrarCatalogo(catalogo);
        }

        // Método manual de ordenamiento por inserción (estable)
        static void OrdenarPorInsercion(Titulo[] arreglo)
        {
            for (int i = 1; i < arreglo.Length; i++)
            {
                Titulo clave = arreglo[i];
                int j = i - 1;

                // Ordena de MAYOR a MENOR por PrestamosHistoricos.
                // Al usar estrictamente '<' (y no '<='), si dos libros tienen los mismos préstamos,
                // no se intercambian entre sí, preservando su orden alfabético previo (Estabilidad).
                while (j >= 0 && arreglo[j].PrestamosHistoricos < clave.PrestamosHistoricos)
                {
                    arreglo[j + 1] = arreglo[j];
                    j--;
                }
                arreglo[j + 1] = clave;
            }
        }

        static void MostrarCatalogo(Titulo[] catalogo)
        {
            Console.WriteLine($"{"Título",-30} | {"Préstamos Históricos",-20} | {"Disponibles",-12}");
            Console.WriteLine(new string('-', 70));
            foreach (var t in catalogo)
            {
                Console.WriteLine($"{t.Nombre,-30} | {t.PrestamosHistoricos,-20} | {t.ObtenerDisponibilidad()}/{t.Ejemplares.Length}");
            }
        }
    }
}