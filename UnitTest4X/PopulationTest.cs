using Logic.PopulationClasses;
using NUnit.Framework;

namespace UnitTest4X {
    [TestFixture]
    public class PopulationTest {
        [TestCase]
        public void Add_CorrectParameter_ValueIncreased() {
            long populationValue = 10_000_000_000;
            long maxPopualationValue = 20_000_000_000;
            long growthValue = 1_000_000_000;

            Population population = new Population(populationValue, maxPopualationValue);
            population.Add(growthValue);

            Assert.AreEqual(populationValue + growthValue, population.Value);
        }

        [TestCase]
        public void Add_ParameterLowerThanZero_ValueNotChanged() {
            long populationValue = 10_000_000_000;
            long maxPopualationValue = 20_000_000_000;
            long growthValue = -1_000_000_000;

            Population population = new Population(populationValue, maxPopualationValue);
            population.Add(growthValue);

            Assert.AreEqual(populationValue, population.Value);
        }

        [TestCase]
        public void Add_TrySetValueGreaterThanMaxValue_ValueNotChanged() {
            long populationValue = 10_000_000_000;
            long maxPopualationValue = 20_000_000_000;
            long growthValue = 15_000_000_000;

            Population population = new Population(populationValue, maxPopualationValue);
            population.Add(growthValue);

            Assert.AreEqual(populationValue, population.Value);
        }

        [TestCase]
        public void Subtract_CorrectParameter_ValueDecreased() {
            long populationValue = 10_000_000_000;
            long maxPopualationValue = 20_000_000_000;
            long decreaseValue = 5_000_000_000;

            Population population = new Population(populationValue, maxPopualationValue);
            population.Subtract(decreaseValue);

            Assert.AreEqual(populationValue - decreaseValue, population.Value);
        }

        [TestCase]
        public void Subtract_ParameterGreaterThanValue_ValueNotChanged() {
            long populationValue = 10_000_000_000;
            long maxPopualationValue = 20_000_000_000;
            long decreaseValue = 15_000_000_000;

            Population population = new Population(populationValue, maxPopualationValue);
            population.Subtract(decreaseValue);

            Assert.AreEqual(populationValue, population.Value);
        }

        [TestCase]
        public void Subtract_ParameterLowerThanZero_ValueNotChanged() {
            long populationValue = 10_000_000_000;
            long maxPopualationValue = 20_000_000_000;
            long decreaseValue = -5_000_000_000;

            Population population = new Population(populationValue, maxPopualationValue);
            population.Subtract(decreaseValue);

            Assert.AreEqual(populationValue, population.Value);
        }
    }
}
