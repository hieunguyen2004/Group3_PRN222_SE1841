using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAO.Models;

[Table("tokenForgetPassword")]
public partial class TokenForgetPassword
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("token")]
    [StringLength(255)]
    public string? Token { get; set; }

    [Column("expiryTime", TypeName = "datetime")]
    public DateTime? ExpiryTime { get; set; }

    [Column("isUsed")]
    public bool? IsUsed { get; set; }

    [Column("userId")]
    public int? UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("TokenForgetPasswords")]
    public virtual User? User { get; set; }
}
