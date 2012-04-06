using System;

namespace Common.SettingsStorage
{
    public class DuplicateSettingsKeyException: Exception
    {
        private readonly string _tagName;

        public DuplicateSettingsKeyException(string tagName)
        {
            _tagName = tagName;
        }

        public override string Message
        {
            get { return "The following tag is already in use: " + _tagName; }
        }
    }
}
