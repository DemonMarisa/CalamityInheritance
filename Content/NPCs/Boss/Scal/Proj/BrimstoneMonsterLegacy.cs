using CalamityInheritance.Assets.Sounds;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Core.CIWorlds;
using LAP.Core.MiscDate;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.NPCs.Boss.Scal.Proj
{
    public class BrimstoneMonsterLegacy : CIBossProj
    {
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
            if (!LAPInfo.AnyBossHere)
            {
                Projectile.Kill();
                return;
            }

            int choice = (int)Projectile.ai[1];
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.soundDelay = 1125 - choice * 225;
                SoundEngine.PlaySound(CISounds.BrimstoneMonsterSpawn, Projectile.Center);
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
                SoundEngine.PlaySound(CISounds.BrimstoneMonsterDrone, Projectile.Center);
            }

            bool revenge = CIWorld.Revenge;
            bool death = CIWorld.Death;

            Lighting.AddLight(Projectile.Center, 3f * Projectile.Opacity, 0f, 0f);

            float inertia = (revenge ? 4.5f : 5f) + speedAdd;
            float speed = (revenge ? 1.5f : 1.35f) + speedAdd * 0.25f;
            float minDist = 160f;

            if (Projectile.timeLeft < 90)
                Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 90f, 0f, 1f);
            else
                Projectile.Opacity = MathHelper.Clamp(1f - (Projectile.timeLeft - 35910) / 90f, 0f, 1f);

            int target = (int)Projectile.ai[0];
            if (target >= 0 && Main.player[target].active && !Main.player[target].dead)
            {
                if (Projectile.Distance(Main.player[target].Center) > minDist)
                {
                    Vector2 moveDirection = LAPUtilities.GetVector2(Projectile.Center, Main.player[target].Center);
                    Projectile.velocity = (Projectile.velocity * (inertia - 1f) + moveDirection * speed) / inertia;
                }
            }
            else
            {
                Projectile.ai[0] = Player.FindClosest(Projectile.Center, 1, 1);
                Projectile.netUpdate = true;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => LAPUtilities.CircularHitboxCollision(Projectile.Center, 170f, targetHitbox);

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Request<Texture2D>(Texture).Value;
            Color DrawColor = Color.Red;
            DrawColor *= Projectile.Opacity;

            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, DrawColor, Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
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
            }

            return true;
        }


        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (info.Damage <= 0 || Projectile.Opacity != 1f)
                return;

            target.ScalDebuffs(360, 480, 300);
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
            behindNPCs.Add(index);
        }
    }
}
