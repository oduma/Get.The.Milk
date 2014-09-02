using System.IO;
using GetTheMilk.GameLevels;

namespace GetTheMilk.UI.Console.ViewModels
{
    public class GameViewModel: GameBaseViewModel
    {

        private string _description;

        private string _fullStory;

        public GameViewModel()
        {
            var gameSettings = GameSettings.GetInstance();
            Description = gameSettings.Description;
            FullStory =
                File.OpenText(Path.Combine(gameSettings.DefaultPaths.GameData,
                                           gameSettings.DefaultPaths.GameDescriptionFileName)).ReadToEnd();

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
    }
}