using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MovieManager.APP.Common
{
    class ProgressBarInfo : INotifyPropertyChanged
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                PropChanged("Message");
            }
        }

        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                PropChanged("Value");
            }
        }

        private int _maximum = 100;
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                PropChanged("Maximum");
            }
        }

        private bool _isIndeterminate;
        public bool IsIndeterminate
        {
            get { return _isIndeterminate; }
            set
            {
                _isIndeterminate = value;
                PropChanged("IsIndeterminate");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void PropChanged(string title)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(title));
            }
        }
    }
}
