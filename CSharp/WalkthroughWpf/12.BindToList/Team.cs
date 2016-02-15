using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _11.DataBinding;

namespace _12.BindToList
{
    sealed class Team
    {
        private readonly IList<Person> m_members;

        public Team()
        {
            m_members = new List<Person>();
        }

        public void Add(Person person)
        {
            m_members.Add(person);
        }

        public IEnumerable<Person> Members
        {
            get { return m_members; }
        }
    }
}
