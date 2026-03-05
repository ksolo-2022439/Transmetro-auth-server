using TransmetroConecta.Auth.Application.DTOs;
using TransmetroConecta.Auth.Application.Interfaces;

namespace TransmetroConecta.Auth.Application.Services;

public class TransactionService : ITransactionService
{
    /// <summary>
    // Procesa una transacción simulada validando la tarjeta mediante el algoritmo de Luhn y retorna el resultado de la operación.
    /// </summary>
    public async Task<TransactionResponseDto> ProcessPaymentAsync(TransactionRequestDto request)
    {
        if (request.Amount <= 0)
        {
            return new TransactionResponseDto { IsSuccess = false, Message = "El monto debe ser mayor a cero." };
        }

        if (!IsValidLuhn(request.CardNumber))
        {
            return new TransactionResponseDto { IsSuccess = false, Message = "Número de tarjeta inválido." };
        }

        await Task.Delay(1500);

        return new TransactionResponseDto
        {
            IsSuccess = true,
            Message = "Recarga procesada exitosamente.",
            TransactionId = Guid.NewGuid().ToString()
        };
    }

    /// <summary>
    // Valida un número de tarjeta de crédito o débito utilizando el algoritmo de Luhn.
    /// </summary>
    private bool IsValidLuhn(string cardNumber)
    {
        int sum = 0;
        bool alternate = false;
        char[] charArray = cardNumber.Replace(" ", "").ToCharArray();
        Array.Reverse(charArray);

        foreach (char c in charArray)
        {
            if (!char.IsDigit(c)) return false;

            int n = c - '0';

            if (alternate)
            {
                n *= 2;
                if (n > 9) n -= 9;
            }

            sum += n;
            alternate = !alternate;
        }

        return (sum % 10 == 0);
    }
}