using JobApplicationAPI.Data;
using JobApplicationAPI.Services;
using JobApplicationAPI.Services.Interfaces;
using JobApplicationAPI.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework with in-memory database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("JobApplicationsDb"));

// Register services
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();

// Register validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateApplicationValidator>();

var app = builder.Build();

// Initialize database with sample data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
    
    if (!dbContext.Jobs.Any())
    {
        dbContext.Jobs.AddRange(
            new Models.Job
            {
                Id = 1,
                Title = "Senior Backend Developer",
                Location = "Johannesburg",
                Department = "Engineering",
                Description = "Build APIs",
                Status = "open"
            },
            new Models.Job
            {
                Id = 2,
                Title = "Frontend Developer",
                Location = "Cape Town",
                Department = "Engineering",
                Description = "Build UI",
                Status = "open"
            }
        );
        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
