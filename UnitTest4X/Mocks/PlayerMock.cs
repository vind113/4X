using Logic.GameClasses;
using Logic.PlayerClasses;
using Logic.Resourse;
using Logic.SpaceObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest4X.Mocks
{
    class PlayerMock : IPlayer
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public double Money => throw new NotImplementedException();

        public ReadOnlyObservableCollection<StarSystem> StarSystems => throw new NotImplementedException();

        public Stockpile Stockpile => throw new NotImplementedException();

        public Resourses OwnedResourses { get; set; }

        public CitizenHub Hub => throw new NotImplementedException();

        public Ships Ships => throw new NotImplementedException();

        public long TotalPopulation { get; set; }

        public double PopulationGrowthFactor => throw new NotImplementedException();

        public int StarSystemsCount => throw new NotImplementedException();

        public int OwnedStars => throw new NotImplementedException();

        public int OwnedPlanets => throw new NotImplementedException();

        public int ColonizedPlanets => throw new NotImplementedException();

        public event EventHandler<PopulationChangedEventArgs> PopulationChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<StockpileChangedEventArgs> StockpileChanged;

        public void AddStarSystem(StarSystem system)
        {
            throw new NotImplementedException();
        }

        public void AddToColonizationQueue(Planet planet)
        {
            throw new NotImplementedException();
        }

        public void NextTurn(bool isAutoColonizationEnabled, bool isDiscoveringNewStarSystems)
        {
            throw new NotImplementedException();
        }

        public void RemoveStarSystem(StarSystem system)
        {
            throw new NotImplementedException();
        }

        public void TryToColonizeQueue()
        {
            throw new NotImplementedException();
        }
    }
}
