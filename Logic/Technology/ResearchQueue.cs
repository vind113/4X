using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.TechnologyClasses {
    public class ResearchQueue {
        private const int MaximumSimultaneousResearches = 3;

        private List<TechnologyResearch> technologiesBeingResearched = new List<TechnologyResearch>();
        private Queue<TechnologyResearch> researchQueue = new Queue<TechnologyResearch>();

        public void Add(TechnologyResearch research) {
            if (IsNotInResearch(research)) {
                researchQueue.Enqueue(research);
            }

            //Не будет работать как задумано
            if(technologiesBeingResearched.Count < MaximumSimultaneousResearches) {
                technologiesBeingResearched.Add(researchQueue.Dequeue());
            }
        }

        private bool IsNotInResearch(TechnologyResearch research) {
            return (!researchQueue.Contains(research)) && (!technologiesBeingResearched.Contains(research));
        }
    }
}
