using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles.HeldProj;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.ExoWeapons;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace CalamityInheritance.Content.Projectiles.ExoWeapons
{
    public class ExoGladiusProj : CIBaseShortswordProjectile
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<ExoGladius>();
        public override string Texture => GetInstance<ExoGladius>().Texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NoMeleeSpeedVelocityScaling[Projectile.type] = true;
        }

        public const int OnHitIFrames = 3;
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(28);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = GetInstance<TrueMelee>(); ;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override Action<Projectile> EffectBeforePullback => (proj) =>
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 3f, ProjectileType<ExoGladProj>(), Projectile.damage * 1, Projectile.knockBack, Projectile.owner, 0, Projectile.ai[1]);
        };
        public override void SetVisualOffsets()
        {
            const int HalfSpriteWidth = 56 / 2;
            const int HalfSpriteHeight = 56 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);
        }
        public override void ExtraBehavior()
        {
            if (Main.rand.NextBool(5))
            {
                int rainbowDust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.RainbowTorch, Projectile.direction * 2, 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.3f);
                Main.dust[rainbowDust].velocity *= 0.2f;
                Main.dust[rainbowDust].noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIMiracleBlight>(), 300);
            SpawnMeteor(Main.player[Projectile.owner]);
            Projectile.Owner().SetImmuneTimeForAllTypes(30);
        }
        private void SpawnMeteor(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                var source = player.GetSource_FromThis();
                if (player.CI().HasCount("ExoGladCometFireCD"))
                {
                    int damage = player.GetWeaponDamage(player.ActiveItem()) * 2;
                    CIUtils.ProjectileRain(source, player.Center, 400f, 100f, 500f, 800f, 25f, ProjectileType<ExoGladComet>(), damage, 15f, player.whoAmI, 0, Projectile.ai[1]);
                    player.CI().AddCount("ExoGladCometFireCD", 3);
                }
            }
        }
    }
}
