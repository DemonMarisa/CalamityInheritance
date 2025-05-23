﻿using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Content.Items.Weapons.Magic;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class StratusSphereHold : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        public override LocalizedText DisplayName => CalamityUtils.GetItemName<StratusSphere>();
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.65f, 0f, 0.1f);

            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;

            float piConditional = 0f;
            Vector2 playerRotate = player.RotatedRelativePoint(player.MountedCenter, true);
            if (Projectile.spriteDirection == -1)
            {
                piConditional = MathHelper.Pi;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 3)
            {
                Projectile.frame = 0;
            }
            Projectile.ai[0] += 1f;
            int aiSoundDelay = 0;
            if (Projectile.ai[0] >= 240f)
            {
                aiSoundDelay++;
            }
            if (Projectile.ai[0] >= 480f)
            {
                aiSoundDelay++;
            }
            int soundDelayer = 60;
            int soundDelayMult = 2;
            Projectile.ai[1] -= 1f;
            bool isActive = false;
            if (Projectile.ai[1] <= 0f)
            {
                Projectile.ai[1] = soundDelayer - soundDelayMult * aiSoundDelay;
                isActive = true;
            }
            bool canUseItem = !player.CantUseHoldout();
            if (Projectile.localAI[0] > 0f)
            {
                Projectile.localAI[0] -= 1f;
            }
            int manaCost = (int)(20f * player.manaCost);
            if (Projectile.localAI[1] == 0f)
            {
                if (player.statMana < manaCost)
                {
                    if (player.manaFlower)
                    {
                        player.QuickMana();
                        if (player.statMana >= manaCost)
                        {
                            player.manaRegenDelay = (int)player.maxRegenDelay;
                            player.statMana -= manaCost;
                        }
                        else
                        {
                            Projectile.Kill();
                            isActive = false;
                        }
                    }
                    else
                    {
                        Projectile.Kill();
                        isActive = false;
                    }
                }
                else
                {
                    if (player.statMana >= manaCost)
                    {
                        player.statMana -= manaCost;
                        player.manaRegenDelay = (int)player.maxRegenDelay;
                    }
                }
                Projectile.localAI[1] += 1f;
                Projectile.soundDelay = soundDelayer - soundDelayMult * aiSoundDelay;

                if (isActive)
                    SoundEngine.PlaySound(SoundID.Item117, Projectile.Center);
            }
            else if (Projectile.soundDelay <= 0 && canUseItem)
            {
                if (player.statMana < manaCost)
                {
                    if (player.manaFlower)
                    {
                        player.QuickMana();
                        if (player.statMana >= manaCost)
                        {
                            player.manaRegenDelay = (int)player.maxRegenDelay;
                            player.statMana -= manaCost;
                        }
                        else
                        {
                            Projectile.Kill();
                            isActive = false;
                        }
                    }
                    else
                    {
                        Projectile.Kill();
                        isActive = false;
                    }
                }
                else
                {
                    if (player.statMana >= manaCost)
                    {
                        player.statMana -= manaCost;
                        player.manaRegenDelay = (int)player.maxRegenDelay;
                    }
                }
                Projectile.soundDelay = soundDelayer - soundDelayMult * aiSoundDelay;
                if (Projectile.ai[0] != 1f && isActive)
                {
                    SoundEngine.PlaySound(SoundID.Item117, Projectile.Center);
                }
                Projectile.localAI[0] = 12f;
            }
            if (isActive && Main.myPlayer == Projectile.owner)
            {
                float coreVelocity = 7f;
                int weaponDamage2 = player.GetWeaponDamage(player.ActiveItem());
                float weaponKnockback2 = player.ActiveItem().knockBack;
                if (canUseItem)
                {
                    weaponKnockback2 = player.GetWeaponKnockback(player.ActiveItem(), weaponKnockback2);
                    float scaleFactor12 = player.ActiveItem().shootSpeed * Projectile.scale;
                    Vector2 playerRotateCopy = playerRotate;
                    Vector2 projSpawnDirection = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - playerRotateCopy;
                    if (player.gravDir == -1f)
                    {
                        projSpawnDirection.Y = Main.screenHeight - Main.mouseY + Main.screenPosition.Y - playerRotateCopy.Y;
                    }
                    Vector2 projSpawnDirectNormalize = Vector2.Normalize(projSpawnDirection);
                    if (float.IsNaN(projSpawnDirectNormalize.X) || float.IsNaN(projSpawnDirectNormalize.Y))
                    {
                        projSpawnDirectNormalize = -Vector2.UnitY;
                    }
                    projSpawnDirectNormalize *= scaleFactor12;
                    if (projSpawnDirectNormalize.X != Projectile.velocity.X || projSpawnDirectNormalize.Y != Projectile.velocity.Y)
                    {
                        Projectile.netUpdate = true;
                    }
                    Projectile.velocity = projSpawnDirectNormalize * 0.55f;
                    Vector2 normalCoreVel = Vector2.Normalize(Projectile.velocity) * coreVelocity;
                    if (float.IsNaN(normalCoreVel.X) || float.IsNaN(normalCoreVel.Y))
                    {
                        normalCoreVel = -Vector2.UnitY;
                    }
                    Vector2 randomSpawnOffset = playerRotateCopy + Utils.RandomVector2(Main.rand, 0, 0);
                    int StratusSphereProj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), randomSpawnOffset.X, randomSpawnOffset.Y, normalCoreVel.X, normalCoreVel.Y, ModContent.ProjectileType<StratusSphereProj>(), weaponDamage2, weaponKnockback2, Projectile.owner, 0f, 0f);
                }
                else
                {
                    Projectile.Kill();
                }
            }
            Vector2 offset = Projectile.velocity.SafeNormalize(Vector2.Zero) * 25f;
            Projectile.position = player.RotatedRelativePoint(player.MountedCenter, true) - Projectile.Size / 2f + offset;
            Projectile.rotation = Projectile.velocity.ToRotation() + piConditional;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2((double)(Projectile.velocity.Y * Projectile.direction), (double)(Projectile.velocity.X * Projectile.direction));
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture2D13 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int framing = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type];
            int y6 = framing * Projectile.frame;
            Vector2 origin = new Vector2(11f, 12f);
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"{GenericProjRoute.ProjRoute}/Magic/StratusSphereHoldGlow").Value, Projectile.Center  - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(new Rectangle(0, y6, texture2D13.Width, framing)), Color.White, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
        }

        public override bool? CanDamage() => false;
    }
}
