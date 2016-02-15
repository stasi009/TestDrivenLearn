using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _11.DataBinding;

namespace _12.BindToList
{
    sealed class Family
    {
        public string Name { get; set; }

        public PersonCollection Members{ get; set;}
    }
}
