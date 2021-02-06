using Newtonsoft.Json;
using RemoteCockpitClasses.Animations.Items;
using RemoteCockpitClasses.Animations.Triggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace RemoteCockpitClasses.Animations
{
    [DebuggerDisplay("\\{Configuration\\} {Name}")]
    [XmlRoot("InstrumentDefinition")]
    public class Configuration
    {
        private string _name;
        private string _author;
        private InstrumentType _type;
        private DateTime _createDate;
        private string _backgroundImagePath;
        private string[] _aircraft;
        private int _animationUpdateInMs;
        private IAnimationItem[] _animations;
        public Configuration()
        {

        }

        public string Name { get { return _name; } set { if (_name != value) { _name = value; HasChanged = true; } } }
        [XmlElement("Author")]
        public string Author { get { return _author; } set { if (_author != value) { _author = value; HasChanged = true; } } }
        [XmlElement("Type")]
        public InstrumentType Type { get { return _type; } set { if (_type != value) { _type = value; HasChanged = true; } } }
        [XmlElement("CreateDate")]
        public DateTime CreateDate { get { return _createDate; } set { if (_createDate != value) { _createDate = value; HasChanged = true; } } }
        [XmlElement("BackgroundImagePath")]
        public string BackgroundImagePath { get { return _backgroundImagePath; } set { if (_backgroundImagePath != value) { _backgroundImagePath = value; HasChanged = true; } } }
        [XmlElement("Aircraft")]
        public string [] Aircraft { get { return _aircraft ?? new string[0]; } set { if (_aircraft != value) { _aircraft = value; HasChanged = true; } } }
        [XmlElement("AnimationUpdateInMs")]
        public int AnimationUpdateInMs { get { return _animationUpdateInMs; } set { if (_animationUpdateInMs != value) { _animationUpdateInMs = value; HasChanged = true; } } }
        [JsonConverter(typeof(ConcreteJSONConverter<AnimationDrawing[]>))]
        [XmlElement("Animations")]
        public AnimationXMLConverter Animations { get { return _animations; } set { if (_animations != value) { _animations = value; HasChanged = true; } } }
        [JsonIgnore]
        public ClientRequest[] ClientRequests
        {
            get
            {
                return Animations
                    .Where(x => ((IAnimationItem)x).Triggers?.Any(y => y is AnimationTriggerClientRequest) == true)
                    .SelectMany(x => ((IAnimationItem)x).Triggers?
                        .Where(y => y is AnimationTriggerClientRequest)
                        .Select(y => ((AnimationTriggerClientRequest)y).Request))
                    .ToArray();
            }
        }
        [JsonIgnore]
        public bool HasChanged { get; set; } = false;
    }
}
