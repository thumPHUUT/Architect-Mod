using ArchitectMod.Survivors.Architect.Achievements;
using RoR2;
using UnityEngine;

namespace ArchitectMod.Survivors.Architect
{
    public static class ArchitectUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                ArchitectMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(ArchitectMasteryAchievement.identifier),
                ArchitectSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
