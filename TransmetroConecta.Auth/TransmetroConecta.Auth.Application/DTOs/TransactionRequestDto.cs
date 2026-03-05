namespace TransmetroConecta.Auth.Application.DTOs;

public class TransactionRequestDto
{
    public string CardNumber { get; set; } = string.Empty;
    public string ExpirationDate { get; set; } = string.Empty;
    public string CVV { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}