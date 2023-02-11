namespace HKH.CurrencyFormat
{
    public static class CNYExtensions
    {
        /// <summary>
        /// to CNY Lower
        /// </summary>
        /// <param name="strCNY"></param>
        /// <returns></returns>
        public static double ToCNYLower(this string strCNY)
        {
            return new CNYFormatter().Format(strCNY);
        }

        /// <summary>
        /// to CNY Upper
        /// </summary>
        /// <param name="dblCNY"></param>
        /// <returns></returns>
        public static string ToCNYUpper(this double dblCNY)
        {
            return new CNYFormatter().Format(dblCNY);
        }
    }
}
