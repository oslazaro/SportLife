using SportLife.Models;
using SportLife.Views.TodoPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace SportLife.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LivePage : ContentPage
    {
        public List<Liga> partidosLive;
        public Dictionary<Partido, List<Label>> listaObjetosPartido;
        public List<Label> minutos;

        public LivePage()
        {
            InitializeComponent();

            partidosLive = Programa.getPartidosLive((List<Liga>)App.Current.Properties["listaPartidos"]);
            listaObjetosPartido = new Dictionary<Partido, List<Label>>();
            minutos = new List<Label>();
            this.Title = "En Directo";


            actualizarDatos();
            

        }

        public void actualizarDatos()
        {
            StackLayout stackLayout = new StackLayout();
            ScrollView scroll = new ScrollView();
            scroll.Content = stackLayout;
            this.Content = scroll;
            int row = 0;
            foreach (Liga liga in partidosLive)
            {
                row = 0;
                Grid encabezadoLiga = new Grid();
                encabezadoLiga.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });
                //encabezadoLiga.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1) });
                encabezadoLiga.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });
                encabezadoLiga.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90, GridUnitType.Star) });

                Image bandera = new Image { Source = Liga.nombreIcono(liga.nombre), VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, BackgroundColor = Color.Transparent };
                Label nomLiga = new Label { Text = liga.nombre.ToString(), VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold, BackgroundColor = Color.Transparent, TextColor = Color.White };
                BoxView bvFondoEnc = new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#DFB651") };

                encabezadoLiga.Children.Add(bvFondoEnc, 0, 0);
                Grid.SetColumnSpan(bvFondoEnc, 2);

                encabezadoLiga.Children.Add(bandera, 0, 0);
                encabezadoLiga.Children.Add(nomLiga, 1, 0);

                

                Grid gridPartidos = new Grid();
                gridPartidos.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Grid gridPartido = new Grid();


                gridPartido.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(13, GridUnitType.Star) });
                gridPartido.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(12, GridUnitType.Star) });
                gridPartido.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(45, GridUnitType.Star) });
                gridPartido.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) });
                gridPartido.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) });

                int i = 0;
                foreach (Partido partido in liga.partidos)
                {
                    if (partido.estado.Equals(EstadoPartido.EN_DIRECTO))
                    {
                        List<Label> listaObjetos = new List<Label>();
                        gridPartido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
                        gridPartido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
                        gridPartido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1) });

                        Label lblFav = new Label { Text = "ICN", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center };
                        BoxView bvInferior = new BoxView { BackgroundColor = Color.LightGray, HeightRequest = 1, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.FillAndExpand };
                        BoxView bvFondo = new BoxView { BackgroundColor = Color.Transparent, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
                        TapGestureRecognizer tgr = new TapGestureRecognizer();
                        tgr.Tapped += delegate (object sender2, EventArgs e2)
                        {
                            Tgr_Tapped(partido);
                        };
                        bvFondo.GestureRecognizers.Add(tgr);
                        gridPartido.Children.Add(lblFav, 0, row);
                        Grid.SetRowSpan(lblFav, 2);
                        gridPartido.Children.Add(new Label { Text = "ICN" }, 1, row);
                        gridPartido.Children.Add(new Label { Text = partido.local, VerticalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center }, 2, row);
                        gridPartido.Children.Add(new Label { Text = "ICN" }, 1, row + 1);
                        gridPartido.Children.Add(new Label { Text = partido.visitante, VerticalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center }, 2, row + 1);

                        Label lblMinuto = new Label { Text = partido.minuto, TextColor = Color.Red, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center };
                        gridPartido.Children.Add(lblMinuto, 3, row);
                        Grid.SetRowSpan(lblMinuto, 2);
                        string resultado = Utils.Cadenas.borrarEspacios(partido.resultado);

                        Label lblResultadoLocal = new Label { Text = Utils.Cadenas.borrarEspacios(partido.resultado.Split('-')[0]), VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center };
                        Label lblResultadoVisitante = new Label { Text = Utils.Cadenas.borrarEspacios(partido.resultado.Split('-')[1]), VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center };
                        gridPartido.Children.Add(lblResultadoLocal, 4, row);
                        gridPartido.Children.Add(lblResultadoVisitante, 4, row + 1);

                        if (i != (liga.partidos.Count - 1) && liga.partidos.Count != 1)
                        {
                            gridPartido.Children.Add(bvInferior, 0, row + 2);
                            Grid.SetColumnSpan(bvInferior, 5);
                        }

                        

                        minutos.Add(lblMinuto);
                        listaObjetos.Add(lblMinuto);
                        listaObjetos.Add(lblResultadoLocal);
                        listaObjetos.Add(lblResultadoVisitante);
                        listaObjetosPartido.Add(partido, listaObjetos);
                        gridPartido.Children.Add(bvFondo, 0, row);
                        Grid.SetRowSpan(bvFondo, 2);
                        Grid.SetColumnSpan(bvFondo, 5);
                        gridPartidos.Children.Add(gridPartido, 0, 0);

                        if (liga.partidos.Count > 1)
                        {
                            row += 3;
                        }
                        i++;

                    }
                }
                stackLayout.Children.Add(encabezadoLiga);
                stackLayout.Children.Add(gridPartidos);
            }
            Thread hiloActualizador = new Thread(() => actualizadorMinutos(minutos));
            hiloActualizador.Start();
            Thread hiloActualizadorDatos = new Thread(() => actualizadorDatos());
            hiloActualizadorDatos.Start();
        }

        private void Tgr_Tapped(Partido partido)
        {
            Navigation.PushAsync(new LivePages.MatchPage(partido), false);
        }
        private void actualizadorMinutos(List<Label> minutos)
        {
            while (true)
            {
                Thread.Sleep(1000);
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (Label lbl in minutos)
                    {
                        try
                        {
                            if (!lbl.Text.Contains("Des"))
                            {
                                lbl.Text = lbl.Text.Contains("'") ? lbl.Text.Substring(0, (lbl.Text.Length - 1)) : lbl.Text + "'";
                            }
                        }catch(Exception e) { }
                    }
                });

            }
        }
        private void actualizadorDatos()
        {
            while (true)
            {
                Thread.Sleep((60 - DateTime.Now.Second) * 1000);
                List<Liga> datosActualizados = (List<Liga>)App.Current.Properties["listaPartidos"];
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (Liga liga in Programa.getPartidosLive(datosActualizados))
                    {
                        foreach (Partido partido in liga.partidos)
                        {
                            foreach (KeyValuePair<Partido,List<Label>> entry in listaObjetosPartido)
                            {
                                if (entry.Key.local.Equals(partido.local))
                                {
                                    List<Label> listaObjetos = entry.Value;
                                    listaObjetos[0].Text = partido.minuto;
                                    listaObjetos[1].Text = Utils.Cadenas.borrarEspacios(partido.resultado.Split('-')[0]);
                                    listaObjetos[2].Text = Utils.Cadenas.borrarEspacios(partido.resultado.Split('-')[1]);
                                }
                            }

                        }
                    }
                });

            }
        }
    }
}