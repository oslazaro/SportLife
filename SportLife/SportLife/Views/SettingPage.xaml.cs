using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SportLife.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingPage : ContentPage
	{
        
		public SettingPage ()
		{
			InitializeComponent ();

            Thread t = new Thread(modificaLabel);
            t.Start();
		}

        private void modificaLabel()
        {
            while (true)
            {
                Thread.Sleep(1000);
                UpdateHour(DateTime.Now.ToString());
            }
        }
        void UpdateHour(string newHour)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                lbl.Text = newHour;
            });
        }
    }
}