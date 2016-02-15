using System;
using System.Text;

namespace CSharpBasicTest.Dispose
{
    /// <summary>
    /// If a target is referenced onlyby one or more weak references, 
    /// the GC will consider the target eligible for collection
    /// </summary>
    public sealed class TestWeakReference
    {
        public static void TestMain()
        {
            var weak = new WeakReference(new StringBuilder("weak"));
            Console.WriteLine(weak.Target); // weak
            GC.Collect();
            Console.WriteLine(weak.Target); // (nothing)
            Console.WriteLine(weak.IsAlive);
        }
    }
}
