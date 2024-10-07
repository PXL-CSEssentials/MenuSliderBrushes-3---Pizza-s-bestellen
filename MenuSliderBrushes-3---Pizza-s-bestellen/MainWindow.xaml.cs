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
using System.Windows.Threading;

namespace MenuSliderBrushes_3___Pizza_s_bestellen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double PriceQuattroStagioni = 12.5;
        const double PriceCapricciosa = 13;
        const double PriceSalami = 12;
        const double PriceProsciutto = 11;
        const double PriceQuattroFromaggi = 12.5;
        const double PriceHawai = 12;
        const double PriceMargherita = 9;
        const double PriceExtraMozzarella = 0.5;
        const double PriceExtraSalami = 0.5;
        const double PriceExtraAnsjovis = 0.5;
        const double PriceExtraArtisjok = 0.5;

        TextBox _textbox;
        Slider _slider;
        Button _minusButton;
        Button _plusButton;
        Random _random = new Random();
        int _totalNumber = 0;

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            pizzaImage.Source = new BitmapImage(new Uri($"/fotos/Pizza{_random.Next(1, 4)}.jpg", UriKind.RelativeOrAbsolute));
        }

        private string PizzaOrder(ref double totalPrice, string type, int numberOfPizzas, double price)
        {
            if (numberOfPizzas > 0)
            {
                totalPrice += numberOfPizzas * price;
                _totalNumber += numberOfPizzas;
                return $"{numberOfPizzas} x €{price} - {type}\n";
            }
            return "";
        }

        private void PlaceOrder(object sender, RoutedEventArgs e)
        {
            double totalPrice = 0;
            _totalNumber = 0;
            string result = "";
            result += $"Naam: {nameTextBox.Text}\n";
            result += $"Telefoonnummer: {phoneTextBox.Text}\n";
            result += $"E-mail: {mailTextBox.Text}\n";
            result += $"Adres: {addressTextBox.Text}\n";
            result += $"Woonplaats: {cityTextBox.Text} - {zipcodeTextBox.Text}\n";
            result += "\nU heeft de volgende pizza's besteld\n-------------------------\n";
            result += PizzaOrder(ref totalPrice, "Quattro Stagioni", int.Parse(quattroStagioniTextBox.Text), PriceQuattroStagioni);
            result += PizzaOrder(ref totalPrice, "Capricciosa", int.Parse(capricciosaTextBox.Text), PriceCapricciosa);
            result += PizzaOrder(ref totalPrice, "Salami", int.Parse(salamiTextBox.Text), PriceSalami);
            result += PizzaOrder(ref totalPrice, "Prosciutto", int.Parse(prosciuttoTextBox.Text), PriceProsciutto);
            result += PizzaOrder(ref totalPrice, "Quattro Fromaggi", int.Parse(quattroFromaggiTextBox.Text), PriceQuattroFromaggi);
            result += PizzaOrder(ref totalPrice, "Hawai", int.Parse(hawaiTextBox.Text), PriceHawai);
            result += PizzaOrder(ref totalPrice, "Margherita", int.Parse(margheritaTextBox.Text), PriceMargherita);
            result += $"\r\n";
            if (extraMozzarellaCheckBox.IsChecked == true)
            {
                totalPrice += _totalNumber * PriceExtraMozzarella;
                result += $"{_totalNumber} x {PriceExtraMozzarella} - extra mozzarella\r\n";
            }
            if (extraSalamiCheckBox.IsChecked == true)
            {
                totalPrice += _totalNumber * PriceExtraSalami;
                result += $"{_totalNumber} x {PriceExtraSalami} - extra salami\r\n";
            }
            if (extraAnsjovisCheckBox.IsChecked == true)
            {
                totalPrice += _totalNumber * PriceExtraAnsjovis;
                result += $"{_totalNumber} x {PriceExtraAnsjovis} - extra ansjovis\r\n";
            }
            if (extraArtisjokCheckBox.IsChecked == true)
            {
                totalPrice += _totalNumber * PriceExtraArtisjok;
                result += $"{_totalNumber} x {PriceExtraArtisjok} - extra artisjok\r\n";
            }
            result += $"Totaalbedrag = €{totalPrice}";
            answerTextBlock.Text = result;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GetObjects(sender);
            _textbox.Text = $"{_slider.Value}";
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            //int increment = btn.Content.ToString() == "+" ? 1 : -1;
            int increment;
            if (btn.Content.ToString() == "+")
            {
                increment = 1;
            }
            else
            {
                increment = -1;
            }
            if (btn.Name.StartsWith("quattroStagioni"))
                quattroStagioniSlider.Value += increment;
            else if (btn.Name.StartsWith("capricciosa"))
                capricciosaSlider.Value += increment;
            else if (btn.Name.StartsWith("salami"))
                salamiSlider.Value += increment;
            else if (btn.Name.StartsWith("prosciutto"))
                prosciuttoSlider.Value += increment;
            else if (btn.Name.StartsWith("quattroFromaggi"))
                quattroFromaggiSlider.Value += increment;
            else if (btn.Name.StartsWith("hawai"))
                hawaiSlider.Value += increment;
            else if (btn.Name.StartsWith("margherita"))
                margheritaSlider.Value += increment;
        }

        private void Tb_KeyDown(object sender, KeyEventArgs e)
        {
            //Enkel numerische input
            e.Handled = !
                (
                    (e.Key >= Key.D0 && e.Key <= Key.D9 && e.KeyboardDevice.Modifiers == ModifierKeys.Shift) ||
                    (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                );
        }

        private void Tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsLoaded)
            {
                //Waarde doorsturen naar slider => deze handelt maxvalue (10) af, maar geen blanco => testen
                GetObjects(sender);
                if (string.IsNullOrEmpty(_textbox.Text)) _textbox.Text = "0";
                _slider.Value = double.Parse(_textbox.Text);
                //Check max value
                _plusButton.IsEnabled = double.Parse(_textbox.Text) < _slider.Maximum;
                _minusButton.IsEnabled = double.Parse(_textbox.Text) > _slider.Minimum;
                if (double.Parse(_textbox.Text) > _slider.Value)
                {
                    _textbox.Text = _slider.Value.ToString();
                }
            }
        }

        private void GetObjects(object sender)
        {
            if (sender is TextBox)
            {
                _textbox = (TextBox)sender;
                if (_textbox.Name == nameof(quattroStagioniTextBox))
                    _slider = quattroStagioniSlider;
                else if (_textbox.Name == nameof(capricciosaTextBox))
                    _slider = capricciosaSlider;
                else if (_textbox.Name == nameof(salamiTextBox))
                    _slider = salamiSlider;
                else if (_textbox.Name == nameof(prosciuttoTextBox))
                    _slider = prosciuttoSlider;
                else if (_textbox.Name == nameof(quattroFromaggiTextBox))
                    _slider = quattroFromaggiSlider;
                else if (_textbox.Name == nameof(hawaiTextBox))
                    _slider = hawaiSlider;
                else if (_textbox.Name == nameof(margheritaTextBox))
                    _slider = margheritaSlider;
            }
            if (sender is Slider)
            {
                _slider = (Slider)sender;
                if (_slider.Name == nameof(quattroStagioniSlider))
                    _textbox = quattroStagioniTextBox;
                else if (_slider.Name == nameof(capricciosaSlider))
                    _textbox = capricciosaTextBox;
                else if (_slider.Name == nameof(salamiSlider))
                    _textbox = salamiTextBox;
                else if (_slider.Name == nameof(prosciuttoSlider))
                    _textbox = prosciuttoTextBox;
                else if (_slider.Name == nameof(quattroFromaggiSlider))
                    _textbox = quattroFromaggiTextBox;
                else if (_slider.Name == nameof(hawaiSlider))
                    _textbox = hawaiTextBox;
                else if (_slider.Name == nameof(margheritaSlider))
                    _textbox = margheritaTextBox;
            }
            switch (_slider.Name)
            {
                case nameof(quattroStagioniSlider):
                    _minusButton = quattroStagioniMinButton;
                    _plusButton = quattroStagioniPlusButton;
                    break;
                case nameof(capricciosaSlider):
                    _minusButton = capricciosaMinButton;
                    _plusButton = capricciosaPlusButton;
                    break;
                case nameof(salamiSlider):
                    _minusButton = salamiMinButton;
                    _plusButton = salamiPlusButton;
                    break;
                case nameof(prosciuttoSlider):
                    _minusButton = prosciuttoMinButton;
                    _plusButton = prosciuttoPlusButton;
                    break;
                case nameof(quattroFromaggiSlider):
                    _minusButton = quattroFromaggiMinButton;
                    _plusButton = quattroFromaggiPlusButton;
                    break;
                case nameof(hawaiSlider):
                    _minusButton = hawaiMinButton;
                    _plusButton = hawaiPlusButton;
                    break;
                case nameof(margheritaSlider):
                    _minusButton = margheritaMinButton;
                    _plusButton = margheritaPlusButton;
                    break;
            }
        }
    }
}
