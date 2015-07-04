using System;
using Windows.UI.Xaml.Controls;

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
