using AutoMapper;
using DataLibrary.Entity;
using MainApp.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;

namespace MainApp.Helpers
{
    public class DataImporter
    {
        Mapper mapper;

        public DataImporter()
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDTO>().ReverseMap());
            mapper = new Mapper(config);
        }

        public List<EmployeeDTO> ImportJsonData(OpenFileDialog openFileDialog)
        {
            Stream fileStream = openFileDialog.OpenFile();

            try
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string fileContent = reader.ReadToEnd();
                    List<Employee>? employees = JsonConvert.DeserializeObject<List<Employee>>(fileContent);

                    if (employees == null)
                    {
                        return null;
                    }

                    return mapper.Map<List<EmployeeDTO>>(employees);
                }
            }
            catch (Exception)
            {
                return null;
            }

            
        }

        public List<EmployeeDTO> ImportCSVData(string fileName)
        {
            List<EmployeeDTO> readEmployees = new List<EmployeeDTO>();

            try
            {
                readEmployees = File.ReadAllLines(fileName)
                   .Skip(1)
                   .Select(x => x.Split(','))
                   .Select(x => x.Select(x => x.Replace("\"", "")).ToArray())
                   .Select(x => new EmployeeDTO
                   {
                       Name = x[1],
                       SecondName = x[2],
                       Surname = x[3],
                       Address = x[4],
                       Birthday = Convert.ToDateTime(x[5]),
                       Phone = int.Parse(x[6]),
                       Position = x[7],
                       Status = x[8],
                       Salary = int.Parse(x[9]),
                       HireDate = Convert.ToDateTime(x[10]),
                       FireDate = (x[11].Equals("")) ? null : Convert.ToDateTime(x[11]),

                   }).ToList();
            }
            catch (Exception)
            {
                return null;
            }
            

            return readEmployees;
        }
    }
}
