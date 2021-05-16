using System;

namespace Lalena.UI
{
    public static class DoubleExtensions
    {
        public static string Round(this double number, int decimals) 
            => Math.Round(number, decimals, MidpointRounding.AwayFromZero).ToString("0.######");
    }
}