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

        public static MapLayer ConvertLayer(TurboMapReader.MapLayer mapLayer)
        {
            int howManyTiles = mapLayer.data.Length;
            int[] Tiles = mapLayer.data;
            MapLayer myGroundLayer = new MapLayer();
            myGroundLayer.name = mapLayer.name;
            myGroundLayer.mapTiles = Tiles;
            return myGroundLayer;
        }

        public static Map ConvertTiledMapToMap(TiledMap turboMap)
        {
            // Luo tyhjä kenttä
            Map rogueMap = new Map();

            // Muunna tason "ground" tiedot
            TurboMapReader.MapLayer groundLayer = turboMap.GetLayerByName("ground");
            
            // TODO: Lue kentän leveys. Kaikilla TurboMapReader.MapLayer olioilla on sama leveys
            int width = groundLayer.width; //??????????????????????????????????????????????????????
            rogueMap.mapWidth = width;
            // Kuinka monta kenttäpalaa tässä tasossa on?
            // TODO: lue tason palat
            rogueMap.layers = new MapLayer[3];
            // Tallenna taso kenttään
            rogueMap.layers[0] = ConvertLayer(turboMap.GetLayerByName("ground")) ;
            rogueMap.layers[1] = ConvertLayer(turboMap.GetLayerByName("enemies"));
            rogueMap.layers[2] = ConvertLayer(turboMap.GetLayerByName("items"));

            // Lopulta palauta kenttä
            return rogueMap;
        }

    }

}
