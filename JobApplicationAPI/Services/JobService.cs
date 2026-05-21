using JobApplicationAPI.Data;
using JobApplicationAPI.Models.Dtos;
using JobApplicationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationAPI.Services;

public class JobService : IJobService
{
    private readonly ApplicationDbContext _context;

    public JobService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<JobDto>> GetAllOpenJobsAsync()
    {
        var jobs = await _context.Jobs
            .Where(j => j.Status == "open")
            .ToListAsync();

        return jobs.Select(j => new JobDto
        {
            Id = j.Id,
            Title = j.Title,
            Location = j.Location,
            Department = j.Department,
            Description = j.Description,
            Status = j.Status
        }).ToList();
    }

    public async Task<JobDto?> GetJobByIdAsync(int jobId)
    {
        var job = await _context.Jobs.FindAsync(jobId);

        if (job == null)
            return null;

        return new JobDto
        {
            Id = job.Id,
            Title = job.Title,
            Location = job.Location,
            Department = job.Department,
            Description = job.Description,
            Status = job.Status
        };
    }
}
