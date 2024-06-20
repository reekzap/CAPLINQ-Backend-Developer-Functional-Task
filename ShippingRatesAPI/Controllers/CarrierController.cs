using Microsoft.AspNetCore.Mvc;
using ShippingRatesAPI.Models;
using ShippingRatesAPI.Repositories.Interfaces;

namespace ShippingRatesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarrierController : ControllerBase
    {
        private readonly ICarrierRepository _carrierRepository;

        public CarrierController(ICarrierRepository carrierRepository)
        {
            _carrierRepository = carrierRepository;
        }


        /// <summary>
        /// Returns all carrier
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCarriers()
        {
            var carriers = await _carrierRepository.GetAllCarriersAsync();
            return Ok(carriers);
        }

        /// <summary>
        /// Create new carrier
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddCarrier([FromBody] Carrier carrier)
        {
            await _carrierRepository.AddCarrierAsync(carrier);
            return Ok();
        }

        /// <summary>
        /// Update specific carrier details
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCarrier(int id, [FromBody] Carrier carrier)
        {
            if (id != carrier.Id)
            {
                return BadRequest();
            }

            await _carrierRepository.UpdateCarrierAsync(carrier);
            return Ok();
        }

        /// <summary>
        /// Remove specific carrier
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCarrier(int id)
        {
            await _carrierRepository.RemoveCarrierAsync(id);
            return Ok();
        }

        /// <summary>
        /// Disable specific carrier
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Expected Input:
        ///
        /// ```string
        /// "You Reason to Disable"
        /// ```
        /// Expected Output:
        ///
        /// ```json
        /// {
        ///     "message": "Carrier disabled successfully."
        /// }
        /// ```
        /// </remarks>
        [HttpPost("{id}/disable-carrier")]
        public async Task<IActionResult> DisableCarrier(int id, [FromBody] string reason)
        {
            try
            {
                var result = await _carrierRepository.DisableCarrierAsync(id, reason);
                return Ok(new { message = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Enable specific carrier
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}/enable-carrier")]
        public async Task<IActionResult> EnableCarrier(int id)
        {
            try
            {
                var result = await _carrierRepository.EnableCarrierAsync(id);
                return Ok(new { message = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
