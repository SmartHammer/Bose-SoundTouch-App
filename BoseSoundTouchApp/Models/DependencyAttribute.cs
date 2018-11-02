using System;

namespace BoseSoundTouchApp.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=true)
]
    public class DependencyAttribute : Attribute
    {
        public DependencyAttribute(string dependsOn)
        {
            DependsOn = dependsOn;
        }

        public string DependsOn
        {
            get;
            private set;
        }
    }
}
