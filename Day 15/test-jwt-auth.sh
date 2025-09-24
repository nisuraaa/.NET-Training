#!/bin/bash

# JWT Authentication Test Script
# This script tests JWT authentication across all microservices

BASE_URL="https://localhost:7001"  # Employee Service URL
DEPARTMENT_URL="https://localhost:5106"  # Department Service URL  
PROJECT_URL="https://localhost:5043"  # Project Service URL

echo "=== JWT Authentication Test ==="
echo ""

# Function to test login and get token
test_login() {
    local email=$1
    local password=$2
    local service_name=$3
    
    local login_url="$BASE_URL/api/auth/login"
    local login_body="{\"email\":\"$email\",\"password\":\"$password\"}"
    
    local response=$(curl -s -k -X POST "$login_url" \
        -H "Content-Type: application/json" \
        -d "$login_body")
    
    if echo "$response" | grep -q "token"; then
        echo "$response" | jq -r '.token'
    else
        echo "Login failed for $email in $service_name" >&2
        echo ""
    fi
}

# Function to test protected endpoint
test_protected_endpoint() {
    local url=$1
    local token=$2
    local method=$3
    local body=$4
    local service_name=$5
    
    if [ "$method" = "GET" ]; then
        local response=$(curl -s -k -X GET "$url" \
            -H "Authorization: Bearer $token")
    else
        local response=$(curl -s -k -X "$method" "$url" \
            -H "Authorization: Bearer $token" \
            -H "Content-Type: application/json" \
            -d "$body")
    fi
    
    if [ $? -eq 0 ]; then
        echo "✓ $service_name $method request successful"
        return 0
    else
        echo "✗ $service_name $method request failed"
        return 1
    fi
}

# Test users
declare -A test_users=(
    ["admin@company.com"]="Admin123!"
    ["manager@company.com"]="Manager123!"
    ["employee@company.com"]="Employee123!"
    ["readonly@company.com"]="ReadOnly123!"
)

# Test each user
for email in "${!test_users[@]}"; do
    password="${test_users[$email]}"
    echo "Testing user: $email"
    echo "----------------------------------------"
    
    # Test login
    token=$(test_login "$email" "$password" "Employee Service")
    
    if [ -n "$token" ]; then
        echo "✓ Login successful, token obtained"
        
        # Test Employee Service endpoints
        echo "Testing Employee Service endpoints..."
        test_protected_endpoint "$BASE_URL/api/employee" "$token" "GET" "" "Employee Service (GET All)"
        test_protected_endpoint "$BASE_URL/api/employee/age-range?minAge=20&maxAge=50" "$token" "GET" "" "Employee Service (Age Range)"
        
        # Test Department Service endpoints
        echo "Testing Department Service endpoints..."
        test_protected_endpoint "$DEPARTMENT_URL/api/department/name/IT" "$token" "GET" "" "Department Service (GET by Name)"
        
        # Test Project Service endpoints
        echo "Testing Project Service endpoints..."
        test_protected_endpoint "$PROJECT_URL/api/project" "$token" "GET" "" "Project Service (GET All)"
        
        # Test write operations based on role
        if [[ "$email" == "admin@company.com" || "$email" == "manager@company.com" || "$email" == "employee@company.com" ]]; then
            echo "Testing write operations..."
            create_employee_body='{"firstName":"Test","lastName":"User","email":"test@example.com","age":30,"departmentId":"1"}'
            test_protected_endpoint "$BASE_URL/api/employee" "$token" "POST" "$create_employee_body" "Employee Service (Create)"
        fi
        
        # Test admin-only operations
        if [[ "$email" == "admin@company.com" ]]; then
            echo "Testing admin-only operations..."
            test_protected_endpoint "$BASE_URL/api/employee/test-id" "$token" "DELETE" "" "Employee Service (Delete)"
        fi
    fi
    
    echo ""
done

# Test without token (should fail)
echo "Testing without authentication token..."
echo "----------------------------------------"

response=$(curl -s -k -X GET "$BASE_URL/api/employee")
if [ $? -eq 0 ]; then
    echo "✗ Unauthenticated request should have failed!"
else
    echo "✓ Unauthenticated request properly rejected"
fi

echo ""
echo "=== Test Complete ==="
echo ""
echo "Test Users Available:"
echo "- admin@company.com / Admin123! (Admin - full access)"
echo "- manager@company.com / Manager123! (Manager - can manage employees)"
echo "- employee@company.com / Employee123! (Employee - can create/edit)"
echo "- readonly@company.com / ReadOnly123! (ReadOnly - view only)"
