using System;
using Logic.Resource;

namespace Logic.Buildings {
    [Serializable]
    public class RingWorldBuilder : Builder {
        public RingWorldBuilder(string ringWorldName) :
            base(1200, new ReadOnlyResources(1E18, 1E19, 1E15), new Habitat(ringWorldName)) {

        }
    }
}
