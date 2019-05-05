using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// controller for a line
    /// the line also uses a joint between the 2 end to make sure they stay where they should be
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class LineController : MainBehavior {
        //both positions 
        public Transform Position1;
        public Transform Position2;
        //both rigid bodies
        public Rigidbody Rigidbody1;
        public Rigidbody Rigidbody2;
        //the line renderer
        [HideInInspector]
        public LineRenderer line;
        //the joint
        [HideInInspector]
        public ConfigurableJoint Joint;

        public override void OnStart(OnStartProperties onStartProperties) {
            base.OnStart(onStartProperties);
            //create joint
            Joint = Rigidbody1.gameObject.AddComponent<ConfigurableJoint>();
            //set the other part of the line to the joint
            Joint.connectedBody = Rigidbody2;
            //set the anchor
            Joint.anchor = Vector3.zero;
            //do all the configuration
            Joint.xMotion = ConfigurableJointMotion.Limited;
            Joint.yMotion = ConfigurableJointMotion.Limited;
            Joint.zMotion = ConfigurableJointMotion.Limited;
            Joint.autoConfigureConnectedAnchor = false;
            Joint.configuredInWorldSpace = true;
            //define the limit
            SoftJointLimit LinearLimit = Joint.linearLimit;
            LinearLimit.limit = Vector3.Distance(Rigidbody1.position, Rigidbody2.position) + 0.1f;
            Joint.linearLimit = LinearLimit;
            //define the spring
            var spring = Joint.linearLimitSpring;
            spring.spring = 10000f;
            Joint.linearLimitSpring = spring;
            //allow collision
            Joint.enableCollision = true;
        }

        public void Delete() {
            Destroy(Joint);
            Destroy(gameObject);
        }

        public override void OnUpdate(OnUpdateProperties onUpdateProperties) {
            base.OnUpdate(onUpdateProperties);
            //set the connected anchor to match the difference between the position and the rigidbody
            Joint.connectedAnchor = Position2.position - Rigidbody2.position;
            //if both positions are fine, set the linerendere
            if (Position1 != null && Position2 != null) {
                GetComponent<LineRenderer>().SetPositions(new Vector3[] { Position1.position, Position2.position });
            }
        }
    }
}