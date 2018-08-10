﻿using Logic.SpaceObjects;
using System;

namespace Logic.SpaceObjects {
    public class SpaceBuildingCompletedEventArgs : EventArgs {
        private Habitat habitat;

        public SpaceBuildingCompletedEventArgs(Habitat habitat) {
            this.habitat = habitat ?? throw new ArgumentNullException(nameof(habitat));
        }

        public Habitat Habitat { get => this.habitat; }
    }
}
