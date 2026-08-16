using CalamityInheritance.Content.Buff.Armor.Rogue;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorBonus
{
    public class AncientAstralBonus
    {
        public static void AncientAstralArmorBonus_OnHitNPC(CIPlayer ciplayer, Player player, NPC.HitInfo hit)
        {
            if (player.whoAmI != Main.myPlayer)
                return;
            if (hit.Damage > 10 && hit.Crit)
            {
                if (!ciplayer.HasCount("AncientAstralCritsCD"))
                {
                    player.NCHeal(40);
                    SoundEngine.PlaySound(CISoundID.SoundFallenStar with { Volume = 0.7f }, player.Center);
                    player.AddBuff(BuffType<AncientAstralBuff>(), 300); //5秒
                    CIUtils.DustCircle(player.Center, 18f, 1.8f, DustID.HallowedWeapons, false, 8f);
                    ciplayer.AddCount("AncientAstralCritsCD", 900);
                }
            }
        }
        public static void AncientAstralArmorBonus_OnHurt(CIPlayer player)
        {
            if (player.Player.whoAmI != Main.myPlayer)
                return;
            for (int n = 0; n < 9; n++) //生成一些落星，或者说我也a不知道，反正是一些落星
            {
                int astralStarsDMG = (int)player.Player.CalcIntDamage<ThrowingDamageClass>(400);
                astralStarsDMG = CIUtils.DamageSoftCap(astralStarsDMG, 400);
                Projectile star = CIUtils.ProjectileRain(player.Player.GetSource_FromThis(), player.Player.Center, 400f, 100f, 500f, 800f, 29f,
                ProjectileID.StarVeilStar, astralStarsDMG, 4f, player.Player.whoAmI);
                star.DamageType = GetInstance<ThrowingDamageClass>();
                star.usesLocalNPCImmunity = true;
                star.localNPCHitCooldown = 5;
            }
        }
    }
}
