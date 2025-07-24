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

    [InverseProperty("Seeker")]
    public virtual ICollection<Cv> Cvs { get; set; } = new List<Cv>();

    [InverseProperty("Seeker")]
    public virtual ICollection<SaveJob> SaveJobs { get; set; } = new List<SaveJob>();

    [ForeignKey("UserId")]
    [InverseProperty("JobSeekers")]
    public virtual User User { get; set; } = null!;
}
