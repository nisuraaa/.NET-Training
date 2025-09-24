#!/bin/bash

# Bash script to test event-driven communication
# Make sure all services are running before executing this script

echo "Testing Event-Driven Microservices Communication"
echo "============================================="

# Test 1: Create a Department
echo ""
echo "1. Creating Department..."
DEPARTMENT_RESPONSE=$(curl -s -k -X POST "https://localhost:5106/api/Department" \
  -H "Content-Type: application/json" \
  -d '{"name": "Engineering"}')

DEPARTMENT_ID=$(echo $DEPARTMENT_RESPONSE | jq -r '.id')
DEPARTMENT_NAME=$(echo $DEPARTMENT_RESPONSE | jq -r '.name')
echo "Department created: $DEPARTMENT_NAME with ID: $DEPARTMENT_ID"

# Wait a moment for events to propagate
sleep 2

# Test 2: Create an Employee
echo ""
echo "2. Creating Employee..."
EMPLOYEE_RESPONSE=$(curl -s -k -X POST "https://localhost:5043/api/Employee" \
  -H "Content-Type: application/json" \
  -d '{"name": "John Doe", "age": 30, "salary": 75000, "departmentName": "Engineering", "isManager": "false"}')

EMPLOYEE_ID=$(echo $EMPLOYEE_RESPONSE | jq -r '.id')
EMPLOYEE_NAME=$(echo $EMPLOYEE_RESPONSE | jq -r '.name')
echo "Employee created: $EMPLOYEE_NAME with ID: $EMPLOYEE_ID"

# Wait a moment for events to propagate
sleep 2

# Test 3: Create a Project
echo ""
echo "3. Creating Project..."
PROJECT_RESPONSE=$(curl -s -k -X POST "https://localhost:5043/api/Project" \
  -H "Content-Type: application/json" \
  -d '{"name": "Microservices Migration"}')

PROJECT_ID=$(echo $PROJECT_RESPONSE | jq -r '.id')
PROJECT_NAME=$(echo $PROJECT_RESPONSE | jq -r '.name')
echo "Project created: $PROJECT_NAME with ID: $PROJECT_ID"

# Wait a moment for events to propagate
sleep 2

# Test 4: Update Department
echo ""
echo "4. Updating Department..."
UPDATE_RESPONSE=$(curl -s -k -X PUT "https://localhost:5106/api/Department/$DEPARTMENT_ID" \
  -H "Content-Type: application/json" \
  -d '{"name": "Software Engineering"}')

UPDATED_NAME=$(echo $UPDATE_RESPONSE | jq -r '.name')
echo "Department updated: $UPDATED_NAME"

# Wait a moment for events to propagate
sleep 2

# Test 5: Update Employee
echo ""
echo "5. Updating Employee..."
EMPLOYEE_UPDATE_RESPONSE=$(curl -s -k -X PUT "https://localhost:5043/api/Employee/$EMPLOYEE_ID" \
  -H "Content-Type: application/json" \
  -d '{"name": "John Smith", "age": 31, "salary": 80000, "departmentName": "Software Engineering"}')

UPDATED_EMPLOYEE_NAME=$(echo $EMPLOYEE_UPDATE_RESPONSE | jq -r '.name')
echo "Employee updated: $UPDATED_EMPLOYEE_NAME"

echo ""
echo "Event-driven communication test completed!"
echo "Check the service logs to see event consumption messages."
echo "Also check RabbitMQ Management UI at http://localhost:15672"
