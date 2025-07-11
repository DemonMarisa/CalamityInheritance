﻿using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using CalamityMod.Events;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.World;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityInheritance.Core;
using CalamityInheritance.World;
using CalamityMod.Particles;
using Terraria.DataStructures;

namespace CalamityInheritance.NPCs.Boss.SCAL.Proj
{
    public class BrimstoneMonsterLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Boss.Projectiles";

        public static readonly SoundStyle SpawnSound = new("CalamityMod/Sounds/Custom/SCalSounds/BrimstoneMonsterSpawn");
        public static readonly SoundStyle DroneSound = new("CalamityMod/Sounds/Custom/SCalSounds/BrimstoneMonsterDrone");

        private float speedAdd = 0f;
        private float speedLimit = 0f;

        public override void SetDefaults()
        {
            
            Projectile.width = 320;
            Projectile.height = 320;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.hide = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 36000;
            Projectile.Opacity = 0f;
            CooldownSlot = ImmunityCooldownID.Bosses;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(speedAdd);
            writer.Write(Projectile.localAI[0]);
            writer.Write(speedLimit);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            speedAdd = reader.ReadSingle();
            Projectile.localAI[0] = reader.ReadSingle();
            speedLimit = reader.ReadSingle();
        }

        public override void AI()
        {
            if (!CalamityPlayer.areThereAnyDamnBosses)
            {
                Projectile.active = false;
                Projectile.netUpdate = true;
                return;
            }

            int choice = (int)Projectile.ai[1];
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.soundDelay = 1125 - (choice * 225);
                SoundEngine.PlaySound(SpawnSound, Projectile.Center);
                Projectile.localAI[0] += 1f;
                switch (choice)
                {
                    case 0:
                        speedLimit = 10f;
                        break;
                    case 1:
                        speedLimit = 20f;
                        break;
                    case 2:
                        speedLimit = 30f;
                        break;
                    case 3:
                        speedLimit = 40f;
                        break;
                    default:
                        break;
                }
            }

            if (speedAdd < speedLimit)
                speedAdd += 0.04f;

            if (Projectile.soundDelay <= 0 && (choice == 0 || choice == 2))
            {
                Projectile.soundDelay = 420;
                SoundEngine.PlaySound(DroneSound, Projectile.Center);
            }

            bool revenge = CalamityWorld.revenge || BossRushEvent.BossRushActive;
            bool death = CalamityWorld.death || BossRushEvent.BossRushActive;

            Lighting.AddLight(Projectile.Center, 3f * Projectile.Opacity, 0f, 0f);

            float inertia = (revenge ? 4.5f : 5f) + speedAdd;
            float speed = (revenge ? 1.5f : 1.35f) + (speedAdd * 0.25f);
            float minDist = 160f;

            if (NPC.AnyNPCs(ModContent.NPCType<SoulSeekerSupreme>()))
            {
                inertia *= 1.5f;
                speed *= 0.5f;
            }

            if (Projectile.timeLeft < 90)
                Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 90f, 0f, 1f);
            else
                Projectile.Opacity = MathHelper.Clamp(1f - ((Projectile.timeLeft - 35910) / 90f), 0f, 1f);

            int target = (int)Projectile.ai[0];
            if (target >= 0 && Main.player[target].active && !Main.player[target].dead)
            {
                if (Projectile.Distance(Main.player[target].Center) > minDist)
                {
                    Vector2 moveDirection = Projectile.SafeDirectionTo(Main.player[target].Center, Vector2.UnitY);
                    Projectile.velocity = (Projectile.velocity * (inertia - 1f) + moveDirection * speed) / inertia;
                }
            }
            else
            {
                Projectile.ai[0] = Player.FindClosest(Projectile.Center, 1, 1);
                Projectile.netUpdate = true;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => CalamityUtils.CircularHitboxCollision(Projectile.Center, 170f, targetHitbox);

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            lightColor.R = (byte)(255 * Projectile.Opacity);
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override bool CanHitPlayer(Player player)
        {
            // 这一段还是复制的原灾爆改的，详情看原灾吧
            if (Projectile.Opacity < 1f)
                return false;

            bool cannotBeHurt = player.HasIFrames() || player.creativeGodMode;
            if (cannotBeHurt)
                return true;

            if (Colliding(Projectile.Hitbox, player.Hitbox) == false)
                return false;

            if ((bool)Colliding(Projectile.Hitbox, player.Hitbox))
            {
                player.ScalDebuffs(360, 480, 300);

                player.statLife -= Projectile.damage * 3;

                GlowOrbParticle orb = new GlowOrbParticle(player.Center, new Vector2(6, 6).RotatedByRandom(360) * Main.rand.NextFloat(0.3f, 1.1f), false, 60, Main.rand.NextFloat(1.55f, 3.75f), Main.rand.NextBool() ? Color.Red : Color.Lerp(Color.Red, Color.Magenta, 0.5f), true, true);
                GeneralParticleHandler.SpawnParticle(orb);
                if (Main.rand.NextBool())
                {
                    GlowOrbParticle orb2 = new GlowOrbParticle(player.Center, new Vector2(6, 6).RotatedByRandom(360) * Main.rand.NextFloat(0.3f, 1.1f), false, 60, Main.rand.NextFloat(1.55f, 3.75f), Color.Black, false, true, false);
                    GeneralParticleHandler.SpawnParticle(orb2);
                }
            }

            return true;
        }


        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage <= 0 || Projectile.Opacity != 1f)
                return;

            target.ScalDebuffs(360, 480 , 300);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
            behindNPCs.Add(index);
        }
    }
}
