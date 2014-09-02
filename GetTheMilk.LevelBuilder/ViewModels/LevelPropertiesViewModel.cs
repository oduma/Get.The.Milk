using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Common;
using GetTheMilk.GameLevels;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public class LevelPropertiesViewModel:ViewModelBase
    {
        private readonly Level _level;

        public int Number
        {
            get { return _level.Number; }
            set
            {
                if(value!=_level.Number)
                {
                    _level.Number = value;
                    RaisePropertyChanged("Number");
                }
            }
        }
        public string MainName
        {
            get { return (_level.Name!=null)?_level.Name.Main:string.Empty; }
            set
            {
                if(_level.Name==null)
                    _level.Name= new Noun();
                if (value != _level.Name.Main)
                {
                    _level.Name.Main = value;
                    RaisePropertyChanged("MainName");
                }
            }
        }
        public string NarratorName
        {
            get { return (_level.Name != null) ? _level.Name.Narrator : string.Empty; }
            set
            {
                if (_level.Name == null)
                    _level.Name = new Noun();
                if (value != _level.Name.Narrator)
                {
                    _level.Name.Narrator = value;
                    RaisePropertyChanged("NarratorName");
                }
            }
        }
        public string Story
        {
            get { return _level.Story; }
            set
            {
                if (value != _level.Story)
                {
                    _level.Story = value;
                    RaisePropertyChanged("Story");
                }
            }
        }
        public string FinishMessage
        {
            get { return _level.FinishMessage; }
            set
            {
                if (value != _level.FinishMessage)
                {
                    _level.FinishMessage = value;
                    RaisePropertyChanged("FinishMessage");
                }
            }
        }
        public LevelPropertiesViewModel(Level level)
        {
            _level = level;
            
        }
    }
}
