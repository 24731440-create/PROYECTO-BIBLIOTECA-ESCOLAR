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
            // Catálogo original de 8 títulos
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
            Console.WriteLine("                  DÍA 5: BÚSQUEDAS EN EL CATÁLOGO");
            Console.WriteLine("=========================================================");

            // -----------------------------------------------------------
            // 1. PRUEBAS DE BÚSQUEDA SECUENCIAL POR TÍTULO
            // -----------------------------------------------------------
            Console.WriteLine("\n--- 1. BÚSQUEDA SECUENCIAL POR TÍTULO ---");

            // Caso Existente
            BuscarPorTitulo(catalogo, "Rayuela");

            // Caso No Encontrado
            BuscarPorTitulo(catalogo, "La Odisea");


            // -----------------------------------------------------------
            // 2. BÚSQUEDA BINARIA PROPIA POR ISBN
            // Requisito: Exige ordenar previamente el arreglo por ISBN
            // -----------------------------------------------------------
            Console.WriteLine("\n--- 2. BÚSQUEDA BINARIA POR ISBN ---");

            // Creamos una copia del catálogo para no alterar la estructura original y la ordenamos por ISBN
            Titulo[] catalogoOrdenadoISBN = (Titulo[])catalogo.Clone();
            OrdenarPorISBN(catalogoOrdenadoISBN);

            // Caso Existente (El Principito)
            BuscarPorISBNBinario(catalogoOrdenadoISBN, "9780156013987");

            // Caso No Encontrado
            BuscarPorISBNBinario(catalogoOrdenadoISBN, "9780000000000");
        }

        // ===============================================================
        // MÉTODOS DEL DÍA 5
        // ===============================================================

        // 1. Búsqueda Secuencial por Título
        static void BuscarPorTitulo(Titulo[] catalogo, string tituloBuscado)
        {
            Console.WriteLine($"\n[Secuencial] Buscando libro por título: '{tituloBuscado}'...");
            bool encontrado = false;

            for (int i = 0; i < catalogo.Length; i++)
            {
                // Comparación de cadenas ignorando mayúsculas/minúsculas
                if (catalogo[i].Nombre.Equals(tituloBuscado, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"   ¡ÉXITO! Encontrado: {catalogo[i].Nombre}");
                    Console.WriteLine($"   ISBN: {catalogo[i].ISBN} | Préstamos: {catalogo[i].PrestamosHistoricos}");
                    Console.WriteLine($"   Ejemplares disponibles: {catalogo[i].ObtenerDisponibilidad()}/{catalogo[i].Ejemplares.Length}");
                    encontrado = true;
                    break; // Termina la búsqueda al encontrarlo
                }
            }

            if (!encontrado)
            {
                Console.WriteLine($"   RESULTADO: El libro '{tituloBuscado}' NO se encuentra en el catálogo.");
            }
        }

        // Ordenamiento por inserción adaptado para ISBN (necesario para Búsqueda Binaria)
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

        // 2. Búsqueda Binaria Propia por ISBN
        static void BuscarPorISBNBinario(Titulo[] catalogoOrdenado, string isbnBuscado)
        {
            Console.WriteLine($"\n[Binaria] Buscando libro por ISBN: '{isbnBuscado}'...");

            int inicio = 0;
            int fin = catalogoOrdenado.Length - 1;
            bool encontrado = false;

            while (inicio <= fin)
            {
                int medio = inicio + (fin - inicio) / 2;
                int comparacion = string.Compare(catalogoOrdenado[medio].ISBN, isbnBuscado, StringComparison.Ordinal);

                if (comparacion == 0) // ¡Encontrado!
                {
                    Console.WriteLine($"   ¡ÉXITO! Encontrado: {catalogoOrdenado[medio].Nombre}");
                    Console.WriteLine($"   ISBN: {catalogoOrdenado[medio].ISBN}");
                    Console.WriteLine($"   Ejemplares disponibles: {catalogoOrdenado[medio].ObtenerDisponibilidad()}/{catalogoOrdenado[medio].Ejemplares.Length}");
                    encontrado = true;
                    break;
                }

                if (comparacion < 0)
                {
                    inicio = medio + 1; // Buscar en la mitad derecha
                }
                else
                {
                    fin = medio - 1;    // Buscar en la mitad izquierda
                }
            }

            if (!encontrado)
            {
                Console.WriteLine($"   RESULTADO: El libro con ISBN '{isbnBuscado}' NO existe en la biblioteca.");
            }
        }
    }
}