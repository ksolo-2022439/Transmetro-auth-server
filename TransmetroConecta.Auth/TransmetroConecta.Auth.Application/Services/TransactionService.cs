using TransmetroConecta.Auth.Application.DTOs;
using TransmetroConecta.Auth.Application.Interfaces;

namespace TransmetroConecta.Auth.Application.Services;

public class TransactionService(IWalletIntegrationService walletIntegrationService) : ITransactionService
{
    /// <summary>
    /// Procesa una transacción simulada validando la tarjeta mediante el algoritmo de Luhn y notifica la acreditación.
    /// </summary>
    public async Task<TransactionResponseDto> ProcessPaymentAsync(Guid userId, TransactionRequestDto request)
    {
        if (!IsValidLuhn(request.CardNumber))
        {
            return new TransactionResponseDto { IsSuccess = false, Message = "Número de tarjeta inválido." };
        }

        await Task.Delay(1500);

        var walletUpdated = await walletIntegrationService.AddFundsAsync(userId, request.Amount);

        if (!walletUpdated)
        {
            return new TransactionResponseDto { IsSuccess = false, Message = "Transacción aprobada, pero falló la sincronización con la billetera." };
        }

        return new TransactionResponseDto
        {
            IsSuccess = true,
            Message = "Recarga procesada exitosamente.",
            TransactionId = Guid.NewGuid().ToString()
        };
    }

    /// <summary>
    /// Valida un número de tarjeta de crédito o débito utilizando el algoritmo de Luhn.
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

    /// <summary>
    /// Procesa el pago inicial de Q20.00 validando la tarjeta ingresada y emite la orden S2S para inicializar la billetera con viajes de cortesía.
    /// </summary>
    public async Task<TransactionResponseDto> PurchaseCardAsync(Guid userId, TransactionRequestDto request)
    {
        if (request.Amount != 20.00m)
        {
            return new TransactionResponseDto { IsSuccess = false, Message = "El costo de emisión de la Tarjeta Ciudadana es exactamente Q20.00." };
        }

        if (!IsValidLuhn(request.CardNumber))
        {
            return new TransactionResponseDto { IsSuccess = false, Message = "Número de tarjeta inválido." };
        }

        await Task.Delay(1500);

        var walletInitialized = await walletIntegrationService.InitializeWalletAsync(userId);

        if (!walletInitialized)
        {
            return new TransactionResponseDto { IsSuccess = false, Message = "Transacción aprobada, pero falló la creación de la billetera. Contacte soporte." };
        }

        return new TransactionResponseDto
        {
            IsSuccess = true,
            Message = "Tarjeta Ciudadana adquirida exitosamente. Se han acreditado 5 viajes de cortesía.",
            TransactionId = Guid.NewGuid().ToString()
        };
    }
}