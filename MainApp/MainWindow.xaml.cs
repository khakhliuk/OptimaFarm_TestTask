using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Windows;
using AutoMapper;
using DataLibrary.Entity;
using MainApp.Helpers;
using MainApp.Models;
using MainApp.Service;
using MainApp.Windows;
using Microsoft.Win32;
using WPFCustomMessageBox;

namespace MainApp
{
    public partial class MainWindow : Window
    {
        EmployeeService _employeeService;

        public MainWindow()
        {
            InitializeComponent();
            InitializeButtons();
            _employeeService = new EmployeeService();
        }

        private void InitializeButtons()
        {
            AddButton.Click += AddButton_Click;
            EditButton.Click += EditButton_Click;
            DeleteAllButton.Click += DeleteAllButton_Click;
            FindButton.Click += FindButton_Click;
            SearchCancelButton.Click += SearchCancelButton_Click;
            FireButton.Click += FireButton_Click;
            InfoButton.Click += InfoButton_Click;
            ImportButton.Click += ImportButton_Click;
            ExportButton.Click += ExportButton_Click;
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = CustomMessageBox.ShowYesNo("Which format you want to use?", "Export data", ".Json", ".Csv");
            DataExporter dataExporter = new DataExporter();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            List<Employee> employees = _employeeService.GetAll();

            if (result == MessageBoxResult.Yes)
            {
                saveFileDialog.Filter = "Json file (*.json)|*.json";
                saveFileDialog.FileName = "data.json";

                if (saveFileDialog.ShowDialog() == true)
                {
                    dataExporter.ExportAsJson(saveFileDialog.FileName, employees);
                }
            }
            else
            {
                saveFileDialog.Filter = "Csv file (*.csv)|*.csv";
                saveFileDialog.FileName = "data.csv";

                if (saveFileDialog.ShowDialog() == true)
                {
                    dataExporter.ExportAsCSV(saveFileDialog.FileName, DbGrid, employees);
                }
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            DataImporter dataImporter = new DataImporter();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".json";
            openFileDialog.Filter = "Json File (*.json)|*.json|Excel Document (*.csv)|*.csv";
            bool? dialogResult = openFileDialog.ShowDialog();
            List<EmployeeDTO> employeeDTOs = new List<EmployeeDTO>();

            if (dialogResult == true)
            {
                if (Path.GetExtension(openFileDialog.FileName).ToLower().Equals(".json"))
                {
                    employeeDTOs = dataImporter.ImportJsonData(openFileDialog);
                }
                else
                {
                    employeeDTOs = dataImporter.ImportCSVData(openFileDialog.FileName);
                }

                if (employeeDTOs == null)
                {
                    MessageBox.Show("Error: File is empty");
                    return;
                }

                _employeeService.AddEmployeeRange(employeeDTOs);
                MessageBox.Show("Done");
            }
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            StatisticWindow statisticWindow = new StatisticWindow(_employeeService);
            statisticWindow.Show();
        }

        private void FireButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to fire this employee?", "Are you sure?",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Employee customer = (Employee)DbGrid.SelectedItem;
                _employeeService.SetStatusAsFired(customer.Id);
                DbGrid.Items.Refresh();
            }
            else
            {
                return;
            }

            MessageBox.Show("Done");
        }

        private void SearchCancelButton_Click(object sender, RoutedEventArgs e)
        {
            DbGrid.ItemsSource = _employeeService.DbContext.Employees.Local.ToObservableCollection();
            SearchTextBox.Text = "";
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            List<Employee> employees = new List<Employee>();

            if (SearchComboBox.SelectedIndex == 0)
            {
                employees = _employeeService.SearchByName(SearchTextBox.Text);
            }
            else
            {
                int id;

                if (!int.TryParse(SearchTextBox.Text, out id))
                {
                    MessageBox.Show("Wrong input");
                }

                employees = _employeeService.SearchById(id);
            }

            if (employees != null)
            {
                DbGrid.ItemsSource = employees;
            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow addEmployee = new AddEmployeeWindow(_employeeService);
            addEmployee.Show();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (DbGrid.SelectedItem == null)
            {
                MessageBox.Show("Select row which you want to edit");
                return;
            }

            EditEmpWindow editEmpWindow = new EditEmpWindow(_employeeService, (Employee)DbGrid.SelectedItem);
            editEmpWindow.Show();
        }

        private void DeleteAllButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to delete all data?", "Are you sure?", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _employeeService.DeleteAll();
            }
            else
            {
                return;
            }

            MessageBox.Show("Deleted");
        }

        
    }
}
