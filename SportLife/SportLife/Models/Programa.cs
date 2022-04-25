using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SportLife.Models;

namespace SportLife
{
    class Programa
    {
        public static List<Liga> listaLigas;
        private static bool infoDisponible = false;

        public static List<Liga> getPartidos()
        {

            HtmlWeb hw = new HtmlWeb();
            string path = "http://movil.resultados-futbol.com/";
            listaLigas = new List<Liga>();
            var doc = new HtmlDocument();
            doc.Text = "";
            while (doc.Text.Equals(""))
            {
                try
                {
                    doc = hw.Load(path);
                }
                catch (System.Net.WebException)
                {
                    doc.Text = "";
                }
            }

            var ligas = doc.DocumentNode.SelectSingleNode("//div[@id='tab01']").ChildNodes;
            int i = 0;
            String liga = "";
            foreach (HtmlNode nodo in ligas)
            {
                Liga objetoLiga = new Liga(liga);
                if (compruebaValidezLiga(nodo.ChildNodes["div"]))
                {
                    liga = Utils.Cadenas.borrarCaracterInnecesario(nodo.ChildNodes["div"].ChildNodes["strong"].InnerText, "»");
                    string enlaceLiga = nodo.ChildNodes["div"].ChildNodes["strong"].FirstChild.OuterHtml.Split('"')[1];
                }
                else if (compruebaValidezTable(nodo))
                {
                    HtmlNodeCollection nodos = nodo.SelectNodes("tr");
                    
                    foreach (HtmlNode prt in nodos)
                    {
                        Partido partido = new Partido();

                        //Hacer función para saber si está finalizado o no el resultado.
                        String estado = prt.ChildNodes["td"].ChildNodes["table"].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText;
                        partido.local = prt.ChildNodes["td"].ChildNodes["table"].ChildNodes[2].ChildNodes[1].ChildNodes[2].InnerHtml;
                        String urlPartido = "";
                        if (prt.Attributes.Any(x => x.Name.Equals("data-href") && !x.Equals("")))
                        {
                            infoDisponible = true;
                            urlPartido = prt.Attributes["data-href"].Value;

                        }
                        else
                        {
                            urlPartido = prt.ChildNodes["td"].ChildNodes["table"].ChildNodes[2].Attributes["data-href"].Value;
                        }
                        switch (estado)
                        {
                            case "Hoy":
                                partido.estado = EstadoPartido.SIN_EMPEZAR;
                                partido.resultado = prt.ChildNodes["td"].ChildNodes["table"].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerHtml;
                                break;
                            case "Finalizado":
                                partido.estado = EstadoPartido.FINALIZADO;
                                partido.resultado = prt.ChildNodes["td"].ChildNodes["table"].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerHtml;
                                break;
                            case "Aplazado":
                                partido.estado = EstadoPartido.APLAZADO;
                                partido.resultado = prt.ChildNodes["td"].ChildNodes["table"].ChildNodes[2].ChildNodes[3].ChildNodes[1].ChildNodes[0].InnerHtml;
                                break;
                            default:
                                partido.minuto = estado;
                                partido.estado = EstadoPartido.EN_DIRECTO;
                                partido.resultado = prt.ChildNodes["td"].ChildNodes["table"].ChildNodes[2].ChildNodes[3].ChildNodes[1].InnerHtml;
                                break;
                        }
                        partido.visitante = prt.ChildNodes["td"].ChildNodes["table"].ChildNodes[2].ChildNodes[5].ChildNodes[2].InnerHtml;

                        if (infoDisponible && !urlPartido.Equals(""))
                        {
                            objetoLiga.partidos.Add(getEstadisticasPartido("http://" + urlPartido.Substring(2, urlPartido.Length - 2) + "", partido));
                        }
                        else
                        {
                            objetoLiga.partidos.Add(partido);
                        }

                        if (!listaLigas.Contains(objetoLiga))
                        {
                            listaLigas.Add(objetoLiga);
                        }

                    }

                }

                i++;
            }
            return listaLigas;


        }

        public static List<Liga> getPartidosLive(List<Liga> partidosDisponibles)
        {
            List<Liga> ligaConPartidosLive = new List<Liga>(); ;
            List<Partido> partidosLive = new List<Partido>();
            ligaConPartidosLive = new List<Liga>();
            foreach (Liga liga in partidosDisponibles)
            {
                partidosLive = new List<Partido>();

                foreach (Partido partido in liga.partidos)
                {
                    if (partido.estado.Equals(EstadoPartido.EN_DIRECTO))
                    {
                        partidosLive.Add(partido);
                    }

                }
                if (partidosLive.Count > 0)
                {
                    ligaConPartidosLive.Add(liga);
                }
            }

            return ligaConPartidosLive;
        }
        private static bool compruebaValidezTable(HtmlNode nodo)
        {
            try
            {
                if (nodo.Name.Equals("table") && (nodo.Attributes["class"].Value.Equals("table-results") || nodo.Attributes["class"].Value.Equals("table-results table-results-bets l") || nodo.Attributes["class"].Value.Equals("table-results l")))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        protected static bool compruebaValidezLiga(HtmlNode nodo)
        {
            try
            {
                if (nodo.Name.Equals("div") && !nodo.ChildNodes["strong"].InnerText.Equals(""))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }


        public static Partido getEstadisticasPartido(string url, Partido partido)
        {


            HtmlWeb web = new HtmlWeb();
            try
            {
                var doc = web.Load(url);

            

                var divL = doc.DocumentNode.SelectSingleNode("//div[@class='sb-team team_left']");
                var imagenesL = divL.ChildNodes[1].ChildNodes["a"].ChildNodes["img"];
                partido.localFile = imagenesL.GetAttributeValue("src", "");

                var divV = doc.DocumentNode.SelectSingleNode("//div[@class='sb-team team_right']");
                var imganesV = divV.ChildNodes[1].ChildNodes["a"].ChildNodes["img"];
                partido.visitanteFile = imganesV.GetAttributeValue("src", "");


                var estadisticas = doc.DocumentNode.SelectSingleNode("//div[@class='estadisticas']");

                partido.posesionLocal = estadisticas.ChildNodes[5].ChildNodes["tr"].ChildNodes[1].ChildNodes["span"].InnerHtml;
                partido.posesionVisitante = estadisticas.ChildNodes[5].ChildNodes["tr"].ChildNodes[3].ChildNodes["span"].InnerHtml;

                partido.tirosPuertaLocal = estadisticas.ChildNodes[7].ChildNodes[1].ChildNodes[1].InnerHtml;
                partido.tirosPuertasVisitante = estadisticas.ChildNodes[7].ChildNodes[1].ChildNodes[5].InnerHtml; ;

                partido.tirosFueraLocal = estadisticas.ChildNodes[7].ChildNodes[3].ChildNodes[1].InnerHtml;
                partido.tirosFueraVisitante = estadisticas.ChildNodes[7].ChildNodes[3].ChildNodes[5].InnerHtml; ;

                var totaldedisparoslocal = estadisticas.ChildNodes[7].ChildNodes[5].ChildNodes[1].InnerHtml;
                var totaldedispartosvisitante = estadisticas.ChildNodes[7].ChildNodes[5].ChildNodes[5].InnerHtml;

                partido.paradasLocal = estadisticas.ChildNodes[7].ChildNodes[7].ChildNodes[1].InnerHtml;
                partido.paradasVisitante = estadisticas.ChildNodes[7].ChildNodes[7].ChildNodes[5].InnerHtml;

                partido.cornersLocal = estadisticas.ChildNodes[7].ChildNodes[9].ChildNodes[1].InnerHtml;
                partido.cornersVisitante = estadisticas.ChildNodes[7].ChildNodes[9].ChildNodes[5].InnerHtml;

                partido.fuerasDeJuegoLocal = estadisticas.ChildNodes[7].ChildNodes[11].ChildNodes[1].InnerHtml;
                partido.fuerasDeJuegoVisitante = estadisticas.ChildNodes[7].ChildNodes[11].ChildNodes[5].InnerHtml;

                partido.tarjetasAmarillasLocal = estadisticas.ChildNodes[7].ChildNodes[13].ChildNodes[1].InnerHtml;
                partido.tarjetasAmarillasVisitante = estadisticas.ChildNodes[7].ChildNodes[13].ChildNodes[5].InnerHtml;

                partido.tarjetasRojasLocal = estadisticas.ChildNodes[7].ChildNodes[15].ChildNodes[1].InnerHtml;
                partido.tarjetasRojasVisitante = estadisticas.ChildNodes[7].ChildNodes[15].ChildNodes[5].InnerHtml;

                partido.faltasLocal = estadisticas.ChildNodes[7].ChildNodes[17].ChildNodes[1].InnerHtml;
                partido.faltasVisitante = estadisticas.ChildNodes[7].ChildNodes[17].ChildNodes[5].InnerHtml;




                string infoMinuto = doc.DocumentNode.SelectSingleNode("//ul[@class='board']").ChildNodes[1].ChildNodes[0].InnerText;
                if (infoMinuto.Contains("min."))
                {
                    if (infoMinuto.Contains("Minuto") && !partido.minuto.Contains("Des"))
                    {
                        partido.minuto = Convert.ToInt16(infoMinuto.Substring(5, infoMinuto.Length - 6)) > Convert.ToInt16(partido.minuto.Contains("'") ? partido.minuto.Substring(0, (partido.minuto.Length - 1)) : partido.minuto) ? infoMinuto.Substring(5, infoMinuto.Length - 6) : partido.minuto;
                    }
                    else if (partido.minuto.Contains("Des"))
                    {
                        partido.minuto = "Des";

                    }
                }
           
            if (estadisticas.ChildNodes[7].ChildNodes[19].ChildNodes[3].InnerHtml.Equals("Rechaces"))
            {
                var rechaceslocal = estadisticas.ChildNodes[7].ChildNodes[19].ChildNodes[1].InnerHtml;
                var rechacesvisitante = estadisticas.ChildNodes[7].ChildNodes[19].ChildNodes[5].InnerHtml;
            }
            if (estadisticas.ChildNodes[7].ChildNodes[19].ChildNodes[3].InnerHtml.Equals("Saques de banda"))
            {
                var saquesbandalocal = estadisticas.ChildNodes[7].ChildNodes[19].ChildNodes[1].InnerHtml;
                var saquesbandavisitante = estadisticas.ChildNodes[7].ChildNodes[19].ChildNodes[5].InnerHtml;
            }
            var table = doc.DocumentNode.SelectSingleNode("//div[@id='tab1']");
            int numeroh2 = doc.DocumentNode.SelectNodes("//div[@id='tab1']//h2").Count - 1;
            int clave = 0;
            HtmlNode nodo;
            Clasificacion c = obtenerClasificacion(web, url);
            Console.WriteLine(c.Clasi[2].Liga);
            //var nodoC = doc.DocumentNode.SelectSingleNode("//table[@class='table table-clasificacion tcd']");
            var liga = doc.DocumentNode.SelectSingleNode("//ul[@class='crumbs']");
            nodo = doc.DocumentNode.SelectSingleNode("//strong[@class='logo']");

            int numerodegoles = (nodo.ChildNodes.Count() - 3) / 5;
            // Console.WriteLine(numerodegoles);
            int m = 0;

            int i = 1;
            string actividad = "";
            List<Gol> goles = new List<Gol>();
            List<Tarjeta> tarjetas = new List<Tarjeta>();
            List<Sustitucion> sustitucion = new List<Sustitucion>();
            List<Ocasion> ocasion = new List<Ocasion>();
            List<Otro> otro = new List<Otro>();
            while (i <= numeroh2)
            {
                if (i == 1)
                {
                    actividad = table.ChildNodes[1].InnerHtml;
                    nodo = table.ChildNodes[5];
                }
                if (i == 2)
                {
                    actividad = table.ChildNodes[8].InnerHtml;
                    nodo = table.ChildNodes[12];
                }
                if (i == 3)
                {
                    actividad = table.ChildNodes[15].InnerHtml;
                    nodo = table.ChildNodes[19];
                }
                if (i == 4)
                {
                    actividad = table.ChildNodes[22].InnerHtml;
                    nodo = table.ChildNodes[26];
                }
                if (i == 5)
                {
                    actividad = table.ChildNodes[29].InnerHtml;
                    nodo = table.ChildNodes[33];
                }
                clave = devolverclave(actividad);
                if (clave == 1)//goles
                {
                    goles = buscarGoles(nodo);
                }
                if (clave == 2)//tarjetas
                {
                    tarjetas = buscarTarjeta(nodo);
                }
                if (clave == 3)//sustituciones
                {
                    sustitucion = buscarSustitucion(nodo);
                }
                if (clave == 4)//ocasiones
                {
                    ocasion = buscarOcasion(nodo);
                }
                if (clave == 5)//otros
                {
                    otro = buscarOtro(nodo);
                }
                i++;
            }
            i = 0;

                //Alineacion a = devolverAlineacion(web, url);
                //// string pep = devolverlocal(doc);
                //// Console.WriteLine(devolverlocal(doc));
                ////  Console.WriteLine(devolvervisitante(doc));
                //while (i < 7)
                //{
                //    Console.WriteLine(a.SuplentesLocales[i].Nombre + "  " + a.SuplentesLocales[i].Numero + "  " + a.SuplentesLocales[i].Posicion);

                //    //  Console.WriteLine(otro[i].Jugador);
                //    //   Console.WriteLine(otro[i].Tipo);

                //    //   Console.WriteLine("----------------");
                //    i++;
                //}
                //i = 0;
                //Console.WriteLine("visitantes");
                //while (i < 7)
                //{
                //    Console.WriteLine(a.SuplentesVisitantes[i].Nombre + "  " + a.SuplentesVisitantes[i].Numero + "  " + a.SuplentesVisitantes[i].Posicion);

                //    //  Console.WriteLine(otro[i].Jugador);
                //    //   Console.WriteLine(otro[i].Tipo);

                //    //   Console.WriteLine("----------------");
                //    i++;
                //}


                //goles   tarjetas sustituciones ocasiones goles
                partido.goles = goles;
                partido.sustituciones = sustitucion;
                partido.tarjetas = tarjetas;
            }
            catch (Exception ex)
            {

            }
            return partido;

        }

        protected static string devolverlocal(HtmlDocument doc)
        {
            var div = doc.DocumentNode.SelectSingleNode("//strong[@class='logo']");
            string partido = div.ChildNodes[1].ChildNodes[2].ChildNodes[3].InnerHtml;

            string[] local = partido.Split('-');
            //return local;
            return local[0].Substring(0, local[0].Length - 1);
        }
        protected static string devolvervisitante(HtmlDocument doc)
        {
            var div = doc.DocumentNode.SelectSingleNode("//strong[@class='logo']");
            string partido = div.ChildNodes[1].ChildNodes[2].ChildNodes[3].InnerHtml;

            string[] local = partido.Split('-');
            //return local;
            return local[1].Substring(1);
        }

        protected static int devolverclave(String tipo)
        {
            Dictionary<int, String> diccionario = new Dictionary<int, string>();
            diccionario.Add(1, "GOLES");
            diccionario.Add(2, "TARJETAS");
            diccionario.Add(3, "SUSTITUCIONES");
            diccionario.Add(4, "OCASIONES");
            diccionario.Add(5, "OTROS");
            if (diccionario.ContainsValue(tipo))
            {
                return diccionario.Where(p => p.Value == tipo).FirstOrDefault().Key;
            }
            else
            {
                return 0;
            }
        }
        //otros  asistencia lesioando
        private static List<Ocasion> buscarOcasion(HtmlNode nodo)
        {
            List<Ocasion> ocasion = new List<Ocasion>();
            int numeroOcasion = (nodo.ChildNodes.Count() - 3) / 5; ;
            int m = 0;
            bool local;
            int minuto;
            string tipo = "";//tiro al palo, gol anulado, penalti fallado, penalti parado
            string nombre = "";
            HtmlNode nuevo;
            while ((m) < numeroOcasion)
            {
                nuevo = nodo.ChildNodes[(m * 5) + 5];

                if (nuevo.ChildNodes[1].ChildNodes.Count() == 0)//comprobamos si es esquipo local o no
                {
                    local = false;
                }
                else
                {
                    local = true;
                }
                if (local == true)
                {
                    minuto = Int32.Parse(nuevo.ChildNodes[1].ChildNodes[9].InnerText);
                    tipo = nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[1].InnerText;
                    nombre = nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[3].InnerText;
                }
                else//visitante
                {
                    minuto = Int32.Parse(nuevo.ChildNodes[3].ChildNodes[1].InnerText);
                    tipo = nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[1].InnerText;
                    nombre = nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[3].InnerText;
                }
                Ocasion o = new Ocasion(tipo, minuto, local, nombre);
                ocasion.Add(o);
                m++;
            }

            return ocasion;
        }

        private static Alineacion devolverAlineacion(HtmlWeb web, string direccion)
        {
            Alineacion alineacion;
            string url = direccion + "/alineacion";
            List<Jugador> localesT = new List<Jugador>();
            List<Jugador> visitantesT = new List<Jugador>();
            List<Jugador> localesS = new List<Jugador>();
            List<Jugador> visitantesS = new List<Jugador>();
            var doc = web.Load(url);
            int numeroLocal;
            string jugadorLocal;
            int numeroVisitante;
            string posicionV;
            string posicionL;
            string jugadorVisitante;
            var titularesLocales = doc.DocumentNode.SelectSingleNode("//ul[@class='local ']");
            var titularesSuplentes = doc.DocumentNode.SelectSingleNode("//ul[@class='visitante ']");
            int i = 1;
            while (i < 22)
            {
                numeroLocal = Int32.Parse(titularesLocales.ChildNodes[i].ChildNodes[5].InnerHtml);
                jugadorLocal = titularesLocales.ChildNodes[i].ChildNodes[3].InnerText;
                numeroVisitante = Int32.Parse(titularesSuplentes.ChildNodes[i].ChildNodes[5].InnerHtml);
                jugadorVisitante = titularesSuplentes.ChildNodes[i].ChildNodes[3].InnerText;
                Jugador j = new Jugador(numeroLocal, jugadorLocal, "");
                Jugador jV = new Jugador(numeroVisitante, jugadorVisitante, "");
                localesT.Add(j);
                visitantesT.Add(jV);
                i = i + 2;
            }

            var suplentesL = doc.DocumentNode.SelectSingleNode("//ul[@class='aligns-list aligns-list-x2 team1']");
            var suplentesV = doc.DocumentNode.SelectSingleNode("//ul[@class='aligns-list aligns-list-x2 team2']");
            int numSuplentesL = (suplentesL.ChildNodes.Count - 1) / 2;
            int numSuplentesV = (suplentesV.ChildNodes.Count - 1) / 2;
            if (numSuplentesL == numSuplentesV)
            {
                for (i = 1; i < numSuplentesL * 2; i = i + 2)
                {
                    numeroLocal = Int32.Parse(suplentesL.ChildNodes[i].ChildNodes[5].ChildNodes[1].InnerHtml);
                    jugadorLocal = suplentesL.ChildNodes[i].ChildNodes[3].InnerText;
                    numeroVisitante = Int32.Parse(suplentesV.ChildNodes[i].ChildNodes[5].ChildNodes[1].InnerHtml);
                    jugadorVisitante = suplentesV.ChildNodes[i].ChildNodes[3].InnerText;
                    posicionV = suplentesV.ChildNodes[i].ChildNodes[5].ChildNodes[3].InnerHtml;
                    posicionL = suplentesL.ChildNodes[i].ChildNodes[5].ChildNodes[3].InnerHtml;

                    Jugador j = new Jugador(numeroLocal, jugadorLocal, posicionL);
                    Jugador jV = new Jugador(numeroVisitante, jugadorVisitante, posicionV);
                    localesS.Add(j);
                    visitantesS.Add(jV);
                }
            }
            else
            {
                for (i = 1; i < numSuplentesL * 2; i = i + 2)
                {
                    numeroLocal = Int32.Parse(suplentesL.ChildNodes[i].ChildNodes[5].ChildNodes[1].InnerHtml);
                    jugadorLocal = suplentesL.ChildNodes[i].ChildNodes[3].InnerText;
                    posicionL = suplentesL.ChildNodes[i].ChildNodes[5].ChildNodes[3].InnerHtml;
                    Jugador j = new Jugador(numeroLocal, jugadorLocal, posicionL);
                    localesS.Add(j);
                }
                for (i = 1; i < numSuplentesV; i = i + 2)
                {
                    numeroVisitante = Int32.Parse(suplentesV.ChildNodes[i].ChildNodes[5].ChildNodes[1].InnerHtml);
                    jugadorVisitante = suplentesV.ChildNodes[i].ChildNodes[3].InnerText;
                    posicionV = suplentesV.ChildNodes[i].ChildNodes[5].ChildNodes[3].InnerHtml;
                    Jugador jV = new Jugador(numeroVisitante, jugadorVisitante, posicionV);
                    visitantesS.Add(jV);
                }
            }
            alineacion = new Alineacion(localesT, visitantesT, localesS, visitantesS);
            return alineacion;
        }

        private static List<Sustitucion> buscarSustitucion(HtmlNode nodo)
        {
            List<Sustitucion> sustitucion = new List<Sustitucion>();
            int numerosustitucion = (nodo.ChildNodes.Count() - 3) / 5; ;
            int m = 0;
            bool local;
            int minuto;
            string sale = "";
            string entra = "";
            HtmlNode nuevo;
            while ((m) < numerosustitucion / 2)
            {

                nuevo = nodo.ChildNodes[(m * 10) + 5];

                if (nuevo.ChildNodes[1].ChildNodes.Count() == 0)//comprobamos si es esquipo local o no
                {
                    local = false;
                }
                else
                {
                    local = true;
                }
                if (local == true)
                {
                    minuto = Int32.Parse(nuevo.ChildNodes[1].ChildNodes[9].InnerHtml);
                    if (nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[1].InnerHtml.Equals("Entra"))
                    {
                        entra = nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[3].InnerText;
                        nuevo = nodo.ChildNodes[(m * 10) + 10];
                        sale = nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[3].InnerText;
                    }
                    else
                    {
                        sale = nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[3].InnerText;
                        nuevo = nodo.ChildNodes[(m * 10) + 10];
                        entra = nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[3].InnerText;
                    }
                }
                else//visitante
                {
                    minuto = Int32.Parse(nuevo.ChildNodes[3].ChildNodes[1].InnerHtml);
                    if (nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[1].InnerHtml.Equals("Entra"))
                    {
                        entra = nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[3].InnerText;
                        nuevo = nodo.ChildNodes[(m * 10) + 10];
                        sale = nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[3].InnerText;
                    }
                    else
                    {
                        sale = nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[3].InnerText;
                        nuevo = nodo.ChildNodes[(m * 10) + 10];
                        entra = nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[3].InnerText;
                    }
                }
                Sustitucion s = new Sustitucion(local, minuto, entra, sale);
                sustitucion.Add(s);
                m++;
            }
            return sustitucion;
        }


        private static List<Tarjeta> buscarTarjeta(HtmlNode nodo)
        {
            List<Tarjeta> tarjeta = new List<Tarjeta>();
            int numerotarjetas = (nodo.ChildNodes.Count() - 3) / 5;
            int m = 0;

            bool amarilla2 = false;
            bool local;
            int minuto;
            string nombre;
            bool amarilla = true;
            HtmlNode nuevo;
            while (m < numerotarjetas)
            {
                nuevo = nodo.ChildNodes[(m * 5) + 5];
                if (nuevo.ChildNodes[1].ChildNodes.Count() == 0)//comprobamos si es esquipo local o no
                {
                    local = false;
                }
                else
                {
                    local = true;
                }
                if (local == true)
                {
                    if ("T. Amarilla".Equals(nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[1].InnerHtml))
                    {
                        amarilla = true;
                        amarilla2 = false;
                    }
                    if ("T. Roja".Equals(nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[1].InnerHtml))
                    {
                        amarilla = false;
                        amarilla2 = false;
                    }
                    if ("2a Amarilla y Roja".Equals(nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[1].InnerHtml))
                    {
                        amarilla = false;
                        amarilla2 = true;
                    }
                    nombre = nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[3].InnerText;
                    minuto = Int32.Parse(nuevo.ChildNodes[1].ChildNodes[9].InnerText);
                }
                else//visitante
                {
                    if ((nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[1].InnerHtml).Equals("T. Amarilla"))
                    {
                        amarilla = true;
                        amarilla2 = false;
                    }
                    if ("T. Roja".Equals(nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[1].InnerHtml))
                    {
                        amarilla = false;
                        amarilla2 = false;
                    }
                    if ("2a Amarilla y Roja".Equals(nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[1].InnerHtml))
                    {
                        amarilla = false;
                        amarilla2 = true;
                    }
                    nombre = nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[3].InnerText;
                    minuto = Int32.Parse(nuevo.ChildNodes[3].ChildNodes[1].InnerText);
                }
                Tarjeta t = new Tarjeta(amarilla, nombre, minuto, local, amarilla2);
                tarjeta.Add(t);
                m++;
            }



            return tarjeta;
        }

        protected static List<Otro> buscarOtro(HtmlNode nodo)
        {

            List<Otro> otro = new List<Otro>();

            int numerootro = (nodo.ChildNodes.Count() - 3) / 5;
            int m = 0;

            bool local;
            int minuto = 0;
            string nombre = "";
            string tipo = "";
            HtmlNode nuevo;
            while (m < numerootro)
            {
                nuevo = nodo.ChildNodes[(m * 5) + 5];
                if (nuevo.ChildNodes[1].ChildNodes.Count() == 0)//comprobamos si es esquipo local o no
                {
                    local = false;
                }
                else
                {
                    local = true;
                }
                if (local == true)
                {
                    minuto = Int32.Parse(nuevo.ChildNodes[1].ChildNodes[9].InnerHtml);
                    tipo = nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[1].InnerHtml;
                    nombre = nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[3].InnerText;
                }
                else
                {
                    minuto = Int32.Parse(nuevo.ChildNodes[3].ChildNodes[1].InnerHtml);
                    tipo = nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[1].InnerHtml;
                    nombre = nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[3].InnerText;
                }
                Otro o = new Otro(local, tipo, nombre, minuto);
                otro.Add(o);
                m++;
            }


            return otro;
        }

        protected static Info buscarInfo(HtmlWeb web, string url)
        {
            Info i;
            url = url + "/info";
            var doc = web.Load(url);
            var nodoI = doc.DocumentNode.SelectSingleNode("//div[@class='dm_info']");
            return null;
        }
        protected static Clasificacion obtenerClasificacion(HtmlWeb web, string url)
        {
            Clasificacion c;
            List<Fila> lista = new List<Fila>();
            var doc = web.Load(url);
            //string liga = "";
            string liga = doc.DocumentNode.SelectSingleNode("//ul[@class='crumbs']").ChildNodes[3].ChildNodes[1].InnerHtml;
            liga = liga.Replace(" ", "");
            liga = liga.Replace("\n", "");
            var nodoC = doc.DocumentNode.SelectSingleNode("//table[@class='table table-clasificacion tcd']");
            int tamaño = ((nodoC.ChildNodes.Count() - 3) / 2) - 1;
            int posicion = 1;
            int puntos;
            int golesFavor;
            int golescontra;
            int partidosGanados;
            int partidosPerdidos;
            int partidosEmpatados;
            string equipo;
            int i = 5;

            while (i < nodoC.ChildNodes.Count() - 1)
            {
                equipo = nodoC.ChildNodes[i].ChildNodes[5].ChildNodes[1].InnerText;
                puntos = Int32.Parse(nodoC.ChildNodes[i].ChildNodes[7].InnerText);
                golesFavor = Int32.Parse(nodoC.ChildNodes[i].ChildNodes[17].InnerHtml);
                golescontra = Int32.Parse(nodoC.ChildNodes[i].ChildNodes[19].InnerHtml);
                partidosGanados = Int32.Parse(nodoC.ChildNodes[i].ChildNodes[11].InnerHtml);
                partidosPerdidos = Int32.Parse(nodoC.ChildNodes[i].ChildNodes[15].InnerHtml);
                partidosEmpatados = Int32.Parse(nodoC.ChildNodes[i].ChildNodes[13].InnerHtml);
                Fila f = new Fila(liga, equipo, posicion, puntos, partidosGanados, partidosEmpatados, partidosPerdidos, golesFavor, golescontra);
                lista.Add(f);

                i = i + 2;
                posicion++;
            }
            c = new Clasificacion(lista, tamaño);
            return c;
        }


        protected static List<Gol> buscarGoles(HtmlNode nodo) //devolvemos una lista de goles
        {
            List<Gol> gol = new List<Gol>();
            int numerodegoles = (nodo.ChildNodes.Count() - 3) / 5;
            int m = 0;

            bool local;
            int minuto;
            String nombre;
            int tipo = 0;

            HtmlNode nuevo;
            while (m < numerodegoles)
            {
                nuevo = nodo.ChildNodes[(m * 5) + 5];
                if (nuevo.ChildNodes[1].ChildNodes.Count() == 0)//comprobamos si es esquipo local o no
                {
                    local = false;
                }
                else
                {
                    local = true;
                }
                if (local == true)
                {
                    minuto = Int32.Parse(nuevo.ChildNodes[1].ChildNodes[9].InnerHtml);
                    nombre = nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[3].ChildNodes[0].InnerHtml;
                    if (nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[1].InnerHtml.Equals("Gol de penalti"))
                    {
                        tipo = 2;
                    }
                    if (nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[1].InnerHtml.Equals("Gol"))
                    {
                        tipo = 1;
                    }
                    if (nuevo.ChildNodes[1].ChildNodes[3].ChildNodes[1].InnerHtml.Equals("Gol propia puerta"))
                    {
                        tipo = 1;
                    }
                }
                else
                {
                    minuto = Int32.Parse(nuevo.ChildNodes[3].ChildNodes[1].InnerHtml);
                    nombre = nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[3].ChildNodes[0].InnerHtml;
                    if (nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[1].InnerHtml.Equals("Gol de penalti"))
                    {
                        tipo = 2;
                    }
                    if (nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[1].InnerHtml.Equals("Gol"))
                    {
                        tipo = 1;
                    }
                    if (nuevo.ChildNodes[3].ChildNodes[5].ChildNodes[1].InnerHtml.Equals("Gol propia puerta"))
                    {
                        tipo = 1;
                    }
                }
                Gol g = new Gol(minuto, tipo, nombre, local);
                gol.Add(g);
                m++;
            }
            return gol;
        }
    }





    class Ocasion
    {
        private string tipo;//tiro al palo
        private int minuto;
        private bool local;
        private string nombre;

        public bool Local
        {
            get { return local; }
            set { local = value; }
        }

        public int Minuto
        {
            get { return minuto; }
            set { minuto = value; }
        }
        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public Ocasion(string t, int m, bool l, string n)
        {
            this.tipo = t;
            this.minuto = m;
            this.local = l;
            this.nombre = n;
        }
    }
    public class Sustitucion
    {
        private bool local;
        private int minuto;
        private string entra;
        private string sale;

        public bool Local
        {
            get { return local; }
            set { local = value; }
        }

        public int Minuto
        {
            get { return minuto; }
            set { minuto = value; }
        }

        public string Entra
        {
            get { return entra; }
            set { entra = value; }
        }
        public string Sale
        {
            get { return sale; }
            set { sale = value; }
        }
        public Sustitucion(bool l, int m, string e, string s)
        {
            this.local = l;
            this.minuto = m;
            this.entra = e;
            this.sale = s;
        }
    }

    public class Tarjeta
    {
        private bool amarilla; //si es true la tarjeta es amarilla y false es roja
        private string jugador;
        private int minuto;
        private bool local;
        private bool amarilla2;//si es true la tarjeta es segunda amarilla y por tanto roja 

        public bool Amarilla2
        {
            get { return amarilla2; }
            set { amarilla2 = value; }
        }
        public int Minuto
        {
            get { return minuto; }
            set { minuto = value; }
        }
        public string Jugador
        {
            get { return jugador; }
            set { jugador = value; }
        }
        public bool Amarilla
        {
            get { return amarilla; }
            set { amarilla = value; }
        }
        public bool Local
        {
            get { return local; }
            set { local = value; }
        }
        public Tarjeta(bool a, string j, int m, bool l, bool amarilla2)
        {
            this.amarilla = a;
            this.jugador = j;
            this.minuto = m;
            this.local = l;
            this.amarilla2 = amarilla2;
        }
    }

    class Otro
    {
        private int minuto;
        private string jugador;
        private bool local;
        private string tipo;
        public bool Local
        {
            get { return local; }
            set { local = value; }
        }
        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
        public int Minuto
        {
            get { return minuto; }
            set { minuto = value; }
        }
        public string Jugador
        {
            get { return jugador; }
            set { jugador = value; }
        }
        public Otro(bool l, string t, string j, int m)
        {
            this.local = l;
            this.tipo = t;
            this.minuto = m;
            this.jugador = j;
        }
    }
    public class Gol
    {
        private int minuto;
        public int Minuto
        {
            get { return minuto; }
            set { minuto = value; }
        }
        private int tipo;//gol 1 o gol de penalti 2 o gol el propia 3
        public int Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
        private string jugador;
        public string Jugador
        {
            get { return jugador; }
            set { jugador = value; }
        }
        private bool local; // true equipo local false equipo visitante
        public bool Local
        {
            get { return local; }
            set { local = value; }
        }
        public Gol(int m, int t, string j, bool l)
        {
            this.jugador = j;
            this.minuto = m;
            this.tipo = t;
            this.local = l;
        }
    }
}

