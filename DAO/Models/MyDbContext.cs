using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAO.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Cv> Cvs { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobSeeker> JobSeekers { get; set; }

    public virtual DbSet<Recruiter> Recruiters { get; set; }

    public virtual DbSet<SaveJob> SaveJobs { get; set; }

    public virtual DbSet<TokenForgetPassword> TokenForgetPasswords { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Database=PRN222_G3_SU25;Uid=sa;Pwd=123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasOne(d => d.Cv).WithMany(p => p.Applications).HasConstraintName("FK_Application_CV");

            entity.HasOne(d => d.Job).WithMany(p => p.Applications).HasConstraintName("FK_Application_Job");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Company__AD545990684A5AFC");
        });

        modelBuilder.Entity<Cv>(entity =>
        {
            entity.HasOne(d => d.Seeker).WithMany(p => p.Cvs).HasConstraintName("FK_CV_JobSeeker1");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.JobId).HasName("PK__Job__164AA1A8F0AA0CA1");

            entity.HasOne(d => d.Category).WithMany(p => p.Jobs).HasConstraintName("FK_Job_Category");

            entity.HasOne(d => d.Recruiter).WithMany(p => p.Jobs).HasConstraintName("FK_Job_Recruiter");
        });

        modelBuilder.Entity<JobSeeker>(entity =>
        {
            entity.HasKey(e => e.SeekerId).HasName("PK__JobSeeke__789B9750FFEF85D0");

            entity.HasOne(d => d.User).WithMany(p => p.JobSeekers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_JobSeeker_Users");
        });

        modelBuilder.Entity<Recruiter>(entity =>
        {
            entity.HasOne(d => d.Company).WithMany(p => p.Recruiters).HasConstraintName("FK_Recruiter_Company");

            entity.HasOne(d => d.User).WithMany(p => p.Recruiters)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recruiter_Users");
        });

        modelBuilder.Entity<SaveJob>(entity =>
        {
            entity.HasOne(d => d.Job).WithMany(p => p.SaveJobs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaveJob_Job");

            entity.HasOne(d => d.Seeker).WithMany(p => p.SaveJobs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaveJob_JobSeeker");
        });

        modelBuilder.Entity<TokenForgetPassword>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.TokenForgetPasswords).HasConstraintName("FK_tokenForgetPassword_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.TokenExpiry)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
