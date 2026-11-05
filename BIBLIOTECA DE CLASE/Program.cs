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
            // Catálogo de 8 títulos
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

            Console.WriteLine("=========================================================================");
            Console.WriteLine("   DÍA 7: MEDICIÓN DE COMPARACIONES DE BÚSQUEDA BINARIA POR ISBN");
            Console.WriteLine("=========================================================================");

            // 1. Preparar una copia ordenada por ISBN (requisito indispensable)
            Titulo[] catalogoISBN = (Titulo[])catalogo.Clone();
            OrdenarPorISBN(catalogoISBN);

            // Arreglo plano solo con los ISBNs ordenados (para Array.BinarySearch)
            string[] isbnsOrdenados = new string[catalogoISBN.Length];
            for (int i = 0; i < catalogoISBN.Length; i++)
            {
                isbnsOrdenados[i] = catalogoISBN[i].ISBN;
            }

            // 2. Las 5 consultas requeridas (3 existentes y 2 no existentes)
            string[] consultasISBN = new string[5]
            {
                "9780156013987", // Existente (El Principito)
                "9788424116484", // Existente (Don Quijote de la Mancha)
                "9788437604183", // Existente (Pedro Páramo)
                "9780000000000", // No existente (Caso borde inferior)
                "9789999999999"  // No existente (Caso borde superior)
            };

            Console.WriteLine($"\n{"Consulta ISBN",-16} | {"Existe",-8} | {"Comparaciones (Propia)",-22} | {"Array.BinarySearch (Índice)",-28}");
            Console.WriteLine(new string('-', 82));

            // 3. Ejecución y medición de las 5 consultas
            foreach (string isbn in consultasISBN)
            {
                int comparacionesPropia = 0;
                int indicePropio = BusquedaBinariaConConteo(catalogoISBN, isbn, out comparacionesPropia);

                // Verificación mediante la función nativa del lenguaje
                int indiceOficial = Array.BinarySearch(isbnsOrdenados, isbn);

                bool existe = indicePropio != -1;
                string resultadoOficial = indiceOficial >= 0 ? $"Encontrado (idx {indiceOficial})" : $"No Encontrado (idx {indiceOficial})";

                Console.WriteLine($"{isbn,-16} | {(existe ? "SÍ" : "NO"),-8} | {comparacionesPropia,-22} | {resultadoOficial,-28}");
            }

            Console.WriteLine("\nMediciones completadas con éxito.");
        }

        // Ordenamiento por inserción manual por ISBN
        static void OrdenarPorISBN(Titulo[] arreglo)
        {
            for (int i = 1; i < arreglo.Length; i++)
            {
                Titulo clave = arreglo[i];
                int j = i - 1;

                while (j >= 0 && string.Compare(arreglo[j].ISBN, clave.ISBN, StringComparison.Ordinal) > 0)
                {
                    arreglo[j + 1] = arreglo[j];
                    j--;
                }
                arreglo[j + 1] = clave;
            }
        }

        // Búsqueda Binaria Propia con Contador de Comparaciones
        static int BusquedaBinariaConConteo(Titulo[] arreglo, string isbn, out int comparaciones)
        {
            comparaciones = 0;
            int inicio = 0;
            int fin = arreglo.Length - 1;

            while (inicio <= fin)
            {
                int medio = inicio + (fin - inicio) / 2;

                // Se incrementa el contador por la comparación realizada
                comparaciones++;
                int comp = string.Compare(arreglo[medio].ISBN, isbn, StringComparison.Ordinal);

                if (comp == 0)
                {
                    return medio; // Encontrado
                }

                if (comp < 0)
                {
                    inicio = medio + 1;
                }
                else
                {
                    fin = medio - 1;
                }
            }

            return -1; // No encontrado
        }
    }
}