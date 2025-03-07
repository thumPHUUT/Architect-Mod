using System;
using UnityEngine;

namespace ArchitectMod.Survivors.Architect
{
    public static class ArchitectStaticValues
    {
        //the re-assignable deletable projectile can be found in components: 
        
        

        //these values are x100 (so 1f is 100% dmg)
        public const float staffDamageCoefficient = 4f;

        public const float gunDamageCoefficient = 4.2f;

        public const float bombDamageCoefficient = 16f;

        public const float placeWallDamageCoefficient = 10f;

        //coeff here is gcf of most damage values
        public const float hurlDamageCoefficient = 0.5f;

        public static bool isCloseHurl;

        //hurldmgmult is default value, re-set to barrier and evade mult based on which ability was most recently cast
        public static float hurlDamageMultiplier = 1;
        public const float hurlBarrierMultiplier = 7; //7*50->350
        public const float hurlEvadeMultiplier = 10; //10*50->500
        

    }
}