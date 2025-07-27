namespace TopCVWeb.Models
{
    public class ApplicantViewModel
    {
        public int ApplicationId { get; set; }
        public int CvId { get; set; }
        public string? FullName { get; set; }
        public DateOnly? SubmitDate { get; set; }
        public string? Status { get; set; }

        public string? Email { get; set; }
    }
}
