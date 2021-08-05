using SynetecAssessmentApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Interfaces
{
    public interface IBonusPoolService
    {
        Task<List<EmployeeDto>> GetEmployeesAsync();
        Task<BonusPoolCalculatorResultDto> CalculateAsync(int bonusPoolAmount, int selectedEmployeeId);
        Task<bool> EmployeeExsist(int selectedEmployeeId);
    }
}
