using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Entity
{
    public class Employee
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? SecondName { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public DateTime Birthday { get; set; }
        public int Phone { get; set; }
        public string? Position { get; set; }
        public string? Status { get; set; }
        public int Salary { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? FireDate { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
