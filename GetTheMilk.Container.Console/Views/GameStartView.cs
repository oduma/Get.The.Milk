using GetTheMilk.Abstracted.UI.Models;

namespace GetTheMilk.Container.Console.Views
{
    public class GameStartView:BaseActivityView<GameStartViewModel>
    {
        public override void Render()
        {
            System.Console.Clear();
            RenderHeader();
            System.Console.WriteLine("-------------------------------------------------------------------------------");
            System.Console.WriteLine("Player setup");
            System.Console.Write("Name: ");
            Model.CharacterSetup.Name=System.Console.ReadLine();
            System.Console.Write("Boost (M)oney or (E)xperience: ");
            Model.CharacterSetup.MoneyOrExperience = System.Console.ReadLine();
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
            System.Console.WriteLine();
            RenderActivities();
            CollectInput();
        }
    }
}
