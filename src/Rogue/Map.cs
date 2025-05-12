using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rogue
{
    internal class Map
    {
        public int mapWidth;
        public MapLayer[] layers;
        public List<Enemy> enemies;
        List<Item> items;
        public MapLayer GetLayer(string layerName)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (layers[i].name == layerName)
                {
                    return layers[i];
                }
            }
            Console.WriteLine($"Error: No layer with name: {layerName}");
            return null; // Wanted layer was not found!
        }
    }
}
