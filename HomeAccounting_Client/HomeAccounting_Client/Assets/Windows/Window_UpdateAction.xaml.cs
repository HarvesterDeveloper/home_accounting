using System;
using System.Collections.Generic;
using System.Windows;
using HomeAccounting_Client.Assets.Pages;

namespace HomeAccounting_Client.Assets.Windows
{
    public partial class Window_UpdateAction : Window
    {
        public Page_ActionsNComments Context { get; set; }
        public Int32 ActionId { get; set; }

        public Window_UpdateAction()
        {
            InitializeComponent();
            ComboBox_Goals.ItemsSource = Model.GetUserGoalStringed();
        }

        private void ButtonClick_Update(object sender, RoutedEventArgs e)
        {
            String updateResult = Model.UpdateAction(ActionId, Int32.Parse(ComboBox_Goals.Items.GetItemAt(ComboBox_Goals.SelectedIndex).ToString().Split(new char[] { '|' })[0]), TextBox_Source.Text, (Boolean)CheckBox_IsIncome.IsChecked, Decimal.Parse(TextBox_Amount.Text), DateTime.Parse(TextBox_Datetime.Text), TextBox_Commentary.Text);

            if (updateResult == "") { Context.RefreshList(); Close(); } else MessageBox.Show(updateResult, "Ошибка");
        }
    }
}
