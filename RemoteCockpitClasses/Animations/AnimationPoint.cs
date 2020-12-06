using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace RemoteCockpitClasses.Animations
{
    public class AnimationPoint
    {
        public float X = 0;
        public float Y = 0;
        public AnimationPoint()
        {

        }
        public AnimationPoint(float x, float y)
        {
            X = x;
            Y = y;
        }
        [JsonIgnore]
        public PointF Point
        {
            get
            {
                return new PointF(X, -Y);
            }
        }
    }
}
