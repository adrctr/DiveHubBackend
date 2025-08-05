﻿namespace DiveHub.Core.Entities;

public class User
{
    public int UserId { get; set; }
    public string Auth0UserId { get; set; } = string.Empty; // Nouveau champ pour lier avec Auth0
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? Picture { get; set; } = string.Empty; // Champ optionnel pour la photo de profil
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Dive> Dives { get; set; } = new List<Dive>();
}
