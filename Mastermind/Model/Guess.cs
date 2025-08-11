namespace Mastermind.Model
{
    public class Guess : PassCode
    {
        public Guess(int l) : base(l) { }

        public void SetDigit(int i, int value)
        {
            if(i < length && i >= 0)
            {
                code[i] = value;
            }
        }

        public void Print()
        {
            foreach(int i in code)
            {
                Console.Write(i);
            }
        }
    }
}
