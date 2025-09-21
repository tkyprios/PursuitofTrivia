using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pursuit_of_Trivia
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                ShowWelcomeScreen();
                Game game = new Game();
                game.StartGame();
            }
        }


        private static void ShowWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("-- Welcome to Pursuit of Trivia! --");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Press any key to enter game setup...");
            Console.ReadKey(true);
        }
    }
}
