using CalamityMod.Buffs.StatDebuffs;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Projectiles.Summon.Umbrella
{
    public class MagicBulletOld : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Summon";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bullet");
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.light = 0.5f;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 7;
            Projectile.scale = 1.18f;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = 1;
            AIType = 242;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Summon;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.BetsysCurse, 180);
            target.AddBuff(BuffID.Ichor, 180);
            target.AddBuff(BuffType<MarkedforDeath>(), 180);
            target.AddBuff(BuffType<ArmorCrunch>(), 180);
            target.AddBuff(BuffType<ArmorCrunch>(), 180);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.BetsysCurse, 180);
            target.AddBuff(BuffID.Ichor, 180);
            target.AddBuff(BuffType<MarkedforDeath>(), 180);
            target.AddBuff(BuffType<ArmorCrunch>(), 180);
            target.AddBuff(BuffType<ArmorCrunch>(), 180);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(189, 51, 164, Projectile.alpha);
        }
    }
}
