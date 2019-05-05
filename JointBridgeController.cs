using System.Collections.Generic;
using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// inheritor of the level action controller
    /// it changes the target position of a joint when activated
    /// </summary>
    class JointBridgeController : LevelActionController {
        /// <summary>
        /// the amount of activators
        /// </summary>
        public int Triggers = 1;
        /// <summary>
        /// the joint ot change
        /// </summary>
        private ConfigurableJoint joint;
        /// <summary>
        /// the amount of current triggers
        /// </summary>
        private List<LevelTriggerController> triggersOn = new List<LevelTriggerController>();
        /// <summary>
        /// when we get an off trigger
        /// </summary>
        /// <param name="from">the trigger</param>
        public override void OffAction(LevelTriggerController from) {
            //remove it from the list if its in the list
            if (triggersOn.Contains(from)) {
                triggersOn.Remove(from);
            }
            //if its less than the trigger amount, go back to the original state
            if (Triggers > triggersOn.Count) {
                joint.targetPosition = new Vector3(-3, joint.targetPosition.y, joint.targetPosition.z);
            }
        }
        /// <summary>
        /// when we get an on trigger
        /// </summary>
        /// <param name="from">the trigger</param>
        public override void OnAction(LevelTriggerController from) {
            //if it isnt in the list, add it
            if (!triggersOn.Contains(from)) {
                triggersOn.Add(from);
            }
            //if there is enough triggers, go to the other state
            if(Triggers <= triggersOn.Count) {
                joint.targetPosition = new Vector3(3, joint.targetPosition.y, joint.targetPosition.z);
            }
        }
        /// <summary>
        /// inherited start function
        /// </summary>
        /// <param name="onStartProperties"></param>
        public override void OnStart(OnStartProperties onStartProperties) {
            base.OnStart(onStartProperties);
            //get the joint 
            joint = GetComponent<ConfigurableJoint>();
        }
        public override void OnUpdate(OnUpdateProperties onUpdateProperties) {
            base.OnUpdate(onUpdateProperties);
            
        }
    }
}