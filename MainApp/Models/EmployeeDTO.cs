using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Models
{
    public class EmployeeDTO
    {
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
    }
}
