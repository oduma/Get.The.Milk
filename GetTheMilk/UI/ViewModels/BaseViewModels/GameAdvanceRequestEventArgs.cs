namespace GetTheMilk.UI.ViewModels.BaseViewModels
{
    public class GameAdvanceRequestEventArgs
    {
        public Game Game { get; private set; }

        public string Message { get; private set; }

        public string ActionName { get; private set; }

        public GameAdvanceRequestEventArgs(Game game, string message, string actionName)
        {
            Game = game;
            Message = message;
            ActionName = actionName;
        }
    }
}
