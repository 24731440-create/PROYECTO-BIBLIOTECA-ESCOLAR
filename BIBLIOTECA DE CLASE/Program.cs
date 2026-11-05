using System;

namespace BibliotecaEscolar
{
    // Modelo de clase que gestiona sus propios ejemplares sin celdas vacías
    public class Titulo
    {
        public string Nombre { get; set; }
        public string ISBN { get; set; }
        public int PrestamosHistoricos { get; set; }

        // true = Disponible, false = Prestado
        public bool[] Ejemplares { get; set; }

        public Titulo(string nombre, string isbn, int prestamosHistoricos, int cantidadEjemplares)
        {
            Nombre = nombre;
            ISBN = isbn;
            PrestamosHistoricos = prestamosHistoricos;

            // Arreglo dimensionado exactamente a la cantidad de copias
            Ejemplares = new bool[cantidadEjemplares];
            for (int i = 0; i < cantidadEjemplares; i++)
            {
                Ejemplares[i] = true; // Inician disponibles por defecto
            }
        }

        // Método para calcular los ejemplares disponibles
        public int ObtenerDisponibilidad()
        {
            int disponibles = 0;
            for (int i = 0; i < Ejemplares.Length; i++)
            {
                if (Ejemplares[i]) disponibles++;
            }
            return disponibles;
        }

        // Método para registrar el préstamo de un ejemplar
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
            // 1. Carga inicial de 8 títulos con datos de prueba (Día 2)
            Titulo[] catalogo = new Titulo[8]
            {
                new Titulo("Cien Años de Soledad", "9780307474728", 15, 5),
                new Titulo("Don Quijote de la Mancha", "9788424116484", 20, 3),
                new Titulo("El Principito", "9780156013987", 20, 4), // Empate en préstamos para estabilidad
                new Titulo("Pedro Páramo", "9788437604183", 8, 1),
                new Titulo("Rayuela", "9788437604572", 12, 2),
                new Titulo("Ficciones", "9788420633121", 5, 2),
                new Titulo("La Ciudad y los Perros", "9788466332149", 10, 3),
                new Titulo("La Casa de los Espíritus", "9780307474803", 18, 4)
            };

            // Ordenamiento inicial por préstamos de mayor a menor (Día 4)
            OrdenarPorInsercionPrestamos(catalogo);

            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("=========================================================================");
                Console.WriteLine("            SISTEMA DE GESTIÓN DE BIBLIOTECA ESCOLAR - C#");
                Console.WriteLine("=========================================================================");
                Console.WriteLine("1. Mostrar Catálogo (Ordenado por Préstamos de Mayor a Menor)");
                Console.WriteLine("2. Registrar Préstamo de un Libro");
                Console.WriteLine("3. Buscar Libro por Título (Búsqueda Secuencial)");
                Console.WriteLine("4. Buscar Libro por ISBN (Búsqueda Binaria Propia)");
                Console.WriteLine("5. Ejecutar Prueba de Escritorio Completa (3 Préstamos)");
                Console.WriteLine("6. Ejecutar Tabla de Mediciones de Búsqueda Binaria (Día 7)");
                Console.WriteLine("7. Salir");
                Console.Write("\nSeleccione una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        MostrarCatalogo(catalogo);
                        Pausar();
                        break;
                    case "2":
                        OpcionRegistrarPrestamo(catalogo);
                        Pausar();
                        break;
                    case "3":
                        OpcionBuscarPorTitulo(catalogo);
                        Pausar();
                        break;
                    case "4":
                        OpcionBuscarPorISBN(catalogo);
                        Pausar();
                        break;
                    case "5":
                        EjecutarPruebaEscritorio(catalogo);
                        Pausar();
                        break;
                    case "6":
                        EjecutarMedicionesDia7(catalogo);
                        Pausar();
                        break;
                    case "7":
                        salir = true;
                        Console.WriteLine("\n¡Gracias por utilizar el sistema!");
                        break;
                    default:
                        Console.WriteLine("\nOpción inválida. Presione cualquier tecla para intentar de nuevo...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // =========================================================================
        // ALGORITMOS REQUERIDOS (ORDENAMIENTO E INSERCIÓN MANUAL)
        // =========================================================================

        // Ordenamiento por Inserción Manual por Préstamos (Mayor a Menor)
        static void OrdenarPorInsercionPrestamos(Titulo[] arreglo)
        {
            for (int i = 1; i < arreglo.Length; i++)
            {
                Titulo clave = arreglo[i];
                int j = i - 1;

                while (j >= 0 && arreglo[j].PrestamosHistoricos < clave.PrestamosHistoricos)
                {
                    arreglo[j + 1] = arreglo[j];
                    j--;
                }
                arreglo[j + 1] = clave;
            }
        }

        // Ordenamiento por Inserción Manual por ISBN (Ascendente)
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

        // Búsqueda Binaria Propia por ISBN con contador de comparaciones
        static int BusquedaBinariaISBN(Titulo[] arreglo, string isbn, out int comparaciones)
        {
            comparaciones = 0;
            int inicio = 0;
            int fin = arreglo.Length - 1;

            while (inicio <= fin)
            {
                int medio = inicio + (fin - inicio) / 2;
                comparaciones++;
                int comp = string.Compare(arreglo[medio].ISBN, isbn, StringComparison.Ordinal);

                if (comp == 0) return medio;
                if (comp < 0) inicio = medio + 1;
                else fin = medio - 1;
            }
            return -1;
        }

        // =========================================================================
        // FUNCIONALIDADES DEL MENÚ
        // =========================================================================

        static void MostrarCatalogo(Titulo[] catalogo)
        {
            Console.WriteLine("\n-------------------------------------------------------------------------");
            Console.WriteLine($"{"ISBN",-15} | {"Título",-25} | {"Préstamos",-10} | {"Disponibles",-12}");
            Console.WriteLine("-------------------------------------------------------------------------");
            foreach (var t in catalogo)
            {
                Console.WriteLine($"{t.ISBN,-15} | {t.Nombre,-25} | {t.PrestamosHistoricos,-10} | {t.ObtenerDisponibilidad()}/{t.Ejemplares.Length}");
            }
        }

        static void OpcionRegistrarPrestamo(Titulo[] catalogo)
        {
            Console.Write("\nIngrese el título del libro a prestar: ");
            string nombre = Console.ReadLine();
            bool encontrado = false;

            for (int i = 0; i < catalogo.Length; i++)
            {
                if (catalogo[i].Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))
                {
                    encontrado = true;
                    if (catalogo[i].RegistrarPrestamo())
                    {
                        Console.WriteLine($"-> ¡Préstamo realizado! Quedan {catalogo[i].ObtenerDisponibilidad()} copias disponibles.");
                        OrdenarPorInsercionPrestamos(catalogo); // Mantener el catálogo ordenado por préstamos
                    }
                    else
                    {
                        Console.WriteLine($"-> No quedan ejemplares disponibles de '{catalogo[i].Nombre}'.");
                    }
                    break;
                }
            }

            if (!encontrado) Console.WriteLine($"-> No se encontró ningún libro titulado '{nombre}'.");
        }

        static void OpcionBuscarPorTitulo(Titulo[] catalogo)
        {
            Console.Write("\nIngrese el título a buscar: ");
            string busqueda = Console.ReadLine();
            bool encontrado = false;

            for (int i = 0; i < catalogo.Length; i++)
            {
                if (catalogo[i].Nombre.Equals(busqueda, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"\n¡ÉXITO! Encontrado: {catalogo[i].Nombre}");
                    Console.WriteLine($"ISBN: {catalogo[i].ISBN} | Préstamos: {catalogo[i].PrestamosHistoricos} | Disponibles: {catalogo[i].ObtenerDisponibilidad()}/{catalogo[i].Ejemplares.Length}");
                    encontrado = true;
                    break;
                }
            }
            if (!encontrado) Console.WriteLine($"-> El título '{busqueda}' no existe en el catálogo.");
        }

        static void OpcionBuscarPorISBN(Titulo[] catalogo)
        {
            Console.Write("\nIngrese el ISBN a buscar: ");
            string isbn = Console.ReadLine();

            // Copia ordenada por ISBN exclusivamente para la búsqueda binaria
            Titulo[] copiaISBN = (Titulo[])catalogo.Clone();
            OrdenarPorISBN(copiaISBN);

            int compCount;
            int idx = BusquedaBinariaISBN(copiaISBN, isbn, out compCount);

            if (idx != -1)
            {
                Console.WriteLine($"\n¡ÉXITO (Búsqueda Binaria)! Libro: {copiaISBN[idx].Nombre}");
                Console.WriteLine($"ISBN: {copiaISBN[idx].ISBN} | Disponibles: {copiaISBN[idx].ObtenerDisponibilidad()}/{copiaISBN[idx].Ejemplares.Length}");
                Console.WriteLine($"Comparaciones realizadas: {compCount}");
            }
            else
            {
                Console.WriteLine($"-> El ISBN '{isbn}' no se encuentra registrado.");
            }
        }

        static void EjecutarPruebaEscritorio(Titulo[] catalogo)
        {
            Console.WriteLine("\n=========================================================================");
            Console.WriteLine("    PRUEBA DE ESCRITORIO (CATÁLOGO ANTES Y DESPUÉS DE 3 PRÉSTAMOS)");
            Console.WriteLine("=========================================================================");

            Console.WriteLine("\n--- CATÁLOGO ANTES DE LOS PRÉSTAMOS ---");
            MostrarCatalogo(catalogo);

            Console.WriteLine("\nRealizando 3 préstamos simulados:");
            Console.WriteLine("1. Préstamo de 'Cien Años de Soledad'");
            catalogo[1].RegistrarPrestamo();

            Console.WriteLine("2. Segundo préstamo de 'Cien Años de Soledad'");
            catalogo[1].RegistrarPrestamo();

            Console.WriteLine("3. Préstamo de 'Pedro Páramo'");
            for (int i = 0; i < catalogo.Length; i++)
            {
                if (catalogo[i].Nombre == "Pedro Páramo") catalogo[i].RegistrarPrestamo();
            }

            // Actualizar ordenamiento tras los préstamos
            OrdenarPorInsercionPrestamos(catalogo);

            Console.WriteLine("\n--- CATÁLOGO DESPUÉS DE LOS PRÉSTAMOS ---");
            MostrarCatalogo(catalogo);
        }

        static void EjecutarMedicionesDia7(Titulo[] catalogo)
        {
            Console.WriteLine("\n=========================================================================");
            Console.WriteLine("   MEDICIÓN DE COMPARACIONES DE BÚSQUEDA BINARIA ");
            Console.WriteLine("=========================================================================");

            Titulo[] catalogoISBN = (Titulo[])catalogo.Clone();
            OrdenarPorISBN(catalogoISBN);

            string[] isbnsOrdenados = new string[catalogoISBN.Length];
            for (int i = 0; i < catalogoISBN.Length; i++)
            {
                isbnsOrdenados[i] = catalogoISBN[i].ISBN;
            }

            string[] consultas = new string[5]
            {
                "9780156013987",
                "9788424116484",
                "9788437604183",
                "9780000000000",
                "9789999999999"
            };

            Console.WriteLine($"\n{"ISBN Consulta",-16} | {"Existe",-8} | {"Comparaciones",-15} | {"Array.BinarySearch",-25}");
            Console.WriteLine(new string('-', 72));

            foreach (string isbn in consultas)
            {
                int comp;
                int idxPropio = BusquedaBinariaISBN(catalogoISBN, isbn, out comp);
                int idxOficial = Array.BinarySearch(isbnsOrdenados, isbn);

                bool existe = idxPropio != -1;
                string resOficial = idxOficial >= 0 ? $"Encontrado (idx {idxOficial})" : $"No Encontrado (idx {idxOficial})";

                Console.WriteLine($"{isbn,-16} | {(existe ? "SÍ" : "NO"),-8} | {comp,-15} | {resOficial,-25}");
            }
        }

        static void Pausar()
        {
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}