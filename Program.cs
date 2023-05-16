using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static System.Formats.Asn1.AsnWriter;

namespace CS_Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            //checks to create a scoreboard for all players, if it doesn't exist, creates it
            string allPath = Directory.GetCurrentDirectory() + @"\All Scores.txt";

            if (File.Exists(allPath))
            {
                //if it exists, do nothing
            }
            else
            {
                //if it doesn't exist, create the scorecard
                using var sw = new StreamWriter(allPath);

                sw.WriteLine("All Scores");
            }

            //asks users name and creates a player for it
            Console.Write("Enter Your Name: ");

            String name = Console.ReadLine();

            Program.CreatePlayer(name);

            //starts game
            while (true)
            {
                
                //menu select
                Console.WriteLine("Hello " + name + ", what would you like to do here?");

                Console.WriteLine("(1) Play Game \n(2) Check Scores \n(3) Switch User \n(4) Exit");

                String mainMenuOption = Console.ReadLine();

                //menu options

                //plays the game
                if (mainMenuOption.Equals("1"))
                {
                    Program.TheGame(name);
                    mainMenuOption = "";
                    Console.Clear();
                }
                //checks the scores
                else if (mainMenuOption.Equals("2"))
                {
                    Program.SortScore(name);
                    mainMenuOption = "";
                    Console.WriteLine("Press Enter to Continue");
                    Console.ReadLine();
                    Console.Clear();
                }
                //switches the user to a different player
                else if (mainMenuOption.Equals("3"))
                {
                    Console.Clear();
                    Console.Write("Enter Your Name: ");

                    name = Console.ReadLine();
                    Program.CreatePlayer(name);
                }
                //exits the app
                else if (mainMenuOption.Equals("4"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Sorry, the input " + mainMenuOption + " is not recognized by the system\nPress Enter to go back");
                    mainMenuOption = "";
                    Console.ReadLine();

                    Console.Clear();
                }
            }
            



        }

        static void TheGame(string playerName)
        {
            //declars rng
            Random rnd = new Random();
            //declares random number generator and variables to guess with
            int theNum = rnd.Next(0, 1001);
            int guesses = 0;
            int guess = -1;
            string placeHolderGuess = "";
            Console.Clear();

            while (true)
            {
                //asks user to guess a number
                Console.Write("Guess the number: ");

                placeHolderGuess = Console.ReadLine();
                //checks if placeHolderGuess works
                if (int.TryParse(placeHolderGuess, out guess))
                {
                    guesses++;
                }
                //checks if placeHolderGuess should exit the game
                else if (placeHolderGuess.ToLower().Equals("x"))
                {
                    break;
                }
                //confirms that placeHolderGuess does not work
                else
                {
                    Console.WriteLine("Sorry, the input " + placeHolderGuess +" is not recognized by the system");
                    continue;
                }
                //checks guess and gives hints if it's wrong
                if (guess == theNum)
                {
                    break;
                }
                else if(guess > theNum)
                {
                    Console.WriteLine("Too High! You have guessed " + guesses + " times");
                }
                else if(guess < theNum)
                {
                    Console.WriteLine("Too Low! You have guessed " + guesses + " times");
                }
                //tells user to exit if they are stumped
                if(guesses > 50)
                {
                    Console.WriteLine("By the way, you can type x to exit the game");
                }

            }
            //checks if user cant finish the game
            if (placeHolderGuess.ToLower().Equals("x"))
            {
                Console.Clear();
                Console.WriteLine("Guess you just couldn't hack it");
                guesses = 2147483647;
            }
            //checks if user got a lucky guess
            else if (guesses <= 1)
            {
                Console.WriteLine("Well now thats just lucky, only took you one try");
            }
            //checks various guess counts
            else if (guesses <= 5)
            {
                Console.WriteLine("HOLY COW, you got it in only " + guesses + " guesses!");
            }
            else if (guesses <= 10)
            {
                Console.WriteLine("Nice, you got it in only " + guesses + " guesses!");
            }
            else if(guesses <= 20)
            {
                Console.WriteLine("Not bad, you got it in " + guesses + " guesses");
            }
            else
            {
                Console.WriteLine("Took you a while, but you still got it in" + guesses + " guesses");
            }
            //Updates the scores of a user with their new score
            UpdateScore(Convert.ToString(guesses), playerName);

            //checks if user wants to play again
            while (true) {
                Console.WriteLine("Would you like to \n(1) Restart\n(2) Back to menu");

                String gameOption = Console.ReadLine();
                Console.Clear();
                //sends you back to the menu
                if (gameOption.Equals("1"))
                {
                    Program.TheGame(playerName);
                }
                //restarts the game
                else if (gameOption.Equals("2"))
                {
                    break;
                }
                //checks if the input does not work
                else
                {
                    Console.WriteLine("Sorry, the input " + gameOption + " is not recognized by the system\nPress Enter to go back");
                    Console.ReadLine();
                    Console.Clear();
                }
            }








        }

        //creates a new player for the game
        static void CreatePlayer(string playerName)
        {
            //gets the path of the players scorecard
            string path = Directory.GetCurrentDirectory() + @"\" + playerName + ".txt";


            if(File.Exists(path))
            {
                //if it exists, do nothing
            }
            else
            {
                //if it doesn't exist, create the scorecard
                using var sw = new StreamWriter(path);

                sw.WriteLine(playerName);
            }
            
        }

        //updates the score for the player
        static void UpdateScore(string score, string playerName)
        {
            //gets the directory of the players scorecard
            string path = Directory.GetCurrentDirectory() + @"\" + playerName + ".txt";
            string allPath = Directory.GetCurrentDirectory() + @"\All Scores.txt";

            //adds the new score to the end of the scorecard
            File.AppendAllText(path, score + Environment.NewLine);
            File.AppendAllText(allPath, score + Environment.NewLine);
        }

        //sorts the players scorecard
        static void SortScore(string playerName)
        {
            Console.Clear();
            //prints the players name
            Console.WriteLine(playerName + "'s Scores: ");

            //gets the current players path
            string path = Directory.GetCurrentDirectory() + @"\" + playerName + ".txt";

            //writes out the path for bug fixing purposes
            //Console.WriteLine(path);

            //adds the whole scorecard to an array, including the player name, we need to remove this shortly
            string[] scoresArray = File.ReadLines(path).ToArray();

            //creates an array 1 less than the scoresArray to put the sorted array without the player name into
            int[] editedScoresArray = new int[scoresArray.Length-1];

            //goes through everything after the name in ScoresArray and adds it to the editedScoresArray
            for(int i = 1; i < scoresArray.Length; i++)
            {
                //converts the scoresArray to int and puts it into the editecScoresArray
                editedScoresArray[i-1] = Convert.ToInt32(scoresArray[i]);
            }

            //sorts the editedScoresArray
            Array.Sort(editedScoresArray);

            //writes everything in the editedScoresArray to the console
            foreach (int score in editedScoresArray)
            {
                Console.WriteLine(score.ToString());
            }
            Console.WriteLine();


        }



    }


}
