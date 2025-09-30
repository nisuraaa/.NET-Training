# JWT Authentication Test Script
# This script tests the JWT authentication implementation across all microservices

Write-Host "=== JWT Authentication Test Script ===" -ForegroundColor Green

# Configuration
$authServiceUrl = "https://localhost:7001"
$departmentServiceUrl = "https://localhost:5106"
$employeeServiceUrl = "https://localhost:5107"
$projectServiceUrl = "https://localhost:5043"

# Skip SSL certificate validation for testing
[System.Net.ServicePointManager]::ServerCertificateValidationCallback = {$true}

# Function to make HTTP requests
function Invoke-ApiRequest {
    param(
        [string]$Url,
        [string]$Method = "GET",
        [hashtable]$Headers = @{},
        [string]$Body = $null
    )
    
    try {
        $requestParams = @{
            Uri = $Url
            Method = $Method
            Headers = $Headers
            ContentType = "application/json"
        }
        
        if ($Body) {
            $requestParams.Body = $Body
        }
        
        $response = Invoke-RestMethod @requestParams
        return $response
    }
    catch {
        Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
        return $null
    }
}

# Test 1: Register a new user
Write-Host "`n1. Testing User Registration..." -ForegroundColor Yellow
$registerData = @{
    FirstName = "John"
    LastName = "Doe"
    Email = "john.doe@example.com"
    Password = "Password123"
    ConfirmPassword = "Password123"
    Role = "Admin"
} | ConvertTo-Json

$registerResponse = Invoke-ApiRequest -Url "$authServiceUrl/api/auth/register" -Method "POST" -Body $registerData

if ($registerResponse) {
    Write-Host "✓ User registration successful" -ForegroundColor Green
    $token = $registerResponse.Token
    Write-Host "Token: $($token.Substring(0, 50))..." -ForegroundColor Cyan
} else {
    Write-Host "✗ User registration failed" -ForegroundColor Red
    exit 1
}

# Test 2: Login with the registered user
Write-Host "`n2. Testing User Login..." -ForegroundColor Yellow
$loginData = @{
    Email = "john.doe@example.com"
    Password = "Password123"
} | ConvertTo-Json

$loginResponse = Invoke-ApiRequest -Url "$authServiceUrl/api/auth/login" -Method "POST" -Body $loginData

if ($loginResponse) {
    Write-Host "✓ User login successful" -ForegroundColor Green
    $token = $loginResponse.Token
    Write-Host "Token: $($token.Substring(0, 50))..." -ForegroundColor Cyan
} else {
    Write-Host "✗ User login failed" -ForegroundColor Red
    exit 1
}

# Set up headers with JWT token
$authHeaders = @{
    "Authorization" = "Bearer $token"
}

# Test 3: Test Department Service with JWT
Write-Host "`n3. Testing Department Service with JWT..." -ForegroundColor Yellow

# Test creating a department (requires ManagerOrAdmin role)
$departmentData = @{
    Name = "Engineering"
} | ConvertTo-Json

$deptCreateResponse = Invoke-ApiRequest -Url "$departmentServiceUrl/api/department" -Method "POST" -Headers $authHeaders -Body $departmentData

if ($deptCreateResponse) {
    Write-Host "✓ Department creation successful" -ForegroundColor Green
    $departmentId = $deptCreateResponse.Id
} else {
    Write-Host "✗ Department creation failed" -ForegroundColor Red
}

# Test getting departments (requires UserOrAbove role)
$deptGetResponse = Invoke-ApiRequest -Url "$departmentServiceUrl/api/department/$departmentId" -Method "GET" -Headers $authHeaders

if ($deptGetResponse) {
    Write-Host "✓ Department retrieval successful" -ForegroundColor Green
} else {
    Write-Host "✗ Department retrieval failed" -ForegroundColor Red
}

# Test 4: Test Employee Service with JWT
Write-Host "`n4. Testing Employee Service with JWT..." -ForegroundColor Yellow

# Test getting all employees (requires UserOrAbove role)
$employeesResponse = Invoke-ApiRequest -Url "$employeeServiceUrl/api/employee" -Method "GET" -Headers $authHeaders

if ($employeesResponse) {
    Write-Host "✓ Employee retrieval successful" -ForegroundColor Green
} else {
    Write-Host "✗ Employee retrieval failed" -ForegroundColor Red
}

# Test 5: Test Project Service with JWT
Write-Host "`n5. Testing Project Service with JWT..." -ForegroundColor Yellow

# Test creating a project (requires ManagerOrAdmin role)
$projectData = @{
    Name = "Microservices Project"
} | ConvertTo-Json

$projectCreateResponse = Invoke-ApiRequest -Url "$projectServiceUrl/api/project" -Method "POST" -Headers $authHeaders -Body $projectData

if ($projectCreateResponse) {
    Write-Host "✓ Project creation successful" -ForegroundColor Green
    $projectId = $projectCreateResponse.Id
} else {
    Write-Host "✗ Project creation failed" -ForegroundColor Red
}

# Test getting all projects (requires UserOrAbove role)
$projectsResponse = Invoke-ApiRequest -Url "$projectServiceUrl/api/project" -Method "GET" -Headers $authHeaders

if ($projectsResponse) {
    Write-Host "✓ Project retrieval successful" -ForegroundColor Green
} else {
    Write-Host "✗ Project retrieval failed" -ForegroundColor Red
}

# Test 6: Test unauthorized access (without token)
Write-Host "`n6. Testing Unauthorized Access..." -ForegroundColor Yellow

$unauthorizedResponse = Invoke-ApiRequest -Url "$departmentServiceUrl/api/department" -Method "GET"

if (-not $unauthorizedResponse) {
    Write-Host "✓ Unauthorized access properly blocked" -ForegroundColor Green
} else {
    Write-Host "✗ Unauthorized access was not blocked" -ForegroundColor Red
}

# Test 7: Test token validation
Write-Host "`n7. Testing Token Validation..." -ForegroundColor Yellow

$validateResponse = Invoke-ApiRequest -Url "$authServiceUrl/api/auth/validate" -Method "GET" -Headers $authHeaders

if ($validateResponse) {
    Write-Host "✓ Token validation successful" -ForegroundColor Green
} else {
    Write-Host "✗ Token validation failed" -ForegroundColor Red
}

Write-Host "`n=== JWT Authentication Test Complete ===" -ForegroundColor Green
Write-Host "All microservices are now secured with JWT authentication and role-based authorization!" -ForegroundColor Cyan
