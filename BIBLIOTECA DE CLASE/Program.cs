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
            // Carga inicial de 8 títulos
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

            // Se ordena inicialmente por préstamos (de mayor a menor) con el algoritmo manual por inserción
            OrdenarPorInsercionPrestamos(catalogo);

            bool salir = false;
            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("=========================================================");
                Console.WriteLine("     SISTEMA DE GESTIÓN DE BIBLIOTECA ESCOLAR (DÍA 6)");
                Console.WriteLine("=========================================================");
                Console.WriteLine("1. Mostrar Catálogo Completo (Ordenado por Préstamos)");
                Console.WriteLine("2. Registrar Préstamo de un Libro");
                Console.WriteLine("3. Buscar Libro por Título (Secuencial)");
                Console.WriteLine("4. Buscar Libro por ISBN (Binaria Propia)");
                Console.WriteLine("5. Ejecutar Prueba de Escritorio Completa (3 Préstamos)");
                Console.WriteLine("6. Salir");
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
                        salir = true;
                        Console.WriteLine("\n¡Saliendo del sistema! ¡Hasta luego!");
                        break;
                    default:
                        Console.WriteLine("\nOpción no válida. Presione cualquier tecla para intentar de nuevo...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // ===============================================================
        // MÉTODOS DE ORDENAMIENTO Y BÚSQUEDA MANUALES
        // ===============================================================

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

        static int BusquedaBinariaISBN(Titulo[] catalogoOrdenadoISBN, string isbnBuscado)
        {
            int inicio = 0;
            int fin = catalogoOrdenadoISBN.Length - 1;

            while (inicio <= fin)
            {
                int medio = inicio + (fin - inicio) / 2;
                int comp = string.Compare(catalogoOrdenadoISBN[medio].ISBN, isbnBuscado, StringComparison.Ordinal);

                if (comp == 0) return medio;
                if (comp < 0) inicio = medio + 1;
                else fin = medio - 1;
            }
            return -1;
        }

        // ===============================================================
        // OPCIONES INTERACTIVAS Y PRUEBA DE ESCRITORIO
        // ===============================================================

        static void MostrarCatalogo(Titulo[] catalogo)
        {
            Console.WriteLine("\n------------------------------------------------------------------");
            Console.WriteLine($"{"ISBN",-15} | {"Título",-25} | {"Préstamos",-10} | {"Disponibles",-12}");
            Console.WriteLine("------------------------------------------------------------------");
            foreach (var t in catalogo)
            {
                Console.WriteLine($"{t.ISBN,-15} | {t.Nombre,-25} | {t.PrestamosHistoricos,-10} | {t.ObtenerDisponibilidad()}/{t.Ejemplares.Length}");
            }
        }

        static void OpcionRegistrarPrestamo(Titulo[] catalogo)
        {
            Console.Write("\nIngrese el título del libro que desea prestar: ");
            string nombre = Console.ReadLine();
            bool encontrado = false;

            for (int i = 0; i < catalogo.Length; i++)
            {
                if (catalogo[i].Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))
                {
                    encontrado = true;
                    if (catalogo[i].RegistrarPrestamo())
                    {
                        Console.WriteLine($"-> ¡Préstamo registrado exitosamente! Copias restantes: {catalogo[i].ObtenerDisponibilidad()}");
                        OrdenarPorInsercionPrestamos(catalogo); // Reordena por cambios en préstamos históricos
                    }
                    else
                    {
                        Console.WriteLine($"-> No hay ejemplares disponibles de '{catalogo[i].Nombre}'.");
                    }
                    break;
                }
            }

            if (!encontrado) Console.WriteLine($"-> El título '{nombre}' no fue encontrado.");
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
                    Console.WriteLine($"\n¡Éxito! Título: {catalogo[i].Nombre} | ISBN: {catalogo[i].ISBN} | Disponibles: {catalogo[i].ObtenerDisponibilidad()}/{catalogo[i].Ejemplares.Length}");
                    encontrado = true;
                    break;
                }
            }
            if (!encontrado) Console.WriteLine($"-> El título '{busqueda}' no existe.");
        }

        static void OpcionBuscarPorISBN(Titulo[] catalogo)
        {
            Console.Write("\nIngrese el ISBN a buscar: ");
            string isbn = Console.ReadLine();

            // Generamos una copia ordenada por ISBN exclusivamente para la búsqueda binaria
            Titulo[] copiaISBN = (Titulo[])catalogo.Clone();
            OrdenarPorISBN(copiaISBN);

            int idx = BusquedaBinariaISBN(copiaISBN, isbn);

            if (idx != -1)
            {
                Console.WriteLine($"\n¡Éxito (Búsqueda Binaria)! Título: {copiaISBN[idx].Nombre} | ISBN: {copiaISBN[idx].ISBN} | Disponibles: {copiaISBN[idx].ObtenerDisponibilidad()}/{copiaISBN[idx].Ejemplares.Length}");
            }
            else
            {
                Console.WriteLine($"-> El ISBN '{isbn}' no fue encontrado en la biblioteca.");
            }
        }

        static void EjecutarPruebaEscritorio(Titulo[] catalogo)
        {
            Console.WriteLine("\n=========================================================");
            Console.WriteLine("    PRUEBA DE ESCRITORIO INTEGRADA (3 PRÉSTAMOS)");
            Console.WriteLine("=========================================================");

            Console.WriteLine("\n--- ESTADO INICIAL DEL CATÁLOGO ---");
            MostrarCatalogo(catalogo);

            Console.WriteLine("\nEjecutando 3 Préstamos de prueba:");
            Console.WriteLine("1. Préstamo de 'Cien Años de Soledad'");
            catalogo[1].RegistrarPrestamo();

            Console.WriteLine("2. Segundo préstamo de 'Cien Años de Soledad'");
            catalogo[1].RegistrarPrestamo();

            Console.WriteLine("3. Préstamo de 'Pedro Páramo'");
            for (int i = 0; i < catalogo.Length; i++)
            {
                if (catalogo[i].Nombre == "Pedro Páramo") catalogo[i].RegistrarPrestamo();
            }

            // Re-ordenamos el catálogo por si cambió la cantidad de préstamos históricos
            OrdenarPorInsercionPrestamos(catalogo);

            Console.WriteLine("\n--- ESTADO FINAL DEL CATÁLOGO DESPUÉS DE LOS PRÉSTAMOS ---");
            MostrarCatalogo(catalogo);
        }

        static void Pausar()
        {
            Console.WriteLine("\nPresione cualquier tecla para volver al menú principal...");
            Console.ReadKey();
        }
    }
}