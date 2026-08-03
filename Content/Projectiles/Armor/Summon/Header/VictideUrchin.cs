using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.Buff.Armor.Summon;
using CalamityInheritance.Content.Items.Armor.ArmorItems.Victide;
using CalamityInheritance.Content.Projectiles.Melee.Flails;
using CalamityInheritance.Core.Path;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Armor.Summon.Header
{
    public class VictideUrchin : CIArmorProj
    {
        public override string LocalizationCategory => LocalizationPath.ArmorProjectile;
        public int dust = 3;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.netImportant = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
        }

        public override void AI()
        {
            Owner.AddBuff(BuffType<VictideSummonSetBuff>(), 2);
            if (Owner.dead || !Owner.CI().victideSummon)
            {
                Projectile.Kill();
            }
            float sizeScale = Main.mouseTextColor / 200f - 0.35f;
            sizeScale *= 0.2f;
            Projectile.scale = sizeScale + 0.95f;
            Projectile.Center = Owner.Center + Vector2.UnitY * (Owner.gfxOffY - 60f);
            dust--;
            if (dust >= 0)
            {
                int dustAmt = 50;
                for (int d = 0; d < dustAmt; d++)
                {
                    int index = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 16f), Projectile.width, Projectile.height - 16, DustID.BubbleBurst_Purple, 0f, 0f, 0, default, 1f);
                    Main.dust[index].velocity *= 2f;
                    Main.dust[index].scale *= 1.15f;
                }
            }
            if (Owner.miscCounter % 60 == 0)
            {
                NPC npc = LAPUtilities.FindClosestTarget(Projectile.Center, 500f, false);
                if (npc is not null)
                {
                    Vector2 ToNPCVel = LAPUtilities.GetVector2(Projectile.Center, npc.Center);
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 fireVel = ToNPCVel.RotatedByRandom(0.2f) * 9f * Main.rand.NextFloat(0.6f, 1f);
                        fireVel = CIUtils.GetGravityCompensatedVelocity(Projectile.Center, npc.Center, 0.3f, fireVel.Length() * 1.5f);
                        Projectile.NewProj(ProjectileType<UrchinBallSpike>(), Projectile.Center, fireVel);
                    }
                }
            }
        }
        public override Color? GetAlpha(Color lightColor) => new Color(200, 200, 200, 200);
        public override bool? CanDamage()  
        {
            return false;
        }
    }
}
