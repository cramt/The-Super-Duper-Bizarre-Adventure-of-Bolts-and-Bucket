using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    class PushPlatformController : LevelActionController {
        public float Length;
        public float Force;
        public Vector3 Direction;
        private Vector3 ogPosition;
        private bool inActions = false;
        private Rigidbody rigid;

        public override void OffAction(LevelTriggerController from) {
            
        }

        public override void OnAction(LevelTriggerController from) {
            if (inActions) {
                return;
            }
            inActions = true;
            rigid.AddForce(Direction * Force);
        }
        public override void OnStart(OnStartProperties onStartProperties) {
            base.OnStart(onStartProperties);
            ogPosition = transform.position;
            rigid = GetComponent<Rigidbody>();
        }
        public override void OnUpdate(OnUpdateProperties onUpdateProperties) {
            base.OnUpdate(onUpdateProperties);
            if (Vector3.Distance(ogPosition, transform.position) > Length) {
                rigid.velocity = Vector3.zero;
                transform.position = ogPosition;
                inActions = false;
            }
            else if(inActions) {
                rigid.AddForce(Direction * Force);
            }
        }
    }
}