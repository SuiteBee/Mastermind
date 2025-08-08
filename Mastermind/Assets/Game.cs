namespace Mastermind.Assets
{
    /// <summary>
    /// Manages game state and contains ruleset
    /// </summary>
    public class Game
    {
        /////////////////////////////////////////
        //State
        /////////////////////////////////////////
        public bool Continue
        {
            get
            {
                return Remaining > 0;
            }
        }

        private bool Win { get; set; }
        private bool GameOver { get; set; }
        private int Remaining { get; set; }
        private string Hint { get; set; } = string.Empty;

        /////////////////////////////////////////
        //Parameters
        /////////////////////////////////////////
        public readonly Rules Ruleset;
        private readonly int[] Secret;

        /// <summary>
        /// Initialize game state and generate secret code
        /// </summary>
        /// <param name="n">Number of allowed attempts</param>
        /// <param name="d">Number of digits in secret code</param>
        /// <param name="lower">Lower value limit allowed for integers in secret</param>
        /// <param name="upper">Upper value limit allowed for integers in secret</param>
        public Game(int n, int d, int lower, int upper)
        {
            Ruleset = new Rules(n, d, lower, upper);
            Win = false;
            GameOver = false;
            Remaining = n;

            Secret = Generate_Secret(d);
        }

        /// <summary>
        /// Display rules and wait for user input to start
        /// </summary>
        public void Intro()
        {
            UserInterface.Initialize();
            UserInterface.Show_Rules(Ruleset.Text);
            UserInterface.Wait();
        }

        /// <summary>
        /// Read user input and progress game state
        /// </summary>
        /// <returns>formatted user input</returns>
        public int[] Begin_Round()
        {
            var status = Get_Status();
            var attempt = UserInterface.Read_Chars(status, Ruleset.Length);

            Update_State(attempt);
            return attempt;
        }

        /// <summary>
        /// Check game state and end session or generate hint
        /// </summary>
        /// <param name="attempt">formatted user input</param>
        public void End_Round(int[] attempt)
        {
            if(Win || GameOver)
            {
                End_Session();
            }
            else
            {
                Hint = Generate_Hint(attempt);
            }
        }

        /// <summary>
        /// Offer retry and get input
        /// </summary>
        /// <returns>user response</returns>
        public static bool Play_Again()
        {
            return UserInterface.Retry();
        }

        /// <summary>
        /// Game has ended
        /// </summary>
        private void End_Session()
        {
            Remaining = 0;
            UserInterface.Finish(Win);
        }

        /// <summary>
        /// Generate a pseudorandom array of integers within class bounds (RangeLower to RangeUpper)
        /// </summary>
        /// <param name="d">length of generated a array</param>
        /// <returns>array of d length</returns>
        private static int[] Generate_Secret(int d)
        {
            var newSecret = new int[d];
            for (int i = 0; i < d; i++)
            {
                newSecret[i] = Rules.Random();
            }
            return newSecret;
        }

        /// <summary>
        /// Return remaining attempts and potential hint
        /// </summary>
        private string[] Get_Status()
        {
            string suggestion = string.IsNullOrEmpty(Hint) ? "None" : Hint;
            return [
                $"You have {Remaining} attempts remaining",
                $"Hint: {suggestion}"
            ];

        }

        /// <summary>
        /// Check submitted guess against secret and update game state
        /// </summary>
        /// <param name="attempt"></param>
        private void Update_State(int[] attempt)
        {
            Win = Secret.SequenceEqual(attempt);

            Remaining -= 1;
            if (Remaining == 0)
            {
                GameOver = true;
            }
        }

        /// <summary>
        /// Compares validated input to secret
        /// </summary>
        /// <param name="attempt">User Input</param>
        /// <returns>Hint</returns>
        private string Generate_Hint(int[] attempt)
        {
            int perfectMatch = 0;
            int imperfectMatch = 0;

            for (int i = 0; i < attempt.Length; i++)
            {
                //Perfect Match: Correct digit in correct position
                if (attempt[i] == Secret[i])
                {
                    perfectMatch += 1;
                }
                //Imperfect Match: Correct digit in incorrect position
                else if (attempt.Contains(Secret[i]))
                {
                    imperfectMatch += 1;
                }
            }

            return Get_Hint(perfectMatch, imperfectMatch);
        }

        /// <summary>
        /// Supplies hint from generation results
        /// </summary>
        /// <param name="p">Perfect Match</param>
        /// <param name="i">Imperfect Match</param>
        /// <returns>Formatted Hint</returns>
        private static string Get_Hint(int p, int i)
        {
            string tmpHint = string.Empty;
            while (p > 0)
            {
                tmpHint += "+";
                p -= 1;
            }

            while (i > 0)
            {
                tmpHint += "-";
                i -= 1;
            }

            return tmpHint;
        }
    }
}
