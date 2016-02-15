using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace _11.DataBinding
{
    public sealed class Person : INotifyPropertyChanged
    {
        #region "implement INotifyPropertyChanged"
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region "member variables"

        private uint m_ssn;
        private string m_name;
        private int m_age;

        #endregion

        #region "properties"

        public uint SSN
        {
            get { return m_ssn; }
            set
            {
                if (m_ssn != value)
                {
                    m_ssn = value;
                    NotifyChange("SSN");
                }
            }
        }

        public string Name
        {
            get { return m_name; }
            set
            {
                // note: call 'Equals' on 'value', other than 'm_name'
                // in case 'm_name' is null the first time
                // if (m_name == null || !value.Equals(m_name))
                // XXX if (!m_name.Equals(value))
                if (!value.Equals(m_name))
                {
                    m_name = value;
                    NotifyChange("Name");
                }
            }
        }

        public int Age
        {
            get { return m_age; }
            set
            {
                if (m_age != value)
                {
                    m_age = value;
                    NotifyChange("Age");
                }
            }
        }

        public IList<string> Traits { get; set; }

        #endregion

        #region "override methods"

        // note: this method can be a special method to give content to show on the screen
        // the string returned can work as content of the content
        public override string ToString()
        {
            return string.Format("{0}:{1}", m_ssn, m_name);
        }

        #endregion

        #region "helper method"

        private void NotifyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
