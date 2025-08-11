namespace Mastermind.Model
{
    class Mask : PassCode
    {
        public Mask(Secret s, int l) : base(l, s) { }

        public void Hide(int i)
        {
            code[i] = 0;
        }

        public bool Has_Digit(Guess s, int i)
        {
            return Digit_Elsewhere(s, i);
        }
    }
}
