using System;
using Logic.Resource;

namespace Logic.PlayerClasses {
    [Serializable]
    public class Stockpile {
        public double Money { get; set; } = 0;
        public IMutableResources PlayerResources { get; }

        public Stockpile() {
            this.Money = 111_222_333_444;
            this.PlayerResources = new Resources(0, 0, 0);
        }

        public Stockpile(double money, IMutableResources playerResources) {
            this.Money = money;
            this.PlayerResources = playerResources ?? throw new ArgumentNullException(nameof(playerResources));
        }
    }
}
