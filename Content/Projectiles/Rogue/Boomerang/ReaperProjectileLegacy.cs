using CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue.Boomerang
{
    public class ReaperProjectileLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => GetInstance<TheReaperLegacy>().Texture;
        public ref float AttackState => ref Projectile.ai[0];
        public ref float CantHomeIn => ref Projectile.ai[1];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = Projectile.MaxUpdates * 120;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Projectile.MaxUpdates * 9; // can't hit too fast, but can hit many many times
            Projectile.DamageType = RogueDamageClass.Instance;
            Projectile.netImportant = true;
        }
        public override void AI()
        {
            if (CantHomeIn > 0)
                CantHomeIn--;
            Projectile.rotation += 0.4f;
            if (AttackState == 0)
                Projectile.HomeInNPC(900f, 14f, 14f);
            else
            {
                if (CantHomeIn == 0)
                    Projectile.HomeInNPC(1500f, 14f, 0, 10);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color color = Color.SkyBlue * lightColor.A;
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], color, 1);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            CantHomeIn = 35;
            Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.PiOver4);
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 180);
            target.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), 180);
            if (AttackState == 0)
            {
                if (Projectile.Calamity().stealthStrike)
                {
                    Vector2 a = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * 12f;
                    float angle = MathHelper.TwoPi / 3f;
                    for (int i = 0; i < 3; i++)
                    {
                        a = a.RotatedBy(angle * i);
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, a, ProjectileType<ReaperFuryLegacy>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
            }
            AttackState++;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            AttackState++;
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 180);
            target.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), 180);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
        }
    }
}
