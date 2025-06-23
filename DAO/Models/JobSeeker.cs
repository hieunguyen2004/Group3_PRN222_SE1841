using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAO.Models;

[Table("JobSeeker")]
public partial class JobSeeker
{
    [Key]
    [Column("seekerId")]
    public int SeekerId { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [Column("skills")]
    [StringLength(255)]
    public string? Skills { get; set; }

    [Column("industry")]
    [StringLength(255)]
    public string Industry { get; set; } = null!;

    [Column("experience")]
    [StringLength(255)]
    public string Experience { get; set; } = null!;

    [Column("position")]
    [StringLength(255)]
    public string Position { get; set; } = null!;

    [Column("profession")]
    [StringLength(255)]
    public string Profession { get; set; } = null!;

    [Column("location")]
    [StringLength(255)]
    public string Location { get; set; } = null!;

    [Column("salary")]
    [StringLength(255)]
    public string Salary { get; set; } = null!;

    [InverseProperty("Seeker")]
    public virtual ICollection<Cv> Cvs { get; set; } = new List<Cv>();

    [InverseProperty("Seeker")]
    public virtual ICollection<SaveJob> SaveJobs { get; set; } = new List<SaveJob>();

    [ForeignKey("UserId")]
    [InverseProperty("JobSeekers")]
    public virtual User User { get; set; } = null!;
}
