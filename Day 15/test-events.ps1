# PowerShell script to test event-driven communication
# Make sure all services are running before executing this script

Write-Host "Testing Event-Driven Microservices Communication" -ForegroundColor Green
Write-Host "=============================================" -ForegroundColor Green

# Test 1: Create a Department
Write-Host "`n1. Creating Department..." -ForegroundColor Yellow
$departmentResponse = Invoke-RestMethod -Uri "https://localhost:5106/api/Department" -Method POST -Body '{"name": "Engineering"}' -ContentType "application/json" -SkipCertificateCheck
Write-Host "Department created: $($departmentResponse.name) with ID: $($departmentResponse.id)" -ForegroundColor Green

# Wait a moment for events to propagate
Start-Sleep -Seconds 2

# Test 2: Create an Employee
Write-Host "`n2. Creating Employee..." -ForegroundColor Yellow
$employeeResponse = Invoke-RestMethod -Uri "https://localhost:5043/api/Employee" -Method POST -Body '{"name": "John Doe", "age": 30, "salary": 75000, "departmentName": "Engineering", "isManager": "false"}' -ContentType "application/json" -SkipCertificateCheck
Write-Host "Employee created: $($employeeResponse.name) with ID: $($employeeResponse.id)" -ForegroundColor Green

# Wait a moment for events to propagate
Start-Sleep -Seconds 2

# Test 3: Create a Project
Write-Host "`n3. Creating Project..." -ForegroundColor Yellow
$projectResponse = Invoke-RestMethod -Uri "https://localhost:5043/api/Project" -Method POST -Body '{"name": "Microservices Migration"}' -ContentType "application/json" -SkipCertificateCheck
Write-Host "Project created: $($projectResponse.name) with ID: $($projectResponse.id)" -ForegroundColor Green

# Wait a moment for events to propagate
Start-Sleep -Seconds 2

# Test 4: Update Department
Write-Host "`n4. Updating Department..." -ForegroundColor Yellow
$updateResponse = Invoke-RestMethod -Uri "https://localhost:5106/api/Department/$($departmentResponse.id)" -Method PUT -Body '{"name": "Software Engineering"}' -ContentType "application/json" -SkipCertificateCheck
Write-Host "Department updated: $($updateResponse.name)" -ForegroundColor Green

# Wait a moment for events to propagate
Start-Sleep -Seconds 2

# Test 5: Update Employee
Write-Host "`n5. Updating Employee..." -ForegroundColor Yellow
$employeeUpdateResponse = Invoke-RestMethod -Uri "https://localhost:5043/api/Employee/$($employeeResponse.id)" -Method PUT -Body '{"name": "John Smith", "age": 31, "salary": 80000, "departmentName": "Software Engineering"}' -ContentType "application/json" -SkipCertificateCheck
Write-Host "Employee updated: $($employeeUpdateResponse.name)" -ForegroundColor Green

Write-Host "`nEvent-driven communication test completed!" -ForegroundColor Green
Write-Host "Check the service logs to see event consumption messages." -ForegroundColor Cyan
Write-Host "Also check RabbitMQ Management UI at http://localhost:15672" -ForegroundColor Cyan
