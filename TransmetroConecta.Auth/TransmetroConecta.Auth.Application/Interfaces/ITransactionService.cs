using TransmetroConecta.Auth.Application.DTOs;

namespace TransmetroConecta.Auth.Application.Interfaces;

public interface ITransactionService
{
    /// <summary>
    // Procesa una transacción simulada validando la tarjeta mediante el algoritmo de Luhn y retorna el resultado de la operación.
    /// </summary>
    Task<TransactionResponseDto> ProcessPaymentAsync(TransactionRequestDto request);
}