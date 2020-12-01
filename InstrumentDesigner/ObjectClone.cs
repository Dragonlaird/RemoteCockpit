using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft;
using Newtonsoft.Json;

/// <summary>
/// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
/// Provides a method for performing a deep copy of an object.
/// Binary Serialization is used to perform the copy.
/// </summary>
public static class ObjectClone
{
    /// <summary>
    /// Perform a deep Copy of the object.
    /// </summary>
    /// <typeparam name="T">The type of object being copied.</typeparam>
    /// <param name="source">The object instance to copy.</param>
    /// <returns>The copied object.</returns>
    public static T Clone<T>(T source)
    {
        //if (!typeof(T).IsSerializable)
        //{
        //    throw new ArgumentException("The type must be serializable.", nameof(source));
        //}

        // Don't serialize a null object, simply return the default for that object
        if (Object.ReferenceEquals(source, null))
        {
            return default(T);
        }
        var sourceType = source.GetType();
        if(sourceType.IsInterface || sourceType.IsAbstract)
        {
            return default(T); // Cannot instantiate an Interface or Abstract class
        }
        var result = Activator.CreateInstance(sourceType);
        foreach(var prop in sourceType.GetProperties())
        {
            if (prop.CanWrite)
            {
                prop.SetValue(result, prop.GetValue(source));
            }
        }
        return (T)result;
    }
}
