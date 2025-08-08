namespace Mastermind.Assets
{
    /// <summary>
    /// Handles reading and writing from the console
    /// </summary>
    public static class UserInterface
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

        private static string Msg = string.Empty;
        
        /// <summary>
        /// Set console width
        /// </summary>
        public static void Initialize()
        {
            //To fit ASCII headers
            Console.WindowWidth = 100;
        }

        /// <summary>
        /// Display initial start screen with title and rules
        /// </summary>
        /// <param name="ruleText"></param>
        public static void Show_Rules(string[] ruleText)
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
        public static void Wait()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Read only valid next n key presses
        /// </summary>
        /// <returns>validated input</returns>
        public static int[] Read_Chars(string[] status, int n)
        {
            int[] input = new int[n];
            int count = n;

            //Read next 4 valid key presses
            while (count > 0)
            {
                Display_Refresh();
                Display_Multiple(status);
                Add_Space(1);
                Display_Msg();
                Display_Input(input);

                int result = ReadKeyValue();

                //Progress after valid input
                if(result != -1)
                {
                    input[n - count] = result;
                    count -= 1;
                }
            }

            return input;
        }

        /// <summary>
        /// Show ASCII art
        /// </summary>
        /// <param name="playerWins"></param>
        public static void Finish(bool playerWins)
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
        public static bool Retry()
        {
            Add_Space(1);
            Console.Write("Would you like to play again? [press y/n to continue/exit]");

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
        private static int ReadKeyValue()
        {
            char key = Console.ReadKey(true).KeyChar;

            if (Rules.TryInput(key, out int value))
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
        private static void Display_Input(int[] arr)
        {
            Console.Write("Enter your next guess: ");
            foreach (int i in arr)
            {
                Console.Write(i);
            }
        }

        /// <summary>
        /// Display message once and clear
        /// </summary>
        private static void Display_Msg()
        {
            Console.WriteLine(Msg);
            Msg = string.Empty;
        }

        /// <summary>
        /// Clear console and print title
        /// </summary>
        private static void Display_Refresh()
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
