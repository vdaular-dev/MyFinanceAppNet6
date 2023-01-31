﻿using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class ExpenseModel : BaseModel
{
#nullable disable
    public ulong Id { get; set; }

    [Required]
    public string Description { get; set; }
#nullable enable
}
