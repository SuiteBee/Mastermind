using Mastermind.State;

namespace Mastermind.Model
{
    class Secret: PassCode
    {
        public Secret(int l) : base(l) 
        {
            Generate();
        }

        private void Generate()
        {
            for (int i = 0; i < code.Length; i++)
            {
                code[i] = Rules.RandomDigit();
            }
        }

        /// <summary>
        /// Compare a passcode to secret and return a hint
        /// </summary>
        /// <param name="attempt"></param>
        public string Check(Guess attempt)
        {
            string hint = string.Empty;
            var mask = new Mask(length, code);

            for (int i = 0; i < attempt.Length; i++)
            {
                //Perfect Match: Correct digit in correct position
                if (Equals_Digit(attempt, i))
                {
                    mask.Hide(i);
                    hint = "+" + hint;
                }
                //Imperfect Match: Correct digit in incorrect position
                else if (mask.Has_Digit(attempt, i))
                {
                    hint += "-";
                }
            }

            return hint;
        }
    }
}
