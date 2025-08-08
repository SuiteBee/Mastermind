namespace Mastermind.Assets
{
    /// <summary>
    /// Keeps the game within its given bounds
    /// </summary>
    public class Rules
    {
        public int Length
        {
            get{ return Digits; }
        }

        public string[] Text
        {
            get{ return Parameters; }  
        }

        private readonly int Attempts;
        private readonly int Digits;
        private static int RangeLower;
        private static int RangeUpper;
        private readonly string[] Parameters = new string[7];

        /// <summary>
        /// Initialize rule set
        /// </summary>
        /// <param name="n">Number of allowed attempts</param>
        /// <param name="d">Number of digits in secret code</param>
        /// <param name="lower">Lower value limit allowed for integers in secret</param>
        /// <param name="upper">Upper value limit allowed for integers in secret</param>
        public Rules(int n, int d, int lower, int upper)
        {
            Attempts = n;
            Digits = d;
            RangeLower = lower;
            RangeUpper = upper;

            Parameters = [
                $"Welcome! You have {Attempts} guesses to determine my secret {Digits} digit passcode",
                "These are the rules",
                $"1.) The numbers can be any random integers with range {RangeLower}-{RangeUpper}",
                "2.) Each guess will result in a hint being provided",
                "3.) A number with the correct value and CORRECT position will be awarded a (+)",
                "4.) A number with the correct value and INCORRECT position will be awarded a (-)",
                "Note: The hint does not specify which digits are correct"
            ];
        }

        /// <summary>
        /// Generate random integer within rules range
        /// </summary>
        /// <returns>pseudorandom integer</returns>
        public static int Random()
        {
            return new Random().Next(RangeLower, RangeUpper + 1);
        }

        /// <summary>
        /// Parse key press character and set value
        /// </summary>
        /// <param name="keyChar">Pressed key</param>
        /// <param name="value">external value</param>
        /// <returns>valid</returns>
        public static bool TryInput(char keyChar, out int value)
        {
            double num = char.GetNumericValue(keyChar);
            bool isNumeric = num > 0;
            value = (int)num;

            return isNumeric && num >= RangeLower && num <= RangeUpper;
        }
    }
}
