namespace Logic.Player {
    internal static class CitizenHub {
        static double citizensInHub = 0;
        static double maximumCount;

        public static double CitizensInHub { get => citizensInHub; set => citizensInHub = value; }
        public static double MaximumCount { get => maximumCount; set => maximumCount = value; }
    }
}
