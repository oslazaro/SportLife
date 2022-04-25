using System;
using System.Collections.Generic;
using System.Text;

namespace SportLife.Utils
{
    class Cadenas
    {
        public static string borrarCaracterInnecesario(string nombre, string caracterInnecesario)
        {
            string string1 = nombre;
            string string2 = caracterInnecesario;
            if (nombre.Contains(caracterInnecesario))
            {
                string string1_part1 = string1.Substring(0, string1.IndexOf(string2));
                string string1_part2 = string1.Substring(string1.IndexOf(string2) + string2.Length, string1.Length - (string1.IndexOf(string2) + string2.Length));
                return (string1_part1 + string1_part2).TrimStart().TrimEnd();
            }
            else
            {
                return nombre.TrimStart().TrimEnd();
            }


        }
        public static string borrarEspacios(string nombre)
        {
            return nombre.TrimStart().TrimEnd();
        }
    }
}
