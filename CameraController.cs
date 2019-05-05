using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// controller for the camera
    /// it follows x amount of players
    /// </summary>
    public class CameraController : MainBehavior {
        /// <summary>
        /// list of players
        /// </summary>
        public List<PlayerCharacterController> Players;
        /// <summary>
        /// the offset from the middle of the players
        /// </summary>
        Vector3 offset;
        /// <summary>
        /// override for when shit starts 
        /// </summary>
        /// <param name="onStartProperties"></param>
        public override void OnStart(OnStartProperties onStartProperties) {
            base.OnStart(onStartProperties);
            //define the center of the players
            Vector3 center = Players.Select(x => x.transform.position).ToList().CenterPoint();
            //define the offset
            offset = transform.position - center;
        }
        /// <summary>
        /// override the inherited onupdate function
        /// </summary>
        /// <param name="onUpdateProperties"></param>
        public override void OnUpdate(OnUpdateProperties onUpdateProperties) {
            base.OnUpdate(onUpdateProperties);
            //find center
            Vector3 center = Players.Select(x => x.transform.position).ToList().CenterPoint();
            //add center to offset
            Vector3 pos = center + offset;
            //set that as the new position
            transform.position = pos;
        }
    }
}