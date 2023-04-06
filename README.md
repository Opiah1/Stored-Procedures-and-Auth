# Stored-Procedures-and-Auth
Experimenting with passing input and output values to my stored procedure and implementing Authentication in .NET core Web API

This project is done in .NET 6, its a webAPI that allows creating a user, logging in and Balance Enquiry. Makes use of repository as well as dependency injection, making my controller
thin and keeping pplication logic separate from the controller. It can be used as the backbone of a banking application.

# Create
Sample Request
curl -X 'POST' \
  'https://localhost:7254/api/UserOperations/CreateAccount' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{
  "userName": "Opiah1",\
  "fullName": "Test1",\
  "email": "*********",\
  "balance": 500,\
  "password": "*********"\
}'\
Sample Response 
{
  "responseCode": "00",\
  "responseDescription": "success",\
  "accountNumber": "5555332634"\
}


# Login
Sample Request
curl -X 'POST' \
  'https://localhost:7254/api/UserOperations/Login' \
  -H 'accept: application/json' \
  -H 'Content-Type: application/json' \
  -d '{\
  "userName": "Opiah1",\
  "password": ""*********""\
}'\
Sample Response
{
  "token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiT3BpYWgxIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoiZGF2aWQub3BpYWhAaW5mb3NpZ2h0b25saW5lLmNvbSIsImV4cCI6MTY4MDc5MDM4MX0.udHFOg8vED4bgDuriAEm6c7O_oZN_eFa15kZ9dnviZWV6MGwpnDHEIys9bTJQUYq0_zZrmm2J2wgYw_lppKJzg",\
  "refreshToken": null
}

# Balance Enquiry
For this method you have to append the token generated via login within the header of your request \
Sample Request\
curl -X 'GET' \
  'https://localhost:7254/api/UserOperations/BalanceEnquiry?accountnumber=5555332634' \
  -H 'accept: application/json' \
  -H 'Authorization: bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiT3BpYWgxIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoiZGF2aWQub3BpYWhAaW5mb3NpZ2h0b25saW5lLmNvbSIsImV4cCI6MTY4MDc5MDM4MX0.udHFOg8vED4bgDuriAEm6c7O_oZN_eFa15kZ9dnviZWV6MGwpnDHEIys9bTJQUYq0_zZrmm2J2wgYw_lppKJzg'\
  Sample Response\
  {\
  "retval": "0",\
  "retmessage": "Account Balance Enquiry Successful...",\
  "enquiry": {\
    "fullName": "David Opiah",\
    "usableBalance": 500,\
    "accountNumber": "5555332634"\
  }\
}\
retval is an output parameter within the stored procedure, and is default if result is performed and 1 if otherwise e.g\
{\
  "retval": "1",\
  "retmessage": "Account Has No Valid Customer Record",\
  "enquiry": null\
}\
