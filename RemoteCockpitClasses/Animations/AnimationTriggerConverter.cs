using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RemoteCockpitClasses.Animations.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RemoteCockpitClasses.Animations
{
    public class AnimationTriggerConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IAnimationTrigger));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var assignableClasses = GetAssignableClasses(objectType);
            var obj = serializer.Deserialize<JToken>(reader);

            foreach (var assignableClass in assignableClasses)
            {
                if (IsMatch(assignableClass, obj))
                {
                    return serializer.Deserialize(reader, assignableClass);

                }
            }
            return null;
        }

        private bool IsMatch(Type type, JToken obj)
        {
            bool result = true;
            foreach (JProperty childProperty in obj.Children())
            {
                if (type.GetProperty(childProperty.Name) == null)
                {
                    result = false;
                    break;
                }
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
            // Left as an exercise to the reader :)
            throw new NotImplementedException();
        }
    }
}
