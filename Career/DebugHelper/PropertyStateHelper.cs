using System;
using System.Reflection;
namespace Career.DebugHelper;
public static class PropertyStateHelper{    public static void LogPropertyState(object obj)    {        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);        foreach (PropertyInfo property in properties)
        {
            Console.WriteLine("{0}: {1}", property, property.GetValue(obj));
        }    }}