using System;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.BaseProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Utilities;
using LAP.Core.Utilities;

namespace CalamityInheritance.Content.Projectiles.Melee.Shortsword
{
    public class ExoGladiusProj : BaseShortswordProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Melee";
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
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>(); ;
            Projectile.timeLeft = 360;
            Projectile.extraUpdates = 1;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override Action<Projectile> EffectBeforePullback => (proj) =>
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 6f, ModContent.ProjectileType<ExoGladProj>(), Projectile.damage * 1, Projectile.knockBack, Projectile.owner, 0f, 0f);
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
            target.AddBuff(ModContent.BuffType<ElementalMix>(), 300);
            SpawnMeteor(Main.player[Projectile.owner]);
            GiveImmue(Main.player[Projectile.owner], 45, 45);
        }

        public static void GiveImmue(Player player, int immuneCD, int wantedImmuneTime)
        {
            if ((player.CIMod().LoreExo || player.CIMod().PanelsLoreExo ) && player.CIMod().DNAImmnueActive == 0)
            {
                player.CIMod().DNAImmnue = wantedImmuneTime;
                player.CIMod().DNAImmnueActive = immuneCD;
            }
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>($"{Generic.WeaponPath}/Melee/Shortsword/ExoGladiusGlow").Value;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), Projectile.scale, spriteEffects, 0);
        }
        private void SpawnMeteor(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                var source = player.GetSource_FromThis();
                if (player.CIMod().GlobalFireDelay <= 0)
                {
                    int damage = player.GetWeaponDamage(player.ActiveItem()) * 2;
                    CalamityUtils.ProjectileRain(source, player.Center, 400f, 100f, 500f, 800f, 25f, ModContent.ProjectileType<ExoGladComet>(), damage, 15f, player.whoAmI);
                    player.CIMod().GlobalFireDelay = 3;
                }
            }
        }
    }
}
