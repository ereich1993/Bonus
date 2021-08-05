using Moq;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FizzWare.NBuilder;
using SynetecAssessmentApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace SynetecAssesmentApi.Tests
{
    public class BonusPoolControllerShould
    {
        [Fact]
        public async Task GetAlldReturnsListOfEmployeeDtos()
        {
            var builder = new Builder();
            var employees = (List<EmployeeDto>)builder.CreateListOfSize<EmployeeDto>(5).Build();
            var bonusPoolServiceMock = new Mock<IBonusPoolService>();
            bonusPoolServiceMock.Setup(a => a.GetEmployeesAsync()).ReturnsAsync(employees).Verifiable();

            var controller = new BonusPoolController(bonusPoolServiceMock.Object);

            var result = await controller.GetAll();

            bonusPoolServiceMock.Verify();
            var emp = Assert.IsType<OkObjectResult>(result);
            var resultEmployees = Assert.IsType<List<EmployeeDto>>(emp.Value);
            Assert.Equal(resultEmployees,employees);
        }

        [Fact]
        public async Task CalculateBonusReturnBadRequestIfBadInput()
        {
            var bonusPoolServiceMock = new Mock<IBonusPoolService>();
            bonusPoolServiceMock.Setup(a => a.EmployeeExsist(1)).ReturnsAsync(true);
            var controller = new BonusPoolController(bonusPoolServiceMock.Object);

            var result = await controller.CalculateBonus(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task CalculateBonusReturnBadRequestIfInputIsLessOrZero(int totalBonusPoll)
        {
            var bonusPoolServiceMock = new Mock<IBonusPoolService>();
            bonusPoolServiceMock.Setup(a => a.EmployeeExsist(1)).ReturnsAsync(true);
            var controller = new BonusPoolController(bonusPoolServiceMock.Object);

            var result = await controller.CalculateBonus(new CalculateBonusDto { SelectedEmployeeId = 1, TotalBonusPoolAmount = totalBonusPoll});

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task CalculateBonusReturnBadRequestWhenEmployeeDoesntExist()
        {
            var bonusPoolServiceMock = new Mock<IBonusPoolService>();
            bonusPoolServiceMock.Setup(a => a.EmployeeExsist(1)).ReturnsAsync(false).Verifiable();
            var controller = new BonusPoolController(bonusPoolServiceMock.Object);

            var result = await controller.CalculateBonus(new CalculateBonusDto
            {SelectedEmployeeId = 1, TotalBonusPoolAmount = 100
            });


            bonusPoolServiceMock.Verify();
            Assert.IsType<BadRequestResult>(result);
        }


        [Fact]
        public async Task CalculateBonusReturBonusDto()
        {
            int bonusPoolAmonut = 100;
            int selectedEmployeeId = 1;
            var builder = new Builder();

            var bonus = new BonusPoolCalculatorResultDto
            {
                Amount = 21321,
                Employee = builder.CreateNew<EmployeeDto>().With(a => a.Department = builder.CreateNew<DepartmentDto>().Build()).Build()
            };
            var bonusPoolServiceMock = new Mock<IBonusPoolService>();
            bonusPoolServiceMock.Setup(a => a.EmployeeExsist(selectedEmployeeId)).ReturnsAsync(true);
            bonusPoolServiceMock.Setup(a => a.CalculateAsync(bonusPoolAmonut, selectedEmployeeId)).ReturnsAsync(bonus);
            var controller = new BonusPoolController(bonusPoolServiceMock.Object);

            var result = await controller.CalculateBonus(new CalculateBonusDto { SelectedEmployeeId =1, TotalBonusPoolAmount = 100 });

            var dto = Assert.IsType<OkObjectResult>(result);
            var bonusresult = Assert.IsType<BonusPoolCalculatorResultDto>(dto.Value);
            
        }


    }
}
