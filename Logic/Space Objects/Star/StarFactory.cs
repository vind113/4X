using Logic.SupportClasses;

namespace Logic.SpaceObjects {
    public static class StarFactory {
        /// <summary>
        ///     Сгенерировать звезду с заданым именем
        /// </summary>
        /// <param name="name">
        ///     Имя звезды
        /// </param>
        /// <returns>
        ///     Возвращает экземпляр класса <see cref="Star"/>
        /// </returns>
        public static Star GenerateStar(string name) {
            int radius = 0;
            LuminosityClass luminosityClass;

            double starFraction = HelperRandomFunctions.GetRandomDouble();

            if (starFraction < 0.0003) {
                radius = HelperRandomFunctions.GetRandomInt(4_620_000, 10_000_000);
                luminosityClass = LuminosityClass.O;

            }
            else if (starFraction < 0.0013) {
                radius = HelperRandomFunctions.GetRandomInt(1_260_000, 4_620_000);
                luminosityClass = LuminosityClass.B;

            }
            else if (starFraction < 0.006) {
                radius = HelperRandomFunctions.GetRandomInt(805_000, 1_260_000);
                luminosityClass = LuminosityClass.A;

            }
            else if (starFraction < 0.03) {
                radius = HelperRandomFunctions.GetRandomInt(728_000, 805_000);
                luminosityClass = LuminosityClass.F;

            }
            //реальное соотношение 0.076
            else if (starFraction < 0.3) {
                radius = HelperRandomFunctions.GetRandomInt(672_000, 728_000);
                luminosityClass = LuminosityClass.G;

            }
            //реальное соотношение 0.12
            else if (starFraction < 0.4) {
                radius = HelperRandomFunctions.GetRandomInt(490_000, 672_000);
                luminosityClass = LuminosityClass.K;

            }
            else {
                radius = HelperRandomFunctions.GetRandomInt(360_000, 490_000);
                luminosityClass = LuminosityClass.M;
            }

            return new Star(name, radius, luminosityClass);
        }
    }
}
