using CalamityInheritance.Core.CIWorlds;
using LAP.Core.IDSets;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.NPCs.Boss.Scal.ScalWorm
{
    public class SCalWormBody : ModNPC
    {
        public bool setAlpha = false;
        public int maxLife = CIWorld.Death ? 2000000 : CIWorld.Revenge ? 1200000 : 1000000;
        public static Asset<Texture2D> AltTexture;
        public static Asset<Texture2D> AltTextureGlow;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers nPCBestiaryDrawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers();
            nPCBestiaryDrawModifiers.Hide = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = nPCBestiaryDrawModifiers;
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            //注释死完了

            // ��������
            NPC.damage = 0; //70
            NPC.npcSlots = 5f;
            NPC.width = 20; //28
            NPC.height = 20; //28
            NPC.defense = 0;
            NPC.SetDR(0.00001f);
            NPC.LAP().unbreakableDR = true;
            NPC.lifeMax = maxLife;

            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.scale = 1.35f;
            NPC.alpha = 255;
            NPC.chaseable = false;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.netAlways = true;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.dontCountMe = true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override void AI()
        {
            if (NPC.ai[2] > 0f)
            {
                NPC.realLife = (int)NPC.ai[2];
            }
            bool shouldDie = false;
            if (NPC.ai[1] <= 0f)
            {
                shouldDie = true;
            }
            else if (Main.npc[(int)NPC.ai[1]].life <= 0 || !Main.npc[(int)NPC.ai[1]].active || NPC.life <= 0)
            {
                shouldDie = true;
            }
            if (shouldDie)
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.checkDead();
                return;
            }
            ProcessAlpha();
            UpdatePositionAndRotation();
        }
        public void ProcessAlpha()
        {
            // ��ȡ����NPC�������Boss��ͷ��
            NPC parent = Main.npc[(int)NPC.ai[1]];

            if (parent.alpha < 128 && !setAlpha)
            {
                // ����Ч��
                if (NPC.alpha != 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        int dust = Dust.NewDust(NPC.Center, NPC.width, NPC.height, DustID.TheDestroyer, 0f, 0f, 100, default, 2f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].noLight = true;
                    }
                }

                NPC.alpha = Math.Max(NPC.alpha - 42, 0);
                setAlpha = NPC.alpha <= 0;
            }
            else
            {
                NPC.alpha = parent.alpha;
            }
        }

        public void UpdatePositionAndRotation()
        {
            NPC parent = Main.npc[(int)NPC.ai[1]];
            Vector2 npcCenter = NPC.Center;
            Vector2 parentCenter = parent.Center;

            Vector2 directionToParent = parentCenter - npcCenter;

            NPC.rotation = directionToParent.ToRotation() + MathHelper.PiOver2;
            float distanceToParent = directionToParent.Length();
            float maintainDistance = NPC.width / 2f + parent.width / 2f;

            if (distanceToParent > maintainDistance && NPC.ai[1] < Main.npc.Length)
            {
                Vector2 movement = directionToParent * ((distanceToParent - maintainDistance) / distanceToParent);
                NPC.position += movement;
            }

            // ����������
            NPC.spriteDirection = (directionToParent.X > 0f).ToDirectionInt();
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (!LAPIDSet.ProtectedProj.Contains(projectile.type) && projectile.timeLeft > 2 && !projectile.minion && ProjectileID.Sets.DrawScreenCheckFluff[projectile.type] < 481)
            {
                if (projectile.penetrate == -1)
                {
                    projectile.penetrate = 1;
                }
                else if (projectile.penetrate >= 1)
                {
                    projectile.penetrate = 1;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Texture2D texture2D15 = NPC.localAI[3] / 2f % 2f == 0f ? Request<Texture2D>($"{Texture}Alt").Value : TextureAssets.Npc[NPC.type].Value;
            Vector2 vector11 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / 2);

            Vector2 vector43 = NPC.Center - Main.screenPosition;
            vector43 -= new Vector2(texture2D15.Width, texture2D15.Height) * NPC.scale / 2f;
            vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
            spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

            if (NPC.localAI[3] / 2f % 2f == 0f)
            {
                texture2D15 = Request<Texture2D>($"{Texture}AltGlow").Value;
                Color color37 = Color.Lerp(Color.White, Color.Red, 0.5f);

                spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);
            }

            return false;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreKill()
        {
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LifeDrain, 0f, 0f, 100, default, 2f);
                    Main.dust[dust].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[dust].scale = 0.5f;
                        Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LifeDrain, 0f, 0f, 100, default, 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 5f;
                    dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.LifeDrain, 0f, 0f, 100, default, 2f);
                    Main.dust[dust].velocity *= 2f;
                }
            }
        }
    }
}
