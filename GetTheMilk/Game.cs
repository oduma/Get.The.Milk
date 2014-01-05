namespace GetTheMilk
{
    public class Game
    {
        public readonly string Name = "Get The Milk";

        public readonly string Description = "Game description - the short version";

        private static readonly Game Instance= new Game();

        private Game()
        {
            
        }

        public readonly string FullDescription = @"Full Story Line 1
Full Story Line 2
Full Story Line 3
Full Story Line 4
Full Story Line 5
Full Story Line 6
Full Story Line 7
Full Story Line 8
Full Story Line 9
Full Story Line 10
Full Story Line 11
Full Story Line 12
Full Story Line 13
Full Story Line 14
Full Story Line 15
Full Story Line 16
Full Story Line 17
Full Story Line 18
Full Story Line 19
Full Story Line 20
Full Story Line 21
Full Story Line 22
Full Story Line 23
Full Story Line 24
Full Story Line 25
Full Story Line 26
Full Story Line 27
Full Story Line 28
Full Story Line 29
Full Story Line 30
Full Story Line 31
Full Story Line 32
Full Story Line 33
Full Story Line 34
Full Story Line 35
Full Story Line 36
Full Story Line 37
Full Story Line 38
Full Story Line 39
Full Story Line 40
Full Story Line 41
Full Story Line 42
Full Story Line 43
Full Story Line 44
Full Story Line 45
Full Story Line 46
Full Story Line 47
";

        public static Game CreateGameInstance()
        {
            return Instance;
        }

    }
}
