using Logic.Resourse;
using System;

namespace Logic.PlayerClasses {
    public class Ships {
        private readonly Resourses colonizerCost = new Resourses(1E10, 1E10, 1E9);

        public bool GetColonizer(Player player) {
            Resourses playerResourses = player.OwnedResourses;

            if (playerResourses == null) {
                throw new ArgumentNullException(nameof(Player.OwnedResourses));
            }

            if (playerResourses >= colonizerCost) {
                playerResourses.Substract(colonizerCost);
                return true;
            }

            return false;
        }
    }
}
