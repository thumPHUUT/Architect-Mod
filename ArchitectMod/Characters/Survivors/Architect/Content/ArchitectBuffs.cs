using RoR2;
using UnityEngine;

namespace ArchitectMod.Survivors.Architect
{
    public static class ArchitectBuffs
    {
        // armor buff gained during roll
        public static BuffDef armorBuff;

        public static void Init(AssetBundle assetBundle)
        {
            armorBuff = Modules.Content.CreateAndAddBuff("ArchitectArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

        }
    }
}
