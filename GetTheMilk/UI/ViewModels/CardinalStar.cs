using GetTheMilk.GameLevels;

namespace GetTheMilk.UI.ViewModels
{
    public class CardinalStar
    {
        public DirectionShortcut North { get { return new DirectionShortcut {Direction = Direction.North, Shortcut = "W"}; } }
        public DirectionShortcut South { get { return new DirectionShortcut { Direction = Direction.South, Shortcut = "Z" }; } }
        public DirectionShortcut East { get { return new DirectionShortcut { Direction = Direction.East, Shortcut = "S" }; } }
        public DirectionShortcut West { get { return new DirectionShortcut { Direction = Direction.West, Shortcut = "A" }; } }
        public DirectionShortcut Top { get { return new DirectionShortcut { Direction = Direction.Top, Shortcut = "E" }; } }
        public DirectionShortcut Bottom { get { return new DirectionShortcut { Direction = Direction.Bottom, Shortcut = "D" }; } }

        public static Direction GetDirectionByShortcut(string shortcut)
        {
            switch(shortcut)
            {
                case "W":
                    return Direction.North;
                case "Z":
                    return Direction.South;
                case "S":
                    return Direction.East;
                case "A":
                    return Direction.West;
                case "E":
                    return Direction.Top;
                case "D":
                    return Direction.Bottom;
            }
            return Direction.None;
        }
    }
}
