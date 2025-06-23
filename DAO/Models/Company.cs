using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAO.Models;

[Table("Company")]
public partial class Company
{
    [Key]
    [Column("companyId")]
    public int CompanyId { get; set; }

    [Column("companyName")]
    [StringLength(255)]
    public string? CompanyName { get; set; }

    [Column("city")]
    [StringLength(255)]
    public string? City { get; set; }

    [Column("district")]
    [StringLength(255)]
    public string? District { get; set; }

    [Column("commune")]
    [StringLength(255)]
    public string? Commune { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("location")]
    [StringLength(255)]
    public string? Location { get; set; }

    [Column("phone")]
    [StringLength(50)]
    public string? Phone { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Column("website")]
    [StringLength(255)]
    public string? Website { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("detail")]
    public string? Detail { get; set; }

    [Column("companyType")]
    [StringLength(255)]
    public string? CompanyType { get; set; }

    [Column("imageCompany")]
    [StringLength(255)]
    public string? ImageCompany { get; set; }

    [Column("logoCompany")]
    [StringLength(255)]
    public string? LogoCompany { get; set; }

    [Column("statusCompany")]
    [StringLength(50)]
    public string? StatusCompany { get; set; }

    [InverseProperty("Company")]
    public virtual ICollection<Recruiter> Recruiters { get; set; } = new List<Recruiter>();
}
