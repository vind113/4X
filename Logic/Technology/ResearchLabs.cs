namespace Logic.TechnologyClasses {
    public class ResearchLabs {
        private ResearchQueue researchQueue = new ResearchQueue();

        public void Add(TechnologyResearcher research) {
            researchQueue.Add(research);
        }

        public void Remove(TechnologyResearcher research) {
            researchQueue.Remove(research);
        }
    }
}
