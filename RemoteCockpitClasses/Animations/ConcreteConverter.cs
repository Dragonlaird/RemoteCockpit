using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteCockpitClasses.Animations
{
    public class ConcreteConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader,
         Type objectType, object existingValue, JsonSerializer serializer)
        {
            List<IAnimationItem> result = new List<IAnimationItem>();
            var obj = serializer.Deserialize(reader);
            foreach (var elem in (Newtonsoft.Json.Linq.JArray)obj)
            {
                foreach (JProperty prop in elem)
                {
                    if (prop.Name == "Type")
                    {
                        var value = prop.Value;
                        switch(value.ToString())
                        {
                            case "Drawing":
                                result.Add(prop.ToObject<AnimationDrawing>());
                                break;
                            case "Image":
                                result.Add(prop.ToObject<AnimationImage>());
                                break;
                        }
                    }
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
}
