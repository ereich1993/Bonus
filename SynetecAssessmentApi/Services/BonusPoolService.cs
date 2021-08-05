using Microsoft.EntityFrameworkCore;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Interfaces;
using SynetecAssessmentApi.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services
{
    public class BonusPoolService : IBonusPoolService
    {
        
        private readonly IEmployeeBonusQueryRepository employeeBonusQueryRepository;

        public BonusPoolService(IEmployeeBonusQueryRepository employeeBonusQueryRepository)
        {
            this.employeeBonusQueryRepository = employeeBonusQueryRepository;
        }

        public async Task<List<EmployeeDto>> GetEmployeesAsync()
        {
            IEnumerable<Employee> employees = await employeeBonusQueryRepository.GetEmployees();
               
            List<EmployeeDto> result = new List<EmployeeDto>();

            foreach (var employee in employees)
            {
                result.Add(
                    new EmployeeDto
                    {
                        Fullname = employee.Fullname,
                        JobTitle = employee.JobTitle,
                        Salary = employee.Salary,
                        Department = new DepartmentDto
                        {
                            Title = employee.Department.Title,
                            Description = employee.Department.Description
                        }
                    });
            }

            return result;
        }

        public async Task<bool> EmployeeExsist(int selectedEmployeeId)
        {
            var employee = await employeeBonusQueryRepository.GetEmployeeById(selectedEmployeeId);
            return employee == null ? false : true;
        }

        public async Task<BonusPoolCalculatorResultDto> CalculateAsync(int bonusPoolAmount, int selectedEmployeeId)
        {
            if (bonusPoolAmount <= 0)
            {
                throw new ArgumentException();
            }
            //load the details of the selected employee using the Id
            var employee = await employeeBonusQueryRepository.GetEmployeeById(selectedEmployeeId);
            if (employee == null)
            {
                throw new ArgumentException();
            }

            //get the total salary budget for the company
            var employees  = await employeeBonusQueryRepository.GetEmployees();
            int totalSalary = employees.Sum(a => a.Salary);
            //calculate the bonus allocation for the employee
            decimal bonusPercentage = (decimal)employee.Salary / (decimal)totalSalary;
            int bonusAllocation = (int)(bonusPercentage * bonusPoolAmount);

            return new BonusPoolCalculatorResultDto
            {
                Employee = new EmployeeDto
                {
                    Fullname = employee.Fullname,
                    JobTitle = employee.JobTitle,
                    Salary = employee.Salary,
                    Department = new DepartmentDto
                    {
                        Title = employee.Department.Title,
                        Description = employee.Department.Description
                    }
                },

                Amount = bonusAllocation
            };
        }
    }
}
