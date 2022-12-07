using System;
using System.Windows;
using HomeAccounting_Client.Assets.Database;
using HomeAccounting_Client.Assets.Pages;

namespace HomeAccounting_Client.Assets.Windows
{
    public partial class Window_AddGoal : Window
    {
        public Page_Goals Context { get; set; }

        public Window_AddGoal()
        {
            InitializeComponent();
        }

        private void ButtonClick_Add(object sender, RoutedEventArgs e)
        {
            String addResult = Model.AddGoal(TextBox_Name.Text, TextBox_Description.Text, Decimal.Parse(TextBox_GoalMoney.Text));

            if (addResult == "") { Context.RefreshList(); Close(); } else MessageBox.Show(addResult, "Ошибка");
        }
    }
}
