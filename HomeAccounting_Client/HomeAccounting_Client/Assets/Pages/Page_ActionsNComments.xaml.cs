using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HomeAccounting_Client.Assets.Database;
using HomeAccounting_Client.Assets.Windows;

namespace HomeAccounting_Client.Assets.Pages
{
    public partial class Page_ActionsNComments : Page
    {
        private List<action> _Actions = null;
        public Page_ActionsNComments()
        {
            InitializeComponent();
            DataContext = this;
            RefreshList();
        }

        public void RefreshList()
        {
            _Actions = Model.Database.actions.Where(action => action.owner_id == Model.UserId).ToList();
            DataGrid_Main.ItemsSource = _Actions;
        }

        private void ButtonClick_Add(object sender, RoutedEventArgs e)
        {
            Window_AddAction windowAddAction = new Window_AddAction();
            windowAddAction.Context = this;
            windowAddAction.Show();
        }

        private void ButtonClick_Delete(object sender, RoutedEventArgs e)
        {
            if(DataGrid_Main.SelectedIndex > -1)
            {
                Model.Database.actions.Remove(_Actions[DataGrid_Main.SelectedIndex]);
                Model.Database.SaveChanges();
                RefreshList();
            }
        }

        private void ButtonClick_Copy(object sender, RoutedEventArgs e)
        {
            if (DataGrid_Main.SelectedIndex > -1)
            {
                Model.Database.actions.Add(_Actions[DataGrid_Main.SelectedIndex]);
                Model.Database.SaveChanges();
                RefreshList();
            }
        }

        private void ButtonClick_Update(object sender, RoutedEventArgs e)
        {
            if (DataGrid_Main.SelectedIndex > -1)
            {
                Window_UpdateAction windowUpdateAction = new Window_UpdateAction();
                windowUpdateAction.Context = this;
                windowUpdateAction.ActionId = _Actions[DataGrid_Main.SelectedIndex].id;
                windowUpdateAction.TextBox_Source.Text = _Actions[DataGrid_Main.SelectedIndex].source;
                windowUpdateAction.CheckBox_IsIncome.IsChecked = _Actions[DataGrid_Main.SelectedIndex].is_income;
                windowUpdateAction.TextBox_Amount.Text = _Actions[DataGrid_Main.SelectedIndex].amount.ToString();
                windowUpdateAction.TextBox_Datetime.Text = _Actions[DataGrid_Main.SelectedIndex].date.ToString();
                windowUpdateAction.TextBox_Commentary.Text = _Actions[DataGrid_Main.SelectedIndex].commentary;
                windowUpdateAction.Show();
            }
        }
    }
}