using DataLibrary.Entity;
using MainApp.Service;
using System.Windows;
using System.Windows.Controls;

namespace MainApp.Windows
{
    public partial class EditEmpWindow : Window
    {
        EmployeeService _employeeService;
        Employee _employee;

        public EditEmpWindow(EmployeeService employeeService, Employee employee)
        {
            InitializeComponent();
            SaveEmployeeButton.Click += SaveEmployeeButton_Click;
            _employeeService = employeeService;
            _employee = employee;
        }

        private void SaveEmployeeButton_Click(object sender, RoutedEventArgs e)
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

            if (!int.TryParse(PhoneText.Text, out _))
            {
                MessageBox.Show("Phone number is wrong");
                return;
            }

            if (!int.TryParse(SalaryText.Text, out _))
            {
                MessageBox.Show("Salary is wrong");
                return;
            }

            if (BirthdayDatepicker.SelectedDate == null || HireDatepicker.SelectedDate == null)
            {
                MessageBox.Show("Enter right date");
                return;
            }

            _employeeService.UpdateEployee(_employee);
            this.Close();
            MessageBox.Show("Updated");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _employee;
        }
    }
}
