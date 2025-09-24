# JWT Authentication Implementation

This document describes the JWT (JSON Web Token) authentication implementation across the microservices architecture.

## Overview

JWT authentication has been implemented across all microservices to provide secure, stateless authentication and role-based authorization. The implementation includes:

- JWT token generation and validation
- Role-based access control (RBAC)
- OAuth 2.0 compatible token-based security
- Cross-service authentication

## Architecture

### Shared Authentication Components

All authentication components are located in the `SharedEvents` project to ensure consistency across microservices:

- **JwtConfiguration**: Configuration settings for JWT tokens
- **UserRoles**: Predefined user roles (Admin, Manager, Employee, ReadOnly)
- **User**: User entity with roles and authentication data
- **LoginRequest/LoginResponse**: DTOs for authentication requests
- **IJwtService**: Service interface for JWT operations
- **JwtService**: Implementation of JWT token generation and validation
- **IUserService**: Service interface for user management
- **UserService**: Implementation of user authentication and management
- **JwtExtensions**: Extension methods for easy JWT configuration

### User Roles and Permissions

| Role | Permissions |
|------|-------------|
| **Admin** | Full access to all operations |
| **Manager** | Can manage employees and projects, view all data |
| **Employee** | Can create/edit data, view all data |
| **ReadOnly** | Can only view data |

### Authorization Policies

- **AdminOnly**: Requires Admin role
- **ManagerOrAdmin**: Requires Manager or Admin role
- **EmployeeOrAbove**: Requires Employee, Manager, or Admin role
- **ReadOnlyOrAbove**: Requires any role (ReadOnly, Employee, Manager, or Admin)
- **WriteAccess**: Requires Employee, Manager, or Admin role (excludes ReadOnly)

## API Endpoints

### Authentication Endpoints

All authentication endpoints are available in the Employee Service:

#### POST /api/auth/login
Authenticate user and receive JWT token.

**Request:**
```json
{
  "email": "admin@company.com",
  "password": "Admin123!"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "base64-encoded-refresh-token",
  "expiration": "2024-01-01T12:00:00Z",
  "userId": "user-id",
  "email": "admin@company.com",
  "roles": ["Admin"]
}
```

#### POST /api/auth/refresh
Refresh an expired JWT token.

**Request:**
```json
{
  "token": "expired-jwt-token"
}
```

#### POST /api/auth/validate
Validate a JWT token.

**Request:**
```json
{
  "token": "jwt-token-to-validate"
}
```

#### GET /api/auth/test-users
Get list of test users for development.

### Protected Endpoints

All microservice endpoints now require authentication. Here are the authorization requirements:

#### Employee Service
- **GET /api/employee** - ReadOnlyOrAbove
- **GET /api/employee/{id}** - ReadOnlyOrAbove
- **POST /api/employee** - WriteAccess
- **PUT /api/employee/{id}** - WriteAccess
- **DELETE /api/employee/{id}** - ManagerOrAdmin
- **GET /api/employee/department/{name}** - ReadOnlyOrAbove
- **GET /api/employee/age-range** - ReadOnlyOrAbove

#### Department Service
- **GET /api/department/{id}** - ReadOnlyOrAbove
- **GET /api/department/name/{name}** - ReadOnlyOrAbove
- **POST /api/department** - WriteAccess

#### Project Service
- **GET /api/project** - ReadOnlyOrAbove
- **GET /api/project/{id}** - ReadOnlyOrAbove
- **GET /api/project/name/{name}** - ReadOnlyOrAbove
- **POST /api/project** - WriteAccess

## Configuration

### JWT Settings

JWT configuration is stored in `appsettings.json` for each microservice:

```json
{
  "Jwt": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "MicroservicesAuth",
    "Audience": "MicroservicesUsers",
    "ExpirationMinutes": 60
  }
}
```

### Service Configuration

Each microservice includes JWT authentication in its `Program.cs`:

```csharp
// Add JWT Authentication and Authorization
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddJwtAuthorization();

// In the pipeline
app.UseAuthentication();
app.UseAuthorization();
```

## Test Users

The following test users are available for development and testing:

| Email | Password | Role | Permissions |
|-------|----------|------|-------------|
| admin@company.com | Admin123! | Admin | Full access |
| manager@company.com | Manager123! | Manager | Manage employees and projects |
| employee@company.com | Employee123! | Employee | Create/edit data |
| readonly@company.com | ReadOnly123! | ReadOnly | View only |

## Testing

### Manual Testing

1. **Start all microservices**
2. **Get authentication token:**
   ```bash
   curl -X POST https://localhost:7001/api/auth/login \
     -H "Content-Type: application/json" \
     -d '{"email":"admin@company.com","password":"Admin123!"}'
   ```

3. **Use token in requests:**
   ```bash
   curl -X GET https://localhost:7001/api/employee \
     -H "Authorization: Bearer YOUR_JWT_TOKEN"
   ```

### Automated Testing

Run the provided test scripts:

**PowerShell (Windows):**
```powershell
.\test-jwt-auth.ps1
```

**Bash (Linux/macOS):**
```bash
./test-jwt-auth.sh
```

## Security Features

### Token Security
- **HMAC SHA-256** signing algorithm
- **Configurable expiration** (default: 60 minutes)
- **Refresh token** support for seamless re-authentication
- **Token validation** with proper error handling

### Role-Based Access Control
- **Granular permissions** based on user roles
- **Policy-based authorization** for flexible access control
- **Cross-service consistency** in authorization rules

### Security Best Practices
- **Secure secret key** (minimum 32 characters)
- **Token expiration** to limit exposure
- **HTTPS required** for production
- **Proper error handling** without information leakage

## Implementation Details

### Token Structure

JWT tokens contain the following claims:
- `sub` (Subject): User ID
- `email`: User email address
- `name`: User full name
- `role`: User roles (multiple roles supported)
- `jti` (JWT ID): Unique token identifier
- `iss` (Issuer): Token issuer
- `aud` (Audience): Token audience
- `exp` (Expiration): Token expiration time

### Error Handling

Authentication errors return appropriate HTTP status codes:
- **401 Unauthorized**: Invalid credentials or expired token
- **403 Forbidden**: Valid token but insufficient permissions
- **500 Internal Server Error**: Server-side authentication errors

### CORS Configuration

CORS is configured to allow cross-origin requests with proper authentication headers.

## Production Considerations

### Security Recommendations
1. **Use strong secret keys** (minimum 256 bits)
2. **Implement token rotation** for enhanced security
3. **Use HTTPS** in production environments
4. **Implement rate limiting** for authentication endpoints
5. **Log authentication events** for security monitoring
6. **Use secure token storage** on the client side

### Performance Considerations
1. **Token caching** for frequently accessed tokens
2. **Database optimization** for user lookups
3. **Load balancing** considerations for stateless authentication
4. **Token size optimization** for network efficiency

## Troubleshooting

### Common Issues

1. **401 Unauthorized**: Check token validity and expiration
2. **403 Forbidden**: Verify user has required role/permissions
3. **Token validation errors**: Ensure consistent JWT configuration across services
4. **CORS issues**: Verify CORS configuration includes authentication headers

### Debug Information

Enable detailed logging in `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Microsoft.AspNetCore.Authentication": "Debug",
      "Microsoft.AspNetCore.Authorization": "Debug"
    }
  }
}
```

## Future Enhancements

1. **Refresh token rotation** for enhanced security
2. **Multi-factor authentication** support
3. **OAuth 2.0 external providers** integration
4. **Token blacklisting** for logout functionality
5. **Audit logging** for security compliance
6. **Rate limiting** for authentication endpoints
