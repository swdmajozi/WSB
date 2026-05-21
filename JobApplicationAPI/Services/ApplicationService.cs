using JobApplicationAPI.Data;
using JobApplicationAPI.Models;
using JobApplicationAPI.Models.Dtos;
using JobApplicationAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationAPI.Services;

public class ApplicationService : IApplicationService
{
    private readonly ApplicationDbContext _context;

    public ApplicationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationDto> CreateApplicationAsync(CreateApplicationRequest request)
    {
        // Check if job exists and is open
        var job = await _context.Jobs.FindAsync(request.JobId);
        if (job == null || job.Status != "open")
        {
            throw new InvalidOperationException("Job not found or is not open for applications.");
        }

        // Check for duplicate applications
        var existingApplication = await _context.Applications
            .FirstOrDefaultAsync(a => a.JobId == request.JobId && a.Email == request.Email);

        if (existingApplication != null)
        {
            throw new InvalidOperationException("An application already exists for this job with this email address.");
        }

        var application = new Application
        {
            JobId = request.JobId,
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            CoverLetter = request.CoverLetter,
            CreatedAt = DateTime.UtcNow
        };

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        return new ApplicationDto
        {
            Id = application.Id,
            JobId = application.JobId,
            FullName = application.FullName,
            Email = application.Email,
            Phone = application.Phone,
            CoverLetter = application.CoverLetter,
            CreatedAt = application.CreatedAt
        };
    }

    public async Task<IEnumerable<ApplicationDto>> GetAllApplicationsAsync()
    {
        var applications = await _context.Applications.ToListAsync();

        return applications.Select(a => new ApplicationDto
        {
            Id = a.Id,
            JobId = a.JobId,
            FullName = a.FullName,
            Email = a.Email,
            Phone = a.Phone,
            CoverLetter = a.CoverLetter,
            CreatedAt = a.CreatedAt
        }).ToList();
    }

    public async Task<bool> HasApplicationAsync(int jobId, string email)
    {
        return await _context.Applications
            .AnyAsync(a => a.JobId == jobId && a.Email == email);
    }
}
