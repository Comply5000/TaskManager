﻿namespace TaskManager.Application.TaskCategories.DTOs;

public sealed class TaskCategoryDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}