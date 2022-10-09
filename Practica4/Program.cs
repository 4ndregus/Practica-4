using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics;
using System.IO;
using Practica4.Estructura;
using Practica4.Modelo;

namespace Practica4.Helpers
{
    internal class Program
    {
        public static AVL<Persona> AVLDpi;
        //public static Compresion compresion = new Compresion();
        public static List<List<string>> listaCodificados = new List<List<string>>();
        static void Main(string[] args)
        {
            string ubicacionArchivo = "C:\\Users\\agust\\OneDrive - Universidad Rafael Landivar\\URL\\6) Segundo Ciclo 2022\\Estructura de datos II\\Practica-3\\Practica3\\input.csv";
            StreamReader archivo = new StreamReader(ubicacionArchivo);
            string linea;

            AVLDpi = new AVL<Persona>();
            while ((linea = archivo.ReadLine()) != null)
            {
                string[] fila = linea.Split(';'); //Separador

                if (fila[0] == "INSERT")
                {
                    string json = fila[1];
                    Persona nuevaPersona = JsonSerializer.Deserialize<Persona>(json);

                    int i = 0;
                    //Buscar las cartas para cada persona
                    DirectoryInfo dir = new DirectoryInfo(@"C:\Users\agust\OneDrive - Universidad Rafael Landivar\URL\6) Segundo Ciclo 2022\Estructura de datos II\Practica-3\Practica3\inputs");
                    foreach (var value in dir.GetFiles($"REC-{nuevaPersona.dpi}-?.txt"))
                    {
                        nuevaPersona.cartas.Add(i + ": " + value.Name);
                        i++;
                    }

                    //A cada persona se le inserta un arreglo de todas las cartas que se encontraron
                    AVLDpi.insertar(nuevaPersona, nuevaPersona.CompararDpi);
                }
                if (fila[0] == "DELETE")
                {
                    string json = fila[1];
                    Persona nuevaPersona = JsonSerializer.Deserialize<Persona>(json);
                    AVLDpi.eliminar(nuevaPersona, nuevaPersona.CompararDpi);
                }
                if (fila[0] == "PATCH")
                {
                    string json = fila[1];
                    Persona nuevaPersona = JsonSerializer.Deserialize<Persona>(json);
                    Nodo<Persona> nuevoNodo = new Nodo<Persona>(nuevaPersona);

                    int i = 0;
                    //Buscar las cartas para cada persona
                    DirectoryInfo dir = new DirectoryInfo(@"C:\Users\agust\OneDrive - Universidad Rafael Landivar\URL\6) Segundo Ciclo 2022\Estructura de datos II\Practica-3\Practica3\inputs");
                    foreach (var value in dir.GetFiles($"REC-{nuevaPersona.dpi}-?.txt"))
                    {
                        nuevaPersona.cartas.Add(i + ": " + value.Name);
                        i++;
                    }

                    AVLDpi.modificar(nuevoNodo, nuevaPersona.CompararDpi);

                }
            }

            Buscar();
            Console.ReadKey();
        }

        public static void Buscar()
        {
            string dpi;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Ingrese DPI a buscar: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            dpi = Console.ReadLine();

            Persona busqueda = new Persona("", dpi, "", "");
            AVLDpi.buscar(busqueda, busqueda.CompararDpi);

            if (AVLDpi.listaBusqueda.Count() == 0)
            {
                Console.WriteLine("Persona no encontrada");
            }
            else
            {
            Menu:
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"PERSONA ENCONTRADA:\n");

                //Mostar json de la persona
                var options = new JsonSerializerOptions
                {
                    IgnoreReadOnlyProperties = true,
                    WriteIndented = true
                };

                string jsonl = JsonSerializer.Serialize(AVLDpi.listaBusqueda[0], options);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(jsonl);

                try
                {
                    int opcion;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("");
                    Console.WriteLine(" ------------  Menú de Opciones  ----------- ");
                    Console.WriteLine("|         1.Cifrar conversaciones           |");
                    Console.WriteLine("|        2.Descifrar conversaciones         |");
                    Console.WriteLine("|       3.Ver conversaciones cifradas       |");
                    Console.WriteLine("|     4.Ver conversaciones descifradas      |");
                    Console.WriteLine("|             5.Seguir buscando             |");
                    Console.WriteLine("|                  6.Salir                  |");
                    Console.WriteLine(" ------------------------------------------- ");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Selecciona opción: ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    opcion = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Opción no válida");
                    goto Menu;
                }
            }
        }
    }
}
