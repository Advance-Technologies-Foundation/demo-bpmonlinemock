
# BpmonlineMock
The package allows you to solve the basic tasks of writing unit tests of business logic on the bpmonline platform.

The tasks that are designed to solve:
- create and initialize UserConnection.
- creating simple mock and stub objects for working with schema managers, data type managers, and so on.
- initialization of the user.
- mock database.
- mock system settings.

### Installation

In the project with tests, install the package from nuget.org.
>latest release
```powershell
Install-Package BpmonlineMock
```
>or a specific version...
```powershell
Install-Package BpmonlineMock -Version 7.12.3.2
```
### Development
Inherit a test class from a class **BaseConfigurationTestFixture**
```csharp
[TestFixture]
public class SomeTestCase : BaseConfigurationTestFixture
```
Specify, using attributes, a list of required moks
```csharp
[MockSettings(RequireMock.DBEngine)]
[TestFixture]
public class SomeTestCase : BaseConfigurationTestFixture
```
Example project using the package BpmonlineMock [demo-bpmonlinemock](https://github.com/Advance-Technologies-Foundation/demo-bpmonlinemock)