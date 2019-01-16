namespace Logic.PopulationClasses {
    public class ReadOnlyPopualtion {
        private Population innerPopulation;

        public ReadOnlyPopualtion(Population population) {
            innerPopulation = population;
        }
    }
}
