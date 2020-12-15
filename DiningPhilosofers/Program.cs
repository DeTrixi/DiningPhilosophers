using System;
using System.Threading;

namespace DiningPhilosofers
{
    class Program
    {
        // This array hods the truth about witch forks is in use
        static readonly bool[] Forks = {false, false, false, false, false};
        // this baton Object is used for the lock
        private static object baton = new();

        /// <summary>
        /// Main Class
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // creating a lot of Philosopher Threads + naming them
            Thread philosophersZero = new Thread(Eat) {Name = "Philosofers Zero"};
            Thread philosophersOne = new Thread(Eat) {Name = "Philosofers One"};
            Thread philosophersTwo = new Thread(Eat) {Name = "Philosofers Two"};
            Thread philosophersTree = new Thread(Eat) {Name = "Philosofers Tree"};
            Thread philosophersFour = new Thread(Eat) {Name = "Philosofers Four"};
            // Starting up the threads and assigning witch forks belong to
            philosophersZero.Start(new ForksBelongingsModel {Left = 1, Right = 0});
            philosophersOne.Start(new ForksBelongingsModel {Left = 2, Right = 1});
            philosophersTwo.Start(new ForksBelongingsModel {Left = 3, Right = 2});
            philosophersTree.Start(new ForksBelongingsModel {Left = 4, Right = 3});
            philosophersFour.Start(new ForksBelongingsModel {Left = 0, Right = 4});

            // Writes hello teatcher
            Console.WriteLine("Hello Teacher!");
            Console.ReadLine();
        }

        /// <summary>
        /// Eat Method for eating and thinking
        /// </summary>
        /// <param name="input"></param>
        private static void Eat(object input)
        {
            Random ran = new Random();
            ForksBelongingsModel forks = (ForksBelongingsModel) input;

            do
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} is Thinking.........!");
                Thread.Sleep(ran.Next(10000));
                // wrap in a do

                lock (baton)
                {
                    //  look for if forks is available
                    while (Forks[forks.Left]  || Forks[forks.Right] ) Monitor.Wait(baton);
                    Forks[forks.Left] = true;
                    Forks[forks.Right] = true;
                    Monitor.PulseAll(baton);
                }

                Console.WriteLine($"{Thread.CurrentThread.Name} Is eating.....!");
                Thread.Sleep(ran.Next(10000));
                Console.WriteLine($"{Thread.CurrentThread.Name} Has finished eating............! ");

                    Forks[forks.Left] = false;
                    Forks[forks.Right] = false;

            } while (true);
        }
    }

    /// <summary>
    /// Class for setting the fork to the left and the right
    /// </summary>
    public class ForksBelongingsModel
    {
        public int Left { get; set; }
        public int Right { get; set; }
    }

    public class LockAndPush
    {

    }
}

