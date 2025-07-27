using DAO.Models;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CVService: ICVService
    {
        private readonly ICVRepository _cvRepository;
        private readonly IApplicationRepository _applicationRepository;
        public CVService(ICVRepository cvRepository,IApplicationRepository applicationRepository)
        {
            _cvRepository = cvRepository;
            _applicationRepository = applicationRepository;
        }

        public void AddCV(Cv cv) 
        {
            _cvRepository.Add(cv);
        }

        public  Cv GetCVById(int id)
        {
            return  _cvRepository.GetById(id);
        }
        public  bool ExistsByContent(byte[] content)
        {
            return  _cvRepository.ExistsByContent(content);
        }
        public  List<Cv> GetCVsBySeekerId(int seekerId)
        {
            return  _cvRepository.GetCVsBySeekerId(seekerId);
        }

        public void ConfirmCv(int cvId,string email, int appId, int userId)
        {
            _cvRepository.ConfirmCv(cvId);
            var fromEmail = "sontranphamthai@gmail.com";
            var fromPassword = "occy rhrj itvt vdez";
            var job = _applicationRepository.GetJobByApplicationId(appId);
            var company = _applicationRepository.GetCompanyByRecruiterId(_applicationRepository.GetRecruiterIdFromUserId(userId));
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromEmail, fromPassword)
            };

            var mail = new MailMessage(fromEmail, email)
            {
                Subject = "Thông báo CV trúng tuyển ",
                Body = $"Xin chúc mừng! CV của bạn cho công việc {job.JobTitle} của công ty {company.CompanyName} đã được chấp nhận. Chúng tôi sẽ liên hệ với bạn sớm nhất.",
                IsBodyHtml = false
            };

            smtp.Send(mail);
        }

        public void RejectCv(int cvId, string email, int appId, int userId)
        {
            _cvRepository.RejectCv(cvId);
            var fromEmail = "sontranphamthai@gmail.com";
            var fromPassword = "occy rhrj itvt vdez";
            var job = _applicationRepository.GetJobByApplicationId(appId);
            var company = _applicationRepository.GetCompanyByRecruiterId(_applicationRepository.GetRecruiterIdFromUserId(userId));
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromEmail, fromPassword)
            };

            var mail = new MailMessage(fromEmail, email)
            {
                Subject = "Thông báo Reject CV ",
                Body = $"Chúng tôi rất tiếc phải thông báo rằng CV của bạn cho vị trí {job.JobTitle} tại công ty {company.CompanyName} chưa được chấp nhận. " +
       "Chúng tôi đánh giá cao sự quan tâm của bạn và mong rằng sẽ có cơ hội hợp tác trong tương lai.",
                IsBodyHtml = false
            };

            smtp.Send(mail);
        }
    }
}
