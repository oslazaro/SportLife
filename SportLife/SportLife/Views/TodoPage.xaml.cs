using SportLife.Models;
using SportLife.Views.TodoPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace SportLife.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoPage : ContentPage
    {
        List<Liga> partidosDisponibles;
        public TodoPage()
        {
            InitializeComponent();

            partidosDisponibles = (List<Liga>)App.Current.Properties["listaPartidos"];
            int row = 0;
            List<Liga> partidosFavoritos = partidosDisponiblesFavoritos(partidosDisponibles);
            List<Liga> partidosOtros = partidosDisponiblesOtros(partidosDisponibles);

            

            Grid gridLiga = new Grid();
            gridLiga.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });
            gridLiga.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });
            gridLiga.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90, GridUnitType.Star) });
            if (partidosFavoritos.Count > 0)
            {
                partidosFavoritos.OrderBy(item => item.nombre);
                Label lblFavoritos = new Label { BackgroundColor = Color.FromHex("#DFB651"), Text = " Competiciones destacadas", FontAttributes = FontAttributes.Bold, TextColor = Color.White, VerticalTextAlignment = TextAlignment.Center };

                gridLiga.Children.Add(lblFavoritos, 0, row);
                Grid.SetColumnSpan(lblFavoritos, 2);
                row++;
            }
            foreach (Liga liga in partidosFavoritos)
            {
                gridLiga.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });
                TapGestureRecognizer tgr = new TapGestureRecognizer();
                tgr.Tapped += delegate (object sender2, EventArgs e2)
                {
                    Tgr_Tapped(liga);
                };
                Image bandera = new Image { Source = Liga.nombreIcono(liga.nombre), VerticalOptions = LayoutOptions.Center };
                Label nomLiga = new Label { Text = liga.nombre.ToString(), VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start,VerticalOptions=LayoutOptions.Center };
                BoxView bordeSup = new BoxView { BackgroundColor = Color.LightGray, HeightRequest = 1, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand };
                BoxView bordeInf = new BoxView { BackgroundColor = Color.LightGray, HeightRequest = 1, VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.FillAndExpand };
                BoxView bvFondo = new BoxView { BackgroundColor = Color.Transparent, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
                bvFondo.GestureRecognizers.Add(tgr);
                nomLiga.GestureRecognizers.Add(tgr);
                bandera.GestureRecognizers.Add(tgr);
                gridLiga.Children.Add(bvFondo, 0, row);
                Grid.SetColumnSpan(bvFondo, 2);
                gridLiga.Children.Add(bandera, 0, row);
                gridLiga.Children.Add(nomLiga, 1, row);
                gridLiga.Children.Add(bordeSup, 0, row);
                Grid.SetColumnSpan(bordeSup, 2);
                row++;



            }
            if (partidosOtros.Count > 0)
            {
                gridLiga.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });
                partidosOtros.OrderBy(item => item.nombre);
                Label lblOtros = new Label { BackgroundColor = Color.Black, Text = " Resto de competiciones", FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#DFB651"), VerticalTextAlignment = TextAlignment.Center };
                gridLiga.Children.Add(lblOtros, 0, row);
                Grid.SetColumnSpan(lblOtros, 2);
                row++;
            }
            foreach (Liga liga in partidosOtros)
            {
                gridLiga.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });
                Image bandera = new Image { Source = Liga.nombreIcono(liga.nombre), VerticalOptions = LayoutOptions.Center };
                Label nomLiga = new Label { Text = liga.nombre.ToString(), VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalOptions = LayoutOptions.Center };
                BoxView bordeSup = new BoxView { BackgroundColor = Color.LightGray, HeightRequest = 1, VerticalOptions = LayoutOptions.Start, HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(0, -5, 0, 0) };
                BoxView bordeInf = new BoxView { BackgroundColor = Color.LightGray, HeightRequest = 1, VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(0, -5, 0, 0) };
                BoxView bvFondo = new BoxView { BackgroundColor = Color.Transparent, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand };
                TapGestureRecognizer tgr = new TapGestureRecognizer();
                tgr.Tapped += delegate (object sender2, EventArgs e2)
                {
                    Tgr_Tapped(liga);
                };
                bvFondo.GestureRecognizers.Add(tgr);
                nomLiga.GestureRecognizers.Add(tgr);
                bandera.GestureRecognizers.Add(tgr);
                gridLiga.Children.Add(bvFondo, 0, row);
                Grid.SetColumnSpan(bvFondo, 2);
                gridLiga.Children.Add(bandera, 0, row);
                gridLiga.Children.Add(nomLiga, 1, row);

                gridLiga.Children.Add(bordeSup, 0, row);
                Grid.SetColumnSpan(bordeSup, 2);
                row++;
            }
            gridLigas.Children.Add(gridLiga, 0, 0);

        }

        private void Tgr_Tapped(Liga liga)
        {
            Navigation.PushAsync(new LigasPage(liga),false);
        }

        public List<Liga> partidosDisponiblesFavoritos(List<Liga> todosPartidos)
        {
            List<Liga> partidosFavoritos = new List<Liga>();
            foreach (Liga liga in todosPartidos)
            {
                if (liga.esFavorita())
                {
                    partidosFavoritos.Add(liga);
                }
            }
            return partidosFavoritos;
        }
        public List<Liga> partidosDisponiblesOtros(List<Liga> todosPartidos)
        {
            List<Liga> partidosOtros = new List<Liga>();
            foreach (Liga liga in todosPartidos)
            {
                if (!liga.esFavorita())
                {
                    partidosOtros.Add(liga);
                }
            }
            return partidosOtros;
        }

    }
}