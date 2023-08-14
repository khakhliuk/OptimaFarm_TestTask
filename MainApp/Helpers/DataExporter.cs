using AutoMapper;
using DataLibrary.Entity;
using MainApp.Models;
using MainApp.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MainApp.Helpers
{
    public class DataExporter
    {
        Mapper mapper;

        public DataExporter()
        {
            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDTO>().ReverseMap());
            mapper = new Mapper(config);
        }

        public void ExportAsCSV(string path, DataGrid dataGrid, List<Employee> employees)
        {
            List<string> lines = new List<string>();
            string?[] columnNames = dataGrid.Columns.Select(column => column.Header.ToString()).ToArray();

            string header = string.Join(",", columnNames.Select(name => $"\"{name}\""));
            lines.Add(header);

            DataTable dataTable = DataConverter.ListToDataTable(employees);
            var valueLines = dataTable.AsEnumerable().Select(row => string.Join(",", row.ItemArray.Select(val => $"\"{val}\"")));
            lines.AddRange(valueLines);

            File.WriteAllLines(path, lines, Encoding.UTF8);
        }

        public void ExportAsJson(string path, List<Employee> employees)
        {
            string json = JsonConvert.SerializeObject(employees);
            File.WriteAllText(path, json);
        }
    }
}
