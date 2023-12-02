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
        string className = "_2023.Day" + dayNumber;
        // get the type
        Type type = Type.GetType(className);
        // get the method
        MethodInfo method = type.GetMethod("Run");
        // create an instance of the class
        object instance = Activator.CreateInstance(type);
        // invoke the method
        method.Invoke(instance, null);
    }

} else
{
    Console.WriteLine("No args");
}