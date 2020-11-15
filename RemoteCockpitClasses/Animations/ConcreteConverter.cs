using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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
            return result;
        }

        private ST ConvertTo<ST>(ST type, JToken obj) where ST: class
        {
            ST result = null;
            Type checkType = Type.GetType(type.ToString());
            var isInterface = checkType.IsInterface;
            if (isInterface)
            {
                var allAssemblies = AppDomain
                    .CurrentDomain
                    .GetAssemblies();

                List<Type> allClasses = new List<Type>();
                foreach(var classCollection in allAssemblies)
                {
                    try
                    {
                        var classes = classCollection.GetTypes();
                        try
                        {
                            foreach (var cls in classes)
                            {
                                if (!cls.IsAbstract && !cls.IsInterface && checkType.IsAssignableFrom(cls))
                                    allClasses.Add(cls);
                            }
                        }
                        catch { }
                    }
                    catch { }
                }



                foreach (var cls in allClasses)
                {
                    try
                    {
                        var possibleClass = Activator.CreateInstance(cls);
                        var possibleResult = ConvertTo(possibleClass.GetType(), obj);
                        //return (ST)possibleResult;
                    }
                    catch { }
                }

            }
            return result;
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

    public static class GenericType
    {
        public static Type GetEnumeratedType<T>(this T _)
        {
            return typeof(T);
        }

    }
}
