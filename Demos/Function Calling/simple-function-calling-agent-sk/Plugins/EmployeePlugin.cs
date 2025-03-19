using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace SimpleFunctionCallingAgentSK.Plugins;

public class EmployeePlugin
{
    [KernelFunction, Description("Get the employee id of a person")]
    public string GetEmployeeId(
        [Description("The full name of the person with first and last name")] string fullName
    )
    {
        Console.WriteLine($"GetEmployeeId function called with parameter: fullName = {fullName}");
        return "Employee Id: 42";
    }
}
