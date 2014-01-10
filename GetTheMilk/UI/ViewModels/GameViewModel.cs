using System;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.UI.ViewModels
{
    public class GameViewModel: GameBaseViewModel
    {
        private Game _game;

        private string _description;

        private string _fullStory;

        public GameViewModel()
        {
            _game = Game.CreateGameInstance();
            Description = _game.Description;
            FullStory = _game.FullDescription;

        }

        public string FullStory
        {
            get
            {
                return this._fullStory;
            }

            set
            {
                if (value != _fullStory)
                {
                    _fullStory = value;
                    RaisePropertyChanged("FullStory");
                }
            }
        }

        public string Description
        {
            get
            {
                return this._description;
            }

            set
            {
                if (value != _description)
                {
                    _description = value;
                    RaisePropertyChanged("Description");
                }
            }
        }

        public override event EventHandler<GameStartRequestEventArgs> GameStartRequest;
    }
}
