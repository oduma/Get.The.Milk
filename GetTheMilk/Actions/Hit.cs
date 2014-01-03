using GetTheMilk.Objects;
using GetTheMilk.Objects.BaseObjects;

namespace GetTheMilk.Actions
{
    public class Hit
    {
        public Weapon WithWeapon { get; set; }

        public int Power { get; set; }

        public string QuitMessage { get; set; }
    }
}
