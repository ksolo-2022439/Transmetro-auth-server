using System.Net.Http.Json;
using TransmetroConecta.Auth.Application.Interfaces;

namespace TransmetroConecta.Auth.Infrastructure.Integration;

public class WalletIntegrationService(HttpClient httpClient) : IWalletIntegrationService
{
    /// <summary>
    /// Emite una petición HTTP al server-client para crear la billetera virtual del usuario con 5 viajes de cortesía.
    /// </summary>
    public async Task<bool> InitializeWalletAsync(Guid userId)
    {
        var payload = new { UserId = userId, CourtesyTrips = 5, Balance = 0 };
        var response = await httpClient.PostAsJsonAsync("/api/wallets/initialize", payload);
        return response.IsSuccessStatusCode;
    }

    /// <summary>
    /// Emite una petición HTTP al server-client para acreditar el saldo recargado a la billetera virtual.
    /// </summary>
    public async Task<bool> AddFundsAsync(Guid userId, decimal amount)
    {
        var payload = new { UserId = userId, Amount = amount };
        var response = await httpClient.PostAsJsonAsync("/api/wallets/recharge", payload);
        return response.IsSuccessStatusCode;
    }
}