# JWT Authentication Test Script
# This script tests JWT authentication across all microservices

$baseUrl = "https://localhost:7001"  # Employee Service URL
$departmentUrl = "https://localhost:5106"  # Department Service URL  
$projectUrl = "https://localhost:5043"  # Project Service URL

# Test users
$testUsers = @(
    @{ Email = "admin@company.com"; Password = "Admin123!"; Role = "Admin" },
    @{ Email = "manager@company.com"; Password = "Manager123!"; Role = "Manager" },
    @{ Email = "employee@company.com"; Password = "Employee123!"; Role = "Employee" },
    @{ Email = "readonly@company.com"; Password = "ReadOnly123!"; Role = "ReadOnly" }
)

Write-Host "=== JWT Authentication Test ===" -ForegroundColor Green
Write-Host ""

# Function to test login and get token
function Test-Login {
    param($email, $password, $serviceName)
    
    $loginUrl = "$baseUrl/api/auth/login"
    $loginBody = @{
        email = $email
        password = $password
    } | ConvertTo-Json
    
    try {
        $response = Invoke-RestMethod -Uri $loginUrl -Method Post -Body $loginBody -ContentType "application/json" -SkipCertificateCheck
        return $response.token
    }
    catch {
        Write-Host "Login failed for $email in $serviceName`: $($_.Exception.Message)" -ForegroundColor Red
        return $null
    }
}

# Function to test protected endpoint
function Test-ProtectedEndpoint {
    param($url, $token, $method = "GET", $body = $null, $serviceName)
    
    $headers = @{
        "Authorization" = "Bearer $token"
        "Content-Type" = "application/json"
    }
    
    try {
        if ($method -eq "GET") {
            $response = Invoke-RestMethod -Uri $url -Method Get -Headers $headers -SkipCertificateCheck
        }
        else {
            $response = Invoke-RestMethod -Uri $url -Method $method -Headers $headers -Body $body -SkipCertificateCheck
        }
        Write-Host "✓ $serviceName $method request successful" -ForegroundColor Green
        return $true
    }
    catch {
        Write-Host "✗ $serviceName $method request failed: $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
}

# Test each user
foreach ($user in $testUsers) {
    Write-Host "Testing user: $($user.Email) ($($user.Role))" -ForegroundColor Yellow
    Write-Host "----------------------------------------"
    
    # Test login
    $token = Test-Login -email $user.Email -password $user.Password -serviceName "Employee Service"
    
    if ($token) {
        Write-Host "✓ Login successful, token obtained" -ForegroundColor Green
        
        # Test Employee Service endpoints
        Write-Host "Testing Employee Service endpoints..."
        Test-ProtectedEndpoint -url "$baseUrl/api/employee" -token $token -serviceName "Employee Service (GET All)"
        Test-ProtectedEndpoint -url "$baseUrl/api/employee/age-range?minAge=20&maxAge=50" -token $token -serviceName "Employee Service (Age Range)"
        
        # Test Department Service endpoints
        Write-Host "Testing Department Service endpoints..."
        Test-ProtectedEndpoint -url "$departmentUrl/api/department/name/IT" -token $token -serviceName "Department Service (GET by Name)"
        
        # Test Project Service endpoints
        Write-Host "Testing Project Service endpoints..."
        Test-ProtectedEndpoint -url "$projectUrl/api/project" -token $token -serviceName "Project Service (GET All)"
        
        # Test write operations based on role
        if ($user.Role -in @("Admin", "Manager", "Employee")) {
            Write-Host "Testing write operations..."
            $createEmployeeBody = @{
                firstName = "Test"
                lastName = "User"
                email = "test@example.com"
                age = 30
                departmentId = "1"
            } | ConvertTo-Json
            
            Test-ProtectedEndpoint -url "$baseUrl/api/employee" -token $token -method "POST" -body $createEmployeeBody -serviceName "Employee Service (Create)"
        }
        
        # Test admin-only operations
        if ($user.Role -eq "Admin") {
            Write-Host "Testing admin-only operations..."
            Test-ProtectedEndpoint -url "$baseUrl/api/employee/test-id" -token $token -method "DELETE" -serviceName "Employee Service (Delete)"
        }
    }
    
    Write-Host ""
}

# Test without token (should fail)
Write-Host "Testing without authentication token..." -ForegroundColor Yellow
Write-Host "----------------------------------------"

try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/employee" -Method Get -SkipCertificateCheck
    Write-Host "✗ Unauthenticated request should have failed!" -ForegroundColor Red
}
catch {
    Write-Host "✓ Unauthenticated request properly rejected" -ForegroundColor Green
}

Write-Host ""
Write-Host "=== Test Complete ===" -ForegroundColor Green
Write-Host ""
Write-Host "Test Users Available:" -ForegroundColor Cyan
Write-Host "- admin@company.com / Admin123! (Admin - full access)" -ForegroundColor White
Write-Host "- manager@company.com / Manager123! (Manager - can manage employees)" -ForegroundColor White
Write-Host "- employee@company.com / Employee123! (Employee - can create/edit)" -ForegroundColor White
Write-Host "- readonly@company.com / ReadOnly123! (ReadOnly - view only)" -ForegroundColor White
