using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Logic.GameClasses;
using Logic.Resource;
using Logic.SpaceObjects;

namespace Logic.PlayerClasses
{
    public interface IPlayer
    {
        string Name { get; set; }
        double Money { get; }

        ReadOnlyObservableCollection<StarSystem> StarSystems { get; }
        Stockpile Stockpile { get; }
        Resources OwnedResources { get; set; }
        CitizenHub Hub { get; }
        Ships Ships { get; }

        long TotalPopulation { get; }
        double PopulationGrowthFactor { get; }

        int StarSystemsCount { get; }
        int OwnedStars { get; }
        int OwnedPlanets { get; }
        int ColonizedPlanets { get; }

        event EventHandler<PopulationChangedEventArgs> PopulationChanged;
        event PropertyChangedEventHandler PropertyChanged;
        event EventHandler<StockpileChangedEventArgs> StockpileChanged;

        void AddStarSystem(StarSystem system);
        void RemoveStarSystem(StarSystem system);

        void AddToColonizationQueue(Planet planet);
        void NextTurn(bool isAutoColonizationEnabled, bool isDiscoveringNewStarSystems);
        void TryToColonizeQueue();
    }
}