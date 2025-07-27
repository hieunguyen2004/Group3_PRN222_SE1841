using DAO.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Jobs;

public class JobsRepository : IJobsRepository
{
    private readonly MyDbContext _context;

    public JobsRepository(MyDbContext context)
    {
        _context = context;
    }

    public Job? GetByRecruitmentId(int recruiterId)
    {
        return _context.Jobs.FirstOrDefault(j => j.RecruiterId == recruiterId);
    }

    public List<Job> GetJobsByUserId(int? userId)
    {
        return _context.Jobs
            .Include(j => j.Recruiter)
            .Include(j => j.Applications)
            .Where(j => j.Recruiter.UserId == userId)
            .ToList();
    }

    public Job? GetById(int jobId)
    {
        return _context.Jobs
            .Include(j => j.Recruiter) 
            .FirstOrDefault(j => j.JobId == jobId);
    }

    public void Create(Job job)
    {
        _context.Jobs.Add(job);
        _context.SaveChanges();
    }

    public void Update(Job job)
    {
        var existingJob = _context.Jobs.Find(job.JobId);
        if (existingJob == null) return;

        // Cập nhật từng thuộc tính cụ thể
        existingJob.JobTitle = job.JobTitle;
        existingJob.JobDescription = job.JobDescription;
        existingJob.Requirements = job.Requirements;
        existingJob.Position = job.Position;
        existingJob.Experience = job.Experience;
        existingJob.Skills = job.Skills;
        existingJob.Gender = job.Gender;
        existingJob.Profession = job.Profession;
        existingJob.NumberOfSeeker = job.NumberOfSeeker;
        existingJob.JobType = job.JobType;
        existingJob.Location = job.Location;
        existingJob.WorkingTime = job.WorkingTime;
        existingJob.Salary = job.Salary;
        existingJob.EndDate = job.EndDate;
        existingJob.CategoryId = job.CategoryId;
        _context.SaveChanges();
    }

    public void DeleteJob(Job job)
    {
        _context.Jobs.Remove(job);
        _context.SaveChanges();
    }

    public List<Job> GetJobsWithApplicantsCountByRecruiterId(int recruiterId)
    {
        return _context.Jobs
       .Include(j => j.Applications)
       .Where(j => j.RecruiterId == recruiterId)
       .ToList();
    }
}
