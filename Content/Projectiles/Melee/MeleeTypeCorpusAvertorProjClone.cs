using System;
using CalamityMod;
using CalamityMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Weapons;

namespace CalamityInheritance.Content.Projectiles.Melee
{
    public class MeleeTypeCorpusAvertorProjClone : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override string Texture => $"{Generic.WeaponRoute}/Melee/MeleeTypeCorpusAvertor";
        public static readonly int CorpusAvertorLifeStealCap = 100;
        public static readonly float CorpusAvertorLifeStealRange = 3000f;
        public static readonly float CorpusAvertorFreakingChasingRange = 32000f;
        public static readonly float CorpusAvertorFreakingChasingSpeed = 32f;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.04f;

            Projectile.velocity.X *= 1.005f;
            Projectile.velocity.Y *= 1.005f;

            switch ((int)Projectile.ai[0])
            {
                case 20:
                    Projectile.scale = 0.7f;
                    break;
                case 40:
                    Projectile.scale = 0.8f;
                    break;
                case 60:
                    Projectile.scale = 0.9f;
                    break;
                default:
                    break;
            }
            Projectile.width = Projectile.height = (int)(24f * Projectile.scale);

            CalamityUtils.HomeInOnNPC(Projectile, true, CorpusAvertorFreakingChasingRange, CorpusAvertorFreakingChasingSpeed, 20f);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Projectile.timeLeft < 85)
                return new Color((int)(150f * (Projectile.timeLeft / 85f)), 0, 0, Projectile.timeLeft / 5 * 3);
            return new Color(150, 0, 0, 50);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int heal = (int)Math.Round(hit.Damage * 0.05);
            if (heal > CorpusAvertorLifeStealCap)
                heal = CorpusAvertorLifeStealCap;

            if (Main.player[Main.myPlayer].lifeSteal <= 0f || heal <= 0 || target.lifeMax <= 5)
                return;

            CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], heal, ProjectileID.VampireHeal, CorpusAvertorLifeStealRange);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int heal = (int)Math.Round(info.Damage * 0.05);
            if (heal > CorpusAvertorLifeStealCap)
                heal = CorpusAvertorLifeStealCap;

            if (Main.player[Main.myPlayer].lifeSteal <= 0f || heal <= 0)
                return;

            CalamityGlobalProjectile.SpawnLifeStealProjectile(Projectile, Main.player[Projectile.owner], heal, ProjectileID.VampireHeal, CorpusAvertorLifeStealRange);
        }
    }
}
