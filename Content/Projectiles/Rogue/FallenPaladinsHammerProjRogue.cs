﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Typeless;
using CalamityMod;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    public class FallenPaladinsHammerProjRogue : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Rogue/RogueTypeFallenPaladinsHammer";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fallen Paladin's Hammer");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.35f, 0.35f, 0f);
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
            }
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[1] += 1f;
                if (Projectile.ai[1] >= 30f)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                Projectile.tileCollide = false;
                float num42 = 16f;
                float num43 = 3.2f;
                Vector2 vector2 = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
                float num44 = Main.player[Projectile.owner].position.X + Main.player[Projectile.owner].width / 2 - vector2.X;
                float num45 = Main.player[Projectile.owner].position.Y + Main.player[Projectile.owner].height / 2 - vector2.Y;
                float num46 = (float)Math.Sqrt(num44 * num44 + num45 * num45);
                if (num46 > 3000f)
                    Projectile.Kill();
                num46 = num42 / num46;
                num44 *= num46;
                num45 *= num46;
                if (Projectile.velocity.X < num44)
                {
                    Projectile.velocity.X = Projectile.velocity.X + num43;
                    if (Projectile.velocity.X < 0f && num44 > 0f)
                        Projectile.velocity.X = Projectile.velocity.X + num43;
                }
                else if (Projectile.velocity.X > num44)
                {
                    Projectile.velocity.X = Projectile.velocity.X - num43;
                    if (Projectile.velocity.X > 0f && num44 < 0f)
                        Projectile.velocity.X = Projectile.velocity.X - num43;
                }
                if (Projectile.velocity.Y < num45)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y + num43;
                    if (Projectile.velocity.Y < 0f && num45 > 0f)
                        Projectile.velocity.Y = Projectile.velocity.Y + num43;
                }
                else if (Projectile.velocity.Y > num45)
                {
                    Projectile.velocity.Y = Projectile.velocity.Y - num43;
                    if (Projectile.velocity.Y > 0f && num45 < 0f)
                        Projectile.velocity.Y = Projectile.velocity.Y - num43;
                }
                if (Main.myPlayer == Projectile.owner)
                {
                    Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                    Rectangle value2 = new Rectangle((int)Main.player[Projectile.owner].position.X, (int)Main.player[Projectile.owner].position.Y, Main.player[Projectile.owner].width, Main.player[Projectile.owner].height);
                    if (rectangle.Intersects(value2))
                        Projectile.Kill();
                }
            }
            Projectile.rotation += 0.5f;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(250, 250, 250, 50);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 240);
            OnHitEffect();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 240);
            OnHitEffect();
        }

        private void OnHitEffect()
        {
            if (Projectile.owner == Main.myPlayer)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FuckYou>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
                if (proj.WithinBounds(Main.maxProjectiles))
                    Main.projectile[proj].DamageType = ModContent.GetInstance<RogueDamageClass>();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
