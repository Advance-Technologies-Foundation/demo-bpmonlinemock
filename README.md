
# BpmonlineMock
Пакет позволяет решать базовые задачи по написанию Unit тестов бизнес логики на платформе bpmonline

Задачи которые призван решить:
- создание и инициализацию UserConnection 
- создание простых mock и stub объектов для работы с менеджерами схем, менеджерами типов данных и т.п.
- инициализацией пользователя
- mock работы с базой данных
- работа с системными настройками

### Installation

В проекте с тестами установить пакет из nuget.org.
>последний релизный
```powershell
Install-Package BpmonlineMock
```
>или определенной версии...
```powershell
Install-Package BpmonlineMock -Version 7.12.3.2
```
### Development
Наследоваться в тестовом классе от класса **BaseConfigurationTestFixture**
```csharp
[TestFixture]
public class SomeTestCase : BaseConfigurationTestFixture
```
Указать с помощью атрибутов перечень необходимых моков
```csharp
[MockSettings(RequireMock.DBEngine)]
[TestFixture]
public class SomeTestCase : BaseConfigurationTestFixture
```
Пример проекта с использованием пакета BpmonlineMock [demo-bpmonlinemock](https://github.com/Advance-Technologies-Foundation/demo-bpmonlinemock)

License
----

MIT


**Free Software, Hell Yeah!**