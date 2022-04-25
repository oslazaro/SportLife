using System;
using System.Collections.Generic;
using System.Text;

namespace SportLife.Models
{
    public enum EstadoPartido
    {
        SIN_EMPEZAR = 0,
        EN_DIRECTO = 1,
        FINALIZADO = 2,
        APLAZADO = 3
    }
    public class Partido
    {
        public EstadoPartido estado { get; set; }
        public string local { get; set; }
        public string localFile { get; set; }
        public Liga liga { get; set; }
        public string visitante { get; set; }
        public string visitanteFile { get; set; }
        public string resultado { get; set; }
        public string minuto { get; set; }
        public List<Gol> goles { get; set; }
        public List<Tarjeta> tarjetas { get; set; }
        public List<Sustitucion> sustituciones { get; set; }
        public string posesionLocal { get; set; }
        public string posesionVisitante { get; set; }
        public string tirosPuertaLocal { get; set; }
        public string tirosPuertasVisitante { get; set; }
        public string tirosFueraLocal { get; set; }
        public string tirosFueraVisitante { get; set; }
        public string paradasLocal { get; set; }
        public string paradasVisitante { get; set; }
        public string cornersLocal { get; set; }
        public string cornersVisitante { get; set; }
        public string faltasLocal { get; set; }
        public string faltasVisitante { get; set; }
        public string tarjetasAmarillasLocal { get; set; }
        public string tarjetasAmarillasVisitante { get; set; }
        public string tarjetasRojasLocal { get; set; }
        public string tarjetasRojasVisitante { get; set; }
        public string fuerasDeJuegoLocal { get; set; }
        public string fuerasDeJuegoVisitante { get; set; }




        public Partido()
        {
            this.liga = new Liga("");
            this.local = "FC Barcelona";
            this.visitante = "Valencia CF";
            this.localFile = "//resfu.thumbr.io/img_data/escudos/medium/429.jpg";
            this.visitanteFile = "//resfu.thumbr.io/img_data/escudos/medium/2647.jpg";
            this.minuto = "78";
            this.resultado = "1-2";
            this.estado = EstadoPartido.EN_DIRECTO;
            this.posesionLocal = "78%";
            this.posesionVisitante = "22%";
            this.tirosPuertaLocal = "6";
            this.tirosPuertasVisitante = "4";
            this.tirosFueraLocal = "7";
            this.tirosFueraVisitante = "4";
            this.paradasLocal = "1";
            this.paradasVisitante = "5";
            this.cornersLocal = "9";
            this.cornersVisitante = "2";
            this.faltasLocal = "8";
            this.faltasVisitante = "13";
            this.tarjetasAmarillasLocal = "2";
            this.tarjetasAmarillasVisitante = "2";
            this.tarjetasRojasLocal = "0";
            this.tarjetasRojasVisitante = "0";
            this.fuerasDeJuegoLocal = "1";
            this.fuerasDeJuegoVisitante = "0";
            this.goles = new List<Gol>();
            this.sustituciones = new List<Sustitucion>();
            this.tarjetas = new List<Tarjeta>();

        }
        public String ToString()
        {
            return local + " >> " + resultado + " << " + visitante;
        }
    }
}
