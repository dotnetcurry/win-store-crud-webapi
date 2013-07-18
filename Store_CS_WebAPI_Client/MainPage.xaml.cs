using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Store_CS_WebAPI_Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

 

        //private async void btncallhttp_Click(object sender, RoutedEventArgs e)
        //{
        //    var http = new HttpClient();
        //    //var sc = new StreamContent();
        //    //var data = await http.GetAsync(new Uri("http://localhost:8090/WPBAPI_IIS/api/Values"));
        //    var data = await http.GetAsync(new Uri("http://localhost:8090/WPBAPI_IIS/api/Values"));
        //    var stream = await data.Content.ReadAsStringAsync();
        //}
    }
}
