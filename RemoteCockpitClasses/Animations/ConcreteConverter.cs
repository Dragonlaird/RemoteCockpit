using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace RemoteCockpitClasses.Animations
{
    public class ConcreteConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader,
         Type objectType, object existingValue, JsonSerializer serializer)
        {
            List<IAnimationItem> result = new List<IAnimationItem>();
            var obj = serializer.Deserialize<JArray>(reader);
            foreach (var elem in obj)
            {
                var child = ConvertTo(typeof(IAnimationItem), (JToken)elem);
                if (child != null)
                    result.Add((IAnimationItem)child);
            }
            return result?.ToArray();
        }

        private object ConvertTo(Type type, JToken obj)
        {
            Type checkType = Type.GetType(type.ToString());
            var isInterface = checkType.IsInterface;
            if (isInterface)
            {
                var allAssemblies = AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .Where(x => x.FullName.StartsWith("RemoteCockpitClasses"));
                    //.Select(x=> x.GetType("", false, false));
                    //.SelectMany(x => x.GetTypes().Where(y=> y.FullName.StartsWith("RemoteCockpitClasses.Animations")));
                List<Type> assignableClasses = new List<Type>();
                try
                {
                    foreach (var assem in allAssemblies)
                    {
                        try
                        {
                            foreach(var cls in assem.GetTypes())
                            {
                                try
                                {
                                    if (!cls.IsAbstract && !cls.IsInterface && checkType.IsAssignableFrom(cls))
                                        assignableClasses.Add(cls);
                                }
                                catch (Exception ex)
                                {
                                    // Here we can access .Types as a property
                                }
                            }

                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            // Here we can access .Types as a property
                            assignableClasses.AddRange(ex.Types.Where(x => checkType.IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface));
                        }
                    }
                }
                catch { }
                foreach (var assignableType in assignableClasses)
                {
                    // Populate the properties of the object, unless it is an Interface
                    var resultClass = Activator.CreateInstance(assignableType);
                    var resultProperties = resultClass.GetType().GetProperties().Select(x => x.Name);
                    // Iterate through all jsonProperties
                    // If we have a jsonProperty that isn't in the resultClass, this is not the resultClass we want
                    bool classValid = true;
                    foreach(var jsonProperty in obj.Children())
                    {
                        var propertyName = jsonProperty.Path.Substring(jsonProperty.Path.LastIndexOf(".") + 1);
                        if(!resultProperties.Any(x=> x == propertyName))
                        {
                            classValid = false;
                            break;
                        }
                        else
                        {
                            var prop = resultClass.GetType().GetProperty(propertyName);
                            object actualValue = null;

                            // This should just convert
                            if (prop.PropertyType.IsEnum)
                            {
                                actualValue = Enum.Parse(prop.PropertyType, string.Format("{0}", jsonProperty.First()));
                            }
                            else
                            {
                                if (prop.PropertyType.IsArray || typeof(IEnumerable<object>).IsAssignableFrom(prop.PropertyType))
                                {
                                    if (prop.PropertyType.GetElementType().IsInterface)
                                    {
                                        // Call this method again to convert Interface to correct class
                                        actualValue = new List<object>();
                                        //Type t = typeof(List<>).MakeGenericType(prop.PropertyType.GetElementType());
                                        //var list = Activator.CreateInstance(t);

                                        foreach (var child in jsonProperty.Children().First().Children())
                                        {
                                            ((List<object>)actualValue).Add(ConvertTo(prop.PropertyType.GetElementType(), child));
                                        }
                                        MethodInfo method = this.GetType().GetMethod("CloneListAs");
                                        MethodInfo genericMethod = method.MakeGenericMethod(prop.PropertyType.GetElementType());
                                        actualValue = genericMethod.Invoke(null, new[] { actualValue });
                                    }
                                    else
                                    {
                                        actualValue = jsonProperty.First().ToObject(prop.PropertyType);
                                    }
                                }
                                else
                                {
                                    actualValue = jsonProperty.First().ToObject(prop.PropertyType);
                                }
                            }
                            if (resultClass.GetType().GetProperty(propertyName).CanWrite)
                                resultClass.GetType().GetProperty(propertyName).SetValue(resultClass, actualValue);
                        }
                    }
                    if (classValid)
                    {
                        return resultClass;
                    }
                }
            }
            
            return null;
        }

        public static t[] CloneListAs<t>(IList<object> source)
        {
            // Here we can do anything we want with T
            // T == source[0].GetType()
            return source.Cast<t>().ToList().ToArray();
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
