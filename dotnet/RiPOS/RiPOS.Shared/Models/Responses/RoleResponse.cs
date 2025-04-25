﻿namespace RiPOS.Shared.Models.Responses;

public class RoleResponse
{
    public int Id { get; set; }

    public required string Code { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }
}