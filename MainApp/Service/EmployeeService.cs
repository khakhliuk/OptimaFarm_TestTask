using AutoMapper;
using DataLibrary.Data;
using DataLibrary.Entity;
using MainApp.Helpers;
using MainApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MainApp.Service
{
    public class EmployeeService
    {
        public AppDbContext DbContext;
        Mapper mapper;

        public EmployeeService()
        {
            DbContext = new AppDbContext();
            DbContext.Database.EnsureCreated();
            DbContext.Employees.Load();
            ((MainWindow)Application.Current.MainWindow).DataContext = DbContext.Employees.Local.ToObservableCollection();
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDTO>().ReverseMap());
            mapper = new Mapper(config);
        }

        public void AddEmployee(Employee employee)
        {
            DbContext.Employees.Add(employee);
            DbContext.SaveChanges();
        }

        public void AddEmployeeRange(List<EmployeeDTO> employees)
        {
            DbContext.Employees.AddRange(mapper.Map<List<Employee>>(employees));
            DbContext.SaveChanges();
        }

        public void DeleteAll()
        {
            List<Employee> employees = DbContext.Employees.ToList();
            DbContext.Employees.RemoveRange(employees);
            DbContext.SaveChanges();
        }

        public List<Employee> SearchByName(string searchString)
        {
            List<Employee> searchEmployees = DbContext.Employees.Where(emp => 
            emp.Name == searchString || 
            emp.SecondName == searchString || 
            emp.Surname == searchString).ToList();

            return searchEmployees;
        }

        public List<Employee> SearchById(int id)
        {
            List<Employee> searchEmployees = DbContext.Employees.Where(emp => emp.Id == id).ToList();

            return searchEmployees;
        }

        public void SetStatusAsFired(long id)
        {
            Employee employee = DbContext.Employees.FirstOrDefault(emp => emp.Id == id);
            employee.Status = "Уволен";
            employee.FireDate = DateTime.Now;
            DbContext.Employees.Update(employee);
            DbContext.SaveChanges();
        }

        public Statistic CollectStatistics()
        {
            var employees = DbContext.Employees.Where(emp => emp.Status != "Уволен");
            int midSalary = employees.Sum(emp => emp.Salary) / employees.Count();

            Statistic stats = new Statistic()
            {
                EmployeesCount = DbContext.Employees.Count(),
                FiredCount = DbContext.Employees.Where(emp => emp.Status == "Уволен").Count(),
                MidSalary = midSalary
            };

            return stats;
        }

        public void UpdateEployee(Employee employee)
        {
            DbContext.Employees.Update(employee);
            DbContext.SaveChanges();
        }

        public List<Employee> GetAll()
        {
            return DbContext.Employees.ToList();
        }
    }
}
