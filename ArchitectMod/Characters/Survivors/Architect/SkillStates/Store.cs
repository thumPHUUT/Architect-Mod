using EntityStates;
using ArchitectMod.Survivors.Architect;
using System;
using RoR2;
using RoR2.UI;
using UnityEngine;
using ExtraSkillSlots;


namespace ArchitectMod.Survivors.Architect.SkillStates
{
    public class Store : BaseSkillState
    {
        //here for example inputtestbank (in OnEnter)
        public ExtraInputBankTest extrainputBankTest;
        public bool justPressed;
        public bool setNextToHurl;

        public bool keyPressDown;
        public bool avaliableHurlable; //
        public float storeDuration = 1.3f; //this is the time you have to store, not the time it takes to cast (can be made shorter, but need 
                                        //some sort of visual to queue you into the timing. 1s will def be enoguh once this anim is in)
                                        //--might make scale with attack speed
        

        //public InputBankTest.ButtonState skill1;

        public override void OnEnter()
        {
            /*      implementing extrainputbanktest is done as follows:
            this.extrainputBankTest = base.characterBody.gameObject.GetComponent<ExtraInputBankTest>();
            keyPressDown = this.extrainputBankTest.extraSkill1.down;

            names of all buttons:
            public bool CheckAnyButtonDown()
		{
			return this.skill1.down || this.skill2.down || this.skill3.down || this.skill4.down || this.interact.down || this.jump.down || this.sprint.down || this.activateEquipment.down || this.ping.down;
		}   
            examples (either seems fine)
            keyPressDown = this.skill1.down;
            keyPressDown = base.inputBank.skill1.down;
            */

            
            setNextToHurl = false; //must reset boolean each time store is entered. This must be kept to ensure functionality

            //instead of implementing a boolean, can have other skillstates lead into this one,
            //and if keypress doesnt happen within timer (the duration), sets to mainstate
            //this would mean there would be no storing, if skill1 pressed, then obj is hurled (sent to next state)

            base.OnEnter();

            
        }
        
        public override void OnExit()
        {
            base.OnExit();
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            keyPressDown = base.inputBank.skill1.down;

            justPressed = (base.inputBank.skill1.down && !base.inputBank.skill1.wasDown);
            //by using justPressed instead of keyPressDown, gameplaywise, you cannot hurl by just hold down m1, you have to un-click for a second
            //i've decided I dont want this, but it can be easily re-implemented


            if (base.isAuthority && keyPressDown)
            {
                setNextToHurl = true;
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