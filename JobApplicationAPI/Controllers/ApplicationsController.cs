using JobApplicationAPI.Models.Dtos;
using JobApplicationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace JobApplicationAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly IValidator<CreateApplicationRequest> _validator;

    public ApplicationsController(IApplicationService applicationService, IValidator<CreateApplicationRequest> validator)
    {
        _applicationService = applicationService;
        _validator = validator;
    }

    /// <summary>
    /// Get all submitted applications
    /// </summary>
    /// <returns>List of applications</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetApplications()
    {
        var applications = await _applicationService.GetAllApplicationsAsync();
        return Ok(applications);
    }

    /// <summary>
    /// Submit a new job application
    /// </summary>
    /// <param name="request">Application details</param>
    /// <returns>Created application</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationRequest request)
    {
        // Validate request
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                errors = validationResult.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    message = e.ErrorMessage
                })
            });
        }

        try
        {
            var application = await _applicationService.CreateApplicationAsync(request);
            return CreatedAtAction(nameof(GetApplications), new { id = application.Id }, application);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
