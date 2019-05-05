using System.Collections;
using System.Collections.Generic;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// abstract class for all actions to happen in the level
    /// </summary>
    abstract class LevelActionController : MainBehavior {
        /// <summary>
        /// method to override whenever the action is on triggered
        /// </summary>
        /// <param name="from"></param>
        public abstract void OnAction(LevelTriggerController from);
        /// <summary>
        /// method to override whenever the action is off triggered
        /// </summary>
        /// <param name="from"></param>
        public abstract void OffAction(LevelTriggerController from);
    }
}