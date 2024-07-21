using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Matsen.Models;
using System.Drawing.Imaging;
using System.Drawing;

namespace Matsen
{
    public partial class LoginWindow : Window
    {
        private StringBuilder barcodeBuilder = new StringBuilder();

        public LoginWindow()
        {
            InitializeComponent();

        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Ignore modifier keys
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift || e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl ||
                e.Key == Key.LeftAlt || e.Key == Key.RightAlt || e.Key == Key.LWin || e.Key == Key.RWin)
            {
                return;
            }

            // Check if the key is a printable key
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                barcodeBuilder.Append((char)('0' + (e.Key - Key.D0)));
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                barcodeBuilder.Append((char)('0' + (e.Key - Key.NumPad0)));
            }
            else if (e.Key >= Key.A && e.Key <= Key.Z)
            {
                barcodeBuilder.Append((char)('A' + (e.Key - Key.A)));
            }
            else if (e.Key == Key.Enter)
            {
                // Handle the barcode when Enter key is pressed
                string barcode = barcodeBuilder.ToString();
                barcodeBuilder.Clear();
                PerformLogin(barcode: barcode);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            PerformLogin();
        }

        private void PerformLogin(string barcode = null)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (barcode == null && (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)))
            {
                MessageBox.Show("Please enter your username and password or scan your barcode.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new AppDbContext())
            {
                User user;
                if (barcode != null)
                {
                    user = context.Users.FirstOrDefault(u => u.Barcode == barcode);
                }
                else
                {
                    user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
                }

                if (user != null)
                {
                    // Logowanie powiodło się
                    MessageBox.Show($"Welcome, {user.Username}!", "Login Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Przekierowanie na odpowiedni dashboard
                    if (user.Role == "szwaczka")
                    {
                        var dashboard = new SzwaczkaDashboard();
                        dashboard.Show();
                    }
                    else if (user.Role == "krojenie")
                    {
                        //var dashboard = new KrojenieDashboard();
                        //dashboard.Show();
                    }
                    else if (user.Role == "lider")
                    {
                        //var dashboard = new LiderDashboard();
                        //dashboard.Show();
                    }

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid login credentials or barcode. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "Username")
                {
                    textBox.Text = "";
                    textBox.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
            else if (sender is PasswordBox passwordBox)
            {
                PasswordPlaceholder.Visibility = Visibility.Collapsed;
                passwordBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void AddPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = "Username";
                    textBox.Foreground = new SolidColorBrush(Colors.Gray);
                }
            }
            else if (sender is PasswordBox passwordBox)
            {
                if (string.IsNullOrWhiteSpace(passwordBox.Password))
                {
                    PasswordPlaceholder.Visibility = Visibility.Visible;
                    passwordBox.Foreground = new SolidColorBrush(Colors.Gray);
                }
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                PasswordPlaceholder.Visibility = string.IsNullOrEmpty(passwordBox.Password) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
