using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// controller for things that can be picked up by the pick up skill
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class PickUpController : MainBehavior {
        /// <summary>
        /// the rigidbody
        /// </summary>
        public Rigidbody Rigidbody;
        /// <summary>
        /// the collider
        /// </summary>
        public Collider Collider;
        public override void OnStart(OnStartProperties onStartProperties) {
            base.OnStart(onStartProperties);
            //set the rigid body
            Rigidbody = GetComponent<Rigidbody>();
            //set the collider
            Collider = GetComponent<Collider>();
        }

        public override void OnUpdate(OnUpdateProperties onUpdateProperties) {
            base.OnUpdate(onUpdateProperties);

        }
    }
}
