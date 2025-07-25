﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAO.Models;

[Table("CV")]
public partial class Cv
{
    [Key]
    [Column("cvId")]
    public int CvId { get; set; }

    [Column("seekerId")]
    public int? SeekerId { get; set; }

    [Column("cvStatus")]
    [StringLength(50)]
    public string? CvStatus { get; set; }

    [Column("fileName")]
    [StringLength(255)]
    public string? FileName { get; set; }

    [Column("cvLink")]
    public byte[]? CvLink { get; set; }

    [InverseProperty("Cv")]
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    [ForeignKey("SeekerId")]
    [InverseProperty("Cvs")]
    public virtual JobSeeker? Seeker { get; set; }
}
