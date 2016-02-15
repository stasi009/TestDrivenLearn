using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace PlayRx
{
    static class TestGroup
    {
        private static IEnumerable<string> GetConsoleInput()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (input.Equals("exit"))
                    break;
                else
                    yield return input;
            }
        }

        private static void CheckGroup()
        {
            IObservable<string> source = GetConsoleInput().ToObservable();

            IObservable<IGroupedObservable<int, string>> group = from msg in source
                                                                 group msg by msg.Length;

            // single thread and "OnNext" is always serialized
            // so there is no need to synchronize "numGroup"
            int numGroup = 0;
            group.Subscribe(grp =>
                                {
                                    int index = ++numGroup;
                                    Console.WriteLine("{0}-th Group with Key<{1}> Generated", index, grp.Key);

                                    int numMsgs = 0;
                                    grp.Subscribe(msg =>
                                                      {
                                                          ++numMsgs;
                                                          Console.WriteLine("\tkey<{0}>'s {1}-th: '{2}'", grp.Key, numMsgs, msg);
                                                      },
                                                  () => Console.WriteLine("\tkey<{0}> totally has {1} numbers", grp.Key, numMsgs));
                                },
                                () => Console.WriteLine("!!! all groups finished."));
        }

        public static void TestMain()
        {
            CheckGroup();
        }
    }
}
