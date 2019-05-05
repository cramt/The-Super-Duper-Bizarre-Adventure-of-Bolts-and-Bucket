using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    class JointPushController : LevelActionController {
        private ConfigurableJoint joint;

        public override void OffAction(LevelTriggerController from) {
            joint.targetPosition = new Vector3(0, joint.targetPosition.y, joint.targetPosition.x);
        }

        public override void OnAction(LevelTriggerController from) {
            joint.targetPosition = new Vector3(3.7f, joint.targetPosition.y, joint.targetPosition.x);
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