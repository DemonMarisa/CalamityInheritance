using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.RogueMelee;
using CalamityInheritance.Content.Misc;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.RogueMelee
{
    public class ElementalDiskProj_Rogue : CIMeleeRogueProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<ElementalDisk>();
        public override string Texture => GetInstance<ElementalDisk>().Texture;
        private readonly int Lifetime = 400;
        private readonly int ReboundTime = 60;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 56;
            Projectile.height = 56;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
            Projectile.penetrate = -1;
            Projectile.timeLeft = Lifetime;
            ModItem item = Projectile.Owner()?.HeldItem?.ModItem;
            if (item is not null && item is CIMeleeRogue rogue && rogue.IsMelee)
                Projectile.DamageType = DamageClass.Melee;
            else
                Projectile.DamageType = RogueDamage.Instance;
        }

        public override void AI()
        {
            Projectile.rotation += 0.15f * Projectile.direction;
            SpawnProjectilesNearEnemies();
            BoomerangAI();
            LightingAndDust();
        }

        private void BoomerangAI()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.position);
            }
            int timeMult = 1;
            if (Projectile.timeLeft < Lifetime * timeMult - ReboundTime * timeMult)
                Projectile.ai[0] = 1f;
            if (Projectile.ai[0] == 1f)
            {
                Player player = Main.player[Projectile.owner];
                float returnSpeed = 14f;
                float acceleration = 0.6f;
                Projectile.PredictHomeIn(returnSpeed, acceleration, player);
                // Delete the projectile if it touches its owner.
                if (Main.myPlayer == Projectile.owner)
                    if (Projectile.Hitbox.Intersects(player.Hitbox))
                        Projectile.Kill();
            }
        }

        private void SpawnProjectilesNearEnemies()
        {
            if (!Projectile.friendly)
                return;
            int counter = 30; //分裂数量更多、分裂的CD更短
            if (Main.player[Projectile.owner].miscCounter % counter == 0)
            {
                int splitProj = ProjectileType<ElementalDiskProjClone>();
                if (Projectile.owner == Main.myPlayer & Projectile.FinalExtraUpdate())
                {
                    float angle = MathHelper.TwoPi / 8f;
                    for (int i = 0; i < 8; i++)
                    {
                        Vector2 spawnPos = Projectile.Center;
                        Vector2 velocity = Projectile.velocity.RotatedBy(angle * i).SafeNormalize(Vector2.UnitX) * 6f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, velocity, splitProj, Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
                    }
                }
            }
            if (Main.player[Projectile.owner].miscCounter % 5 == 0)
            {
                int splitProj = ProjectileType<ElementalDiskProjClone_HomeIn>();
                if (Projectile.owner == Main.myPlayer & Projectile.FinalExtraUpdate())
                {
                    Vector2 spawnPos = Projectile.Center;
                    Vector2 velocity = Projectile.velocity.RotateRandom(MathHelper.TwoPi).SafeNormalize(Vector2.UnitX) * 6f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, velocity, splitProj, Projectile.damage / 2, Projectile.knockBack / 2, Projectile.owner);
                }
            }
        }

        private void LightingAndDust()
        {
            if (Main.rand.NextBool(3))
            {
                int rainbow = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RainbowTorch, Projectile.direction * 2, 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.3f);
                Main.dust[rainbow].noGravity = true;
                Main.dust[rainbow].velocity *= 0f;
            }

            Lighting.AddLight(Projectile.Center, 0.15f, 1f, 0.25f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIElementalMix>(), 180);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {

        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
