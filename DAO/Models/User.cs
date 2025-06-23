using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAO.Models;

public partial class User
{
    [Key]
    [Column("userId")]
    public int UserId { get; set; }

    [Column("roleId")]
    public int? RoleId { get; set; }

    [Column("username")]
    [StringLength(255)]
    public string? Username { get; set; }

    [Column("password")]
    [StringLength(50)]
    public string? Password { get; set; }

    [Column("firstname")]
    [StringLength(50)]
    public string? Firstname { get; set; }

    [Column("lastname")]
    [StringLength(50)]
    public string? Lastname { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Column("dateOfBirth")]
    public DateOnly? DateOfBirth { get; set; }

    [Column("gender")]
    [StringLength(20)]
    public string? Gender { get; set; }

    [Column("phone")]
    [StringLength(50)]
    public string? Phone { get; set; }

    [Column("address")]
    [StringLength(255)]
    public string? Address { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("resetToken")]
    [StringLength(255)]
    public string? ResetToken { get; set; }

    [Column("tokenExpiry")]
    public byte[]? TokenExpiry { get; set; }

    [Column("avatar")]
    [StringLength(255)]
    public string? Avatar { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<JobSeeker> JobSeekers { get; set; } = new List<JobSeeker>();

    [InverseProperty("User")]
    public virtual ICollection<Recruiter> Recruiters { get; set; } = new List<Recruiter>();

    [InverseProperty("User")]
    public virtual ICollection<TokenForgetPassword> TokenForgetPasswords { get; set; } = new List<TokenForgetPassword>();
}
