
# How it's work
Service start every 10 minutes
  
1) Set up TestXml.Api as a starter project and run it via IIS Express
2) Right click on UserManagerService and select Debug => Start new instans
A method will be launched that will pull the action with a call GetUsers and write data in cache every 10 minutes.
Additional information about the execution is presented in the logger in the console
3) It is possible to launch the API through Kestrel where the start page for launch is implemented through Swagger

The default database will be populated with default values. Service errors are handled by a custom exception handler

## Solution structure
* Common:
  * TestXml.Abstract Library with abstractions / implementors
  * TestXml.Business Library with business logic
  * TestXml.Data Database interaction library
* TestXml.Api: interaction service with Web
* UserManagerService:  simple console application that installed as a service using Topshelf.
* UserManagerService.Integration: business logic calling by timer (every 10 min) calling actiona from Api

