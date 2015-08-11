using System;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CustomDatePicker.Sample
{

    public class Model : INotifyPropertyChanged
    {
        private DateTime _dt1;
        public DateTime Date1
        {
            get { return _dt1; }
            set { _dt1 = value; NotifyPropertyChanged("Date1"); }
        }
        private DateTime? _dt2;

        public DateTime? Date2
        {
            get { return _dt2; }
            set { _dt2 = value; NotifyPropertyChanged("Date2"); }
        }

        public Model()
        {
            //Date1 = DateTime.MinValue;
            //Date2 = DateTime.Today;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                try { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
                catch (Exception) { }
            }
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
            this.DataContext = DefaultViewModel;
           //datePicker3.SelectedDate = new DateTime(2016,3,8);
            //datePicker2.SelectedDate = new DateTime(2016, 3, 8);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter is DateTime)
                DefaultViewModel.Date1 = (DateTime)e.Parameter;
            //DefaultViewModel.Date2 = new DateTime(2016, 05, 14);
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPage1));
        }
    }
}
