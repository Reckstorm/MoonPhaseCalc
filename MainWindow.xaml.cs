using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoonPhaseCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double PI { get; set; } = 3.1415926535897932385;
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private double Normalize(double value)
        {
            double tempVal = value - Math.Floor(value);
            return tempVal < 0? tempVal+1 : tempVal;
        }

        private void SetPercent(double value)
        {
            this.MoonAgePercent_lb.Content = Math.Round(value, (int)this.Precision_sl.Value);
        }

        private void MoonPositionNew(DateTime date)
        {
            int DefaultYear = 2017;
            int step = 11;
            int LunarNumber = Math.Abs(date.Year - DefaultYear);
            if (LunarNumber % 3 != 0) LunarNumber *= step;
            double lunarMonth = 29.530588853;
            double Age, AgePercent;
            string Phase, Direction;

            Age = date.Day + date.Month + LunarNumber;
            Age = Age > 30 ? Age - 30 : Age;
            if (Age > 12.91963 && Age < 16.61096) AgePercent = 100;
            else if (Age < 12.91963) AgePercent = Age / 12.91963 * 100;
            else AgePercent = (lunarMonth - Age) / 12.91963 * 100;

            if (Age < 1.84566) Phase = "NEW";
            else if (Age < 5.53699) Phase = "Waxing crescent";
            else if (Age < 9.22831) Phase = "First quarter";
            else if (Age < 12.91963) Phase = "Waxing gibbous";
            else if (Age < 16.61096) Phase = "FULL";
            else if (Age < 20.30228) Phase = "Waning gibbous";
            else if (Age < 23.99361) Phase = "Last quarter";
            else if (Age < 27.68493) Phase = "Waning crescent";
            else Phase = "NEW";

            Direction = Age < (lunarMonth / 2) ? "Raising Moon" : "Waning Moon";

            this.MoonAgeDays_lb.Content = Age.ToString();
            SetPercent(AgePercent);
            this.Moonphase_lb.Content = Phase;
            this.Direction_lb.Content = Direction;
        }

        private void Precision_sl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Precision_lb.Content = $"Precision: {this.Precision_sl.Value.ToString()}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MoonPositionNew(this.Date_dp.SelectedDate.Value);
        }

        private void Date_dp_Loaded(object sender, RoutedEventArgs e)
        {
            this.Date_dp.SelectedDate = DateTime.Now;
        }
    }
}
