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

        public List<EmployeeDTO> ImportCSVData(string fileName)
        {
            List<EmployeeDTO> readEmployees = File.ReadAllLines(fileName)
                   .Skip(1)
                   .Select(x => x.Split(','))
                   .Select(x => x.Select(x => x.Replace("\"", "")).ToArray())
                   .Select(x => new EmployeeDTO
                   {
                       Name = x[0],
                       SecondName = x[1],
                       Surname = x[2],
                       Address = x[3],
                       Birthday = Convert.ToDateTime(x[4]),
                       Phone = int.Parse(x[5]),
                       Position = x[6],
                       Status = x[7],
                       Salary = int.Parse(x[8]),
                       HireDate = Convert.ToDateTime(x[9]),
                       FireDate = (x[10].Equals("")) ? null : Convert.ToDateTime(x[10]),

                   }).ToList();

            return readEmployees;
        }
    }
}
