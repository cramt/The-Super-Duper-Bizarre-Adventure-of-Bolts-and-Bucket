using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// 100% like the JointBridgeController, but just with targetRotation instead of targetPosition
    /// </summary>
    class JointRampController : LevelActionController {
        public int Triggers = 1;
        private ConfigurableJoint joint;
        private List<LevelTriggerController> triggersOn = new List<LevelTriggerController>();
        public Quaternion on;
        public Quaternion off;
        public override void OffAction(LevelTriggerController from) {
            if (triggersOn.Contains(from)) {
                triggersOn.Remove(from);
            }
            if (Triggers > triggersOn.Count) {
                joint.targetRotation = off;
            }
        }

        public override void OnAction(LevelTriggerController from) {
            if (!triggersOn.Contains(from)) {
                triggersOn.Add(from);
            }
            if (Triggers <= triggersOn.Count) {
                joint.targetRotation = on;
            }
        }
        public override void OnStart(OnStartProperties onStartProperties) {
            base.OnStart(onStartProperties);
            joint = GetComponent<ConfigurableJoint>();
        }
        public override void OnUpdate(OnUpdateProperties onUpdateProperties) {
            base.OnUpdate(onUpdateProperties);

        }
    }
}