using JobApplicationAPI.Models.Dtos;

namespace JobApplicationAPI.Services.Interfaces;

public interface IJobService
{
    Task<IEnumerable<JobDto>> GetAllOpenJobsAsync();
    Task<JobDto?> GetJobByIdAsync(int jobId);
}
