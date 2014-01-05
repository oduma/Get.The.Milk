using GetTheMilk.Abstracted.UI.Models;

namespace GetTheMilk.Container.Console.Views
{
    public class GamePlayView:BaseActivityView<GamePlayViewModel>
    {
        public override void Render()
        {
            System.Console.Clear();
            RenderHeader();
            System.Console.WriteLine(Model.LevelNo);
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
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            RenderActivities();
            CollectInput();

        }
    }
}
