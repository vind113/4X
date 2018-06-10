using Logic.Resourse;

namespace Logic.PlayerClasses {
    public class Stockpile {
        private double money = 0;               //деньги, доступные игроку
        private Resourses playerResourses;  //ресурсы на складе

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
