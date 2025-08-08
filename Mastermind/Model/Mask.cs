namespace Mastermind.Model
{
    class Mask : PassCode
    {
        public Mask(int l, int[] c) : base(l) 
        {
            code = c.ToArray();
        }

        public void Hide(int i)
        {
            code[i] = 0;
        }

        public bool Has_Digit(PassCode p, int i)
        {
            return Digit_Elsewhere(p, i);
        }
    }
}
