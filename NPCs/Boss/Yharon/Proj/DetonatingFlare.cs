using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework.Graphics;
using CalamityMod;
using Terraria.GameContent;
using LAP.Content.Configs;

namespace CalamityInheritance.NPCs.Boss.Yharon.Proj
{
    public class DetonatingFlare : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.MPAllowedEnemies[Type] = true;
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.damage = 100;
            NPC.width = 64;
            NPC.height = 56;
            NPC.lifeMax = 3000;
            NPC.defense = 20;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit52;
            NPC.DeathSound = SoundID.NPCDeath55;
            NPC.dontTakeDamage = true;
            NPC.alpha = 255;
        }

        public override void AI()
        {
            #region 淡入淡出与基础信息
            if (NPC.alpha > 0)
                NPC.alpha -= 15;
            if (NPC.alpha < 0)
                NPC.alpha = 0;

            //确保转角一直在2pi内
            if (NPC.rotation < 0f)
                NPC.rotation += MathHelper.TwoPi;
            else if (NPC.rotation > MathHelper.TwoPi)
                NPC.rotation -= MathHelper.TwoPi;

            // 瞄准目标
            if (NPC.target < 0 || NPC.target == Main.maxPlayers || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest();

            Player target = Main.player[NPC.target];

            if(NPC.ai[3] == 0)
            {
                NPC.velocity = new Vector2(2f, 0).RotatedByRandom(MathHelper.TwoPi);
                NPC.ai[3]++;
            }

            #endregion
            #region 攻击
            ref float attackTimer = ref NPC.ai[1];
            ref float attackTimer2 = ref NPC.ai[2];
            attackTimer++;
            // 算朝向
            int fireBallTimer = 150;
            int fireBallTimer2 = 180;
            int fireBallTimer3 = 210;
            Vector2 distanceFromDestination = target.Center - NPC.Center;

            NPC.ai[0] = 0.08f;
            if(attackTimer < fireBallTimer3)
            {
                CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, 8f, 0.18f, true);
            }
            if (attackTimer == fireBallTimer || attackTimer == fireBallTimer2 || attackTimer == fireBallTimer3)
            {
                Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation);
                for (int i = 0; i < 4; i++)
                {
                    direction = Vector2.UnitX.RotatedBy(NPC.rotation + i * MathHelper.PiOver2);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 12f, ProjectileID.CultistBossFireBall, 100, 0f);
                }
            }
            if (attackTimer > fireBallTimer3)
            {
                NPC.dontTakeDamage = false;
                attackTimer2++;
                if (attackTimer2 < 60)
                    NPC.velocity *= 0.99f;
                else
                    NPC.ai[0] = 0f;
                if (attackTimer2 == 60)
                {
                    // 向前根据旋转冲刺
                    Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation);
                    direction = direction.SafeNormalize(Vector2.UnitX);
                    // 冲刺速度
                    float chargeVelocity = 8f;
                    // 向前冲刺
                    NPC.velocity = direction * chargeVelocity;
                }
                if (attackTimer2 < 120 && attackTimer2 > 60)
                    NPC.velocity *= 1.03f;

            }

            NPC.rotation = NPC.rotation.AngleLerp(NPC.AngleTo(target.Center), NPC.ai[0]);
            CIFunction.BetterAddLight(NPC.Center, Color.Orange);
            #endregion
        }
        public override Color? GetAlpha(Color drawColor)
        {
            return new Color(255, Main.DiscoG, 53, 0);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
            Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
            Color color36 = new Color(255, Main.DiscoG, 53, 0);
            float amount9 = 0.5f;
            int num153 = 10;

            if (!LAPConfig.Instance.PerformanceMode)
            {
                for (int num155 = 1; num155 < num153; num155 += 2)
                {
                    Color color38 = drawColor;
                    color38 = Color.Lerp(color38, color36, amount9);
                    color38 = NPC.GetAlpha(color38);
                    color38 *= (num153 - num155) / 15f;
                    Vector2 vector41 = NPC.oldPos[num155] + new Vector2(NPC.width, NPC.height) / 2f - Main.screenPosition;
                    vector41 -= new Vector2(texture2D15.Width, (texture2D15.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f;
                    vector41 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
                    spriteBatch.Draw(texture2D15, vector41, NPC.frame, color38, NPC.rotation + MathHelper.Pi, vector11, NPC.scale, 0, 0f);
                }
            }

            Vector2 vector43 = NPC.Center - Main.screenPosition;
            vector43 -= new Vector2(texture2D15.Width, texture2D15.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
            vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
            spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation + MathHelper.Pi, vector11, NPC.scale, 0, 0f);
            return false;
        }
    }
}
