using Mastermind.IO;
using Mastermind.Model;

namespace Mastermind.State
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
        private readonly Secret Objective;

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
            Objective = new Secret(d);
            Remaining = n;

            Win = false;
            GameOver = false;
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
        public Guess Begin_Round()
        {
            var status = Get_Status();
            var attempt = UserInterface.Read_Chars(status, Objective.Length);

            Update_State(attempt);
            return attempt;
        }

        /// <summary>
        /// Check game state and end session or generate hint
        /// </summary>
        /// <param name="attempt">formatted user input</param>
        public void End_Round(Guess attempt)
        {
            if(Win || GameOver)
            {
                End_Session();
            }
            else
            {
                Hint = Objective.Check(attempt);
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
        private void Update_State(Guess attempt)
        {
            Win = Objective.Equals(attempt);

            Remaining -= 1;
            if (Remaining == 0)
            {
                GameOver = true;
            }
        }
    }
}
