using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetTheMilk.Abstracted.UI.Models;

namespace GetTheMilk.Container.Console.Views
{
    public class GameStartViewValidation : BaseActivityView<GameStartViewModel>
    {
        public override void Render()
        {
            System.Console.Clear();
            RenderHeader();
            System.Console.WriteLine("-------------------------------------------------------------------------------");
            System.Console.WriteLine("Player setup");
            System.Console.WriteLine("Name: {0}", Model.CharacterSetup.Name);
            System.Console.WriteLine("Money: {0}", Model.CharacterSetup.StartingMoney);
            System.Console.WriteLine("Experience: {0}", Model.CharacterSetup.StartingExperience);
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            RenderActivities();
            CollectInput();
        }
    }
}
