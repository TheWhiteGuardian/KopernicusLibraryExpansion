namespace KLE
{
    class KLEMath
    {
        internal static float OneMinus(float inp) { return 1 - inp; }
        internal static float Remap(float input, float min1, float min2, float max1, float max2)
        {
            return min2 + (input - min1) * (max2 - min2) / (max1 - min1);
        }
    }
}
