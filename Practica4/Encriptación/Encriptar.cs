using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica4.Encriptación
{
    public class Encriptar
    {
        public string cifrar(string clave, string mensaje)
        {

            List<char> listClave = new List<char>();
            listClave.AddRange(clave);

            int filas = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(mensaje.Length) / Convert.ToDecimal(clave.Length)));
            int columnas = clave.Length;
            char[,] matriz = new char[filas, columnas];
            int y = 0;
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    if (y < mensaje.Length) //Verifica si ya se completó el tamaño de cifrado
                    {
                        matriz[i, j] = mensaje[y];
                        y++;
                    }
                }
            }


            string cifrado = "";

            for (int i = 0; i < listClave.Count; i++)
            {
                int posmenor = 0;
                for (int j = 0; j < listClave.Count - 1; j++)
                {
                    char c = listClave[posmenor];
                    char c2 = listClave[j + 1];
                    if (c > c2) //Entra si encuentra un menor
                    {
                        posmenor = j + 1; //Se actualiza la posición del menor
                    }
                }
                listClave[posmenor] = '|';
                for (int l = 0; l < filas; l++)
                {
                    cifrado += matriz[l, posmenor];

                }
            }
            return cifrado;
        }

        public string descifrar(string clave, string cifrado)
        {
            int filas = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(cifrado.Length) / Convert.ToDecimal(clave.Length)));
            int columnas = clave.Length;
            List<char> listClave = new List<char>();
            listClave.AddRange(clave); //Pasar la clave a una lista de char
            listClave.Sort();
            char[,] matriz = new char[filas, columnas];
            char[,] matriz2 = new char[filas, columnas];

            int y = 0; //Tam de las filas

            //Llenar la matriz de columnas a filas
            for (int i = 0; i < columnas; i++)
            {
                for (int j = 0; j < filas; j++)
                {
                    if (y < cifrado.Length) //Verifica si ya se completó el tamaño de cifrado
                    {
                        matriz[j, i] = cifrado[y];
                        y++;
                    }
                }
            }

            string descifrado = "";
            int pos = 0;
            for (int i = 0; i < columnas; i++)
            {
                pos = listClave.IndexOf(clave[i]); //Buscar posición de la clave
                for (int j = 0; j < filas; j++)
                {
                    matriz2[j, i] = matriz[j, pos];
                }
                listClave[pos] = '|'; //Sutituir para ya no buscarlo de nuevo
            }

            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    if (matriz2[i, j] != 0) //No colocar en el descifrado los espacios en 0
                    {
                        descifrado += matriz2[i, j];
                    }
                }
            }
            return descifrado;

        }
    }
}
