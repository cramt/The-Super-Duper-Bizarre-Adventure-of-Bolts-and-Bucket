using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// skill for walking on a line from the linecontroller
    /// </summary>
    public class LineWalkSkill : CharacterSkillBase {
        /// <summary>
        /// name
        /// </summary>
        public override string Name { get; } = "LineWalk";
        /// <summary>
        /// how the character used to do movement
        /// </summary>
        private Action<Vector3> oldHowToMovement;
        /// <summary>
        /// how the character used to check if they were in the air
        /// </summary>
        private Func<bool> oldCheckInAir;
        /// <summary>
        /// the line in question
        /// </summary>
        private LineController targetLine = null;
        /// <summary>
        /// the current distance on the line
        /// </summary>
        private float distance;
        /// <summary>
        /// inherited start method
        /// </summary>
        /// <param name="player"></param>
        /// <param name="startKey"></param>
        public override void Start(PlayerCharacterController player, KeyCode startKey) {
            base.Start(player, startKey);
            //single use only, die on second use
            OnlyAllowSingleUse(e => {
                targetLine = null;
            });
            //get all the LineController and the closets point to the chacters on the line
            var pointsOnLines = UnityEngine.Object.FindObjectsOfType<LineController>().Where(line => {
                return line.Position1.transform.parent != player.transform || line.Position2.transform.parent != player.transform;
            }).Select(line => {
                return new KeyValuePair<LineController, Vector3>(line,
                    Utilities.FindNearestPointOnLine(line.Position2.position, line.Position1.position, player.transform.position)
                    );
            }).ToList();
            //sort them based on the clests point on the line
            pointsOnLines.Sort((x, y) => {
                return Vector3.Distance(x.Value, player.transform.position).CompareTo(Vector3.Distance(y.Value, player.transform.position));
            });
            //get the line and the point
            var point = pointsOnLines.ElementAtOrDefault(0);
            //if that point exists and the distance isnt over 2.5
            if (point.Key != null && Vector3.Distance(point.Value, player.transform.position) < 2.5f) {
                //set the target line
                targetLine = point.Key;
                //set the point we need to go to
                var pointToGoTo = point.Value;
                // calculate this based on the vectors of the line
                pointToGoTo = (targetLine.Position1.position - targetLine.Position2.position).normalized * Vector3.Distance(targetLine.Position1.position, pointToGoTo) + targetLine.Position2.position;
                //just add half of the height of the character to the y pos, so that it looks like he is actually walking on it
                pointToGoTo.y += player.Collider.bounds.extents.y;
                //set the position
                player.transform.position = pointToGoTo;
                //make the player kinematic, since we do the movement manually in here
                player.Rigid.isKinematic = true;
                //make the players collider not work, since that fucks shit up when line walking
                player.Collider.enabled = false;
                //save the old movement way
                oldHowToMovement = player.HowToMovement;
                //save the old way checking if we're in the air
                oldCheckInAir = player.CheckInAir;
                //always on the gound
                player.CheckInAir = () => false;
                //movement modifier
                const float mod = 2.3f;

                //define the current distance
                distance = Vector3.Distance(targetLine.Position1.position, player.transform.position);

                //define a new way for moving when on the line
                player.HowToMovement = new Action<Vector3>(v => {
                    //if we dont have a target
                    if (targetLine == null) {
                        //do nothing
                        return;
                    }
                    //find the differnece
                    var diff = targetLine.Position1.position - targetLine.Position2.position;
                    //check if we're going up or down
                    bool updown = diff.z > diff.x;
                    Vector3 dirForMovement = Vector3.zero;
                    //use that to define which keys mean which direction
                    //this hole thing doesnt work particularly well
                    if (updown) {
                        dirForMovement = Vector3.forward;
                    }
                    else {
                        dirForMovement = Vector3.right;
                    }
                    v = v.normalized;
                    bool? upDown = null;
                    //if we're going in the one direction, add the mod, the other remove the mod
                    if (v == dirForMovement) {
                        distance += mod * Time.deltaTime;
                        upDown = true;
                    }
                    if (v * -1 == dirForMovement) {
                        distance -= mod * Time.deltaTime;
                        upDown = false;
                    }
                    // make sure distance is father than the longest point on the line
                    if(distance > Vector3.Distance(targetLine.Position1.position, targetLine.Position2.position)) {
                        distance = Vector3.Distance(targetLine.Position1.position, targetLine.Position2.position);
                    }
                    // make sure it isnt less than 0
                    if(distance < 0) {
                        distance = 0;
                    }
                    //if we're moving
                    bool isMoving = upDown != null;
                    //set the animator 
                    player.Animator.SetBool("isMoving", isMoving);
                    //look at either position2 or 1 based on the way of moving
                    if (isMoving) {
                        player.transform.LookAt((bool)upDown ? targetLine.Position2.position : targetLine.Position1.position);
                    }
                });
            }
        }
        /// <summary>
        /// inherited method for when removed
        /// </summary>
        public override void Remove() {
            base.Remove();
            //set all the things back
            //not kinematic anymore
            Player.Rigid.isKinematic = false;
            //back to the old way of moving
            if (oldHowToMovement != null) {
                Player.HowToMovement = oldHowToMovement;
            }
            //back to the old way of air control
            if(oldCheckInAir != null) {
                Player.CheckInAir = oldCheckInAir;
            }
            //get collider back
            Player.Collider.enabled = true;
        }
        /// <summary>
        /// inherited method for updats
        /// </summary>
        /// <returns></returns>
        public override bool Update() {
            if (targetLine == null) {
                //stop of the target line doesnt exist
                return false;
            }
            if (distance > Vector3.Distance(targetLine.Position1.position, targetLine.Position2.position)) {
                distance = Vector3.Distance(targetLine.Position1.position, targetLine.Position2.position);
            }
            if (distance < 0) {
                distance = 0;
            }
            //calculate the position based on the distance and the normalized vector from the relative vektor from pos 1 to pos 2
            Vector3 v = ((targetLine.Position1.position - targetLine.Position2.position).normalized * distance) + targetLine.Position2.position;
            // add half of the height of the player to that
            v.y += Mathf.Abs(Player.transform.position.y - Player.transform.GetChild(0).position.y);
            //set the position
            Player.transform.position = v;
            return true;
        }
    }
}
