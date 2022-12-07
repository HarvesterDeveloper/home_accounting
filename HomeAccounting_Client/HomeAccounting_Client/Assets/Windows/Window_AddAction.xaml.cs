using System;
using System.Collections.Generic;
using System.Windows;
using HomeAccounting_Client.Assets.Database;
using HomeAccounting_Client.Assets.Pages;

namespace HomeAccounting_Client.Assets.Windows
{
    public partial class Window_AddAction : Window
    {
        public Page_ActionsNComments Context { get; set; }

        public Window_AddAction()
        {
            InitializeComponent();
            ComboBox_Goals.ItemsSource = Model.GetUserGoalStringed();
        }

        private void ButtonClick_Add(object sender, RoutedEventArgs e)
        {
            String addResult = "";

            if(ComboBox_Goals.SelectedIndex > -1)
            {
                addResult = Model.AddAction(Int32.Parse(ComboBox_Goals.Items.GetItemAt(ComboBox_Goals.SelectedIndex).ToString().Split(new char[] { '|' })[0]), TextBox_Source.Text, (Boolean)CheckBox_IsIncome.IsChecked, Decimal.Parse(TextBox_Amount.Text), DateTime.Parse(TextBox_Datetime.Text), TextBox_Commentary.Text);
            }
            else
            {
                addResult = Model.AddAction(-1, TextBox_Source.Text, (Boolean)CheckBox_IsIncome.IsChecked, Decimal.Parse(TextBox_Amount.Text), DateTime.Parse(TextBox_Datetime.Text), TextBox_Commentary.Text);
            }

            if (addResult==""){Context.RefreshList();Close();} else MessageBox.Show(addResult, "Ошибка");
        }
    }
}