using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace CustomDatePicker.Sample
{

    public class Model
    {
        public DateTime dt1 { get; set; }
        public DateTime? dt2 { get; set; }

        public Model()
        {
            dt1 = DateTime.Today;
            dt2 = DateTime.Today;
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Model _defaultViewModel;
        public Model DefaultViewModel
        {
            get { return _defaultViewModel; }
        }

        public MainPage()
        {
            this.InitializeComponent();
            _defaultViewModel = new Model();
           datePicker3.SelectedDate = DateTime.Today.AddDays(10);
        }
    }
}
