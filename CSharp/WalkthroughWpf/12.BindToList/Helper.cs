using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using _11.DataBinding;

namespace _12.BindToList
{
    static class Helper
    {
        public static ICollectionView MakeTeamView(int number)
        {
            Team team = new Team();
            for (int index = 0; index < number; ++index)
                team.Add(new Person
                {
                    SSN = (uint)index,
                    Name = string.Format("Person{0}", index),
                    Age = 10 + index
                });
            return CollectionViewSource.GetDefaultView(team.Members);
        }

        public static void CircelMoveNext(this ICollectionView view)
        {
            view.MoveCurrentToNext();
            if (view.IsCurrentAfterLast)
                view.MoveCurrentToFirst();// circle
        }

        public static void CircleMovePrevious(this ICollectionView view)
        {
            view.MoveCurrentToPrevious();
            if (view.IsCurrentBeforeFirst)
                view.MoveCurrentToLast();
        }

        public static ObservableCollection<Family> MakeFamiles()
        {
            return new ObservableCollection<Family>
                            {
                                new Family
                                {
                                    Name = "Stooge",
                                    Members = new PersonCollection
                                                  {
                                                      new Person{Name = "Larry",Age = 21,
                                                          Traits =  new List<string>{"Mean","Beautiful"}},
                                                      new Person{Name = "Curly",Age = 38,
                                                          Traits = new List<string>{"Responsible","Fat"}},
                                                      new Person{Name = "Moe",Age = 16,
                                                          Traits =new List<string> {"Slender","Brave"}}
                                                  }
                                },
                                new Family
                                {
                                    Name = "Addams",
                                    Members = new PersonCollection
                                                  {
                                                      new Person{Name = "Phoebe",Age =36,
                                                          Traits = new List<string>{"Strange","Careful"}},
                                                      new Person{Name = "Ross",Age = 24,
                                                          Traits = new List<string>{"Stupid"}},
                                                      new Person{Name = "Rachel",Age = 31,
                                                          Traits =new List<string> {"Good Looking"}}
                                                  }
                                }
                            };
        }// MakeFamiles
    }// Helper
}
