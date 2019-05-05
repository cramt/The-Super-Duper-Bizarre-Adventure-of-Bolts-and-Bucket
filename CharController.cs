using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// class for handling input from the user
    /// </summary>
    public class CharController : MainBehavior {
        /// <summary>
        /// the big guy
        /// </summary>
        public PlayerCharacterController BG;
        /// <summary>
        /// the small guy
        /// </summary>
        public PlayerCharacterController SG;
        public override void OnStart(OnStartProperties onStartProperties) {

        }

        public override void OnUpdate(OnUpdateProperties onUpdateProperties) {
            {
                //the wasd keys for SG
                Vector3 dir = Vector3.zero;
                if (Input.GetKey(KeyCode.W)) {
                    dir += Vector3.forward;
                }
                if (Input.GetKey(KeyCode.A)) {
                    dir += Vector3.left;
                }
                if (Input.GetKey(KeyCode.S)) {
                    dir += Vector3.back;
                }
                if (Input.GetKey(KeyCode.D)) {
                    dir += Vector3.right;
                }
                SG.AddMovement(dir.normalized);
            }
            //his 3 skills
            if (Input.GetKeyDown(KeyCode.E)) {
                SG.UseSkill(0, KeyCode.E);
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                SG.UseSkill(1, KeyCode.Space);
            }
            if (Input.GetKeyDown(KeyCode.Q)) {
                SG.UseSkill(2, KeyCode.Space);
            }


            {
                //the ijkl for big guy
                Vector3 dir = Vector3.zero;
                if (Input.GetKey(KeyCode.I)) {
                    dir += Vector3.forward;
                }
                if (Input.GetKey(KeyCode.J)) {
                    dir += Vector3.left;
                }
                if (Input.GetKey(KeyCode.K)) {
                    dir += Vector3.back;
                }
                if (Input.GetKey(KeyCode.L)) {
                    dir += Vector3.right;
                }
                BG.AddMovement(dir.normalized);
            }
            //his one skill
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                BG.UseSkill(0, KeyCode.Mouse0);
            }
        }
    }
}