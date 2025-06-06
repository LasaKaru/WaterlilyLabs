using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WaterlilyLabs.Models.Data;

public partial class Employee
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(100)]
    public string JobPosition { get; set; } = null!;
}
