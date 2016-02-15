using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyncWPF
{
    interface IView
    {
        void OnStart();
        void OnProgress(int percentage);
        void OnEnd();
    }
}
