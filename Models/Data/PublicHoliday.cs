using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WaterlilyLabs.Models.Data;

[Index("HolidayDate", Name = "UQ__PublicHo__F1ADD9D12578E29A", IsUnique = true)]
public partial class PublicHoliday
{
    [Key]
    public int Id { get; set; }

    public DateOnly HolidayDate { get; set; }

    [StringLength(200)]
    public string Description { get; set; } = null!;
}
