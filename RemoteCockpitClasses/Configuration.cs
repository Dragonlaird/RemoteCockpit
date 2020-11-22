using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace RemoteCockpitClasses.Animations
{
    [DebuggerDisplay("\\{Configuration\\} {Name}")]
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
        public string Author { get { return _author; } set { if (_author != value) { _author = value; HasChanged = true; } } }
        public InstrumentType Type { get { return _type; } set { if (_type != value) { _type = value; HasChanged = true; } } }
        public DateTime CreateDate { get { return _createDate; } set { if (_createDate != value) { _createDate = value; HasChanged = true; } } }
        public string BackgroundImagePath { get { return _backgroundImagePath; } set { if (_backgroundImagePath != value) { _backgroundImagePath = value; HasChanged = true; } } }
        public string [] Aircraft { get { return _aircraft ?? new string[0]; } set { if (_aircraft != value) { _aircraft = value; HasChanged = true; } } }
        public int AnimationUpdateInMs { get { return _animationUpdateInMs; } set { if (_animationUpdateInMs != value) { _animationUpdateInMs = value; HasChanged = true; } } }
        [JsonConverter(typeof(ConcreteConverter<IAnimationItem[]>))]
        public IAnimationItem[] Animations { get { return _animations; } set { if (_animations != value) { _animations = value; HasChanged = true; } } }
        public ClientRequest[] ClientRequests
        {
            get
            {
                return Animations
                    .Where(x => x.Triggers?.Any(y => y.Type == AnimationTriggerTypeEnum.ClientRequest) == true)
                    .SelectMany(x => x.Triggers?
                        .Where(y => y.Type == AnimationTriggerTypeEnum.ClientRequest)
                        .Select(y => ((AnimationTriggerClientRequest)y).Request))
                    .ToArray();
            }
        }
        public bool HasChanged { get; set; } = false;
    }
}
