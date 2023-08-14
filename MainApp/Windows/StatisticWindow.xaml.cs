using MainApp.Helpers;
using MainApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainApp.Windows
{
    public partial class StatisticWindow : Window
    {
        EmployeeService _employeeService;
        public StatisticWindow(EmployeeService employeeService)
        {
            InitializeComponent();
            _employeeService = employeeService;
        }

        private void StatisicWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            Statistic stats = _employeeService.CollectStatistics();
            EmployeeCountLabel.Content = stats.EmployeesCount;
            MidSalaryLabel.Content = stats.MidSalary;
            FiredEmployeeCountLabel.Content = stats.FiredCount;
        }
    }
}
