namespace System//HKH.Common
{
    public static class BooleanExtension
    {
        public static int ToInt(this bool b)
        {
            return b ? 1 : 0;
        }
    }
}
