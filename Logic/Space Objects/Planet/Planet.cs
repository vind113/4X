using System;
using Logic.SupportClasses;
using Logic.PlayerClasses;
using Logic.Resource;
using Logic.PopulationClasses;
using Logic.SpaceObjects.PlanetClasses;

namespace Logic.SpaceObjects {
    /// <summary>
    /// Представляет планету
    /// </summary>
    [Serializable]
    public class Planet : CelestialBody, IPlanet {
        /// <summary>
        /// Инициализирует экземпляр класса планеты
        /// </summary>
        /// <param name="name">
        ///     Имя планеты
        /// </param>
        /// <param name="radius">
        ///     Радуис планеты
        /// </param>
        /// <param name="type">
        ///     Тип планеты
        /// </param>
        /// <param name="population">
        ///     Изначальное население планеты
        /// </param>
        public Planet(string name, double radius, PlanetType type):base(name, radius) {
            if (radius < 2000) {
                throw new ArgumentOutOfRangeException(nameof(radius), "Cannot be lower than 2000");
            }

            this.Type = type;
            this.BodyResource = new PlanetResourceGenerator().GenerateFor(this);
        }

        /// Возвращает тип планеты
        /// </summary>
        public PlanetType Type { get; }

        public override string ToString() {
            return $"{this.Name} is a {this.Type.Name} world with radius of {this.Radius} km " +
                $"and area of {this.Area:E4} km^2. ";
        }
    
        #region Next turn methods
        /// <summary>
        ///     Выполняет все операции для перехода на следующий ход
        /// </summary>
        /// <param name="player">
        ///     Игрок, которому принадлежит планета
        /// </param>
        public virtual void NextTurn(Player player) {
            
        }
        #endregion
    }
}
