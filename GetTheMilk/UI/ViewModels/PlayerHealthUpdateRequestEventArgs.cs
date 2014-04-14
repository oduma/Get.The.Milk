using System;

namespace GetTheMilk.UI.ViewModels
{
    public class PlayerHealthUpdateRequestEventArgs:EventArgs
    {
        public int Health { get; private set; }

        public PlayerHealthUpdateRequestEventArgs(int health)
        {
            Health = health;
        }
    }
}
