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
        IMutableResources OwnedResources { get; }
        CitizenHub Hub { get; }
        IShipsFactory Ships { get; }

        long Population { get; }

        int StarSystemsCount { get; }
        int OwnedStars { get; }
        int OwnedPlanets { get; }
        int ColonizedPlanets { get; }

        event PropertyChangedEventHandler PropertyChanged;
        event EventHandler<StockpileChangedEventArgs> StockpileChanged;

        void AddToColonizationQueue(HabitablePlanet planet);
        void NextTurn();
        void TryToColonizeQueue();
    }
}