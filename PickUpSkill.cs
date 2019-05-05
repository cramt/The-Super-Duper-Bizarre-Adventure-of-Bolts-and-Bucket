using System.Linq;
using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// skill for picking up things
    /// </summary>
    public class PickUpSkill : CharacterSkillBase {
        /// <summary>
        /// the name
        /// </summary>
        public override string Name { get; } = "Pick Up";
        /// <summary>
        /// inherted method for when removed
        /// </summary>
        public override void Remove() {
            //if the pickup exists
            if (pickUp != null) {
                //reset its parent
                pickUp.transform.parent = null;
                //give back its collision detection
                this.pickUp.Rigidbody.detectCollisions = true;
                //give back its kenimatics
                this.pickUp.Rigidbody.isKinematic = false;
                //set it to null
                pickUp = null;
            }
            base.Remove();
        }
        /// <summary>
        /// the object to be picked up
        /// </summary>
        PickUpController pickUp = null;
        /// <summary>
        /// inherited method for when it starts
        /// </summary>
        /// <param name="player"></param>
        /// <param name="startKey"></param>
        public override void Start(PlayerCharacterController player, KeyCode startKey) {
            base.Start(player, startKey);
            //if we're line walking dont do anything
            if (player.Skills.Any(x => x.Name == "LineWalk")) {
                return;
            }
            //allow only single use
            OnlyAllowSingleUse(args => {
                //if we're line walking, ignore
                if (player.Skills.Any(x => x.Name == "LineWalk")) {
                    return;
                }
                done = true;
            });
            //all the pickups
            var pickUps = UnityEngine.Object.FindObjectsOfType<PickUpController>().ToList();
            //sort them based on distance
            pickUps.Sort((x, y) => {
                return Vector3.Distance(x.transform.position, player.transform.position).CompareTo(Vector3.Distance(y.transform.position, player.transform.position));
            });
            //get closest
            var pickUp = pickUps.ElementAtOrDefault(0);
            //if its not null and closer than 2.5
            if (pickUp != null && Vector3.Distance(player.transform.position, pickUp.transform.position) < 2.5f) {
                //set the pickup car to the pickup
                this.pickUp = pickUp;
                //set the rigidbody to not collide
                this.pickUp.Rigidbody.detectCollisions = false;
                //set it to be kinematic
                this.pickUp.Rigidbody.isKinematic = true;
                // set it to be in front of the player
                pickUp.transform.position = (player.transform.rotation * Vector3.back) + player.transform.position;
                //set the player to be its parent
                pickUp.transform.parent = player.transform;
            }
        }
        bool done = false;
        public override bool Update() {
            return !done;
        }
    }
}
