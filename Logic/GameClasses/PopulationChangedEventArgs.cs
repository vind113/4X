using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.GameClasses {
    public class PopulationChangedEventArgs : EventArgs {
        private double population;

        public PopulationChangedEventArgs(double population) {
            if (population >= 0) {
                this.population = population;
            }
        }

        public double Population { get => this.population; }
    }
}
