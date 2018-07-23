using Logic.PlayerClasses;
using System;

namespace Logic.GameClasses {
    public class CitizenHubChangedEventArgs : EventArgs {
        double citizensInHub;
        double maximumCitizenNumber;

        public CitizenHubChangedEventArgs(CitizenHub changedHub) {
            this.citizensInHub = changedHub.CitizensInHub;
            this.maximumCitizenNumber = changedHub.MaximumCount;
        }

        public double CitizensInHub { get => this.citizensInHub; }
        public double MaximumCitizenNumber { get => this.maximumCitizenNumber; }
    }
}
