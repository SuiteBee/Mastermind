using Mastermind.State;
using Mastermind.Model;

namespace Mastermind.IO
{
    /// <summary>
    /// Handles reading and writing from the console
    /// </summary>
    public class User
    {
        #region " ASCII Headers "

        private readonly static string Title = @"
███╗   ███╗ █████╗ ███████╗████████╗███████╗██████╗ ███╗   ███╗██╗███╗   ██╗██████╗ 
████╗ ████║██╔══██╗██╔════╝╚══██╔══╝██╔════╝██╔══██╗████╗ ████║██║████╗  ██║██╔══██╗
██╔████╔██║███████║███████╗   ██║   █████╗  ██████╔╝██╔████╔██║██║██╔██╗ ██║██║  ██║
██║╚██╔╝██║██╔══██║╚════██║   ██║   ██╔══╝  ██╔══██╗██║╚██╔╝██║██║██║╚██╗██║██║  ██║
██║ ╚═╝ ██║██║  ██║███████║   ██║   ███████╗██║  ██║██║ ╚═╝ ██║██║██║ ╚████║██████╔╝
╚═╝     ╚═╝╚═╝  ╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═╝╚═╝     ╚═╝╚═╝╚═╝  ╚═══╝╚═════╝ " + "\n";

        private readonly static string Gameover = @"
 ██████╗  █████╗ ███╗   ███╗███████╗     ██████╗ ██╗   ██╗███████╗██████╗ 
██╔════╝ ██╔══██╗████╗ ████║██╔════╝    ██╔═══██╗██║   ██║██╔════╝██╔══██╗
██║  ███╗███████║██╔████╔██║█████╗      ██║   ██║██║   ██║█████╗  ██████╔╝
██║   ██║██╔══██║██║╚██╔╝██║██╔══╝      ██║   ██║╚██╗ ██╔╝██╔══╝  ██╔══██╗
╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗    ╚██████╔╝ ╚████╔╝ ███████╗██║  ██║
 ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝     ╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═╝
                                                                          " + "\n";

        private readonly static string Success = @"
██╗   ██╗ ██████╗ ██╗   ██╗    ██╗    ██╗██╗███╗   ██╗██╗
╚██╗ ██╔╝██╔═══██╗██║   ██║    ██║    ██║██║████╗  ██║██║
 ╚████╔╝ ██║   ██║██║   ██║    ██║ █╗ ██║██║██╔██╗ ██║██║
  ╚██╔╝  ██║   ██║██║   ██║    ██║███╗██║██║██║╚██╗██║╚═╝
   ██║   ╚██████╔╝╚██████╔╝    ╚███╔███╔╝██║██║ ╚████║██╗
   ╚═╝    ╚═════╝  ╚═════╝      ╚══╝╚══╝ ╚═╝╚═╝  ╚═══╝╚═╝
                                                         " + "\n";

        #endregion

        private string Msg = string.Empty;

        public User()
        {
            //To fit ASCII headers
            Console.WindowWidth = 100;
        }

        /// <summary>
        /// Display initial start screen with title and rules
        /// </summary>
        /// <param name="ruleText"></param>
        public void Show_Rules(string[] ruleText)
        {
            Console.Clear();
            Console.WriteLine(Title);
            Add_Space(1);
            Display_Multiple(ruleText);
            Add_Space(1);
        }

        /// <summary>
        /// Pause execution until keypress
        /// </summary>
        public void Wait_For_Input()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Read valid key presses until ruleset bounds satisfied
        /// </summary>
        /// <returns>validated input</returns>
        public Guess Read_Chars(Rules ruleset, string[] status)
        {
            var input = new Guess(ruleset.CodeLength);
            int count = ruleset.CodeLength;

            //Read valid key presses until input length = ruleset code length
            while (count > 0)
            {
                Display_Refresh();
                Display_Multiple(status);
                Add_Space(1);
                Display_Msg();
                Display_Input(input);

                int result = ReadKeyValue(ruleset);

                //Progress after valid input
                if(result != -1)
                {
                    input.SetDigit(ruleset.CodeLength - count, result);
                    count -= 1;
                }
            }

            return input;
        }

        /// <summary>
        /// Show ASCII art
        /// </summary>
        /// <param name="playerWins"></param>
        public void Finish(bool playerWins)
        {
            Console.Clear();
            if (playerWins)
            {
                Console.WriteLine(Success);
                Add_Space(1);
                Console.WriteLine("Congratulations you figured out the secret code");
            }
            else
            {
                Console.WriteLine(Gameover);
                Add_Space(1);
                Console.WriteLine("Better luck next time...");
            }
        }

        /// <summary>
        /// Offer another attempt and wait for decision
        /// </summary>
        /// <returns>user response</returns>
        public bool Request(string msg)
        {
            Add_Space(1);
            Console.Write(msg);

            while (true)
            {
                ConsoleKeyInfo answer = Console.ReadKey(true);

                if (answer.Key == ConsoleKey.Y)
                {
                    return true;
                }
                else if (answer.Key == ConsoleKey.N)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Capture next key and validate against rules
        /// </summary>
        /// <returns>value of valid key or -1</returns>
        private int ReadKeyValue(Rules ruleset)
        {
            char key = Console.ReadKey(true).KeyChar;

            if (ruleset.TryInput(key, out int value))
            {
                return value;
            }
            else
            {
                var blank = string.IsNullOrWhiteSpace(key.ToString());
                var unknown = key == '\0';
                var entered = blank || unknown ? '?' : key;
                Msg = $"{entered} is invalid";
            }

            return -1;
        }

        /// <summary>
        /// Write each element in array to a line
        /// </summary>
        /// <param name="arr"></param>
        private static void Display_Multiple(string[] arr)
        {
            foreach (string s in arr)
            {
                Console.WriteLine(s);
            }
        }

        /// <summary>
        /// Request user input
        /// </summary>
        /// <param name="arr">Current inputs</param>
        private static void Display_Input(Guess attempt)
        {
            Console.Write("Enter your next guess: ");
            attempt.Print();
        }

        /// <summary>
        /// Display message once and clear
        /// </summary>
        private void Display_Msg()
        {
            Console.WriteLine(Msg);
            Msg = string.Empty;
        }

        /// <summary>
        /// Clear console and print title
        /// </summary>
        private void Display_Refresh()
        {
            Console.Clear();
            Console.WriteLine(Title);
            Add_Space(2);
        }

        /// <summary>
        /// Print n blank lines to console
        /// </summary>
        /// <param name="n">number of lines</param>
        private static void Add_Space(int n)
        {
            while(n > 0)
            {
                Console.WriteLine();
                n -= 1;
            }
        }
    }
}
