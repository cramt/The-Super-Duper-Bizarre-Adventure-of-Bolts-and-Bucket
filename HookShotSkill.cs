using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// the skill for hookshots
    /// </summary>
    public class HookShotSkill : CharacterSkillBase {
        /// <summary>
        /// the name of the damn thing
        /// </summary>
        public override string Name { get; } = "HookShot";
        /// <summary>
        /// the variable for the line that is created
        /// </summary>
        LineController hookShot;
        /// <summary>
        /// the other rigidbody we're connected to
        /// </summary>
        Rigidbody target = null;
        /// <summary>
        /// the point at which that connection is happening
        /// </summary>
        GameObject hitPoint;
        /// <summary>
        /// the inhertied start function 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="startKey"></param>
        public override void Start(PlayerCharacterController player, KeyCode startKey) {
            base.Start(player, startKey);
            //we allow the thing to only be used once
            //and if it happens a second time, drop the target
            OnlyAllowSingleUse(e => {
                target = null;
            });
            //a raycast based on the mouse
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            //does it hit anything
            if (Physics.Raycast(r, out RaycastHit hit)) {
                //define the target
                target = hit.rigidbody;
                //if the target is not null and not itself
                if (target != null && !(Player.Rigid == target)) {
                    //create a new gameobject for the point hit
                    hitPoint = new GameObject("hitPoint");
                    //set its parent as the thing we hit
                    hitPoint.transform.SetParent(target.transform);
                    //set its position to the place we hit it
                    hitPoint.transform.position = hit.point;
                    //the hitpoint game object now rotates with the rigidbody we hit
                }
            }
            //if the target is not null
            if (target != null) {
                //create a new line between the player and the hitpoint
                hookShot = Utilities.SpawnNewLine(player.transform, hitPoint.transform);
                //define the first rigidbody as the player
                hookShot.Rigidbody1 = player.Rigid;
                //define the second rigidbody as the target
                hookShot.Rigidbody2 = target;
            }
        }

        public override void Remove() {
            base.Remove();
            //when we are removed delete the hookshot if its defined
            if (hookShot != null) {
                hookShot.Delete();
            }
            //delete the hitpoint
            Object.Destroy(hitPoint);

        }

        public override bool Update() {
            //kill the skill if there is no target
            return target != null;
        }
    }
}
