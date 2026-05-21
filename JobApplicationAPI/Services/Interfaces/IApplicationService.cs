using JobApplicationAPI.Models.Dtos;

namespace JobApplicationAPI.Services.Interfaces;

public interface IApplicationService
{
    Task<ApplicationDto> CreateApplicationAsync(CreateApplicationRequest request);
    Task<IEnumerable<ApplicationDto>> GetAllApplicationsAsync();
    Task<bool> HasApplicationAsync(int jobId, string email);
}
