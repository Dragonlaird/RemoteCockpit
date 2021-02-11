using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteCockpitClasses.Animations.Items;
using RemoteCockpitClasses.Animations.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace RemoteCockpitClasses.Animations
{
    public class AnimationClassConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IAnimationItem));
        }

        /// <summary>
        /// Allow base class to generate JSON
        /// </summary>
        public override bool CanWrite
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(objectType.IsArray)
            {
                var result = new List<object>();
                var baseClass = objectType.GetElementType();
                foreach(JToken child in serializer.Deserialize<JToken>(reader).Children())
                {
                    var childObj = ReadJson(child.CreateReader(), baseClass, existingValue, serializer);
                    if (childObj != null)
                        result.Add(childObj);
                }
                return result.ToArray();
            }
            var obj = JObject.Load(reader);
            // Create target object based on JObject
            var target = Create(objectType, obj);
            // Populate the object properties
            serializer.Populate(obj.CreateReader(), target);
            return target;
            /*
            var assignableClasses = GetAssignableClasses(objectType);
            var obj = serializer.Deserialize<JToken>(reader);
            object result = objectType.IsArray ? new List<object>() : null;
            if (objectType.IsArray)
            {
                // Need to convert our response to an array of values
                foreach (var childValue in obj.Children())
                {
                    foreach (var assignableClass in assignableClasses)
                    {
                        if (IsMatch(assignableClass, childValue))
                        {
                            var childResult = serializer.Deserialize(childValue.CreateReader(), assignableClass);
                            if (childResult != null)
                                ((List<object>)result).Add(childResult);
                            break;
                        }
                    }
                }
            }
            else
            {
                var matchedClass = assignableClasses.FirstOrDefault(x => IsMatch(x, obj));
                if (matchedClass != null)
                    return serializer.Deserialize(reader, matchedClass); //serializer.Deserialize(reader, matchedClass);
            }
            return result;
            */
        }


        protected object Create(Type objectType, JObject jObject)
        {
            var assignableClasses = GetAssignableClasses(objectType);
            var assignableClass = assignableClasses.FirstOrDefault(x => IsMatch(x, jObject));
            if (assignableClass != null)
            {
                return Activator.CreateInstance(assignableClass);
            }

            throw new InvalidOperationException("No supported type");
        }

        private bool IsMatch(Type type, JObject obj)
        {
            bool result = true;
            foreach (JToken childProperty in obj.Children())
            {
                if (childProperty is JProperty)
                    if (type.GetProperty(((JProperty)childProperty).Name) == null)
                    {
                        result = false;
                        break;
                    }
                if(childProperty is JObject) { }
            }
            return result;
        }

        /// <summary>
        /// All classes that implement/inherit the supplied type
        /// </summary>
        /// <param name="checkType">Base type to check against all classes in this project</param>
        /// <returns>All classes that implement or inherit checkType</returns>
        private List<Type> GetAssignableClasses(Type checkType)
        {
            if (!checkType.FullName.StartsWith("RemoteCockpitClasses"))
                return new List<Type> { checkType };
            var allAssemblies = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(x => x.FullName.StartsWith("RemoteCockpitClasses"));
            List<Type> assignableClasses = new List<Type>();
            var baseType = checkType;
            if (checkType.IsArray)
                baseType = checkType.GetElementType();
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
                                if (!cls.IsAbstract && !cls.IsInterface && baseType.IsAssignableFrom(cls))
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

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
