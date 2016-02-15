using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVM.SKI
{
    internal interface IDAL
    {
        IEnumerable<Event> GetEvents();
        IEnumerable<Competitor> GetCompetitors(int eventId);
        void SaveCompetitor(Competitor competitor);
    }

    sealed class MockMemoryDAL : IDAL
    {
        private readonly Event[] m_events;
        private readonly Competitor[] m_competitors;

        public MockMemoryDAL()
        {
            m_events = new Event[]
                           {
                               new Event{EventId = 1,Name = "Male",Time = new DateTime(2011,9,15)},
                               new Event{EventId = 2,Name = "Female",Time = new DateTime(2011,8,4)}
                           };
            m_competitors = new Competitor[]
                                {
                                    new Competitor {EventId = 1, CompetitorId = 1, Name = "Tom", YearOfBirth = 1981},
                                    new Competitor {EventId = 1, CompetitorId = 2, Name = "Dick", YearOfBirth = 1982},
                                    new Competitor {EventId = 2, CompetitorId = 3, Name = "Mary", YearOfBirth = 1983},
                                    new Competitor {EventId = 2, CompetitorId = 4, Name = "Alice", YearOfBirth = 1984},
                                };
        }

        public IEnumerable<Event> GetEvents()
        {
            return m_events;
        }

        public IEnumerable<Competitor> GetCompetitors(int eventId)
        {
            return from competitor in m_competitors
                   where competitor.EventId == eventId
                   select competitor;
        }

        public void SaveCompetitor(Competitor competitor)
        {
        }
    }
}
