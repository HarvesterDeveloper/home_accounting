using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HomeAccounting_Client.Assets.Pages
{
    public partial class Page_Reports : Page
    {
        private Int32 _Index = 0;
        private List<String> _Reports = new List<String>();

        public Page_Reports()
        {
            InitializeComponent();
            _Reports = Model.GetUserReports();
            TextBox_Report.Text = _Reports[_Index];
        }

        private void ButtonClick_NextMonth(object sender, RoutedEventArgs e)
        {
            if(_Index < _Reports.Count-1)
            {
                _Index++;
                TextBox_Report.Text = _Reports[_Index];
            }
        }

        private void ButtonClick_PreviousMonth(object sender, RoutedEventArgs e)
        {
            if (_Index > 0)
            {
                _Index--;
                TextBox_Report.Text = _Reports[_Index];
            }
        }
    }
}