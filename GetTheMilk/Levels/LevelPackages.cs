using System.Collections.Generic;
using GetTheMilk.Characters.BaseCharacters;

namespace GetTheMilk.Levels
{
    public class LevelPackages
    {
        public string LevelCore { get; set; }

        public string LevelObjects { get; set; }

        public List<CharacterSavedPackages> LevelCharacters { get; set; }
    }
}
