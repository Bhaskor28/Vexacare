using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Vexacare.Domain.Entities.MedicalReport;

namespace Vexacare.Application.MedicalReport
{
    public interface IMedicalReportService
    {
        Task<MedicalReportAnalysisResult> AnalyzeMedicalReportAsync(IFormFile medicalReport, string userAllInfo);
        Task<bool> TestApiConnectionAsync();
    }
}
