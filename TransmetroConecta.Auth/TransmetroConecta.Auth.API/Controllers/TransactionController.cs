using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransmetroConecta.Auth.Application.DTOs;
using TransmetroConecta.Auth.Application.Interfaces;

namespace TransmetroConecta.Auth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    /// <summary>
    // Expone el endpoint protegido para procesar la recarga de saldo de la billetera virtual mediante una tarjeta.
    /// </summary>
    [HttpPost("recharge")]
    public async Task<IActionResult> Recharge([FromBody] TransactionRequestDto request)
    {
        try
        {
            var result = await transactionService.ProcessPaymentAsync(request);
            
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Error interno al procesar la transacción." });
        }
    }
}