using System;
using System.Windows;
using HomeAccounting_Client.Assets;

namespace HomeAccounting_Client
{
    public partial class Window_Registration : Window
    {
        public Window_Registration()
        {
            InitializeComponent();
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            String registrationResult = Model.RequestRegistration(TextBox_Login.Text, PasswordBox_Password.Password != "" ? PasswordBox_Password.Password : TextBox_Password.Text);

            if(registrationResult == "")
            {
                MessageBox.Show("Регистрация прошла успешно","Успех");
                Window_Authorization windowAuthorization = new Window_Authorization();
                windowAuthorization.Show();
                Close();
            }
            else
            {
                MessageBox.Show(registrationResult, "Ошибка");
            }

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Button_ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            String Password = PasswordBox_Password.Password;
            Visibility Visibility = PasswordBox_Password.Visibility;
            double Width = PasswordBox_Password.ActualWidth;

            Button_ShowPassword.Content = Visibility == Visibility.Visible ? "Скрыть" : "Показать";

            PasswordBox_Password.Password = TextBox_Password.Text;
            PasswordBox_Password.Visibility = TextBox_Password.Visibility;
            PasswordBox_Password.Width = TextBox_Password.Width;

            TextBox_Password.Text = Password;
            TextBox_Password.Visibility = Visibility;
            TextBox_Password.Width = Width;
        }
        private void Button_Captcha_Click(object sender, RoutedEventArgs e)
        {
            Window_Captcha windowCaptcha = new Window_Captcha();
            windowCaptcha.Show();
        }
    }
}