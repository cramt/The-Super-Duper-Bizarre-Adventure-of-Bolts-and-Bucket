using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// abstract base for the skills that characters use
    /// </summary>
    public abstract class CharacterSkillBase {
        /// <summary>
        /// the name of the skill
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// abstract method for what happens at each frame
        /// </summary>
        /// <returns>if we should stop and remove the skill or not</returns>
        public abstract bool Update();
        /// <summary>
        /// the player that is using the skill
        /// </summary>
        public PlayerCharacterController Player;
        /// <summary>
        /// the key which start the whole damn thing
        /// </summary>
        public KeyCode StartKey;
        /// <summary>
        /// when the skill is first made
        /// it is virtual so that the inheritor can reuse this function
        /// </summary>
        /// <param name="player">the player that used the skill</param>
        /// <param name="startKey">the key that was used to use the skill</param>
        public virtual void Start(PlayerCharacterController player, KeyCode startKey) {
            //define the player and startkey
            Player = player;
            StartKey = startKey;
        }
        /// <summary>
        /// the reverse of Start, it is called when the skill is done and gets removed
        /// </summary>
        public virtual void Remove() {
            //if the OnlyAllowSingleUse was called
            if (onlyAllowSingleUse) {
                // remove the Player_OnSkillUsed from the listener
                Player.OnSkillUsed -= Player_OnSkillUsed;
            }
        }
        /// <summary>
        /// variable to check if the OnlyAllowSingleUse function was called
        /// </summary>
        private bool onlyAllowSingleUse = false;
        /// <summary>
        /// this the function that the inheritor defines when they allow the skill to be used only once
        /// its a callback for whenever the key to start it, is pressed again
        /// </summary>
        private Action<SkillUsedEventArgs> onlyAllowSingleUseAction;
        /// <summary>
        /// the function that defines if the skill can be used multible times
        /// </summary>
        /// <param name="action">callback for when the skill is trying to be used again</param>
        protected void OnlyAllowSingleUse(Action<SkillUsedEventArgs> action = null) {
            onlyAllowSingleUse = true;
            onlyAllowSingleUseAction = action;
            Player.OnSkillUsed += Player_OnSkillUsed;
        }
        /// <summary>
        /// used as the listened for when a skill is called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Player_OnSkillUsed(object sender, SkillUsedEventArgs e) {
            //if the skill is the same name as this one, set the "dont" var to true
            e.dont = e.Skill.Name == Name;
            //if we dont do it
            if (e.dont) {
                //call the callback
                onlyAllowSingleUseAction?.Invoke(e);
            }
        }
    }
}