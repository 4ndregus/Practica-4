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
using Practica4.Encriptación;

namespace Practica4.Helpers
{
    internal class Program
    {
        public static AVL<Persona> AVLDpi;
        public static Encriptar encriptar = new Encriptar();
        static void Main(string[] args)
        {
            string ubicacionArchivo = "C:\\Users\\agust\\OneDrive - Universidad Rafael Landivar\\URL\\6) Segundo Ciclo 2022\\Estructura de datos II\\Practica-4\\Practica4\\input.csv";
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
                    //Buscar las conversaciones para cada persona
                    DirectoryInfo dir = new DirectoryInfo(@"C:\Users\agust\OneDrive - Universidad Rafael Landivar\URL\6) Segundo Ciclo 2022\Estructura de datos II\Practica-4\Practica4\inputs");
                    foreach (var value in dir.GetFiles($"CONV-{nuevaPersona.dpi}-?.txt"))
                    {
                        nuevaPersona.conversaciones.Add(i + ": " + value.Name);
                        i++;
                    }

                    //A cada persona se le inserta un arreglo de todas las conversaciones que se encontraron
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
                    //Buscar las conversaciones para cada persona
                    DirectoryInfo dir = new DirectoryInfo(@"C:\Users\agust\OneDrive - Universidad Rafael Landivar\URL\6) Segundo Ciclo 2022\Estructura de datos II\Practica-4\Practica4\inputs");
                    foreach (var value in dir.GetFiles($"CONV-{nuevaPersona.dpi}-?.txt"))
                    {
                        nuevaPersona.conversaciones.Add(i + ": " + value.Name);
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

                    if (opcion == 1) //Cifrar conversaciones
                    {
                        string clave;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Ingresa clave de cifrado: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        clave = Console.ReadLine();

                        //Buscar las conversaciones para cada persona
                        DirectoryInfo dir = new DirectoryInfo(@"C:\Users\agust\OneDrive - Universidad Rafael Landivar\URL\6) Segundo Ciclo 2022\Estructura de datos II\Practica-4\Practica4\inputs");
                        foreach (var item in dir.GetFiles($"CONV-{dpi}-?.txt"))
                        {
                            string cifrado = "";

                            string textfile = $@"C:\Users\agust\OneDrive - Universidad Rafael Landivar\URL\6) Segundo Ciclo 2022\Estructura de datos II\Practica-4\Practica4\inputs\{item.Name}";
                            string guardar = $@"C:\Users\agust\OneDrive - Universidad Rafael Landivar\URL\6) Segundo Ciclo 2022\Estructura de datos II\Practica-4\Practica4\encryption\{item.Name}";

                            //Extraer texto del archivo
                            string text = "";
                            if (File.Exists(textfile)) { text = File.ReadAllText(textfile); }

                            //Cifrar la conversación
                            cifrado = encriptar.cifrar(clave, text);

                            File.WriteAllText(guardar, cifrado);

                        }

                        Console.Clear();
                        goto Menu;

                    }
                    else if(opcion == 2) //Descifrar las conversaciones
                    {
                        string clave;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Ingresa clave de cifrado: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        clave = Console.ReadLine();

                        //Buscar las conversaciones para cada persona
                        DirectoryInfo dir = new DirectoryInfo(@"C:\Users\agust\OneDrive - Universidad Rafael Landivar\URL\6) Segundo Ciclo 2022\Estructura de datos II\Practica-4\Practica4\encryption");
                        foreach (var item in dir.GetFiles($"CONV-{dpi}-?.txt"))
                        {
                            string descifrado = "";

                            string textfile = $@"C:\Users\agust\OneDrive - Universidad Rafael Landivar\URL\6) Segundo Ciclo 2022\Estructura de datos II\Practica-4\Practica4\encryption\{item.Name}";
                            string guardar = $@"C:\Users\agust\OneDrive - Universidad Rafael Landivar\URL\6) Segundo Ciclo 2022\Estructura de datos II\Practica-4\Practica4\decryption\{item.Name}";

                            //Extraer texto del archivo
                            string text = "";
                            if (File.Exists(textfile)) { text = File.ReadAllText(textfile); }

                            //Cifrar la conversación
                            descifrado = encriptar.descifrar(clave, text);

                            File.WriteAllText(guardar, descifrado);

                        }

                        Console.Clear();
                        goto Menu;

                    }
                    else if(opcion == 3) //Ver conversaciones cifradas
                    {
                    pedirConv:
                        string conversacion = "";
                        int opcConv;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Selecciona conversación: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        opcConv = int.Parse(Console.ReadLine());

                        if (opcConv > AVLDpi.listaBusqueda[0].conversaciones.Count - 1)
                        {
                            Console.WriteLine("Selecciona una conversación dentro del rango");
                            goto pedirConv;
                        }
                        conversacion = AVLDpi.listaBusqueda[0].conversaciones[opcConv].Substring(AVLDpi.listaBusqueda[0].conversaciones[opcConv].IndexOf(":") + 2);
                        string ruta = $@"C:\Users\agust\OneDrive - Universidad Rafael Landivar\URL\6) Segundo Ciclo 2022\Estructura de datos II\Practica-4\Practica4\encryption\{conversacion}";
                        Process.Start("explorer.exe", ruta);
                        Console.Clear();
                        goto Menu;

                    }
                    else if(opcion == 4) //Ver conversaciones descifradas
                    {
                    pedirConv:
                        string conversacion = "";
                        int opcConv;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Selecciona conversación: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        opcConv = int.Parse(Console.ReadLine());

                        if (opcConv > AVLDpi.listaBusqueda[0].conversaciones.Count - 1)
                        {
                            Console.WriteLine("Selecciona una conversación dentro del rango");
                            goto pedirConv;
                        }
                        conversacion = AVLDpi.listaBusqueda[0].conversaciones[opcConv].Substring(AVLDpi.listaBusqueda[0].conversaciones[opcConv].IndexOf(":") + 2);
                        string ruta = $@"C:\Users\agust\OneDrive - Universidad Rafael Landivar\URL\6) Segundo Ciclo 2022\Estructura de datos II\Practica-4\Practica4\decryption\{conversacion}";
                        Process.Start("explorer.exe", ruta);
                        Console.Clear();
                        goto Menu;

                    }
                    else if (opcion == 5) //Seguir buscando
                    {
                        Buscar();
                    }
                    else if (opcion == 6) //Salir
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.WriteLine("Opción no válida");
                        goto Menu;
                    }
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
