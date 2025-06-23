using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAO.Models;

[Table("SaveJob")]
public partial class SaveJob
{
    [Key]
    [Column("saveJobId")]
    public int SaveJobId { get; set; }

    [Column("seekerId")]
    public int SeekerId { get; set; }

    [Column("jobId")]
    public int JobId { get; set; }

    [Column("saveDate")]
    public DateOnly SaveDate { get; set; }

    [ForeignKey("JobId")]
    [InverseProperty("SaveJobs")]
    public virtual Job Job { get; set; } = null!;

    [ForeignKey("SeekerId")]
    [InverseProperty("SaveJobs")]
    public virtual JobSeeker Seeker { get; set; } = null!;
}
