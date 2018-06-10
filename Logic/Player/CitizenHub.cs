namespace Logic.PlayerClasses {
    public class CitizenHub {
        double citizensInHub = 0;
        double maximumCount = 1E8;

        public double CitizensInHub {
            get => citizensInHub;
            set {
                if (value <= maximumCount && value >= 0) {
                    citizensInHub = value;
                }
            }
        }

        public double MaximumCount {
            get => maximumCount;
            set {
                if (value >= 0) {
                    maximumCount = value;
                }
            }
        }
    }
}
