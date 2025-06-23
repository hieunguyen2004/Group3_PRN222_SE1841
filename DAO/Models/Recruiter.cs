using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAO.Models;

[Table("Recruiter")]
public partial class Recruiter
{
    [Key]
    [Column("recruiterId")]
    public int RecruiterId { get; set; }

    [Column("companyId")]
    public int? CompanyId { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [Column("position2")]
    [StringLength(255)]
    public string Position2 { get; set; } = null!;

    [Column("companyEmail")]
    public string CompanyEmail { get; set; } = null!;

    [ForeignKey("CompanyId")]
    [InverseProperty("Recruiters")]
    public virtual Company? Company { get; set; }

    [InverseProperty("Recruiter")]
    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    [ForeignKey("UserId")]
    [InverseProperty("Recruiters")]
    public virtual User User { get; set; } = null!;
}
