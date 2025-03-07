using RoR2;
using ArchitectMod.Modules.Achievements;

namespace ArchitectMod.Survivors.Architect.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, null)]
    public class ArchitectMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = ArchitectSurvivor.ARCHITECT_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = ArchitectSurvivor.ARCHITECT_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => ArchitectSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}