using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportLife
{
    class Alineacion
    {
        List<Jugador> titularesLocales;
        List<Jugador> titularesVisitantes;
        List<Jugador> suplentesLocales;
        List<Jugador> suplentesVisitantes;

        public List<Jugador> TitularesLocales
        {
            get { return titularesLocales; }
            set { titularesLocales = value; }
        }
        public List<Jugador> TitularesVisitantes
        {
            get { return titularesVisitantes; }
            set { titularesVisitantes = value; }
        }
        public List<Jugador> SuplentesLocales
        {
            get { return suplentesLocales; }
            set { suplentesLocales = value; }
        }
        public List<Jugador> SuplentesVisitantes
        {
            get { return suplentesVisitantes; }
            set { suplentesVisitantes = value; }
        }
        public Alineacion(List<Jugador> titularesL,List<Jugador> titularesV,List<Jugador> suplentesL, List<Jugador>suplentesV)
        {
            this.titularesLocales = titularesL;
            this.titularesVisitantes = titularesV;
            this.suplentesLocales = suplentesL;
            this.suplentesVisitantes = suplentesV;
        }
    }
    class Clasificacion
    {
        private List<Fila> clasi;
        private int tamaño;
        public List<Fila> Clasi
        {
            get { return clasi; }
            set { clasi = value; }
        }
        public int Tamaño
        {
            get { return tamaño; }
            set { tamaño = value; }
        }
        public Clasificacion(List<Fila> clasi, int tamaño)
        {
            this.clasi = clasi;
            this.tamaño = tamaño;
        }
    }
        


    class Fila
    {
        string liga;
        string equipo;
        int posicion;
        int puntos;
        int partidosJugados;
        int partidosGanados;
        int partidosEmpatados;
        int partidosPerdidos;
        int golesFavor;
        int golesContra;

        public int Posicion
        {
            get { return posicion; }
            set { posicion = value; }
        }
        public int Puntos
        {
            get { return puntos; }
            set { puntos = value; }
        }
        public int PartidosGanados
        {
            get { return partidosGanados; }
            set { partidosGanados = value; }
        }
        public int PartidosJugados
        {
            get { return partidosJugados; }
            set { partidosJugados = value; }
        }
        public int PartidosEmpatados
        {
            get { return partidosEmpatados; }
            set { partidosEmpatados = value; }
        }
        public int PartidosPerdidos
        {
            get { return partidosPerdidos; }
            set { partidosPerdidos = value; }
        }
        public int GolesFavor
        {
            get { return golesFavor; }
            set { golesFavor = value; }
        }
        public int GolesContra
        {
            get { return golesContra; }
            set { golesContra = value; }
        }
        public string Liga
        {
            get { return liga; }
            set { liga = value; }
        }
        public string Equipo
        {
            get { return equipo; }
            set { equipo = value; }
        }

        public Fila(string liga, string equipo, int posicion, int puntos, int partidosGanados, int partidosEmpatados, int partidosPerdidos, int golesFavor, int golesContra)
        {
            this.liga = liga;
            this.equipo = equipo;
            this.puntos = puntos;
            this.posicion = posicion;
            this.partidosEmpatados = partidosEmpatados;
            this.partidosGanados = partidosGanados;
            this.partidosPerdidos = partidosPerdidos;
            this.golesContra = golesContra;
            this.golesFavor = golesFavor;
        }

    }


    class Info
    {
        string estadio;
        string arbitro;
        string entrenadorL;
        string entrenadorV;
        public string Estadio
        {
            get { return estadio; }
            set { estadio = value; }
        }
        public string Arbitro
        {
            get { return arbitro; }
            set { arbitro = value; }
        }
        public string EntrenadorL
        {
            get { return entrenadorL; }
            set { entrenadorL = value; }
        }
        public string EntrenadorV
        {
            get { return entrenadorV; }
            set { entrenadorV = value; }
        }
        public Info(string estadio, string arbitro, string entrenadorL, string entrenadorV)
        {
            this.estadio = estadio;
            this.arbitro = arbitro;
            this.entrenadorL = entrenadorL;
            this.entrenadorV = entrenadorV;
        }
    }
    class Jugador
    {
        int numero;
        string nombre;
        string posicion;//en caso de que sea titular no se pone la posicion
        public int Numero
        {
            get { return numero; }
            set { numero = value; }
        }
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        public string Posicion
        {
            get { return posicion; }
            set { posicion = value; }
        }
        public Jugador(int n, string nombre, string p)
        {
            this.numero = n;
            this.nombre = nombre;
            this.posicion = p;
        }
    }
}
