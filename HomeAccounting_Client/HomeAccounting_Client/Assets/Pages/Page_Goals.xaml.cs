using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HomeAccounting_Client.Assets.Database;
using HomeAccounting_Client.Assets.Windows;

namespace HomeAccounting_Client.Assets.Pages
{
    public partial class Page_Goals : Page
    {
        private List<goal> _Goals = null;

        public Page_Goals()
        {
            InitializeComponent();
            DataContext = this;
            RefreshList();
        }

        public void RefreshList()
        {
            _Goals = Model.Database.goals.Where(goal => goal.owner_id == Model.UserId).ToList();
            DataGrid_Main.ItemsSource = _Goals;
        }

        private void ButtonClick_Add(object sender, RoutedEventArgs e)
        {
            Window_AddGoal windowAddGoal = new Window_AddGoal();
            windowAddGoal.Context = this;
            windowAddGoal.Show();
        }

        private void ButtonClick_Delete(object sender, RoutedEventArgs e)
        {
            Model.Database.goals.Remove(_Goals[DataGrid_Main.SelectedIndex]);
            Model.Database.SaveChanges();
            RefreshList();
        }
    }
}