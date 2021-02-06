using RemoteCockpitClasses.Animations.Actions;
using RemoteCockpitClasses.Animations.Items;
using RemoteCockpitClasses.Animations.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace RemoteCockpitClasses.Animations
{
    public class AnimationXMLConverter : IEnumerable<object>, IXmlSerializable
    {
        private List<object> data;
        public AnimationXMLConverter() : base()
        {
            data = new List<object>();
            foreach (var item in this)
            {
                data.Add(item);
            }
        }

        public AnimationXMLConverter(IEnumerable<object> arr)
        {
            data = arr.ToList();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (object val in data)
            {
                yield return val;
            }
        }

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            return (IEnumerator<object>)this.GetEnumerator();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            // Find which element this is, iterate through each class associated with the assciated base interface
            switch (reader.Name)
            {
                case "Actions":
                    reader.ReadStartElement("Actions");
                    while (reader.IsStartElement("Action") && reader.NodeType != XmlNodeType.None)
                    {
                        XmlSerializer serial = null;
                        int typeID = 0;
                        if (reader.AttributeCount == 1)
                        {
                            var attributeVal = XMLHelpers.GetAttributeByName(reader, "Type");
                            int.TryParse(attributeVal, out typeID);
                        }
                        switch (typeID)
                        {
                            case (int)AnimationActionTypeEnum.Clip:
                                serial = new XmlSerializer(typeof(AnimationActionClip));
                                break;
                            case (int)AnimationActionTypeEnum.Rotate:
                                serial = new XmlSerializer(typeof(AnimationActionRotate));
                                break;
                            case (int)AnimationActionTypeEnum.MoveX:
                            case (int)AnimationActionTypeEnum.MoveY:
                                serial = new XmlSerializer(typeof(AnimationActionMove));
                                break;
                            default:
                                break;
                        }
                        if (serial != null)
                        {
                            this.ToList().Add((object)serial.Deserialize(reader));
                        }
                        if (reader.NodeType != XmlNodeType.None)
                            reader.ReadEndElement();
                        break;
                    }
                    break;
                case "Triggers":
                    reader.ReadStartElement("Triggers");
                    while (reader.IsStartElement("Trigger") && reader.NodeType != XmlNodeType.None)
                    {
                        XmlSerializer serial = null;
                        int typeID = 0;
                        if (reader.AttributeCount == 1)
                        {
                            var attributeVal = XMLHelpers.GetAttributeByName(reader, "Type");
                            int.TryParse(attributeVal, out typeID);
                        }
                        switch (typeID)
                        {
                            case (int)AnimationTriggerTypeEnum.ClientRequest:
                                serial = new XmlSerializer(typeof(AnimationTriggerClientRequest));
                                break;
                            case (int)AnimationTriggerTypeEnum.MouseClick:
                                serial = new XmlSerializer(typeof(AnimationTriggerClientMouseClick));
                                break;
                            case (int)AnimationTriggerTypeEnum.Timer:
                                throw new NotImplementedException("AnimationTriggerTimer not defined");
                            default:
                                break;
                        }
                        if (serial != null)
                        {
                            this.ToList().Add((object)serial.Deserialize(reader));
                        }
                        if (reader.NodeType != XmlNodeType.None)
                            reader.ReadEndElement();
                        break;
                    }
                    break;
                case "Animations":
                    reader.ReadStartElement("Animations");
                    while (reader.IsStartElement("Animation") && reader.NodeType != XmlNodeType.None)
                    {
                        XmlSerializer serial = null;
                        int typeID = 0;
                        if (reader.AttributeCount == 1)
                        {
                            var attributeVal = XMLHelpers.GetAttributeByName(reader, "Type");
                            int.TryParse(attributeVal, out typeID);
                        }
                        switch (typeID)
                        {
                            case (int)AnimationItemTypeEnum.Image:
                                serial = new XmlSerializer(typeof(AnimationImage));
                                break;
                            case (int)AnimationItemTypeEnum.Drawing:
                                serial = new XmlSerializer(typeof(AnimationDrawing));
                                break;
                            case (int)AnimationItemTypeEnum.External:
                                serial = new XmlSerializer(typeof(AnimationExternal));
                                break;
                            default:
                                break;
                        }
                        if (serial != null)
                        {
                            this.ToList().Add((object)serial.Deserialize(reader));
                        }
                        if (reader.NodeType != XmlNodeType.None)
                            reader.ReadEndElement();
                        break;
                    }
                    break;
            }
            if (reader.NodeType != XmlNodeType.None)
                reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    internal static class XMLHelpers
    {
        internal static string GetAttributeByName(XmlReader reader, string name)
        {
            string result = null;
            if (!string.IsNullOrWhiteSpace(name) && reader.HasAttributes)
            {
                name = name.ToLower().Trim();
                Console.WriteLine("Attributes of <" + reader.Name + ">");
                while (reader.MoveToNextAttribute())
                {
                    if (reader.Name.ToLower() == name)
                    {
                        result = reader.Value;
                        break;
                    }
                }
                // Move the reader back to the element node.
                reader.MoveToElement();
            }
            return result;
        }
    }
}