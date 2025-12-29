using CalamityInheritance.Content.Items.Weapons.Melee.Shortsword;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.BaseProjectiles;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Melee.Shortsword
{
    public class GalileoGladiusProjLegacy : BaseShortswordProjectile, ILocalizedModType
    {
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<GalileoGladius>();
        public override string Texture => GetInstance<GalileoGladiusLegacy>().Texture;
        public new string LocalizationCategory => "Content.Projectiles.Melee";
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(24);
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ownerHitCheck = true;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }

        public override Action<Projectile> EffectBeforePullback => (proj) =>
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 10f, Type: ProjectileType<GalileosMoonLegacy>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
        };

        public override void SetVisualOffsets()
        {
            const int HalfSpriteWidth = 42 / 2;
            const int HalfSpriteHeight = 46 / 2;

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
                int gladiusDust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, (Main.rand.NextBool() ? 20 : 176), (float)(Main.player[Projectile.owner].direction * 2), 0f, 150, default, 1.3f);
                Main.dust[gladiusDust].velocity *= 0.2f;
                Main.dust[gladiusDust].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<Nightwither>(), 300);
            SpawnMeteor(Main.player[Projectile.owner]);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffType<Nightwither>(), 300);
            SpawnMeteor(Main.player[Projectile.owner]);
        }

        private void SpawnMeteor(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                var source = player.GetSource_FromThis();
                if (player.CIMod().GalileoGladiusCounter<= 0)
                {
                    int damage = player.GetWeaponDamage(player.ActiveItem()) * 2;
                    CalamityUtils.ProjectileRain(source, player.Center, 400f, 100f, 500f, 800f, 25f, ProjectileType<GalileosPlanet>(), damage, 15f, player.whoAmI);
                    player.CIMod().GalileoGladiusCounter = 15;
                }
            }
        }
    }
}
