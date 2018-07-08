using System;
using Logic.Resourse;

namespace Logic.PlayerClasses {
    public class Stockpile {
        private double money = 0;               //деньги, доступные игроку
        private Resourses playerResourses;  //ресурсы на складе

        public Stockpile() {
            this.Money = 0;
            this.PlayerResourses = new Resourses(0, 0, 0);
        }

        public Stockpile(double money, Resourses playerResourses) {
            this.money = money;
            this.playerResourses = playerResourses ?? throw new ArgumentNullException(nameof(playerResourses));
        }

        public double Money {
            get => money;
            set => money = value;
        }

        public Resourses PlayerResourses {
            get => playerResourses;
            set {
                if (value.CommonMetals >= 0 &&
                    value.Hydrogen >= 0 &&
                    value.RareEarthElements >= 0) {

                    playerResourses = value;
                }
            }
        }
    }
}
