using Logic.Resourse;

namespace Logic.Player {
    internal static class Stockpile {
        static double money = 0;               //деньги, доступные игроку
        static Resourses playerResourses;  //ресурсы на складе

        public static double Money { get => money; set => money = value; }
        public static Resourses PlayerResourses { get => playerResourses; set => playerResourses = value; }
    }
}
