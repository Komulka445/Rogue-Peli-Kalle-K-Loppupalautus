using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurboMapReader;

namespace Rogue
{
    internal class MapLoader
    {
        
        public Map LoadMapFromFile(string filename)
        {
            string fileContents;
            using (StreamReader reader = File.OpenText(filename))
            {
                fileContents = reader.ReadToEnd();
            }
            Map loadedMap = JsonConvert.DeserializeObject<Map>(fileContents);
            return loadedMap;
        }
    }

}
