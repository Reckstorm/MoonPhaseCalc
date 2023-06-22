using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        public RadialGradientBrush radial = new RadialGradientBrush()
        {
            GradientOrigin = new Point(0, 0.5),
            RadiusY = 1
        };

        public GradientStop stopOne = new GradientStop(Colors.White, 0.0);
        public GradientStop stopTwo = new GradientStop(Colors.DarkGray, 0.0);

        public Border border = new Border()
        {
            BorderThickness = new Thickness(3.0),
            BorderBrush = Brushes.Black,
            CornerRadius = new CornerRadius(150.0),
            Margin = new Thickness(0, 5.0, 0, 5.0),
            MinHeight = 100,
            MinWidth = 100,
        };
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += InitMoon;
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

            // There is no easy way to calculate % of illuminated moon surface,
            // thus I've used the simple formula just to display the idea
            if (Age > 12.91963 && Age < 16.61096) AgePercent = 100;
            else if (Age < 12.91963) AgePercent = Age / 12.91963 * 100;
            else AgePercent = (lunarMonth - Age) / 12.91963 * 100;

            if (Age < 1.84566)
            {
                Phase = "NEW";
                ChangeGradient(Phase);
            }
            else if (Age < 5.53699)
            {
                Phase = "Waxing crescent";
                ChangeGradient(Phase);
            }
            else if (Age < 9.22831)
            {
                Phase = "First quarter";
                ChangeGradient(Phase);
            }
            else if (Age < 12.91963)
            {
                Phase = "Waxing gibbous";
                ChangeGradient(Phase);
            }
            else if (Age < 16.61096)
            {
                Phase = "FULL";
                ChangeGradient(Phase);
            }
            else if (Age < 20.30228) { Phase = "Waning gibbous"; ChangeGradient(Phase); }
            else if (Age < 23.99361) { Phase = "Last quarter"; ChangeGradient(Phase); }
            else if (Age < 27.68493) { Phase = "Waning crescent"; ChangeGradient(Phase); }
            else { Phase = "NEW"; ChangeGradient(Phase); }

            Direction = Age < (lunarMonth / 2) ? "Raising Moon" : "Waning Moon";

            this.MoonAgeDays_lb.Content = Age.ToString();
            SetPercent(AgePercent);
            this.Moonphase_lb.Content = Phase;
            this.Direction_lb.Content = Direction;
        }

        private void InitMoon(object sender, EventArgs e)
        {
            this.RegisterName("GradientStop1", stopOne);
            this.RegisterName("GradientStop2", stopTwo);
            this.radial.GradientStops.Add(stopOne);
            this.radial.GradientStops.Add(stopTwo);
            this.border.Background = radial;
            this.MoonContainer.Children.Add(this.border);
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

        private void OffsetAnimation(Double one, Double two)
        {
            DoubleAnimation animationOne = new DoubleAnimation();
            animationOne.From = radial.GradientStops[0].Offset;
            animationOne.To = one;
            animationOne.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTargetName(animationOne, "GradientStop1");
            Storyboard.SetTargetProperty(animationOne, new PropertyPath(GradientStop.OffsetProperty));

            DoubleAnimation animationTwo = new DoubleAnimation();
            animationTwo.From = radial.GradientStops[1].Offset;
            animationTwo.To = two;
            animationTwo.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTargetName(animationTwo, "GradientStop2");
            Storyboard.SetTargetProperty(animationTwo, new PropertyPath(GradientStop.OffsetProperty));

            Storyboard gradientStopAnimationStoryboard = new Storyboard();
            gradientStopAnimationStoryboard.Children.Add(animationOne);
            gradientStopAnimationStoryboard.Children.Add(animationTwo);
            gradientStopAnimationStoryboard.Begin(this);
        }

        private void ChangeGradient(string value)
        {
            if (value.Equals("NEW")) OffsetAnimation(0, 0);
            else if (value.Equals("Waxing crescent")) OffsetAnimation(0, 0.5);
            else if (value.Equals("First quarter")) OffsetAnimation(0, 1);
            else if (value.Equals("Waxing gibbous")) OffsetAnimation(0.5, 1);
            else if (value.Equals("FULL")) OffsetAnimation(1, 1);
            else if (value.Equals("Waning gibbous")) OffsetAnimation(1, 0);
            else if (value.Equals("Last quarter")) OffsetAnimation(1, 0.5);
            else if (value.Equals("Waning crescent")) OffsetAnimation(1, 0.75);
        }
    }
}
