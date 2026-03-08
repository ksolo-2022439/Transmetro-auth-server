using TransmetroConecta.Auth.Application.DTOs;

namespace TransmetroConecta.Auth.Application.Interfaces;

public interface ITransactionService
{
    /// <summary>
    /// Procesa una transacción simulada validando la tarjeta y acredita los fondos.
    /// </summary>
    Task<TransactionResponseDto> ProcessPaymentAsync(Guid userId, TransactionRequestDto request);
    Task<TransactionResponseDto> PurchaseCardAsync(Guid userId, TransactionRequestDto request);
}