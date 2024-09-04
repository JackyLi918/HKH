namespace System//HKH.Common
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// parse datetime to timestamp (seconds)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long ToTimeStamp(this DateTime dt)
        {
            var dtStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
            return Convert.ToInt64((dt - dtStart).TotalSeconds);
        }
    }
}
