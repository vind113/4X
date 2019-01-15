using System;
using Logic.Resource;

namespace Logic.PlayerClasses {
    [Serializable]
    public class Stockpile {
        private double money = 0;            //деньги, доступные игроку
        private IMutableResources playerResources;  //ресурсы на складе

        public Stockpile() {
            this.Money = 111_222_333_444;
            this.PlayerResources = new Resources(0, 0, 0);
        }

        public Stockpile(double money, IMutableResources playerResources) {
            this.money = money;
            this.playerResources = playerResources ?? throw new ArgumentNullException(nameof(playerResources));
        }

        public double Money {
            get => money;
            set => money = value;
        }

        public IMutableResources PlayerResources {
            get => playerResources;
            set {
                if (value.CommonMetals >= 0 &&
                    value.Hydrogen >= 0 &&
                    value.RareEarthElements >= 0) {

                    playerResources = value;
                }
            }
        }
    }
}
