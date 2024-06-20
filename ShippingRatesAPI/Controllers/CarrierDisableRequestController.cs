using Microsoft.AspNetCore.Mvc;
using ShippingRatesAPI.Repositories.Interfaces;

namespace ShippingRatesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrierDisableRequestsController : ControllerBase
    {
        private ICarrierDisableRequestRepository _carrierDisableRequestRepository;

        public CarrierDisableRequestsController(ICarrierDisableRequestRepository carrierDisableRequestRepository)
        {
            _carrierDisableRequestRepository = carrierDisableRequestRepository;
        }
        /// <summary>
        /// Returns all request to disable carriers
        /// </summary>
        /// <returns></returns>
        [HttpGet("show-requests")]
        public async Task<IActionResult> GetAllDisableRequests()
        {
            var requests = await _carrierDisableRequestRepository.GetAllDisableRequestsAsync();
            return Ok(requests);
        }

        /// <summary>
        /// Approve request to disable specific carrier
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}/approve-request")]
        public async Task<IActionResult> ApproveDisableRequest(int id)
        {
            try
            {
                var result = await _carrierDisableRequestRepository.ApproveDisableRequestAsync(id);
                return Ok(new { message = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
