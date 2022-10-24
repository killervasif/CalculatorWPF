using System;
using System.Collections.Generic;
using System.Data;
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

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private double _result;

        private void ButtonNumber_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (txt.Text == "0")
                    txt.Text = string.Empty;

                txt.Text += btn.Content.ToString();
            }
        }

        private void ButtonSymbol_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt.Text))
                return;

            if (sender is Button btn)
            {
                if (btn.Content.ToString() == "C" || btn.Content.ToString() == "CE")
                {
                    txt.Text = "0";
                    return;
                }

                if (char.IsDigit(txt.Text[txt.Text.Length - 1]) || txt.Text[txt.Text.Length - 1] == '.' && btn.Content.ToString() != "." || txt.Text[txt.Text.Length - 1] == ',' && btn.Content.ToString() != ".")
                    txt.Text += btn.Content.ToString();
            }
        }

        private void ButtonResult_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt.Text))
                return;

            if (!char.IsDigit(txt.Text[txt.Text.Length - 1]) && txt.Text[txt.Text.Length - 1] != '.')
                return;

            CalculateResult();
            txt.Text = _result.ToString();
        }

        private void ButtonOperation_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt.Text))
                return;

            if (sender is Button btn)
            {
                if (txt.Text.Contains('+') || txt.Text.Contains('-') || txt.Text.Contains('*') || txt.Text.Contains('/'))
                    CalculateResult();

                if (_result == 0)
                {
                    if (!double.TryParse(txt.Text, out _result))
                        return;
                }

                _result = btn.Content.ToString() switch
                {
                    "%" => _result / 100,
                    "+/-" => _result * -1,
                    "1/x" => 1 / _result,
                    "x²" => Math.Pow(_result, 2),
                    "√x" => Math.Sqrt(_result),
                    _ => _result
                };

                if (_result.ToString() == "∞")
                {
                    MessageBox.Show("Cannot Divide by zero");
                    txt.Text = "0";
                    return;
                }

                txt.Text = _result.ToString();
            }
        }


        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt.Text))
                return;

            txt.Text = txt.Text.Remove(txt.Text.Length - 1);
        }

        private void ButtonUnidentified_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Unidentified");

        private void CalculateResult()
        {
            string formattedCalculation = txt.Text;
            try
            {
                _result = double.Parse(new DataTable().Compute(formattedCalculation, null).ToString()!);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _result = 0;
            }
        }


        private void txt_KeyUp(object sender, KeyEventArgs e)
        {
            bool isDigit = e.Key switch
            {
                Key.NumPad0 => true,
                Key.NumPad1 => true,
                Key.NumPad2 => true,
                Key.NumPad3 => true,
                Key.NumPad4 => true,
                Key.NumPad5 => true,
                Key.NumPad6 => true,
                Key.NumPad7 => true,
                Key.NumPad8 => true,
                Key.NumPad9 => true,
                _ => false
            };




            if (isDigit)
            {

                txt.Text += e.Key switch
                {
                    Key.NumPad0 => '0',
                    Key.NumPad1 => '1',
                    Key.NumPad2 => '2',
                    Key.NumPad3 => '3',
                    Key.NumPad4 => '4',
                    Key.NumPad5 => '5',
                    Key.NumPad6 => '6',
                    Key.NumPad7 => '7',
                    Key.NumPad8 => '8',
                    Key.NumPad9 => '9',
                    _ => string.Empty
                };

                return;
            }

            if (string.IsNullOrEmpty(txt.Text))
                return;


            if (char.IsDigit(txt.Text[txt.Text.Length - 1]))
            {
                txt.Text += e.Key switch
                {
                    Key.Divide => '/',
                    Key.Add => '+',
                    Key.Subtract => '-',
                    Key.Multiply => '*',
                    Key.Decimal => '.',
                    _ => string.Empty
                };
            }

            if (e.Key == Key.Enter)
            {
                CalculateResult();
                txt.Text = _result.ToString();
            }
            else if (e.Key == Key.Back)
                txt.Text = txt.Text.Remove(txt.Text.Length - 1);
        }
    }
}
