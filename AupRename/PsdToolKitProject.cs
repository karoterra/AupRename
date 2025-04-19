using System.Text;
using Karoterra.AupDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AupRename
{
    public class PsdToolKitProject : FilterProject
    {
        public JArray Images { get; }

        public PsdToolKitProject(FilterProject filter)
        {
            Name = "PSDToolKit";
            string jsonStr = Encoding.UTF8.GetString(filter.DumpData());
            Images = JArray.Parse(jsonStr);
        }

        public override byte[] DumpData()
        {
            string jsonStr = JsonConvert.SerializeObject(Images) + "\n";
            return Encoding.UTF8.GetBytes(jsonStr);
        }
    }
}
