using GetTheMilk.Characters.BaseCharacters;
using GetTheMilk.Common;
using GetTheMilk.Levels;

namespace GetTheMilk
{
    public class GamePackages
    {
        public ContainerWithActionsPackage PlayerPackages { get; set; }

        public ContainerNoActionsPackage LevelPackages { get; set; }
    }
}
