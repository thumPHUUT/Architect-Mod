using ArchitectMod.Survivors.Architect.SkillStates;

namespace ArchitectMod.Survivors.Architect
{
    public static class ArchitectStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Barrier));

            Modules.Content.AddEntityState(typeof(Evade));

            Modules.Content.AddEntityState(typeof(CastWall));

            Modules.Content.AddEntityState(typeof(PlaceWall));

            Modules.Content.AddEntityState(typeof(Store));

            Modules.Content.AddEntityState(typeof(Hurl));
        }
    }
}
