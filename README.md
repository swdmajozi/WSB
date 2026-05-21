# Job Application REST API

A ASP.NET Core Web API for managing job listings and job applications.

## Features

- View available jobs (open positions only)
- Submit job applications with validation
- View all submitted applications
- Business rule enforcement:
  - Only open jobs accept applications
  - Duplicate applications prevention (same email per job)
  - Required field validation

## Technology Stack

- **Framework**: ASP.NET Core 8.0
- **Language**: C# 12
- **Data Storage**: In-memory (Entity Framework Core)
- **Validation**: FluentValidation

## Project Structure

```
JobApplicationAPI/
├── Models/                 # Data models
│   ├── Job.cs
│   ├── Application.cs
│   └── Dtos/              # Data Transfer Objects
│       ├── JobDto.cs
│       ├── ApplicationDto.cs
│       └── CreateApplicationRequest.cs
├── Services/              # Business logic
│   ├── JobService.cs
│   ├── ApplicationService.cs
│   └── Interfaces/
├── Controllers/           # API endpoints
│   ├── JobsController.cs
│   └── ApplicationsController.cs
├── Validators/            # FluentValidation rules
│   └── CreateApplicationValidator.cs
├── Data/                  # Database context
│   └── ApplicationDbContext.cs
├── Program.cs             # Startup configuration
└── appsettings.json       # Configuration
```

## Setup Instructions

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 / VS Code / Rider

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/swdmajozi/WSB.git
   cd WSB/JobApplicationAPI
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

   The API will start at `https://localhost:5001` or `http://localhost:5000`

### Verify Installation

4. **Test the API**
   ```bash
   # Get all jobs
   curl http://localhost:5000/jobs
   
   # Get all applications
   curl http://localhost:5000/applications
   ```

## API Endpoints

### Jobs

#### GET /jobs
Returns a list of all available jobs with status "open".

**Response (200 OK)**
```json
[
  {
    "id": 1,
    "title": "Senior Backend Developer",
    "location": "Johannesburg",
    "department": "Engineering",
    "description": "Build APIs",
    "status": "open"
  }
]
```

### Applications

#### POST /applications
Submit a new job application.

**Request Body**
```json
{
  "jobId": 1,
  "fullName": "John Doe",
  "email": "john@example.com",
  "phone": "+27 123 456 789",
  "coverLetter": "I am very interested in this position..."
}
```

**Response (201 Created)**
```json
{
  "id": 1,
  "jobId": 1,
  "fullName": "John Doe",
  "email": "john@example.com",
  "phone": "+27 123 456 789",
  "coverLetter": "I am very interested in this position...",
  "createdAt": "2026-05-21T10:30:00Z"
}
```

**Error Responses**
- `400 Bad Request` - Invalid input or validation failed
- `409 Conflict` - Duplicate application (same email for same job) or job not open

#### GET /applications
Returns all submitted applications.

**Response (200 OK)**
```json
[
  {
    "id": 1,
    "jobId": 1,
    "fullName": "John Doe",
    "email": "john@example.com",
    "phone": "+27 123 456 789",
    "coverLetter": "I am very interested in this position...",
    "createdAt": "2026-05-21T10:30:00Z"
  }
]
```

## Mock Data

The application initializes with sample job data:

1. **Senior Backend Developer** - Johannesburg, Engineering
2. **Frontend Developer** - Cape Town, Engineering

Applications are stored in-memory and will be reset on application restart.

## Design Decisions

### 1. **In-Memory Database**
   - Used Entity Framework Core with in-memory provider for simplicity
   - Data persists during application runtime
   - Suitable for development and testing

### 2. **DTOs (Data Transfer Objects)**
   - Separate DTOs from domain models to control API contracts
   - Prevents accidental exposure of internal fields
   - Allows flexible API versioning

### 3. **Service Layer**
   - Business logic separated from controllers
   - Easier to test and maintain
   - Reusable across multiple controllers if needed

### 4. **FluentValidation**
   - Declarative validation rules
   - Clear error messages
   - Easy to test independently

### 5. **HTTP Status Codes**
   - `200 OK` - Successful GET requests
   - `201 Created` - Successful POST requests
   - `400 Bad Request` - Validation errors
   - `409 Conflict` - Business rule violations (duplicates, closed jobs)

### 6. **Error Handling**
   - Global exception handling middleware
   - Consistent error response format
   - Descriptive error messages for debugging

## Example Usage

### Using cURL

```bash
# Get all jobs
curl -X GET http://localhost:5000/jobs

# Submit an application
curl -X POST http://localhost:5000/applications \
  -H "Content-Type: application/json" \
  -d '{
    "jobId": 1,
    "fullName": "Jane Smith",
    "email": "jane@example.com",
    "phone": "+27 987 654 321",
    "coverLetter": "Excited to apply for this role!"
  }'

# Get all applications
curl -X GET http://localhost:5000/applications
```

### Using Postman

1. Import the API endpoints
2. Set base URL to `http://localhost:5000`
3. Create requests for each endpoint
4. Test with sample data

## Testing

Run unit tests (if added):
```bash
dotnet test
```

## Troubleshooting

### Port Already in Use
Change the port in `appsettings.json` or run:
```bash
dotnet run --urls "http://localhost:5002"
```

### Validation Errors
Ensure all required fields are provided:
- `jobId` (must exist and job status must be "open")
- `fullName` (required)
- `email` (required, valid format)
- `phone` (optional)
- `coverLetter` (optional)

## Future Enhancements

- [ ] Persistent database (SQL Server, PostgreSQL)
- [ ] Authentication & Authorization (JWT)
- [ ] Pagination for large datasets
- [ ] Advanced filtering and search
- [ ] File uploads for applications
- [ ] Email notifications
- [ ] Admin dashboard
- [ ] Application status tracking

## License

MIT License
