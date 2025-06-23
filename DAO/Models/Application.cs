using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAO.Models;

[Table("Application")]
public partial class Application
{
    [Key]
    [Column("applicationId")]
    public int ApplicationId { get; set; }

    [Column("jobId")]
    public int? JobId { get; set; }

    [Column("cvId")]
    public int? CvId { get; set; }

    [Column("submitDate")]
    public DateOnly? SubmitDate { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [ForeignKey("CvId")]
    [InverseProperty("Applications")]
    public virtual Cv? Cv { get; set; }

    [ForeignKey("JobId")]
    [InverseProperty("Applications")]
    public virtual Job? Job { get; set; }
}
