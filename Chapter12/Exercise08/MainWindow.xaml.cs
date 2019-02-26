using System;
using System.Windows;

namespace Exercise08
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            areaTextBlock.Text = string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int sideA = Convert.ToInt32(sideATextBox.Text);
            int sideB = Convert.ToInt32(sideBTextBox.Text);
            int sideC = Convert.ToInt32(sideCTextBox.Text);
            int largestSide = Max(sideA, sideB, sideC);

            // Test driehoek: de grootste zijde moet kleiner zijn dan de som van de twee andere
            if (largestSide == sideA && largestSide >= sideB + sideC)
            {
                errorTextBlock.Text = $"Deze zijden kunnen nooit een driehoek vormen: {largestSide} >= {sideB} + {sideC}";
                return;
            }
            if (largestSide == sideB && largestSide >= sideA + sideC)
            {
                errorTextBlock.Text = $"Deze zijden kunnen nooit een driehoek vormen: {largestSide} >= {sideA} + {sideC}";
                return;
            }
            if (largestSide == sideC && largestSide >= sideB + sideA)
            {
                errorTextBlock.Text = $"Deze zijden kunnen nooit een driehoek vormen: {largestSide} >= {sideB} + {sideA}";
                return;
            }

            double s = (sideA + sideB + sideC) / 2.0; // anders afkapping naar int
            double area = Math.Sqrt(s * (s - sideA) * (s - sideB) * (s - sideC));

            areaTextBlock.Text = $"{area:F3}";
        }

        private int Max(int a, int b, int c)
        {
            int m = a;
            if (b > m)
            {
                m = b;
            }
            if (c > m)
            {
                m = c;
            }
            return m;
        }
    }
}
