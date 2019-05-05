using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// the level trigger for a pressure plate
    /// </summary>
    class PressurePlateController : LevelTriggerController {
        /// <summary>
        /// the name
        /// </summary>
        public override string Name { get; } = "Pressure Pate";
        /// <summary>
        /// when a trigger happens
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other) {
            //if no rigidbody, ignore
            if(other.transform.GetComponent<Rigidbody>() == null) {
                return;
            }
            //tell all the actions
            for (int i = 0; i < Actions.Length; i++) {
                Actions[i].OnAction(this);
            }
        }
        /// <summary>
        /// when a trigger unhappens
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other) {
            //if no rigidbody, ignore
            if (other.transform.GetComponent<Rigidbody>() == null) {
                return;
            }
            //tell all the actions
            for (int i = 0; i < Actions.Length; i++) {
                Actions[i].OffAction(this);
            }
        }
    }
}