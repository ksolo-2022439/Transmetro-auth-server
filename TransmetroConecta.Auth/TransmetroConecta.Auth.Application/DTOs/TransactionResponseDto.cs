namespace TransmetroConecta.Auth.Application.DTOs;

public class TransactionResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
}