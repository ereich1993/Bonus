using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FizzWare.NBuilder;
using SynetecAssessmentApi.Domain;
using Moq;
using SynetecAssessmentApi.Interfaces;
using SynetecAssessmentApi.Services;
using SynetecAssessmentApi.Dtos;

namespace SynetecAssesmentApi.Tests
{
    public class BonusPollServiceShould
    {
        [Fact]
        public async Task CalculateAsyncShouldReturnBonusPoolCalculatorResultDto()
        {
            
            var builder = new Builder();
            var rnd = new Random();
            var employee = builder.CreateNew<Employee>()
                .WithFactory(() => new Employee(1, Faker.Name.FullName(),"title", rnd.Next(1, 100), 1))
                .With(a => a.Department = builder.CreateNew<Department>()
                .WithFactory(() => new Department(1,"finance","financeDeparts"))
                .Build())
                .Build();
            var employees = builder.CreateListOfSize<Employee>(5)
                .All()
                .WithFactory(() => new Employee(rnd.Next(1,100), Faker.Name.FullName(), "test job title", rnd.Next(1, 100), 1))
                .With(a => a.Department = builder.CreateNew<Department>()
                .WithFactory(() => new Department(1, "finance", "financeDeparts"))
                .Build())
                .Build();
            employees.Add(employee);

            var queryRepositoryMock = new Mock<IEmployeeBonusQueryRepository>();
            queryRepositoryMock.Setup(a => a.GetEmployeeById(employee.Id)).ReturnsAsync(employee).Verifiable();
            queryRepositoryMock.Setup(a => a.GetEmployees()).ReturnsAsync(employees).Verifiable();

            var service = new BonusPoolService(queryRepositoryMock.Object);

            var resultDto = await service.CalculateAsync(1, 1);
            Assert.IsType<BonusPoolCalculatorResultDto>(resultDto);
            Assert.IsType<EmployeeDto>(resultDto.Employee);
            Assert.IsType<DepartmentDto>(resultDto.Employee.Department);
            Assert.Equal(resultDto.Employee.Fullname, employee.Fullname);
            Assert.Equal(resultDto.Employee.JobTitle, employee.JobTitle);
            Assert.Equal(resultDto.Employee.Department.Description, employee.Department.Description);
            Assert.Equal(resultDto.Employee.Salary, employee.Salary);
        }

        [Theory]
        [InlineData(100,25)]
        [InlineData(200,50)]
        public async Task CalculateAsyncShouldCorrectlyCalculateBonus(int salary, int expectedBonus)
        {

            var builder = new Builder();
            var rnd = new Random();
            var employee = builder.CreateNew<Employee>()
                .WithFactory(() => new Employee(1, Faker.Name.FullName(), "title", 100, 1))
                .With(a => a.Department = builder.CreateNew<Department>()
                .WithFactory(() => new Department(1, "finance", "financeDeparts"))
                .Build())
                .Build();
            var employees = builder.CreateListOfSize<Employee>(3)
                .All()
                .WithFactory(() => new Employee(rnd.Next(1, 100), Faker.Name.FullName(), "test job title", 100, 1))
                .With(a => a.Department = builder.CreateNew<Department>()
                .WithFactory(() => new Department(1, "finance", "financeDeparts"))
                .Build())
                .Build();
            employees.Add(employee);

            var queryRepositoryMock = new Mock<IEmployeeBonusQueryRepository>();
            queryRepositoryMock.Setup(a => a.GetEmployeeById(employee.Id)).ReturnsAsync(employee).Verifiable();
            queryRepositoryMock.Setup(a => a.GetEmployees()).ReturnsAsync(employees).Verifiable();

            var service = new BonusPoolService(queryRepositoryMock.Object);

            var resultDto = await service.CalculateAsync(100, 1);
            var bonus = resultDto.Amount;
        }
    }
}
