namespace System//HKH.Common
{
    public static class NumericExtension
    {
        /// <summary>
        /// Parse json ticks to local datetime
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long ticks)
        {
            var dtStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
            return dtStart.AddTicks(ticks * 10000000);
        }
    }
}
