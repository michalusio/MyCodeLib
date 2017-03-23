namespace Useful.Plotting
{
    public class Range
    {
        public float Diff;
        public float Max;
        public float Min;

        public Range(float min, float max, float d)
        {
            Min = min;
            Max = max;
            Diff = d;
        }
    }
}