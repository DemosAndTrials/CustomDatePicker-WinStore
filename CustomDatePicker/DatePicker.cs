using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace CustomDatePicker
{
    /// <summary>
    /// Custom Windows Store DatePicker
    /// </summary>
    [TemplatePart(Name = "_DayOptions", Type = typeof(ComboBox))]
    [TemplatePart(Name = "_MonthOptions", Type = typeof(ComboBox))]
    [TemplatePart(Name = "_YearOptions", Type = typeof(ComboBox))]
    public sealed class DatePicker : Control
    {
        // properties
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(DatePicker), new PropertyMetadata(default(DateTime), SelectedDateChangedCallback));
        public static readonly DependencyProperty MinYearProperty = DependencyProperty.Register("MinYear", typeof(int), typeof(DatePicker), new PropertyMetadata(default(int)));
        public static readonly DependencyProperty MaxYearProperty = DependencyProperty.Register("MaxYear", typeof(int), typeof(DatePicker), new PropertyMetadata(default(int)));
        public static readonly DependencyProperty DayOptionFormatProperty = DependencyProperty.Register("DayOptionFormat", typeof(string), typeof(DatePicker), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty MonthOptionFormatProperty = DependencyProperty.Register("MonthOptionFormat", typeof(string), typeof(DatePicker), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty PlaceholderDayProperty = DependencyProperty.Register("PlaceholderDay", typeof(string), typeof(DatePicker), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty PlaceholderMonthProperty = DependencyProperty.Register("PlaceholderMonth", typeof(string), typeof(DatePicker), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty PlaceholderYearProperty = DependencyProperty.Register("PlaceholderYear", typeof(string), typeof(DatePicker), new PropertyMetadata(default(string)));
        // events
        public event EventHandler<SelectedDateChangedEventArgs> SelectedDateChanged;
        // fields
        private readonly ObservableCollection<string> daysInRange = new ObservableCollection<string>();
        private readonly ObservableCollection<string> monthsInRange = new ObservableCollection<string>();
        private readonly ObservableCollection<string> yearsInRange = new ObservableCollection<string>();

        public DatePicker()
        {
            DefaultStyleKey = typeof(DatePicker);

            // set default values
            //SelectedDate = DateTime.Today;
            MinYear = DateTime.Now.Year - 10;
            MaxYear = DateTime.Now.Year + 10;
            // "dd dddd"
            DayOptionFormat = "dd";
            MonthOptionFormat = "MMMM";

            PlaceholderDay = "";
            PlaceholderMonth = "";
            PlaceholderYear = "";
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            monthsInRange.Clear();
            monthsInRange.Add(PlaceholderMonth);
            for (int i = 1; i <= 12; i++)
            {
                DateTime monthStart = new DateTime(DateTime.Now.Year, i, 1);
                monthsInRange.Add(monthStart.ToString(MonthOptionFormat));
            }

            yearsInRange.Clear();

            int minYear = MinYear;
            int maxYear = MaxYear;

            yearsInRange.Add(PlaceholderYear);
            for (int i = minYear; i <= maxYear; i++)
            {
                yearsInRange.Add(i.ToString());
            }

            CreateBindings();
            SetSelectedDate(SelectedDate);

            MonthOptions.SelectedIndex = DateTime.Today.Month;
            YearOptions.SelectedIndex = yearsInRange.IndexOf(DateTime.Today.Year.ToString());

            DayOptions.SelectionChanged += DayOptionsOnSelectionChanged;
            MonthOptions.SelectionChanged += MonthOptionsOnSelectionChanged;
            YearOptions.SelectionChanged += YearOptionsOnSelectionChanged;
        }

        /// <summary>
        /// Set new selected date
        /// </summary>
        /// <param name="newSelectedDate">date</param>
        private void SetSelectedDate(DateTime newSelectedDate)
        {
            if (DayOptions != null && MonthOptions != null && YearOptions != null)
            {

                daysInRange.Clear();

                daysInRange.Add(PlaceholderDay);
                for (int i = 1; i <= DateTime.DaysInMonth(newSelectedDate.Year, newSelectedDate.Month); i++)
                {
                    DateTime date = new DateTime(newSelectedDate.Year, newSelectedDate.Month, i);
                    daysInRange.Add(date.ToString(DayOptionFormat));
                }

                if (newSelectedDate != DateTime.MinValue)
                {
                    DayOptions.SelectedIndex = newSelectedDate.Day;
                    MonthOptions.SelectedIndex = newSelectedDate.Month;
                    YearOptions.SelectedItem = newSelectedDate.Year.ToString();
                }
                else
                {
                    DayOptions.SelectedIndex = 0;
                    MonthOptions.SelectedIndex = 0;
                    YearOptions.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// Create bindings for comboboxes
        /// </summary>
        private void CreateBindings()
        {
            Binding dayOptionsBinding = new Binding { Source = daysInRange, Mode = BindingMode.OneWay };
            DayOptions.SetBinding(ItemsControl.ItemsSourceProperty, dayOptionsBinding);

            Binding monthOptionsBinding = new Binding { Source = monthsInRange, Mode = BindingMode.OneWay };
            MonthOptions.SetBinding(ItemsControl.ItemsSourceProperty, monthOptionsBinding);

            Binding yearOptionsBinding = new Binding { Source = yearsInRange, Mode = BindingMode.OneWay };
            YearOptions.SetBinding(ItemsControl.ItemsSourceProperty, yearOptionsBinding);
        }

        /// <summary>
        /// Update selected date
        /// </summary>
        private void UpdateSelectedDateFromInputs()
        {
            if (YearOptions.SelectedIndex > 0 && MonthOptions.SelectedIndex > 0 && DayOptions.SelectedIndex > 0)
            {
                int year = int.Parse(YearOptions.SelectedValue.ToString());
                int month = MonthOptions.SelectedIndex;
                int day = DayOptions.SelectedIndex;

                int maxDaysInMonth = DateTime.DaysInMonth(year, month);
                if (day > maxDaysInMonth)
                {
                    day = maxDaysInMonth;
                    DayOptions.SelectedIndex = maxDaysInMonth - 1;
                }

                if (month == 0)
                    month = 1;

                if (day == 0)
                    day = 1;

                SelectedDate = new DateTime(year, month, day);
            }
            //some input not selected so reset selected date
            else if (YearOptions.SelectedIndex == 0 || MonthOptions.SelectedIndex == 0 || DayOptions.SelectedIndex == 0)
            {
                SelectedDate = DateTime.MinValue;
            }
        }

        private void UpdateDayOptions()
        {
            int selectedDayIndex = DayOptions.SelectedIndex;
            int month = MonthOptions.SelectedIndex;

            if (month != 0)
            {
                daysInRange.Clear();
                daysInRange.Add(PlaceholderDay);
                for (int i = 1; i <= DateTime.DaysInMonth(SelectedDate.Year, month); i++)
                {
                    DateTime date = new DateTime(SelectedDate.Year, month, i);
                    daysInRange.Add(date.ToString(DayOptionFormat));
                }

                DayOptions.SelectedIndex = selectedDayIndex;
            }
        }

        private void DayOptionsOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            UpdateSelectedDateFromInputs();
        }

        private void MonthOptionsOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            UpdateSelectedDateFromInputs();
            UpdateDayOptions();
        }

        private void YearOptionsOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            UpdateSelectedDateFromInputs();
            UpdateDayOptions();
        }

        private static void SelectedDateChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            DateTime oldValue = (DateTime)args.OldValue;
            DateTime newValue = (DateTime)args.NewValue;

            if (newValue != oldValue)
            {
                DatePicker datePicker = (DatePicker)obj;
                datePicker.SetSelectedDate(newValue);

                if (datePicker.SelectedDateChanged != null)
                    datePicker.SelectedDateChanged(datePicker, new SelectedDateChangedEventArgs(newValue));
            }
        }

        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public int MinYear
        {
            get { return (int)GetValue(MinYearProperty); }
            set { SetValue(MinYearProperty, value); }
        }

        public int MaxYear
        {
            get { return (int)GetValue(MaxYearProperty); }
            set { SetValue(MaxYearProperty, value); }
        }

        public string DayOptionFormat
        {
            get { return (string)GetValue(DayOptionFormatProperty); }
            set { SetValue(DayOptionFormatProperty, value); }
        }

        public string MonthOptionFormat
        {
            get { return (string)GetValue(MonthOptionFormatProperty); }
            set { SetValue(MonthOptionFormatProperty, value); }
        }

        public string PlaceholderDay
        {
            get { return (string)GetValue(PlaceholderDayProperty); }
            set { SetValue(PlaceholderDayProperty, value); }
        }

        public string PlaceholderMonth
        {
            get { return (string)GetValue(PlaceholderMonthProperty); }
            set { SetValue(PlaceholderMonthProperty, value); }
        }

        public string PlaceholderYear
        {
            get { return (string)GetValue(PlaceholderYearProperty); }
            set { SetValue(PlaceholderYearProperty, value); }
        }

        private ComboBox DayOptions
        {
            get { return (ComboBox)GetTemplateChild("_DayOptions"); }
        }

        private ComboBox MonthOptions
        {
            get { return (ComboBox)GetTemplateChild("_MonthOptions"); }
        }

        private ComboBox YearOptions
        {
            get { return (ComboBox)GetTemplateChild("_YearOptions"); }
        }
    }
}
