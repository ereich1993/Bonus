using SynetecAssessmentApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Interfaces
{
    public interface IEmployeeBonusQueryRepository
    {
        IEnumerable<Employee> GetEmployees();
    }
}
