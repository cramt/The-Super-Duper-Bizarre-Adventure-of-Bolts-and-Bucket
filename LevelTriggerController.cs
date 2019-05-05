namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// abstract class for triggers of the level action controller
    /// </summary>
    abstract class LevelTriggerController : MainBehavior {
        public LevelActionController[] Actions;
        public abstract string Name { get; }
    }
}