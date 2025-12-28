using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod.Systems.Collections;
using Terraria.ModLoader;

namespace CalamityInheritance.System
{
    public class PostSet : ModSystem
    {
        public override void PostSetupContent()
        {
            CalamityProjectileSets.ShouldNotBeReflected[ModContent.ProjectileType<MurasamaSlashnew1>()] = false;
            CalamityProjectileSets.ShouldNotBeReflected[ModContent.ProjectileType<MurasamaSlashold>()] = false;
            CalamityProjectileSets.ShouldNotBeReflected[ModContent.ProjectileType<ExoArrowTealExoLore>()] = false;
            CalamityProjectileSets.ShouldNotBeReflected[ModContent.ProjectileType<DragonBowFlameRework>()] = false;
            CalamityProjectileSets.ShouldNotBeReflected[ModContent.ProjectileType<RogueTypeHammerTruePaladinsProjClone>()] = false;
            CalamityProjectileSets.ShouldNotBeReflected[ModContent.ProjectileType<RogueTypeHammerTruePaladinsProj>()] = false;
        }
    }
}
