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
        public Configuration()
        {

        }
        public Configuration(AnimationDrawing[] animationDrawings)
        {
            Animations = animationDrawings;
        }
        //public Configuration(AnimationImage[] animationImages)
        //{
        //    Animations = new List<IAnimationItem>(animationImages).ToArray();
        //}
        //public Configuration(AnimationDrawing[] animationDrawings, AnimationImage[] animationImages)
        //{
        //    List<IAnimationItem> items = new List<IAnimationItem>(animationDrawings);
        //    items.AddRange(animationImages);
        //    Animations = items.ToArray();
        //}
        public string Name { get; set; }
        public string Author { get; set; }
        public InstrumentType Type { get; set; }
        public DateTime CreateDate { get; set; }
        public string BackgroundImagePath { get; set; }
        public string [] Aircraft { get; set; }
        public AnimationDrawing[] Animations { get; set; }
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

    }
}
