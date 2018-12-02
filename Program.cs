using System;
using System.Speech.Synthesis;

namespace SnakeGame
{
    class Program
    {
        public static int Apple = 64;
        public static int Snake = 214;

        static void Main(string[] args)
        {

            #region Vars
            int[] xPosition = new int[50];
            xPosition[0] = 35;
            int[] yPosition = new int[50];
            yPosition[0] = 20;

            int xAppleDim = 10;
            int yAppleDim = 10;
            int appleEaten = 0;

            decimal gameSpeed = 150m;

            bool isGameOn = true;
            bool iswallHit = false;
            bool isAppleEaten = false;
            bool isStayInMenu = true;

            string userAction = "";


            Random random = new Random();

            Console.CursorVisible = false;
            #endregion

            // Build welcome screen
            ShowMenu(out userAction);

            do
            {
                switch (userAction)
                {
                    // Give player option to read directions
                    #region Case Direction
                    case "1":
                    case "d":
                    case "directions":
                        Console.Clear();
                        BuildWall();
                        Console.SetCursorPosition(5, 5);
                        Console.WriteLine("1) Resize the console window so you can see all.");

                        Console.SetCursorPosition(5, 6);
                        Console.WriteLine("    4 sides of playing field boarder.");

                        Console.SetCursorPosition(5, 7);
                        Console.WriteLine("2) Use the arrow key to move the snake around the field.");

                        Console.SetCursorPosition(5, 8);
                        Console.WriteLine("3) The snake will die if it runs into the wall.");

                        Console.SetCursorPosition(5, 9);
                        Console.WriteLine("4) You gain point by eating the apple,");

                        Console.SetCursorPosition(5, 10);
                        Console.WriteLine("    but your snake will also go faster and get longer.");

                        Console.SetCursorPosition(5, 12);
                        Console.WriteLine("Press enter to return to the main menu.");
                        Console.ReadLine();
                        Console.Clear();
                        ShowMenu(out userAction);

                        break;
                    #endregion

                    #region Case Play
                    case "2":
                    case "p":
                    case "play":

                        Console.Clear();
                        #region Game Setup
                        // Get the snake to appear on the screen
                        PaintSnake(appleEaten, xPosition, yPosition, out xPosition, out yPosition);

                        // Get the apple to appear on the screen for the first time
                        SetApplePositionOnScreen(random, out xAppleDim, out yAppleDim);
                        PaintApple(xAppleDim, yAppleDim);

                        // Build Boundary
                        BuildWall();
                        #endregion

                        // Get the snake to move
                        ConsoleKey command = Console.ReadKey().Key;
                        do
                        {
                            #region Change Direction
                            switch (command)
                            {
                                case ConsoleKey.LeftArrow:
                                    Console.SetCursorPosition(xPosition[0], yPosition[0]);
                                    Console.Write(" ");
                                    xPosition[0]--;
                                    break;

                                case ConsoleKey.UpArrow:
                                    Console.SetCursorPosition(xPosition[0], yPosition[0]);
                                    Console.Write(" ");
                                    yPosition[0]--;
                                    break;

                                case ConsoleKey.RightArrow:
                                    Console.SetCursorPosition(xPosition[0], yPosition[0]);
                                    Console.Write(" ");
                                    xPosition[0]++;
                                    break;

                                case ConsoleKey.DownArrow:
                                    Console.SetCursorPosition(xPosition[0], yPosition[0]);
                                    Console.Write(" ");
                                    yPosition[0]++;
                                    break;
                            }
                            #endregion


                            #region Playing Game
                            // Paint the snake, Make snake longer
                            PaintSnake(appleEaten, xPosition, yPosition, out xPosition, out yPosition);


                            // Detect when snake hits boundry
                            iswallHit = DidSnakeHitWall(xPosition[0], yPosition[0]);

                            if (iswallHit)
                            {
                                isGameOn = false;
                                Console.SetCursorPosition(28, 20);
                                Console.WriteLine("The snake hit the wall and died.");


                                // Show score
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.SetCursorPosition(15, 21);
                                Console.Write("Your Score is " + appleEaten * 100 + "!");
                                Console.SetCursorPosition(15, 22);
                                Console.WriteLine("Press enter to continue...");
                                appleEaten = 0;
                                Console.ReadLine();
                                Console.Clear();

                                // Give player option to replay the game
                                ShowMenu(out userAction);
                            }


                            // Detect when apple was eaten
                            isAppleEaten = DetermineIfTheAppleWasEaten(xPosition[0], yPosition[0], xAppleDim, yAppleDim);

                            // Place apple on Board (random)
                            if (isAppleEaten)
                            {
                                SetApplePositionOnScreen(random, out xAppleDim, out yAppleDim);
                                PaintApple(xAppleDim, yAppleDim);

                                // keep track of how many apples were eaten
                                appleEaten++;
                                // make snake faster
                                gameSpeed *= .925m;

                                // make snake longer
                            }



                            // Slow game down
                            if (Console.KeyAvailable) command = Console.ReadKey().Key;
                            System.Threading.Thread.Sleep(Convert.ToInt32(gameSpeed));
                            #endregion


                        } while (isGameOn);

                        break;
                    #endregion

                    case "3":
                    case "e":
                    case "exit":
                        isStayInMenu = false;
                        Console.Clear();
                        break;

                    default:
                        Console.WriteLine("Your input was not understood, Please press enter and try again.");
                        Console.ReadLine();
                        Console.Clear();
                        ShowMenu(out userAction);
                        break;
                }


            } while (isStayInMenu);

            // Give Player option to replay the game

            Console.ReadLine();
        }

        #region Menu
        private static void ShowMenu(out string userAction)
        {
            string menu = "1) Directions\n 2) Play\n 3) Exit \n\n\n" + @"";

            Console.WriteLine(menu);
            System.Threading.Thread.Sleep(100);

            SpeechSynthesizer toSpeak = new SpeechSynthesizer();
            toSpeak.SetOutputToDefaultAudioDevice();
            toSpeak.Speak("The snake game!");

            userAction = Console.ReadLine().ToLower();

        }
        #endregion

        #region Methods
        private static void PaintSnake(int appleEaten, int[] xPositionIn, int[] yPositionIn, out int[] xPositionOut, out int[] yPositionOut)
        {
            // Paint the head
            Console.SetCursorPosition(xPositionIn[0], yPositionIn[0]);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine((char)Snake);

            // paint the body
            for (int i = 1; i < appleEaten + 1; i++)
            {
                Console.SetCursorPosition(xPositionIn[i], yPositionIn[i]);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("o");
            }


            // Erase last part of the snake
            Console.SetCursorPosition(xPositionIn[appleEaten + 1], yPositionIn[appleEaten + 1]);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" ");

            // Record location of each body part
            for (int i = appleEaten + 1; i > 0; i--)
            {
                xPositionIn[i] = xPositionIn[i - 1];
                yPositionIn[i] = yPositionIn[i - 1];
            }

            // Return the new array

            xPositionOut = xPositionIn;
            yPositionOut = yPositionIn;
        }

        private static bool DetermineIfTheAppleWasEaten(int xPosition, int yPosition, int xAppleDim, int yAppleDim)
        {
            if (xPosition == xAppleDim && yPosition == yAppleDim) return true;
            return false;
        }

        private static void PaintApple(int xAppleDim, int yAppleDim)
        {
            Console.SetCursorPosition(xAppleDim, yAppleDim);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write((char)Apple);
        }

        private static void SetApplePositionOnScreen(Random random, out int xAppleDim, out int yAppleDim)
        {
            xAppleDim = random.Next(0 + 2, 70 - 2);
            yAppleDim = random.Next(0 + 2, 40 - 2);
        }

        private static bool DidSnakeHitWall(int xPosition, int yPosition)
        {
            if (xPosition == 1 || xPosition == 70 || yPosition == 1 || yPosition == 40)
                return true;
            return false;
        }

        private static void BuildWall()
        {
            for (int i = 1; i < 41; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(1, i);
                Console.Write("#");
                Console.SetCursorPosition(70, i);
                Console.Write("#");
            }

            for (int i = 1; i < 71; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(i, 1);
                Console.Write("#");
                Console.SetCursorPosition(i, 40);
                Console.Write("#");
            }
        }
        #endregion
    }
}
