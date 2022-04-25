using SportLife.Models;
using SportLife.Views.TodoPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace SportLife.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : TabbedPage
    {
        public HomePage ()
        {
            if (!App.Current.Properties.Any(x => x.Key.Equals("listaPartidos")))
            {
                List<Liga> listaPartidos = Programa.getPartidos();
                App.Current.Properties.Add("listaPartidos", listaPartidos);
            }

            Thread hiloActualizadorDatos = new Thread(() => actualizadorDatos());
            hiloActualizadorDatos.Start();

            InitializeComponent();

            
            this.CurrentPageChanged += HomePage_CurrentPageChanged;
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            this.CurrentPage = this.Children[2];
            this.BarBackgroundColor = Color.Black;
            this.BarTextColor = Color.FromHex("#DFB651");



        }
        private void actualizadorDatos()
        {
            while (true)
            {
                Thread.Sleep((60 - DateTime.Now.Second) * 1000);
                App.Current.Properties["listaPartidos"] = Programa.getPartidos();
            }
        }
        private void HomePage_CurrentPageChanged(object sender, EventArgs e)
        {
            this.Title = CurrentPage.Title;
        }
    }
}