using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.UI;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Cooldowns;
using CalamityMod.Dusts;
using CalamityMod.Items.Armor.Bloodflare;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.CIPlayer
{
    public partial class CalamityInheritancePlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Player.dead)
                return;

            CalamityPlayer modPlayer = Player.Calamity();
            if (CalamityInheritanceKeybinds.BoCLoreTeleportation.JustPressed && (BoCLoreTeleportation || PanelsBoCLoreTeleportation) && Main.myPlayer == Player.whoAmI)
            {
                if (!Player.chaosState)
                {
                    Vector2 vector31;
                    vector31.X = Main.mouseX + Main.screenPosition.X;
                    if (Player.gravDir == 1f)
                    {
                        vector31.Y = Main.mouseY + Main.screenPosition.Y - Player.height;
                    }
                    else
                    {
                        vector31.Y = Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
                    }
                    vector31.X -= Player.width / 2;
                    if (vector31.X > 50f && vector31.X < Main.maxTilesX * 16 - 50 && vector31.Y > 50f && vector31.Y < Main.maxTilesY * 16 - 50)
                    {
                        int tileX = (int)(vector31.X / 16f);
                        int tileY = (int)(vector31.Y / 16f);
                        if ((Main.tile[tileX, tileY].WallType != 87 || tileY <= Main.worldSurface || NPC.downedPlantBoss) && !Collision.SolidCollision(vector31, Player.width, Player.height))
                        {
                            Player.Teleport(vector31, 1, 0);
                            NetMessage.SendData(MessageID.TeleportEntity, -1, -1, null, 0, Player.whoAmI, vector31.X, vector31.Y, 1, 0, 0);
                            Player.AddBuff(BuffID.ChaosState, 480, true);
                            Player.AddBuff(BuffID.Confused, 150, true);
                        }
                    }
                }
            }
            if (CalamityKeybinds.ArmorSetBonusHotKey.JustPressed)
            {
                if (AuricbloodflareRangedSoul && !Player.HasCooldown(BloodflareRangedSet.ID))
                {
                    if (Player.whoAmI == Main.myPlayer)
                        Player.AddCooldown(BloodflareRangedSet.ID, 1800);

                    SoundEngine.PlaySound(BloodflareHeadRanged.ActivationSound, Player.Center);
                    for (int d = 0; d < 64; d++)
                    {
                        int dust = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 16f), Player.width, Player.height - 16, (int)CalamityDusts.Necroplasm, 0f, 0f, 0, default, 1f);
                        Main.dust[dust].velocity *= 3f;
                        Main.dust[dust].scale *= 1.15f;
                    }
                    int dustAmt = 36;
                    for (int d = 0; d < dustAmt; d++)
                    {
                        Vector2 source = Vector2.Normalize(Player.velocity) * new Vector2(Player.width / 2f, Player.height) * 0.75f;
                        source = source.RotatedBy((double)((d - (dustAmt / 2 - 1)) * MathHelper.TwoPi / dustAmt), default) + Player.Center;
                        Vector2 dustVel = source - Player.Center;
                        int phanto = Dust.NewDust(source + dustVel, 0, 0, (int)CalamityDusts.Necroplasm, dustVel.X * 1.5f, dustVel.Y * 1.5f, 100, default, 1.4f);
                        Main.dust[phanto].noGravity = true;
                        Main.dust[phanto].noLight = true;
                        Main.dust[phanto].velocity = dustVel;
                    }
                    float spread = 45f * 0.0174f;
                    double startAngle = Math.Atan2(Player.velocity.X, Player.velocity.Y) - spread / 2;
                    double deltaAngle = spread / 8f;
                    double offsetAngle;

                    int damage = (int)Player.GetTotalDamage<RangedDamageClass>().ApplyTo(300f);

                    if (Player.whoAmI == Main.myPlayer)
                    {
                        var source = Player.GetSource_Misc("1");
                        for (int i = 0; i < 8; i++)
                        {
                            float ai1 = Main.rand.NextFloat() + 0.5f;
                            float randomSpeed = Main.rand.Next(1, 7);
                            float randomSpeed2 = Main.rand.Next(1, 7);
                            offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                            int soul = Projectile.NewProjectile(source, Player.Center.X, Player.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f) + randomSpeed, ModContent.ProjectileType<BloodflareSoulold>(), damage, 0f, Player.whoAmI, 0f, ai1);
                            if (soul.WithinBounds(Main.maxProjectiles))
                                Main.projectile[soul].DamageType = DamageClass.Generic;
                            int soul2 = Projectile.NewProjectile(source, Player.Center.X, Player.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f) + randomSpeed2, ModContent.ProjectileType<BloodflareSoulold>(), damage, 0f, Player.whoAmI, 0f, ai1);
                            if (soul2.WithinBounds(Main.maxProjectiles))
                                Main.projectile[soul2].DamageType = DamageClass.Generic;
                        }
                    }
                }
            }
            if (CalamityInheritanceKeybinds.AegisHotKey.JustPressed)
            {
                if (ElysianAegis)
                {
                    ElysianGuard = !ElysianGuard;
                }
            }
            if (CalamityInheritanceKeybinds.AstralArcanumUIHotkey.JustPressed && AstralArcanumEffect && !CalamityPlayer.areThereAnyDamnBosses)
            {
                AstralArcanumUI.Toggle();
            }
            if(CalamityInheritanceKeybinds.QOLUIHotKey.JustPressed)
            {
                DraedonsPanelUI.Active = !DraedonsPanelUI.Active;
            }
        }
    }
}
