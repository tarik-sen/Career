using System;
using System.Reflection;
namespace Career.DebugHelper;
public static class PropertyStateHelper
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        {
            Console.WriteLine("{0}: {1}", property, property.GetValue(obj));
        }