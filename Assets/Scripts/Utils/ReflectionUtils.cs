using System;
using System.Collections.Generic;
using System.Reflection;

public static class ReflectionUtils
{
    /// <summary>
    /// This function is costly should only be done in editor and cooked into the data
    /// </summary>
    public static List<Type> GetAllChildrenOfType(Type type)
    {
        List<Type> children = new List<Type>();
        Type[] types = Assembly.GetExecutingAssembly().GetTypes();

        for (int i = 0; i < types.Length; ++i)
        {
            Type t = types[i];
            if (type.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            {
                children.Add(t);
            }
        }

        return children;
    }

    public static List<T> GetAllChildrenOfTypeAndCreateInstance<T>()
    {
        Type type = typeof(T);
        List<T> children = new List<T>();
        Type[] types = Assembly.GetCallingAssembly().GetTypes();

        for (int i = 0; i < types.Length; ++i)
        {
            Type t = types[i];
            if (type.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
            {
                T obj = (T) Activator.CreateInstance(t);
                children.Add(obj);
            }
        }

        return children;
    }
}