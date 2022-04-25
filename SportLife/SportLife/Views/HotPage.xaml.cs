using SportLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLife.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HotPage : ContentPage
    {
        public HotPage()
        {
            InitializeComponent();
            //this.Title = liga.nombre;
            BindingContext = this;

            this.Title = "Partidos calientes";
            //this.PropertyChanged += LigasPage_PropertyChanged;

            Grid encabezadoLiga = new Grid();
            encabezadoLiga.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });
            //encabezadoLiga.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1) });
            encabezadoLiga.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });
            encabezadoLiga.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90, GridUnitType.Star) });
            Image bandera = new Image { Source = "pais.png", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, BackgroundColor = Color.Transparent };
            Label nomLiga = new Label { Text = "ESPAÑA: " + "Copa del rey", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, VerticalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold, BackgroundColor = Color.Transparent, TextColor = Color.White };
            BoxView bvFondoEnc = new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.FromHex("#DFB651") };

            encabezadoLiga.Children.Add(bvFondoEnc, 0, 0);
            Grid.SetColumnSpan(bvFondoEnc, 2);

            encabezadoLiga.Children.Add(bandera, 0, 0);
            encabezadoLiga.Children.Add(nomLiga, 1, 0);

            stackContenedor.Children.Add(encabezadoLiga);

            Grid gridPartidos = new Grid();
            gridPartidos.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            stackContenedor.Children.Add(gridPartidos);



            Grid gridPartido = new Grid();
            gridPartido.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(13, GridUnitType.Star) });
            gridPartido.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(12, GridUnitType.Star) });
            gridPartido.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) });
            gridPartido.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });
            gridPartido.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) });
            int row = 0;

            gridPartido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            gridPartido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            gridPartido.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1) });

            Label lblFav = new Label { Text = "ICN", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center };
            BoxView bvInferior = new BoxView { BackgroundColor = Color.LightGray, HeightRequest = 1, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.FillAndExpand };
            BoxView bvFondoPartido = new BoxView { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, BackgroundColor = Color.Transparent };
            TapGestureRecognizer tgr = new TapGestureRecognizer();
            tgr.Tapped += (s, e) =>
            {
                Navigation.PushAsync(new LivePages.MatchPage(new Partido()));
            };
            bvFondoPartido.GestureRecognizers.Add(tgr);
            gridPartido.Children.Add(lblFav, 0, row);
            Grid.SetRowSpan(lblFav, 2);
            gridPartido.Children.Add(new Label { Text = "ICN" }, 1, row);
            gridPartido.Children.Add(new Label { Text = "Barcelona", VerticalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center }, 2, row);
            gridPartido.Children.Add(new Label { Text = "ICN" }, 1, row + 1);
            gridPartido.Children.Add(new Label { Text = "Valencia", VerticalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center }, 2, row + 1);


            Label lblMinuto = new Label { Text = "78'", TextColor = Color.Red, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center };
            gridPartido.Children.Add(lblMinuto, 3, row);
            Grid.SetRowSpan(lblMinuto, 2);
            gridPartido.Children.Add(new Label { Text = "1", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center }, 4, row);
            gridPartido.Children.Add(new Label { Text = "2", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center }, 4, row + 1);

            gridPartido.Children.Add(bvInferior, 0, row + 2);
            Grid.SetColumnSpan(bvInferior, 5);
            gridPartido.Children.Add(bvFondoPartido, 0, 0);
            Grid.SetRowSpan(bvFondoPartido, 2);
            Grid.SetColumnSpan(bvFondoPartido, 5);
            row = row + 3;

            gridPartidos.Children.Add(gridPartido, 0, 0);

        }

    }
}
