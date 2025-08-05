namespace DiveHub.Application.Dto;

public class UserDto
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Picture { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class UserCreateDto
{
    public string Auth0UserId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Picture { get; set; } = string.Empty;
}

public class UserUpdateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Picture { get; set; } = string.Empty;
}
