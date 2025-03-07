using EntityStates;
using ArchitectMod.Survivors.Architect;
using System;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace ArchitectMod.Survivors.Architect.SkillStates
{
    public class Barrier : BaseSkillState
    {
        
        public static float baseDuration = 0.5f; //this is buff timer, not store timer (scales with attack speed for some reason)
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        private float duration;

        //following for store functionality
        public bool setNextToHurl;
        public bool keyPressDown;
        public float storeDuration = 1.3f; //this is the time you have to store

        public override void OnEnter()
        {
            base.OnEnter();

            setNextToHurl = false;

            duration = baseDuration / attackSpeedStat;
            

            //this is the only functional part of this ability specifically. Most otherthings are for its charge
            characterBody.AddTimedBuff(ArchitectBuffs.armorBuff, 2f * duration);
        }
        
        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            keyPressDown = base.inputBank.skill1.down;

            if (base.isAuthority && keyPressDown)
            {
                setNextToHurl = true;
                ArchitectStaticValues.hurlDamageMultiplier = ArchitectStaticValues.hurlBarrierMultiplier;
                ArchitectStaticValues.isCloseHurl = false;

                outer.SetNextState(new Hurl());
                return;
            }
            else if (setNextToHurl)
            {
                //this state exists to prevent setting next state to main after the key has been pressed
                return;
            }
            else if (fixedAge >= storeDuration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }
        
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}