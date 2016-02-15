

using _11.DataBinding;

namespace Dialogs
{
    static class Helper
    {
        public static void CopyFrom(this Person dest, Person src)
        {
            dest.Name = src.Name;
            dest.Age = src.Age;
        }
    }
}
