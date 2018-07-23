using Logic.Resourse;
using System;

namespace Logic.GameClasses {
    public class StockpileChangedEventArgs : EventArgs {
        private double money = 0; 
        private Resourses argResourses;

        public StockpileChangedEventArgs(double money, Resourses argResourses) {
            this.money = money;
            this.argResourses = argResourses ?? throw new ArgumentNullException(nameof(argResourses));
        }

        public double Money { get => this.money; }
        public Resourses ArgResourses { get => this.argResourses; }
    }
}
