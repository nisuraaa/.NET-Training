# JWT Authentication Implementation

This document describes the JWT (JSON Web Token) authentication implementation across the microservices architecture.

## Overview

The microservices now implement JWT-based authentication with OAuth 2.0 principles and role-based authorization. This provides secure, stateless authentication across all services.

## Architecture

### Authentication Service
- **Port**: 7001
- **Purpose**: Centralized authentication and token management
- **Features**:
  - User registration and login
  - JWT token generation and validation
  - Role-based user management
  - Token refresh functionality

### Protected Microservices
- **Department Service** (Port 5106)
- **Employee Service** (Port 5107) 
- **Project Service** (Port 5043)

All microservices now require JWT authentication for access.

## User Roles

The system supports three user roles with different permission levels:

1. **Admin** - Full access to all operations
2. **Manager** - Can create and manage resources, cannot delete
3. **User** - Read-only access to most resources

## API Endpoints

### Authentication Service

#### Register User
```
POST /api/auth/register
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe", 
  "email": "john.doe@example.com",
  "password": "Password123",
  "confirmPassword": "Password123",
  "role": "Admin"
}
```

#### Login
```
POST /api/auth/login
Content-Type: application/json

{
  "email": "john.doe@example.com",
  "password": "Password123"
}
```

#### Refresh Token
```
POST /api/auth/refresh
Content-Type: application/json

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh_token_here"
}
```

#### Validate Token
```
GET /api/auth/validate
Authorization: Bearer <token>
```

#### Logout
```
POST /api/auth/logout
Authorization: Bearer <token>
```

## Authorization Policies

### Department Service
- **Create Department**: `ManagerOrAdmin` role required
- **Get Department**: `UserOrAbove` role required
- **Get Department by Name**: `UserOrAbove` role required

### Employee Service
- **Get All Employees**: `UserOrAbove` role required
- **Get Employee by ID**: `UserOrAbove` role required
- **Create Employee**: `ManagerOrAdmin` role required
- **Update Employee**: `UserOrAbove` role required
- **Delete Employee**: `AdminOnly` role required

### Project Service
- **Create Project**: `ManagerOrAdmin` role required
- **Get All Projects**: `UserOrAbove` role required
- **Get Project by ID**: `UserOrAbove` role required
- **Get Project by Name**: `UserOrAbove` role required

## JWT Configuration

All services use the same JWT configuration:

```json
{
  "Jwt": {
    "SecretKey": "ThisIsAVeryLongSecretKeyForJWTTokenGenerationThatShouldBeAtLeast32CharactersLong",
    "Issuer": "AuthenticationService",
    "Audience": "MicroservicesApp",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

## Usage Examples

### 1. Register and Login
```bash
# Register a new user
curl -X POST https://localhost:7001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@example.com", 
    "password": "Password123",
    "confirmPassword": "Password123",
    "role": "Admin"
  }'

# Login to get token
curl -X POST https://localhost:7001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john.doe@example.com",
    "password": "Password123"
  }'
```

### 2. Access Protected Resources
```bash
# Use the token from login response
TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# Create a department (requires ManagerOrAdmin role)
curl -X POST https://localhost:5106/api/department \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"name": "Engineering"}'

# Get all employees (requires UserOrAbove role)
curl -X GET https://localhost:5107/api/employee \
  -H "Authorization: Bearer $TOKEN"
```

## Swagger Integration

All services now include JWT authentication in their Swagger documentation:

1. Navigate to any service's Swagger UI (e.g., `https://localhost:7001/swagger`)
2. Click the "Authorize" button
3. Enter `Bearer <your_token>` in the format: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`
4. Click "Authorize" to authenticate
5. Now you can test all protected endpoints directly from Swagger

## Security Features

### JWT Token Security
- **Secret Key**: 64-character secret key for signing tokens
- **Expiration**: 60-minute access token lifetime
- **Refresh Tokens**: 7-day refresh token lifetime
- **Algorithm**: HMAC SHA-256 for token signing

### Password Security
- Minimum 6 characters
- Must contain uppercase, lowercase, and numeric characters
- Passwords are hashed using ASP.NET Core Identity

### Role-Based Access Control
- Hierarchical role system (Admin > Manager > User)
- Fine-grained permissions per endpoint
- Automatic role validation on each request

## Testing

Use the provided test script to verify the implementation:

```powershell
.\test-jwt-auth.ps1
```

This script will:
1. Register a new user
2. Login to get a JWT token
3. Test access to all protected endpoints
4. Verify role-based authorization
5. Test unauthorized access scenarios

## Development Notes

### Adding New Protected Endpoints

1. Add `[Authorize]` attribute to the controller or action
2. Specify role requirements using `[Authorize(Policy = "PolicyName")]`
3. Available policies:
   - `AdminOnly` - Only Admin role
   - `ManagerOrAdmin` - Manager or Admin roles
   - `UserOrAbove` - User, Manager, or Admin roles

### Custom Authorization Policies

To add new authorization policies, update the `Program.cs` file in each service:

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CustomPolicy", policy => 
        policy.RequireRole("CustomRole"));
});
```

## Troubleshooting

### Common Issues

1. **401 Unauthorized**: Check if the JWT token is valid and not expired
2. **403 Forbidden**: Verify the user has the required role for the operation
3. **Token validation failed**: Ensure all services use the same JWT configuration

### Debugging

Enable detailed logging in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Microsoft.AspNetCore.Authentication": "Debug"
    }
  }
}
```

## Security Considerations

1. **Production Deployment**: Change the JWT secret key to a secure, randomly generated value
2. **HTTPS Only**: Always use HTTPS in production
3. **Token Storage**: Store refresh tokens securely (database recommended)
4. **Rate Limiting**: Implement rate limiting for authentication endpoints
5. **Audit Logging**: Log all authentication and authorization events

## Future Enhancements

- [ ] Implement refresh token storage in database
- [ ] Add multi-factor authentication (MFA)
- [ ] Implement token blacklisting for logout
- [ ] Add API rate limiting
- [ ] Implement OAuth 2.0 with external providers (Google, Microsoft)
- [ ] Add session management and concurrent session limits
