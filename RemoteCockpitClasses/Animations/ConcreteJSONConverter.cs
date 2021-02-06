using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using RemoteCockpitClasses.Animations.Items;
using RemoteCockpitClasses.Animations.Actions;

namespace RemoteCockpitClasses.Animations
{
    public class ConcreteJSONConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            List<IAnimationItem> result = new List<IAnimationItem>();
            try
            {
                var obj = serializer.Deserialize<JArray>(reader);
                foreach (var elem in obj)
                {
                    var child = ConvertTo(objectType, (JToken)elem);
                    if (child != null)
                        result.Add((IAnimationItem)child);
                }
            }
            catch (Exception ex)//(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("Unable to read Configuration: Could not parse {0}", reader.Path), "Config Parse Failed", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                // Failed to deserialize
            }
            //if (objectType == typeof(AnimationXMLConverter))
            //    return new AnimationXMLConverter(result);
            return result;
        }

        private object ConvertTo(Type type, JToken obj)
        {
            //Type checkType = Type.GetType(type.ToString());
            var isLocalClass = type.Namespace.StartsWith("RemoteCockpitClasses.Animations") || type.IsInterface;
            if (isLocalClass)
            {
                var assignableClasses = GetAssignableClasses(type);
                foreach (var assignableType in assignableClasses)
                {
                    // Populate the properties of the object, unless it is an Interface
                    var resultClass = Activator.CreateInstance(assignableType);
                    var resultProperties = resultClass.GetType().GetProperties().Select(x => x.Name);
                    var resultAttributes = resultClass.GetType().Attributes;
                    // Iterate through all jsonProperties
                    // If we have a jsonProperty that isn't in the resultClass, this is not the resultClass we want
                    bool classValid = true;
                    foreach (var jsonProperty in obj.Children())
                    {
                        var propertyName = jsonProperty.Path.Substring(jsonProperty.Path.LastIndexOf(".") + 1);
                        if (!resultProperties.Any(x => x == propertyName))
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
                                    if (prop.PropertyType.HasElementType && prop.PropertyType.GetElementType().IsInterface)
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
                                        actualValue = ConvertTo(prop.PropertyType, jsonProperty.First());
                                        if (actualValue == null)
                                            actualValue = jsonProperty.First().ToObject(prop.PropertyType);
                                    }
                                }
                                else
                                {
                                    if (prop.PropertyType.Namespace == "RemoteCockpitClasses.Animations")
                                    {
                                        actualValue = ConvertTo(prop.PropertyType, jsonProperty.First());

                                    }
                                    else
                                    {
                                        try
                                        {
                                            actualValue = jsonProperty.First().ToObject(prop.PropertyType);
                                        }
                                        catch
                                        {
                                            actualValue = Activator.CreateInstance(prop.PropertyType, new object[] { jsonProperty.First().ToString() });
                                        }
                                    }
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

        private object ConvertToXmlObject(Type type, JToken obj)
        {
            // Type is AnimationXMLConverter - determine if obj is a single instance or an array
            object result = null;
            var resultIsArray = obj.ToString().StartsWith("[");
            if (resultIsArray)
            {
                // JSON is an array of items, iterate through each
                result = new List<object>();
                foreach(var childItem in obj.Children())
                {
                    var instanceResult = ConvertToXmlObject(type, childItem);
                    if(instanceResult !=null)
                        ((List<object>)result).Add(instanceResult);
                }
                return ((List<object>)result).ToArray();
            }

            // obj is a single instance - convert it to a matching class
            List<Type> matchingClasses = GetMatchingClasses(obj);
            foreach(var possibleType in matchingClasses)
            {
                result = Activator.CreateInstance(possibleType);
                foreach(var jsonProperty in obj.Children())
                {
                    var propertyName = jsonProperty.Path.Substring(jsonProperty.Path.LastIndexOf(".") + 1);

                    var prop = possibleType.GetProperty(propertyName);
                    if (prop == null)
                    {
                        // Property doesn't exist in this class - try any remaining classes
                        result = null;
                        break;
                    }
                    if (result.GetType().GetProperty(propertyName).CanWrite)
                    {

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
                                if (prop.PropertyType.HasElementType && prop.PropertyType.GetElementType().IsInterface)
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
                                    actualValue = ConvertTo(prop.PropertyType, jsonProperty.First()); // Should only be a single instance of base class, it is enumerable so children will map to an instance of an inheritable class
                                    if (actualValue == null)
                                        actualValue = jsonProperty.First().ToObject(prop.PropertyType);
                                }
                            }
                            else
                            {
                                if (prop.PropertyType.Namespace == "RemoteCockpitClasses.Animations")
                                {
                                    actualValue = ConvertTo(prop.PropertyType, jsonProperty.First());

                                }
                                else
                                {
                                    try
                                    {
                                        actualValue = jsonProperty.First().ToObject(prop.PropertyType);
                                    }
                                    catch
                                    {
                                        actualValue = Activator.CreateInstance(prop.PropertyType, new object[] { jsonProperty.First().ToString() });
                                    }
                                }
                            }
                        }
                        // If property type = AnimationXMLConverter - may need to convert the actualValue from an array
                        if (result.GetType().GetProperty(propertyName).PropertyType.Namespace.StartsWith("RemoteCockpitClasses") && actualValue is Array)
                        {
                            // Determine which underlying type is enumerated
                            if (((object[])actualValue).Length > 0)
                            {
                                var firstObject = ((object[])actualValue).FirstOrDefault();
                                if (firstObject != null)
                                {
                                    var elementType = firstObject.GetType();
                                    var elementInterfaces = elementType.GetInterfaces();
                                    if (elementInterfaces.Any(x => x == typeof(Actions.IAnimationAction)))
                                        actualValue = ((Array)actualValue).Cast<Actions.IAnimationAction>();
                                    if (elementInterfaces.Any(x => x == typeof(Triggers.IAnimationTrigger)))
                                        actualValue = ((Array)actualValue).Cast<Triggers.IAnimationTrigger>();
                                    if (elementInterfaces.Any(x => x == typeof(Items.IAnimationItem)))
                                        actualValue = ((Array)actualValue).Cast<Items.IAnimationItem>();
                                    else
                                        actualValue = Convert.ChangeType(actualValue, result.GetType().GetProperty(propertyName).PropertyType);
                                }
                            }
                        }
                        try
                        {
                            if (actualValue is Array && ((object[])actualValue).Length == 0)
                                actualValue = Activator.CreateInstance(result.GetType().GetProperty(propertyName).PropertyType);
                            result.GetType().GetProperty(propertyName).SetValue(result, actualValue);
                        }
                        catch(Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(string.Format("Failed to convert: {0}", propertyName), "Config Malformed", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            //var implementsIEnumerable = ((System.Reflection.TypeInfo)type.PropertyType).ImplementedInterfaces.Any(x => x.Name.StartsWith("IEnumerable"));
            return result;
        }

        /// <summary>
        /// All classes that implement/inherit the supplied type
        /// </summary>
        /// <param name="checkType">Base type to check against all classes in this project</param>
        /// <returns>All classes that implement or inherit checkType</returns>
        private List<Type> GetAssignableClasses(Type checkType)
        {
            var allAssemblies = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(x => x.FullName.StartsWith("RemoteCockpitClasses"));
            List<Type> assignableClasses = new List<Type>();
            try
            {
                foreach (var assem in allAssemblies)
                {
                    try
                    {
                        foreach (var cls in assem.GetTypes())
                        {
                            try
                            {
                                if (!cls.IsAbstract && !cls.IsInterface && checkType.IsAssignableFrom(cls))
                                    assignableClasses.Add(cls);
                            }
                            catch// (Exception ex)
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
            return assignableClasses;
        }

        /// <summary>
        /// Get all possible classes the JSON snippet can morph into
        /// </summary>
        /// <param name="obj">JSON snippet to find matching classes for</param>
        /// <returns>List of suitable classes the JSON snippet can populate</returns>
        public List<Type> GetMatchingClasses(JToken obj)
        {
            List<Type> availablClasses = new List<Type>();
            foreach (var baseClass in GetAssignableClasses(typeof(object)))
            {
                // Try to convert obj to each class, until we find one it fits
                try
                {
                    var targetClass = Activator.CreateInstance(baseClass);
                    var resultProperties = targetClass.GetType().GetProperties().Select(x => x.Name);
                    bool classValid = true;
                    foreach (var jsonProperty in obj.Children())
                    {
                        var propertyName = jsonProperty.Path.Substring(jsonProperty.Path.LastIndexOf(".") + 1);
                        if (!resultProperties.Any(x => x == propertyName))
                        {
                            classValid = false;
                            break;
                        }
                    }
                    if (classValid)
                    {
                        // May be able to convert obj to this class - try it
                        availablClasses.Add(baseClass);
                    }
                }
                catch (Exception ex)
                {
                    // Can't assign to this base class, likely an interface, enum or some other non-instanciatable class
                }
            }

            return availablClasses;
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
