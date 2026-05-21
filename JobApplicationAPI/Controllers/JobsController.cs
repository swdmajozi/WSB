using JobApplicationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    /// <summary>
    /// Get all available jobs with 'open' status
    /// </summary>
    /// <returns>List of available jobs</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJobs()
    {
        var jobs = await _jobService.GetAllOpenJobsAsync();
        return Ok(jobs);
    }
}
