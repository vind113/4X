using Logic.Resource;
using System;

namespace Logic.Buildings {
    [Serializable]
    public class HabitatBuilder : Builder {
        public HabitatBuilder(string habitatName) :
            base(24, new ReadOnlyResources(10E9, 100E9, 100E6), new Habitat(habitatName)) {

        }
    }
}
