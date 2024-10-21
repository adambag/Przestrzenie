using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Przestrzenie
{
    public partial class MainWindow : Window
    {
        private bool isUpdating = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Color_Mode(object sender, SelectionChangedEventArgs e)
        {
            if (ColorSpaceComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedSpace = selectedItem.Content.ToString();
                if (selectedSpace == "RGB")
                {
                    RGBPanel.Visibility = Visibility.Visible;
                    CMYKPanel.Visibility = Visibility.Collapsed;
                }
                else if (selectedSpace == "CMYK")
                {
                    RGBPanel.Visibility = Visibility.Collapsed;
                    CMYKPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private void RGB_Color(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isUpdating) return;

            isUpdating = true;

            RedTextBox.Text = RedSlider.Value.ToString("F0");
            GreenTextBox.Text = GreenSlider.Value.ToString("F0");
            BlueTextBox.Text = BlueSlider.Value.ToString("F0");

            Update_Color();
            RGBtoCMYK();

            isUpdating = false;
        }

        private void RGB_Text(object sender, TextChangedEventArgs e)
        {
            if (isUpdating) return;

            isUpdating = true;

            if (int.TryParse(RedTextBox.Text, out int r))
                RedSlider.Value = Math.Clamp(r, 0, 255);
            if (int.TryParse(GreenTextBox.Text, out int g))
                GreenSlider.Value = Math.Clamp(g, 0, 255);
            if (int.TryParse(BlueTextBox.Text, out int b))
                BlueSlider.Value = Math.Clamp(b, 0, 255);

            Update_Color();
            RGBtoCMYK();

            isUpdating = false;
        }

        private void CMYK_Color(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isUpdating) return;

            isUpdating = true;

            CyanTextBox.Text = CyanSlider.Value.ToString("F0");
            MagentaTextBox.Text = MagentaSlider.Value.ToString("F0");
            YellowTextBox.Text = YellowSlider.Value.ToString("F0");
            BlackTextBox.Text = BlackSlider.Value.ToString("F0");

            CMYKtoRGB();

            isUpdating = false;
        }

        private void CMYK_Text(object sender, TextChangedEventArgs e)
        {
            if (isUpdating) return;

            isUpdating = true;

            if (int.TryParse(CyanTextBox.Text, out int c))
                CyanSlider.Value = Math.Clamp(c, 0, 100);
            if (int.TryParse(MagentaTextBox.Text, out int m))
                MagentaSlider.Value = Math.Clamp(m, 0, 100);
            if (int.TryParse(YellowTextBox.Text, out int y))
                YellowSlider.Value = Math.Clamp(y, 0, 100);
            if (int.TryParse(BlackTextBox.Text, out int k))
                BlackSlider.Value = Math.Clamp(k, 0, 100);

            CMYKtoRGB();

            isUpdating = false;
        }

        private void Update_Color()
        {
            byte r = (byte)RedSlider.Value;
            byte g = (byte)GreenSlider.Value;
            byte b = (byte)BlueSlider.Value;

            ColorPreview.Background = new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        private void RGBtoCMYK()
        {
            double r = RedSlider.Value / 255;
            double g = GreenSlider.Value / 255;
            double b = BlueSlider.Value / 255;

            double k = 1 - Math.Max(r, Math.Max(g, b));
            double c = (1 - r - k) / (1 - k);
            double m = (1 - g - k) / (1 - k);
            double y = (1 - b - k) / (1 - k);

            if (double.IsNaN(c)) c = 0;
            if (double.IsNaN(m)) m = 0;
            if (double.IsNaN(y)) y = 0;

            CyanSlider.Value = c * 100;
            MagentaSlider.Value = m * 100;
            YellowSlider.Value = y * 100;
            BlackSlider.Value = k * 100;
        }

        private void CMYKtoRGB()
        {
            double c = CyanSlider.Value / 100;
            double m = MagentaSlider.Value / 100;
            double y = YellowSlider.Value / 100;
            double k = BlackSlider.Value / 100;

            int r = (int)(255 * (1 - c) * (1 - k));
            int g = (int)(255 * (1 - m) * (1 - k));
            int b = (int)(255 * (1 - y) * (1 - k));

            RedSlider.Value = r;
            GreenSlider.Value = g;
            BlueSlider.Value = b;

            Update_Color();
        }
    }
}
