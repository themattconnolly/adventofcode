// See https://aka.ms/new-console-ddtemplate for more information
// cycle through the args
using System.Reflection;

if (args.Length > 0)
{
    // call a class method dynamically based on the first argument
    if(args[0].StartsWith("day"))
    {
        Console.WriteLine(args[0]);
        // get the day number
        string dayNumber = args[0].Substring(3);
        // get the class name
        string className = "_2025.Day" + dayNumber;
        // get the type
        Type? type = Type.GetType(className);
        if (type == null)
        {
            Console.WriteLine($"Error: Could not find class '{className}'. Make sure the day class exists.");
            return;
        }

        if (args.Length > 1 && args[1].StartsWith("part"))
        {
            string partNumber = args[1].Substring(4);
            // get the method
            MethodInfo? method = type.GetMethod("RunPart" + partNumber);
            if (method == null)
            {
                Console.WriteLine($"Error: Could not find method 'RunPart{partNumber}' in class '{className}'.");
                return;
            }

            // create an instance of the class
            object? instance = Activator.CreateInstance(type);
            if (instance == null)
            {
                Console.WriteLine($"Error: Could not create an instance of class '{className}'.");
                return;
            }

            // invoke the method
            method.Invoke(instance, null);
        }
        else
        {
            Console.WriteLine("Error: Missing or invalid part argument. Expected format: 'dayX partY'");
        }
    }

} else
{
    Console.WriteLine("No args");
}

