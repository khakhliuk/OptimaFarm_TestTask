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
using DataLibrary.Data;
using DataLibrary.Entity;
using MainApp.Service;

namespace MainApp
{
    public partial class AddEmployeeWindow : Window
    {
        EmployeeService _employeeService;

        public AddEmployeeWindow(EmployeeService service)
        {
            InitializeComponent();
            AddEmployeeButton.Click += AddEmployeeButton_Click;
            _employeeService = service;
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in MainRoot.Children)
            {
                if (element is TextBox)
                {
                    TextBox textBox = (TextBox)element;
                    if (textBox.Text.Equals(""))
                    {
                        MessageBox.Show("All fields must be filled");
                        return;
                    }
                }
            }

            int phone;
            if (!int.TryParse(PhoneText.Text, out phone))
            {
                MessageBox.Show("Phone number is wrong");
                return;
            }

            int salary;
            if (!int.TryParse(SalaryText.Text, out salary))
            {
                MessageBox.Show("Salary is wrong");
                return;
            }

            if (BirthdayDatepicker.SelectedDate == null || HireDatepicker.SelectedDate == null)
            {
                MessageBox.Show("Enter right date");
                return;
            }

            Employee employee = new Employee
            {
                Name = NameText.Text,
                SecondName = SecondnameText.Text,
                Surname = SurnameText.Text,
                Address = AdressText.Text,
                Birthday = BirthdayDatepicker.SelectedDate.Value.Date,
                Phone = phone,
                Position = PositionText.Text,
                Status = StatusText.Text,
                Salary = salary,
                HireDate = HireDatepicker.SelectedDate.Value.Date
            };

            _employeeService.AddEmployee(employee);
            this.Close();
            MessageBox.Show("Added.");
        }
    }
}
