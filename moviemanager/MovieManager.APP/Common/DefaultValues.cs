using System;

namespace MovieManager.APP.Common
{
    class DefaultValues
    {
        public static readonly DateTime NULL_DATE = new DateTime(1900,1,1);
        public static readonly DateTime FILTER_START_DATE = DateTime.Now.AddYears(-2);
        public static readonly DateTime FILTER_END_DATE = DateTime.Now.AddYears(-10);
    }
}
