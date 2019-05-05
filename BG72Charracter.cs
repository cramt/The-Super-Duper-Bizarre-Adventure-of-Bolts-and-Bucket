using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// the rolling character
    /// it inherits from the player character controller
    /// </summary>
    public class BG72Charracter : PlayerCharacterController {
        /// <summary>
        /// the main way that the character converts a vector3 into movement
        /// </summary>
        private Action<Vector3> MainMovement;

        /// <summary>
        /// override for when shit starts 
        /// </summary>
        /// <param name="onStartProperties"></param>
        public override void OnStart(OnStartProperties onStartProperties) {
            base.OnStart(onStartProperties);
            //override the inherited list of skills
            Skills = new Type[] {
                typeof(HookShotSkill),
            };
            //define the main way of movement
            MainMovement = dir => {
                float temp = dir.z;
                dir.z = dir.x * -1;
                dir.x = temp;
                Rigid.AddTorque(dir * 3f);
                Rigid.AddForce(dir * 0.05f);
            };
            //override the inheritet main way of movement
            HowToMovement = MainMovement;
            //remove all constraints from the rigidbody
            Rigid.constraints = RigidbodyConstraints.None;
        }
        /// <summary>
        /// is standing still
        /// </summary>
        private bool still = true;
        /// <summary>
        /// override the inherited onupdate function
        /// </summary>
        /// <param name="onUpdateProperties"></param>
        public override void OnUpdate(OnUpdateProperties onUpdateProperties) {
            base.OnUpdate(onUpdateProperties);
            //tell the animator if we're standing still
            Animator.SetBool("still", still);
            //are we still in this frame
            bool newStill = Rigid.angularVelocity == Vector3.zero;
            //are we still in this frame but not in the last
            if (newStill && !still) {
                //here we're about to do the upright animation again

                //push upwards
                Rigid.AddForce(Vector3.up * 16000);
                //make sure the user cant move while this happens
                HowToMovement = new Action<Vector3>(dir => { });
                //define out current rotation, since we have to get it when we're in main thread
                Quaternion _from = transform.rotation;
                //in the next frame
                InvokeFunction(() => {
                    //define out y rotation
                    float y = transform.eulerAngles.y;
                    //start new thread
                    Task.Factory.StartNew(async () => {
                        //define the rotation we're going from and to
                        Quaternion from = _from;
                        //dont change the y, since that would be weird
                        Quaternion to = Quaternion.Euler(0, y, 0);
                        //create a while loop to do a Quaternion.Slerp, also we need to keep track of time
                        bool j = true;
                        float time = 0f;
                        while (j) {
                            //do this on main thread and wait for it to be finished
                            await InvokeFunction(() => {
                                //do the smooth rotation
                                transform.rotation = Quaternion.Slerp(from, to, time);
                                //add to time
                                time += Time.deltaTime;
                                //if our current rotation is that which we want, set j to false
                                j = transform.rotation != to;
                            });
                        }
                    });
                });
                //in one second redefine movement so we can do it again
                Task.Factory.StartNew(() => {
                    Thread.Sleep(1000);
                    HowToMovement = MainMovement;
                });
            }
            //redefine the still
            still = newStill;
        }
    }
}
