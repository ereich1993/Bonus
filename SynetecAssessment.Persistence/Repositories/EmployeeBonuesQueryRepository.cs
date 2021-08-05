using Microsoft.EntityFrameworkCore;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Persistence.Repositories
{
    public class EmployeeBonuesQueryRepository : IEmployeeBonusQueryRepository
    {
        
        private readonly AppDbContext _dbContext;

        public EmployeeBonuesQueryRepository(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var employee = await _dbContext.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(item => item.Id == id);
            return employee;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            IEnumerable<Employee> employees = await _dbContext
                .Employees
                .Include(e => e.Department)
                .ToListAsync();
            return employees;
        }

    }
}
