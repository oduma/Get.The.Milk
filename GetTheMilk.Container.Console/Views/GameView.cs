using System;
using System.Collections.Generic;
using GetTheMilk.Abstracted.UI.Models;
using MonoCross.Navigation;
using MonoCross.Console;

namespace GetTheMilk.Container.Console.Views
{
    public class GameView : BaseActivityView<GameViewModel>
    {
        public override void Render()
        {
            System.Console.Clear();
            RenderHeader();
            System.Console.WriteLine(Model.Description);
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
