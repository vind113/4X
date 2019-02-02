using Logic.Resource;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.TechnologyClasses {
    public class ResearchQueue {
        private const int MaximumSimultaneousResearches = 3;

        private List<TechnologyResearcher> technologiesBeingResearched = new List<TechnologyResearcher>();
        private List<TechnologyResearcher> researchQueue = new List<TechnologyResearcher>();

        public int BeingResearched { get => this.technologiesBeingResearched.Count; }
        public int WaitingInQueue { get => this.researchQueue.Count; }

        public void Add(TechnologyResearcher research) {
            if (research == null) {
                throw new ArgumentNullException(nameof(research));
            }

            if (IsInResearch(research)) {
                return;
            }

            research.ResearchCompleted += this.Research_ResearchCompleted;

            if (technologiesBeingResearched.Count < MaximumSimultaneousResearches) {
                technologiesBeingResearched.Add(research);
            }
            else {
                researchQueue.Add(research);
            }
        }

        private bool IsInResearch(TechnologyResearcher research) {
            return researchQueue.Contains(research) || technologiesBeingResearched.Contains(research);
        }

        private void Research_ResearchCompleted(object sender, ResearchCompletedEventArgs e) {
            if(sender is TechnologyResearcher research) {
                technologiesBeingResearched.Remove(research);
                research.ResearchCompleted -= this.Research_ResearchCompleted;

                if (researchQueue.Count > 0) {
                    technologiesBeingResearched.Add(researchQueue.First());
                    researchQueue.RemoveAt(0);
                }
            }
        }

        public void Remove(TechnologyResearcher research) {
            if (researchQueue.Contains(research)) {
                researchQueue.Remove(research);
            }
        }

        public void OneTurnProgress(IMutableResources from) {
            if (from == null) {
                throw new ArgumentNullException(nameof(from));
            }

            technologiesBeingResearched.ForEach((research) => research.OneTurnProgress(from));
        }
    }
}
