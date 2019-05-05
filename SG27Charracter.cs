using System;
using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// controller for the small guy
    /// </summary>
    public class SG27Charracter : PlayerCharacterController {
        /// <summary>
        /// inherited function for start
        /// </summary>
        /// <param name="onStartProperties"></param>
        public override void OnStart(OnStartProperties onStartProperties) {
            base.OnStart(onStartProperties);
            //set hte speed
            speed = 300;
            //set the skills
            Skills = new Type[] {
                typeof(LineWalkSkill),
                typeof(JumpSkill),
                typeof(PickUpSkill)
            };
            //define how movement works
            HowToMovement = dir => {
                //set the animtor
                Animator.SetBool("isMoving", dir != Vector3.zero);
                //set the rotation to match the way we're walking
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, dir * -1, Time.deltaTime * 10f, 0.0f));
                //add force in set direction (and less if we're in the air)
                Rigid.AddForce(dir * (CheckInAir() ? 0.1f : 1f));
            };
        }

        public override void OnUpdate(OnUpdateProperties onUpdateProperties) {
            base.OnUpdate(onUpdateProperties);
            //tell the animtor if we're in the air
            bool inAir = CheckInAir();
            Animator.SetBool("isGrounded", !inAir);
        }
    }
}
