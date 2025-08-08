namespace Mastermind.Model
{
    public abstract class PassCode
    {
        public int Length
        {
            get { return length; }
        }

        protected int length;
        protected int[] code;

        public PassCode(int l)
        {
            length = l;
            code = new int[l];
        }

        public bool Equals(PassCode p)
        {
            return code.SequenceEqual(p.code);
        }

        protected bool Equals_Digit(PassCode p, int index)
        {
            return code[index] == p.code[index];
        }

        protected bool Digit_Elsewhere(PassCode p, int i)
        {
            int value = p.code[i];
            return code.Contains(value);
        }
    }
}
