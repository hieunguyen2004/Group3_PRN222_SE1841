using DAO.Models;

namespace TopCVWeb.ViewModels
{
    public class JobDetailViewModel
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string Requirements { get; set; }
        public string Location { get; set; }
        public string Position { get; set; }
        public string Experience { get; set; }
        public string Skills { get; set; }
        public string Gender { get; set; }
        public string Profession { get; set; }
        public string JobType { get; set; }
        public int NumberOfSeeker { get; set; }
        public string Salary { get; set; }
        public string WorkingTime { get; set; }
        public DateOnly CreateDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Company Company { get; set; }
        public Category Category { get; set; }
        public bool IsSaved { get; set; }

        public List<JobViewModel> SameCategoryJobs { get; set; }
        public List<JobViewModel> SameCompanyJobs { get; set; }
    }
}
