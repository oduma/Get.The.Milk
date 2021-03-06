namespace GetTheMilk.Actions.BaseActions
{
    public enum ActionResultType
    {
        Ok,
        NotOk,
        Win,
        QuitAccepted,
        GameOver,
        Lost,
        RequestQuit,
        OutOfTheMap,
        Blocked,
        OriginNotOnTheMap,
        UnknownError,
        LevelCompleted,
        CannotPerformThisAction
    }
}
