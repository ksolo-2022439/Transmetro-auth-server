using System.Security.Claims;
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
    /// Expone el endpoint protegido para procesar la recarga de saldo identificando al usuario mediante su token.
    /// </summary>
    [HttpPost("recharge")]
    public async Task<IActionResult> Recharge([FromBody] TransactionRequestDto request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Token inválido o usuario no identificado." });
        }

        var result = await transactionService.ProcessPaymentAsync(userId, request);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Expone el endpoint protegido para la compra inicial de la Tarjeta Ciudadana.
    /// </summary>
    [HttpPost("purchase-card")]
    public async Task<IActionResult> PurchaseCard([FromBody] TransactionRequestDto request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Token inválido o usuario no identificado." });
        }

        var result = await transactionService.PurchaseCardAsync(userId, request);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }
}