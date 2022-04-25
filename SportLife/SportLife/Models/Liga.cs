using System;
using System.Collections.Generic;
using System.Text;

namespace SportLife.Models
{
    
    public class Liga
    {
        public String nombre { get; set; }
        public List<Partido> partidos;
        public List<string> competicionesDestacadas;
        public static Dictionary<string, string> ligasXML;
        public Liga(string nom)
        {
            this.nombre = nom;
            this.partidos = new List<Partido>();
            competicionesDestacadas = new List<string> { "Liga Rumana", "Liga Polaca", "Championship", "Botola Pro","Europa League" };
            ligasXML = new Dictionary<string, string> { ["Liga Suiza"] = "suiza.png", ["Copa Usa"] = "estadosunidos.png", ["Copa COSAFA"] = "mundo.png", ["Europa League"] = "unioneuropea.png", ["Conmebol Sudamerica"] = "mundo.png", ["Liga Ucraniana"] = "ucrania.png", ["1ª Regional Madrid"] = "espana.png" };
        }
        public bool esFavorita()
        {
            if(competicionesDestacadas.Contains(Utils.Cadenas.borrarCaracterInnecesario((this.nombre), "»")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string nombreIcono(string nombreLiga)
        {
            
            return (ligasXML.ContainsKey(nombreLiga) ? ligasXML[nombreLiga] : "mundo.png");
        }
    }
}
