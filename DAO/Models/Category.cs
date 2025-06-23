using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAO.Models;

[Table("Category")]
public partial class Category
{
    [Key]
    [Column("categoryId")]
    public int CategoryId { get; set; }

    [Column("categoryName")]
    public string? CategoryName { get; set; }

    [Column("categoryImage")]
    [StringLength(255)]
    public string? CategoryImage { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}
