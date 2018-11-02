using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using BoseSoundTouchApp.Models;

namespace BoseSoundTouchApp.Services
{
    public class GET_presets : GET
    {
        public GET_presets(ref IPhysicalData physicalData)
            : base(ref physicalData)
        { }

        public new int CycleTimeInMS => 2000;

        public Dictionary<int, IPreset> Presets
        {
            get
            {
                Dictionary<int, IPreset> presets = new Dictionary<int, IPreset>();
                if (Document != null)
                {
                    var value = Document.DescendantNodes()
                        .Where(n => (n.NodeType == XmlNodeType.Element))
                        .Select(n => n as XElement)
                            .Where(n => n.Name.LocalName == "preset")
                            .Select(n => n);
                    foreach (var preset in value)
                    {
                        var id = Supporter.GetLocalAttribute<int>(preset, "id");
                        var source = Supporter.GetAttribute<string>(preset, "ContentItem", "source");
                        var itemName = Supporter.GetValue<string>(preset, "itemName");
                        var itemArt = Supporter.GetValue<string>(preset, "containerArt");
                        presets.Add(id, new Preset(source, itemName, itemArt));
                    }
                }

                return presets;
            }
        }
    }
}
