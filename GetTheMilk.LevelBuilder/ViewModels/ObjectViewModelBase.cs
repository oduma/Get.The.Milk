using GetTheMilk.Objects.BaseObjects;
using GetTheMilk.UI.ViewModels.BaseViewModels;

namespace GetTheMilk.LevelBuilder.ViewModels
{
    public abstract class ObjectViewModelBase:ViewModelBase
    {
        public abstract NonCharacterObject Value { get; set; }
    }
}
