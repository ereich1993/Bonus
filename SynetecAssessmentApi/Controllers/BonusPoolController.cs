using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Interfaces;
using SynetecAssessmentApi.Services;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Controllers
{
    [Route("api/[controller]")]
    public class BonusPoolController : Controller
    {
        private readonly IBonusPoolService bonusPoolService;

        public BonusPoolController(IBonusPoolService bonusPoolService)
        {
            this.bonusPoolService = bonusPoolService;
        }

        [HttpGet("getAllEmployees")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await bonusPoolService.GetEmployeesAsync());
        }

        [HttpPost("calculateBonus")]
        public async Task<IActionResult> CalculateBonus([FromBody] CalculateBonusDto request)
        {
            
            if (request == null || request.TotalBonusPoolAmount <= 0)
            {
                return BadRequest();
            }
            var IsExistingEmployee = await bonusPoolService.EmployeeExsist(request.SelectedEmployeeId);
            if (!IsExistingEmployee)
            {
                return BadRequest();
            }

            var bonus = await bonusPoolService.CalculateAsync(
                request.TotalBonusPoolAmount,
                request.SelectedEmployeeId);

            return Ok(bonus);
        }
    }
}
