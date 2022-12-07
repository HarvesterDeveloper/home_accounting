using System;
using System.Windows;
using HomeAccounting_Client.Assets;

namespace HomeAccounting_Client
{
    public partial class Window_Captcha : Window
    {
        public Window_Captcha()
        {
            InitializeComponent();
            Label_Captcha.Text = "Captcha: " + Model.RequestCaptcha();
        }

        private void ButtonClick_CheckCaptcha(object IncomeSender, RoutedEventArgs IncomeRoutedEventArgs)
        {
            String captchaResult = Model.CheckCaptcha(TextBox_Income.Text);

            if (captchaResult=="") MessageBox.Show("Каптча пройдена!", "Успех"); else MessageBox.Show(captchaResult, "Ошибка");

            this.Close();
        }
    }
}