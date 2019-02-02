using System;
using Logic.Resource;

namespace Logic.TechnologyClasses {
    public class TechnologyResearcher {
        public int ResearchProgress { get; private set; }
        public int ResearchDuration { get; }
        public IComparableResources CostPerTurn { get; }

        private readonly Technology technologyBeingResearched;

        public event EventHandler<ResearchCompletedEventArgs> ResearchCompleted;

        public TechnologyResearcher(Technology tecnology, IBasicResources costPerTurn, int researchDuration) {
            if (costPerTurn == null) {
                throw new ArgumentNullException(nameof(costPerTurn));
            }

            if(tecnology == null) {
                throw new ArgumentNullException(nameof(tecnology));
            }

            this.technologyBeingResearched = tecnology; 
            this.CostPerTurn = new ReadOnlyResources(costPerTurn);
            this.ResearchDuration = researchDuration;
        }

        public void OneTurnProgress(IMutableResources from) {
            if (from == null) {
                throw new ArgumentNullException(nameof(from));
            }

            if (from.CanSubtract(this.CostPerTurn)) {
                from.Subtract(this.CostPerTurn);
                this.ResearchProgress++;
            }

            if(this.ResearchProgress == this.ResearchDuration) {
                OnResearchCompleted();
            }
        }

        private void OnResearchCompleted() {
            ResearchCompleted?.Invoke(this, new ResearchCompletedEventArgs());
        }
    }
}
