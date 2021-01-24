using Newtonsoft.Json;
using RemoteCockpitClasses.Animations.Items;
using RemoteCockpitClasses.Animations.Triggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;

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

        /// <summary>
        /// Load configuration from a file path, JSON definition or XML file.
        /// Will attempt to auto-detect the type of value supplied in configDefinition
        /// </summary>
        /// <param name="configurationDefinition">File path, JSON content or XML content</param>
        public void Load(string configurationDefinition)
        {
            if(configurationDefinition != null)
            {
                configurationDefinition = configurationDefinition.Trim();
                if (configurationDefinition.StartsWith("{"))
                {
                    // Supplied value is JSON - Load from JSON
                    LoadFromJSON(configurationDefinition);
                }
                if (configurationDefinition.StartsWith("<"))
                {
                    // Supplied value is XML - Load from XML
                    LoadFromXML(configurationDefinition);
                }
                if(!configurationDefinition.StartsWith("{") && !configurationDefinition.StartsWith("<"))
                {
                    // Supplied value is a file path - Load from File
                    LoadFromFile(configurationDefinition);
                }
            }
        }

        /// <summary>
        /// Load Configuration from supplied JSON string
        /// </summary>
        /// <param name="configJSON">JSON string containing Configuration Definition</param>
        public void LoadFromJSON(string configJSON)
        {
            var config = JsonConvert.DeserializeObject<Configuration>(configJSON);
            PopulatePropertiesFromConfigObject(config);
        }

        /// <summary>
        /// Load Configuration from supplied XML string
        /// </summary>
        /// <param name="configXML">XML string containing Configuration Definition</param>
        public void LoadFromXML(string configXML)
        {
            var stream = new MemoryStream();
            var bytes = Encoding.ASCII.GetBytes(configXML);
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;
            var serializer = new XmlSerializer(typeof(Configuration));
            var config = (Configuration)serializer.Deserialize(stream);
            // Now map properties and reset HasChanged flag
            PopulatePropertiesFromConfigObject(config);
        }

        /// <summary>
        /// Load Configuration from supplied XML string
        /// </summary>
        /// <param name="configFilePath">Path to File containing Configuration Definition (XML or JSON)</param>
        public void LoadFromFile(string configFilePath)
        {
            var fileContent = System.IO.File.ReadAllText(configFilePath)?.Trim();
            if (fileContent.StartsWith("{"))
                LoadFromJSON(fileContent);
            if (fileContent.StartsWith("<"))
                LoadFromXML(fileContent);
        }

        public void Clear()
        {
            Name = null;
            Author = null;
            Type = InstrumentType.Other;
            CreateDate = DateTime.Now;
            BackgroundImagePath = null;
            Aircraft = new string[0];
            AnimationUpdateInMs = 50;
            Animations = new IAnimationItem[0];
            HasChanged = false;
        }

        private void PopulatePropertiesFromConfigObject(Configuration config)
        {
            Clear();
            foreach(var property in config.GetType().GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(config));
                }
            }
        }

        public string Name { get { return _name; } set { if (_name != value) { _name = value; HasChanged = true; } } }
        public string Author { get { return _author; } set { if (_author != value) { _author = value; HasChanged = true; } } }
        public InstrumentType Type { get { return _type; } set { if (_type != value) { _type = value; HasChanged = true; } } }
        public DateTime CreateDate { get { return _createDate; } set { if (_createDate != value) { _createDate = value; HasChanged = true; } } }
        public string BackgroundImagePath { get { return _backgroundImagePath; } set { if (_backgroundImagePath != value) { _backgroundImagePath = value; HasChanged = true; } } }
        public string [] Aircraft { get { return _aircraft ?? new string[0]; } set { if (_aircraft != value) { _aircraft = value; HasChanged = true; } } }
        public int AnimationUpdateInMs { get { return _animationUpdateInMs; } set { if (_animationUpdateInMs != value) { _animationUpdateInMs = value; HasChanged = true; } } }
        [JsonConverter(typeof(ConcreteConverter<AnimationDrawing[]>))]
        public IAnimationItem[] Animations { get { return _animations; } set { if (_animations != value) { _animations = value; HasChanged = true; } } }
        [JsonIgnore]
        [XmlIgnore]
        public ClientRequest[] ClientRequests
        {
            get
            {
                if (Animations == null)
                    return new ClientRequest[0];
                return Animations
                    .Where(x => x.Triggers?.Any(y => y is AnimationTriggerClientRequest) == true)
                    .SelectMany(x => x.Triggers?
                        .Where(y => y is AnimationTriggerClientRequest)
                        .Select(y => ((AnimationTriggerClientRequest)y).Request))
                    .ToArray();
            }
        }
        [JsonIgnore]
        [XmlIgnore]
        public bool HasChanged { get; set; } = false;
    }
}
