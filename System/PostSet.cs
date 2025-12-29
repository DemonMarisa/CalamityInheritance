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
            CalamityProjectileSets.ShouldNotBeReflected[ProjectileType<MurasamaSlashnew1>()] = false;
            CalamityProjectileSets.ShouldNotBeReflected[ProjectileType<MurasamaSlashold>()] = false;
            CalamityProjectileSets.ShouldNotBeReflected[ProjectileType<ExoArrowTealExoLore>()] = false;
            CalamityProjectileSets.ShouldNotBeReflected[ProjectileType<DragonBowFlameRework>()] = false;
            CalamityProjectileSets.ShouldNotBeReflected[ProjectileType<RogueFallenHammerProjClone>()] = false;
            CalamityProjectileSets.ShouldNotBeReflected[ProjectileType<RogueFallenHammerProj>()] = false;
        }
    }
}
