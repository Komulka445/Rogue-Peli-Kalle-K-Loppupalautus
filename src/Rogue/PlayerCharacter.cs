using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;

namespace Rogue
{
    public class EnumHelper
    {
        public static string ClassAlternatives => string.Join("\n", Enum.GetValues(typeof(Class)).Cast<Class>());
        public static bool IsValidClass(string ans) => Enum.TryParse(ans, out Class c);
    }

    public enum Race
    {
        Human,
        Elf,
        Orc,
        Dwarf
    }

    
    public enum Class
    {
        Melee,
        Ranged,
        Mage
    }

    internal class PlayerCharacter
    {
        public string name;
        public Race rotu;
        public Class hahmoluokka;
    }
}
