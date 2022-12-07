using System.Windows;
using HomeAccounting_Client.Assets.Pages;

namespace HomeAccounting_Client
{
    public partial class Window_MainMenu : Window
    {
        public enum AvaiablePages { ActionsNComments, Goals, Reports }

        public Window_MainMenu()
        {
            InitializeComponent();
        }

        private void ButtonClick_ActionsNComments(object sender, RoutedEventArgs e)
        {
            SwitchPage(AvaiablePages.ActionsNComments);
        }

        private void ButtonClick_Goals(object sender, RoutedEventArgs e)
        {
            SwitchPage(AvaiablePages.Goals);
        }

        private void ButtonClick_Reports(object sender, RoutedEventArgs e)
        {
            SwitchPage(AvaiablePages.Reports);
        }

        public void SwitchPage(AvaiablePages incomingPage)
        {
            switch (incomingPage)
            {
                case AvaiablePages.ActionsNComments: Frame_Main.Navigate(new Page_ActionsNComments()); break;
                case AvaiablePages.Goals: Frame_Main.Navigate(new Page_Goals()); break;
                case AvaiablePages.Reports: Frame_Main.Navigate(new Page_Reports()); break;
                default: break;
            }
        }
    }
}
