using System;
using System.Windows;
using HomeAccounting_Client.Assets;

namespace HomeAccounting_Client
{
    public partial class Window_Authorization : Window
    {   
        public Window_Authorization()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            String connectionResult = Model.ConnectToDatabase();

            if (connectionResult == "") Console.WriteLine("Подключение к БД прошло успешно.", "Подключение к БД."); else Console.WriteLine(connectionResult, "Ошибка");
        }

        private void TooglePasswordVisual()
        {
            String password = PasswodBox_Password.Password;
            Visibility visibility = PasswodBox_Password.Visibility;
            Double width = PasswodBox_Password.ActualWidth;

            Button_ShowPassword.Content = visibility == Visibility.Visible ? "Скрыть" : "Показать";

            PasswodBox_Password.Password = TextBox_Password.Text;
            PasswodBox_Password.Visibility = TextBox_Password.Visibility;
            PasswodBox_Password.Width = TextBox_Password.Width;

            TextBox_Password.Text = password;
            TextBox_Password.Visibility = visibility;
            TextBox_Password.Width = width;
        }

        private void ButtonClick_ShowPassword(object incomeSender, RoutedEventArgs incomeRoutedEventArgs)
        {
            TooglePasswordVisual();
        }

        private void ButtonClick_Authorize(object incomeSender, RoutedEventArgs incomeRoutedEventArgs)
        {
            String pass = "";
            if (TextBox_Password.Visibility == Visibility.Visible) pass = TextBox_Password.Text; else pass = PasswodBox_Password.Password; 

            String authorizationResult = Model.Authorize(TextBox_Login.Text, pass);

            if (authorizationResult == "")
            {
                Console.WriteLine("Вы вошли в аккаунт с id " + Model.UserId.ToString(), "Успешный вход");
                Window_MainMenu windowMenu = new Window_MainMenu();
                windowMenu.Show();
                Close();
            }
            else
            {
                MessageBox.Show(authorizationResult, "Ошибка");
            }
        }

        private void ButtonClick_Registration(object incomeSender, RoutedEventArgs incomeRoutedEventArgs)
        {
            Window_Registration windowRegistration = new Window_Registration();
            windowRegistration.Show();
            Close();
        }

        private void ButtonClick_Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}