using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAO.Models;

[Table("Job")]
public partial class Job
{
    [Key]
    [Column("jobId")]
    public int JobId { get; set; }

    [Column("recruiterId")]
    public int? RecruiterId { get; set; }

    [Column("categoryId")]
    public int? CategoryId { get; set; }

    [Column("jobTitle")]
    [StringLength(255)]
    public string? JobTitle { get; set; }

    [Column("jobDescription")]
    public string? JobDescription { get; set; }

    [Column("requirements")]
    public string? Requirements { get; set; }

    [Column("location")]
    [StringLength(255)]
    public string? Location { get; set; }

    [Column("position")]
    [StringLength(255)]
    public string? Position { get; set; }

    [Column("experience")]
    [StringLength(255)]
    public string? Experience { get; set; }

    [Column("skills")]
    public string? Skills { get; set; }

    [Column("gender")]
    [StringLength(20)]
    public string? Gender { get; set; }

    [Column("profession")]
    [StringLength(255)]
    public string? Profession { get; set; }

    [Column("jobType")]
    [StringLength(255)]
    public string? JobType { get; set; }

    [Column("numberOfSeeker")]
    public int? NumberOfSeeker { get; set; }

    [Column("salary")]
    [StringLength(255)]
    public string? Salary { get; set; }

    [Column("workingTime")]
    [StringLength(255)]
    public string? WorkingTime { get; set; }

    [Column("createDate")]
    public DateOnly? CreateDate { get; set; }

    [Column("endDate")]
    public DateOnly? EndDate { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [InverseProperty("Job")]
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    [ForeignKey("CategoryId")]
    [InverseProperty("Jobs")]
    public virtual Category? Category { get; set; }

    [ForeignKey("RecruiterId")]
    [InverseProperty("Jobs")]
    public virtual Recruiter? Recruiter { get; set; }

    [InverseProperty("Job")]
    public virtual ICollection<SaveJob> SaveJobs { get; set; } = new List<SaveJob>();
}
