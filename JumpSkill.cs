using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// character skill for jumping
    /// </summary>
    public class JumpSkill : CharacterSkillBase {
        /// <summary>
        /// define the name
        /// </summary>
        public override string Name { get; } = "Jump";
        /// <summary>
        /// define the jump modifier
        /// </summary>
        private const float Modifier = 3000;
        /// <summary>
        /// inherited start function
        /// </summary>
        /// <param name="player"></param>
        /// <param name="startKey"></param>
        public override void Start(PlayerCharacterController player, KeyCode startKey) {
            base.Start(player, startKey);
            //if we are line walking or in the air, do nothing
            if (player.SkillsInUse.SingleOrDefault(x => x.Name == "LineWalk") != null || player.CheckInAir()) {
                return;
            }
            //add force upwards
            player.Rigid.AddForce(Vector3.up * Modifier);
            //do the jump animation 
            player.Animator.SetTrigger("jump");
        }
        /// <summary>
        /// inhertied method
        /// </summary>
        public override void Remove() {
            base.Remove();
        }
        /// <summary>
        /// inhertied method
        /// </summary>
        /// <returns></returns>
        public override bool Update() {
            //kill the skill in the beginning, since it only adds force upwards
            return false;
        }
    }
}
