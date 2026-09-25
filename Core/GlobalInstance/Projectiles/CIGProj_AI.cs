using CalamityInheritance.Content.Items.Accessories.Professional;
using CalamityInheritance.Content.Items.Armor.ArmorBonus;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Projectiles
{
    public partial class CIGProj : GlobalProjectile
    {
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            CIPlayer ciplayer = player.CI();
            if (ciplayer.ReaverRogueSet)
                ReaverLegacyBonus.ProjAI_Rogue(player, projectile);
        }
        public override void PostAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            CIPlayer ciplayer = player.CI();
            if (ciplayer.ElemQuiver)
                ElementalQuiver.ProjSpilt(projectile);
            if (ciplayer.NanoTech)
                NanotechOld.ProjSpilt(projectile);
        }
    }
}
