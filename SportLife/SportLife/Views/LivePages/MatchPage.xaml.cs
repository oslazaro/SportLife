using SportLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLife.Views.LivePages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MatchPage : ContentPage
	{
        public Partido Partido { get; set; }
		public MatchPage ()
		{
			InitializeComponent ();
            
        }
        public MatchPage(Partido p)
        {
            InitializeComponent();

            btnEstadísticas.Clicked += BtnEstadísticas_Clicked;
            btnMatch.Clicked += BtnMatch_Clicked; 
            this.Partido = p;

            BindingContext = this;

            imgLocal.Source = "http:"+Partido.localFile;
            imgVisitante.Source = "http:" + Partido.visitanteFile;

            string aux = "";
            if (Partido.resultado.Contains("-"))
            {
                aux = Partido.resultado;
            }
            else
            {
                aux = "vs";
            }
            ;
            this.Title = Partido.local.Replace(" ", String.Empty).Substring(0, 3).ToUpper() + " " + aux + " " + Partido.visitante.Replace(" ", String.Empty).Substring(0, 3).ToUpper();


            BtnEstadísticas_Clicked(null, null);

        }

        private void BtnMatch_Clicked(object sender, EventArgs e)
        {
            gridContenido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            gridContenido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            int row = 7;
            eliminarDatosGrid(gridContenido);
            //Dictionary<bool,List<object>> listaElementos = new Dictionary<bool, List<Object>>();
            //Partido.goles.ForEach(x => listaElementos.Add(x.Local,new List<object> { x.Minuto, "gol",x.Tipo.ToString()}));
            //Partido.tarjetas.ForEach(x => listaElementos.Add(x.Local, new List<object> { x.Minuto, "tarjeta",x.Amarilla,x.Amarilla2 }));
            //Partido.sustituciones.ForEach(x => listaElementos.Add(x.Local, new List<object> { x.Minuto, "sustitucion", x.Entra,x.Sale }));
            //Dictionary<bool, List<object>> listaElementosAux = new Dictionary<bool, List<Object>>();

            ;
            Label lblGoles = new Label { Text = "Goles", HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            gridContenido.Children.Add(lblGoles, 0, row);
            Grid.SetColumnSpan(lblGoles, 12);
            row++;
            foreach(Gol gol in Partido.goles.OrderBy(x => x.Minuto))
            {
                Label lblGol = new Label { Text = "Gol", HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
                int column = 0;
                if (gol.Local)
                {
                    column = 2;
                }
                else
                {
                    column = 10;
                }
                gridContenido.Children.Add(lblGoles, column, row);
                Grid.SetColumnSpan(lblGoles,2);
                row++;
            }
            
            row++;

        }

        private void BtnEstadísticas_Clicked(object sender, EventArgs e)
        {
            gridContenido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            gridContenido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            gridContenido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            gridContenido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            gridContenido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            gridContenido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            gridContenido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            gridContenido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            gridContenido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            gridContenido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            int row = 7;
            eliminarDatosGrid(gridContenido);
            Label lblPosesionLocal = new Label { Text = Partido.posesionLocal, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblPosesion = new Label { Text = "Posesión", HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblPosesionVisitante = new Label { Text = Partido.posesionVisitante, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };

            gridContenido.Children.Add(lblPosesionLocal, 0, row);
            Grid.SetColumnSpan(lblPosesionLocal, 2);
            gridContenido.Children.Add(lblPosesion, 2, row);
            Grid.SetColumnSpan(lblPosesion, 8);
            gridContenido.Children.Add(lblPosesionVisitante, 10, row);
            Grid.SetColumnSpan(lblPosesionVisitante, 2);
            row++;

            Label lblTirosPuertaLocal = new Label { Text=Partido.tirosPuertaLocal, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblTirosPuerta = new Label { Text = "Tiros a puerta", HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblTirosPuertaVisitante = new Label { Text = Partido.tirosPuertaLocal, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };

            gridContenido.Children.Add(lblTirosPuertaLocal, 0, row);
            Grid.SetColumnSpan(lblTirosPuertaLocal, 2);
            gridContenido.Children.Add(lblTirosPuerta, 2, row);
            Grid.SetColumnSpan(lblTirosPuerta, 8);
            gridContenido.Children.Add(lblTirosPuertaVisitante, 10, row);
            Grid.SetColumnSpan(lblTirosPuertaVisitante, 2);
            row++;

            Label lblTirosFueraLocal = new Label { Text = Partido.tirosFueraLocal, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblTirosFuera = new Label { Text = "Tiros fuera", HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblTirosFueraVisitante = new Label { Text = Partido.tirosFueraVisitante, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };

            gridContenido.Children.Add(lblTirosFueraLocal, 0, row);
            Grid.SetColumnSpan(lblTirosFueraLocal, 2);
            gridContenido.Children.Add(lblTirosFuera, 2, row);
            Grid.SetColumnSpan(lblTirosFuera, 8);
            gridContenido.Children.Add(lblTirosFueraVisitante, 10, row);
            Grid.SetColumnSpan(lblTirosFueraVisitante, 2);
            row++;

            Label lblParadasLocal = new Label { Text = Partido.paradasLocal, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblParadas = new Label { Text = "Paradas", HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblParadasVisitante = new Label { Text = Partido.paradasVisitante, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };

            gridContenido.Children.Add(lblParadasLocal, 0, row);
            Grid.SetColumnSpan(lblParadasLocal, 2);
            gridContenido.Children.Add(lblParadas, 2, row);
            Grid.SetColumnSpan(lblParadas, 8);
            gridContenido.Children.Add(lblParadasVisitante, 10, row);
            Grid.SetColumnSpan(lblParadasVisitante, 2);
            row++;

            Label lblCornersLocal = new Label { Text = Partido.cornersLocal, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblCorners = new Label { Text = "Córners", HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblCornersVisitante = new Label { Text = Partido.cornersVisitante, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };

            gridContenido.Children.Add(lblCornersLocal, 0, row);
            Grid.SetColumnSpan(lblCornersLocal, 2);
            gridContenido.Children.Add(lblCorners, 2, row);
            Grid.SetColumnSpan(lblCorners, 8);
            gridContenido.Children.Add(lblCornersVisitante, 10, row);
            Grid.SetColumnSpan(lblCornersVisitante, 2);
            row++;

            Label lblFaltasLocal = new Label { Text = Partido.faltasLocal, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblFaltas = new Label { Text = "Faltas", HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblFaltasVisitante = new Label { Text = Partido.faltasVisitante, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };

            gridContenido.Children.Add(lblFaltasLocal, 0, row);
            Grid.SetColumnSpan(lblFaltasLocal, 2);
            gridContenido.Children.Add(lblFaltas, 2, row);
            Grid.SetColumnSpan(lblFaltas, 8);
            gridContenido.Children.Add(lblFaltasVisitante, 10, row);
            Grid.SetColumnSpan(lblFaltasVisitante, 2);
            row++;

            Label lblTarjertasAmarillasLocal = new Label { Text = Partido.tarjetasAmarillasLocal, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblTarjetasAmarillas = new Label { Text = "Tarjetas amarillas", HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblTarjetasAmarillasVisitante = new Label { Text = Partido.tarjetasAmarillasVisitante, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };

            gridContenido.Children.Add(lblTarjertasAmarillasLocal, 0, row);
            Grid.SetColumnSpan(lblTarjertasAmarillasLocal, 2);
            gridContenido.Children.Add(lblTarjetasAmarillas, 2, row);
            Grid.SetColumnSpan(lblTarjetasAmarillas, 8);
            gridContenido.Children.Add(lblTarjetasAmarillasVisitante, 10, row);
            Grid.SetColumnSpan(lblTarjetasAmarillasVisitante, 2);
            row++;

            Label lblTarjertasRojasLocal = new Label { Text = Partido.tarjetasRojasLocal, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblTarjetasRojas = new Label { Text = "Tarjetas rojas", HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblTarjetasRojasVisitante = new Label { Text = Partido.tarjetasRojasVisitante, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };

             gridContenido.Children.Add(lblTarjertasRojasLocal, 0, row);
            Grid.SetColumnSpan(lblTarjertasRojasLocal, 2);
            gridContenido.Children.Add(lblTarjetasRojas, 2, row);
            Grid.SetColumnSpan(lblTarjetasRojas, 8);
            gridContenido.Children.Add(lblTarjetasRojasVisitante, 10, row);
            Grid.SetColumnSpan(lblTarjetasRojasVisitante, 2);
            row++;

            Label lblFuerjasDeJuegoLocal = new Label { Text = Partido.fuerasDeJuegoLocal, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblFuerasDeJuego = new Label { Text = "Fueras de juego", HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };
            Label lblFuerasDeJuegoVisitante = new Label { Text = Partido.fuerasDeJuegoVisitante, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center };

            gridContenido.Children.Add(lblFuerjasDeJuegoLocal, 0, row);
            Grid.SetColumnSpan(lblFuerjasDeJuegoLocal, 2);
            gridContenido.Children.Add(lblFuerasDeJuego, 2, row);
            Grid.SetColumnSpan(lblFuerasDeJuego, 8);
            gridContenido.Children.Add(lblFuerasDeJuegoVisitante, 10, row);
            Grid.SetColumnSpan(lblFuerasDeJuegoVisitante, 2);
            row++;






        }

        private void eliminarDatosGrid(Grid gridContenido)
        {
            {
                var children = gridContenido.Children.ToList();
                foreach (var child in children.Where(child => Grid.GetRow(child) > 5))
                {
                    gridContenido.Children.Remove(child);
                }
            }
        }
    }
}