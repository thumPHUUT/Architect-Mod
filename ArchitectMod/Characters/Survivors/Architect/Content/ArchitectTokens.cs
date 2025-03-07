using System;
using ArchitectMod.Modules;
using ArchitectMod.Survivors.Architect.Achievements;

namespace ArchitectMod.Survivors.Architect
{
    public static class ArchitectTokens
    {
        public static void Init()
        {
            AddArchitectTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Architect.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddArchitectTokens()
        {
            string prefix = ArchitectSurvivor.ARCHITECT_PREFIX;

            string desc = "The Architect is a melee ranged hybrid survivor, enabled by their unique mobility skills capable of dishing potent ranged damage.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Terrestrial Staff is a strong melee option capable of hurling all energy creations for ranged damage." + Environment.NewLine + Environment.NewLine
             + "< ! > Energy Barrier provides damage reducing armor for a short time and can be hurled for consistently ranged damage." + Environment.NewLine + Environment.NewLine
             + "< ! > Evade leaves behind a hurlable energy barrier which blocks projectiles, allowing for defensive and re-aggressive play." + Environment.NewLine + Environment.NewLine
             + "< ! > Celestial Wall can be wall-ridden for a movement boost and hurled for massive damage." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so they left, searching for a new identity.";
            string outroFailure = "..and so they vanished, forever a blank slate.";

            Language.Add(prefix + "NAME", "Architect");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "The Chosen One");
            Language.Add(prefix + "LORE", "sample lore");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Architect passive");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SLASH_NAME", "Terrestrial Staff");
            Language.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Tokens.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * ArchitectStaticValues.staffDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_BARRIER_NAME", "Energy Barrier");
            Language.Add(prefix + "SECONDARY_BARRIER_DESCRIPTION", Tokens.agilePrefix + $"Create an energy barrier providing <style=cIsUtility>300 armor</style>. Can be hurled for <style=cIsDamage>{100f * ArchitectStaticValues.hurlDamageCoefficient * ArchitectStaticValues.hurlBarrierMultiplier}% damage</style>.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_ROLL_NAME", "Evade");
            Language.Add(prefix + "UTILITY_ROLL_DESCRIPTION", $"Dash a short distance, leaving behind a small barrier that can be hurled for <style=cIsDamage>{100f * ArchitectStaticValues.hurlDamageCoefficient * ArchitectStaticValues.hurlEvadeMultiplier}% damage</style>.");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_BOMB_NAME", "Celestial Wall");
            Language.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Cast a wall that can be ridden for additional movement. Hurl for <style=cIsDamage>{100f * ArchitectStaticValues.hurlDamageCoefficient * ArchitectStaticValues.placeWallDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(ArchitectMasteryAchievement.identifier), "Architect: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(ArchitectMasteryAchievement.identifier), "As Architect, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
