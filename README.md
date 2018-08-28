
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

# Features!
For convenience of creation of mock objects was added **MockSettingsAttribute.**
If you specify the appropriate flag, the mock objects will be created and configured. 

>For example, if you specify the *DBEngine* flag, the *DBEngine*, *DBExecutor*, *DBTypeConverter*, *DBSecurityEngine*, and a number of other objects will be created, which will intercept requests at the DBEngine level.

List of supported moks:
```csharp
None = 0,
DBEngine = 1,
HTTPContext = 2,
DCM = 4,
Features = 8,
ResourceStorage = 16,
DBSecurityEngine = 32,
All = DBEngine | HTTPContext | DCM | Features | ResourceStorage | DBSecurityEngine
```

# Examples
### Set system settings
Set system setting in test method
```csharp
UserConnection.SettingsValues.Add("SupportServiceEmail", "test@gmail.com"); //SysSetting code: "SupportServiceEmail", value: "test@gmail.com"
```

Set system setting on test start
```csharp
protected override void SetupSysSettings() {
    base.SetupSysSettings();
    UserConnection.SettingsValues.Add("SiteUrl", "https:/test");
}
```
### Setup database query mock
Set structure and data in test method.
```csharp
[TestFixture]
[MockSettings(RequireMock.DBEngine)]
public class MyTestCase : BaseConfigurationTestFixture
{
    [Test, Category("PreCommit")]
    public void CheckSelect() {
        new SelectData(UserConnection, "SysAdminOperation")
            .AddColumn("Code", typeof(string))
            .AddColumn("CanExecute", typeof(bool))
        .AddRow(new Dictionary<string, object> {
              { "Code", "CanManageAdministration" },
              { "CanExecute", false }
        })
        .AddRow(new Dictionary<string, object> {
              { "Code", "CanManageData" },
              { "CanExecute", true }
        }).MockUp();
    }
}
```

Set structure on test sturt and data in test method
```csharp
[TestFixture]
[MockSettings(RequireMock.DBEngine)]
public class MyTestCase : BaseConfigurationTestFixture
{
    
    protected override IEnumerable<Type> GetRequiringInitializationSchemas() {
            EntitySchemaManager.AddCustomizedEntitySchema("SysAdminOperation", new Dictionary<string, string> {
                {"Code", "MediumText"},
                {"CanExecute", "Boolean"}
            });
            return new List<Type>();
    }
    
    [Test, Category("PreCommit")]
    public void CheckSelect() {
        new SelectData(UserConnection, "SysAdminOperation")
        .AddRow(new Dictionary<string, object> {
              { "Code", "CanManageAdministration" },
              { "CanExecute", false }
        })
        .AddRow(new Dictionary<string, object> {
              { "Code", "CanManageData" },
              { "CanExecute", true }
        }).MockUp();
    }
}
```

### Create Entity instance
Create entity instance with custom structure
```csharp
[TestFixture]
[MockSettings(RequireMock.DBEngine | RequireMock.ResourceStorage)]
public class MyNewTestCase : BaseConfigurationTestFixture
{
    protected override IEnumerable<Type> GetRequiringInitializationSchemas() {
            EntitySchemaManager.AddCustomizedEntitySchema("Activity", new Dictionary<string, string> {
                {"Sender", "LocalizableStringDataValueType"},
                {"AccountId", "Guid"},
                {"AuthorId", "Guid"},
                {"IsNeedProcess", "Boolean"}
            });
            return new List<Type>();
    }
    
    [Test, Category("PreCommit")]
    public void CheckCreate() {
        var activity = EntitySchemaManager.GetInstanceByName("Activity").CreateEntity(UserConnection);
    }
}
```

### Filling EntitySchemaManager
Add to schema manager Activity schema with custom structure, DimensionSchema and ContactSchema from Configyuration assembly.
```csharp
[TestFixture]
[MockSettings(RequireMock.DBEngine | RequireMock.ResourceStorage)]
public class MyNewTestCase : BaseConfigurationTestFixture
{
    protected override IEnumerable<Type> GetRequiringInitializationSchemas() {
        EntitySchemaManager.AddCustomizedEntitySchema("Activity", new Dictionary<string, string> {
            {"Sender", "LocalizableStringDataValueType"},
            {"AccountId", "Guid"},
            {"AuthorId", "Guid"},
            {"IsNeedProcess", "Boolean"}
        });
        return new[] {
            typeof(DimensionSchema),
            typeof(Terrasoft.Core.Configuration.ContactSchema),
        };
    }
    [Test, Category("PreCommit")]
    public void CheckCreate() {
        //some test code
    }
}
```