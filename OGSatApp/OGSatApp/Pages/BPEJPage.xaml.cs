using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OGSatApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BPEJPage : ContentPage
    {
        public BPEJPage()
        {
            InitializeComponent();
        }

        private void EntrBPEJcode_Completed(object sender, EventArgs e)
        {
            DisplayAlert("test", EntrBPEJcode.Text, "Ok");
        }

        private void BttnGetBPEJ_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}