﻿namespace TaskManager.Core.Identity.DTOs;

public sealed class ConfirmAccountDto
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
}