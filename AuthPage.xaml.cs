using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Сытник41
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
            UserInputTB.Visibility = Visibility.Hidden;
        }
        private string _currentCaptcha = "";
        private readonly Random _random = new Random();
        private const string Alphabet = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz2345689";
        private void GenerateCaptcha()
        {
            StringBuilder captchaBuilder = new StringBuilder();
            TextBlock[] textBlocks = { captchaOneWord, captchaTwoWord, captchaThreeWord, captchaFourWord };
            for (int i = 0; i < 4; i++)
            {
                char ch = Alphabet[_random.Next(Alphabet.Length)];
                captchaBuilder.Append(ch);
                textBlocks[i].Text = ch.ToString();
                textBlocks[i].Foreground = new SolidColorBrush(Color.FromRgb((byte)_random.Next(0, 150), (byte)_random.Next(0, 150), (byte)_random.Next(0, 150)));
                textBlocks[i].RenderTransform = new RotateTransform(_random.Next( - 15, 15));

            }
            _currentCaptcha = captchaBuilder.ToString();
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTB.Text;
            string password = PassTB.Text;
            if (login == "" || password == "")
            {
                MessageBox.Show("Есть пустые поля");
                return;
            }
            User user = Сытник41Entities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);
            if (user != null && (UserInputTB.Visibility == Visibility.Hidden || UserInputTB.Text == _currentCaptcha))
            {
                Manager.MainFrame.Navigate(new ProductPage(user));
                LoginTB.Text = "";
                PassTB.Text = "";
                UserInputTB.Text = "";
                UserInputTB.Visibility = Visibility.Hidden;
                captchaOneWord.Text = "";
                captchaTwoWord.Text = "";
                captchaThreeWord.Text = "";
                captchaFourWord.Text = "";

            }
            else
            {
                MessageBox.Show("Введены неверные данные");
                UserInputTB.Visibility = Visibility.Visible;
                GenerateCaptcha();
                if (UserInputTB.Text == _currentCaptcha)
                {
                    MessageBox.Show("Успешно!");
                    UserInputTB.Text = "";
                }
                else
                {
                    if (UserInputTB.Text != "" && UserInputTB.Text != _currentCaptcha)
                    {
                        LoginBtn.IsEnabled = false;
                        await Task.Delay(10000);
                        LoginBtn.IsEnabled = true;
                        GenerateCaptcha();
                        UserInputTB.Clear();
                    }
                }
            }
        }

        private void LoginAsGuestBtn_Click(object sender, RoutedEventArgs e)
        {
            string role = "Гость";
            Manager.MainFrame.Navigate(new ProductPage(role));
            LoginTB.Text = "";
            PassTB.Text = "";
            UserInputTB.Text = "";
            UserInputTB.Visibility = Visibility.Hidden;
            captchaOneWord.Text = "";
            captchaTwoWord.Text = "";
            captchaThreeWord.Text = "";
            captchaFourWord.Text = "";
        }
    }
}
