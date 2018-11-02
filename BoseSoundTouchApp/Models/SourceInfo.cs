using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoseSoundTouchApp.Models
{
    public class SourceInfo : ISourceInfo
    {
        public SourceInfo(string source, string sourceName, string sourceArt)
        {
            dependsOn = new List<string> { "GET_now_playing" };
            this.source = source;
            this.sourceName = sourceName;
            this.sourceArt = sourceArt;
        }

        public List<string> dependsOn
        {
            get;
            private set;
        }

        public string source
        {
            get;
            private set;
        }

        public string sourceName
        {
            get;
            private set;
        }

        public string sourceArt
        {
            get;
            private set;
        }
    }
}
