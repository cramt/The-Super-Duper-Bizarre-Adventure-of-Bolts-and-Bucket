using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TheSuperDuperBizzareAdventureOfBoltsAndBuckets {
    /// <summary>
    /// event args for when a skill is used 
    /// </summary>
    public class SkillUsedEventArgs : EventArgs {
        public bool dont = false;
        public readonly CharacterSkillBase Skill;
        public SkillUsedEventArgs(CharacterSkillBase Skill) {
            this.Skill = Skill;
        }
    }
    /// <summary>
    /// class for all the types of characters that exist
    /// it requires that the game object has a rigidbody
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class PlayerCharacterController : MainBehavior {
        /// <summary>
        /// list of things currently colliding with
        /// </summary>
        public List<Rigidbody> CurrentlyCollidingWith = new List<Rigidbody>();
        /// <summary>
        /// its rigidbody
        /// </summary>
        public Rigidbody Rigid;
        /// <summary>
        /// its collider
        /// </summary>
        public Collider Collider;
        /// <summary>
        ///  the reflection types of all the skills we have
        /// </summary>
        public Type[] Skills { get; protected set; }
        /// <summary>
        /// the speed
        /// </summary>
        public float speed { get; set; } = 2;
        /// <summary>
        /// skills currently in use
        /// </summary>
        public List<CharacterSkillBase> SkillsInUse = new List<CharacterSkillBase>();
        /// <summary>
        /// the event handler for when a skill is used
        /// </summary>
        public event EventHandler<SkillUsedEventArgs> OnSkillUsed;
        /// <summary>
        /// the animtor
        /// </summary>
        public Animator Animator;
        /// <summary>
        /// function for checking if we're in the air
        /// </summary>
        public Func<bool> CheckInAir;
        /// <summary>
        /// function for how the character should move
        /// </summary>
        public Action<Vector3> HowToMovement;
        /// <summary>
        /// inhertied class for when it starts      
        /// </summary>
        /// <param name="onStartProperties"></param>
        public override void OnStart(OnStartProperties onStartProperties) {
            base.OnStart(onStartProperties);
            // if rigid isnt set, set it
            if (Rigid == null) {
                Rigid = GetComponent<Rigidbody>();
            }
            Rigid.constraints = RigidbodyConstraints.FreezeRotation;
            // if collider isnt set, set it
            if (Collider == null) {
                Collider = GetComponent<Collider>();
            }
            // if animator isnt set, set it
            if (Animator == null) {
                Animator = GetComponent<Animator>();
            }
            //more collider things
            GameObject childCollider = new GameObject("collider");
            var triggerCollider = Utilities.CloneComponent(Collider, childCollider);
            triggerCollider.isTrigger = true;
            childCollider.transform.parent = transform;
            childCollider.transform.localPosition = Vector3.zero;
            // set how we do movement
            HowToMovement = new Action<Vector3>((dir) => {
                Rigid.AddForce(dir);
            });
            //add listeners for when colliding with things
            AddOnCollisionExitListener(e => {
                if (CurrentlyCollidingWith.Contains(e.rigidbody)) {
                    CurrentlyCollidingWith.Remove(e.rigidbody);
                }
            });
            AddOnCollisionEnterListener(e => {
                if (!CurrentlyCollidingWith.Contains(e.rigidbody)) {
                    CurrentlyCollidingWith.Add(e.rigidbody);
                }
            });
            AddOnCollisionStayListener(e => {
                if (!CurrentlyCollidingWith.Contains(e.rigidbody)) {
                    CurrentlyCollidingWith.Add(e.rigidbody);
                }
            });
            //set how movement is defined
            CheckInAir = () => {
                return CurrentlyCollidingWith.Count == 0;
            };
        }
        /// <summary>
        /// inherited method for whenever a new frame is loaded
        /// </summary>
        /// <param name="onUpdateProperties"></param>
        public override void OnUpdate(OnUpdateProperties onUpdateProperties) {
            base.OnUpdate(onUpdateProperties);
            {
                //list of skills to delete
                List<CharacterSkillBase> toDelete = new List<CharacterSkillBase>();
                //loop through skills
                SkillsInUse.ForEach(x => {
                    //update skills and if they say so, add it to the deleted bunch
                    if (!x.Update()) {
                        toDelete.Add(x);
                    }
                });
                //move the skills to be removed
                toDelete.Reverse();
                toDelete.ForEach(x => {
                    SkillsInUse.Remove(x);
                    x.Remove();
                });
            }
        }
        /// <summary>
        /// method for using a skill
        /// </summary>
        /// <param name="index">the index of the skill</param>
        /// <param name="startKey">they key pressed</param>
        public void UseSkill(int index, KeyCode startKey) {
            //create the skill in question
            CharacterSkillBase skill = (CharacterSkillBase)Activator.CreateInstance(Skills[index]);
            //create the event args and send the event
            SkillUsedEventArgs eventArgs = new SkillUsedEventArgs(skill);
            OnSkillUsed?.Invoke(this, eventArgs);
            //if no event say no to it, do it
            if (!eventArgs.dont) {
                //start the skill
                skill.Start(this, startKey);
                //add the skill to list
                SkillsInUse.Add(skill);
            }
        }
        /// <summary>
        /// method for adding movement
        /// </summary>
        /// <param name="dir">the direction</param>
        public void AddMovement(Vector3 dir) {
            // use the howtomovement function
            HowToMovement(dir * 5000 * Time.deltaTime);
        }

    }

}