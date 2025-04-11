using CalamityInheritance.System.DownedBoss;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Dusts;
using CalamityMod.Events;
using CalamityMod.NPCs;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.SCAL.ScalWorm
{
    public class SCalWormHead : ModNPC
    {
        public const int minLength = 51;
        public const int maxLength = 52;
        public float passedVar = 0f;
        public static readonly SoundStyle SepulcherSummonSound = new("CalamityMod/Sounds/Custom/SCalSounds/SepulcherSpawn");
        public int maxLife = CalamityWorld.death ? 2000000 : CalamityWorld.revenge ? 1200000 : 1000000;
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 5f;
            NPC.width = 62; //324
            NPC.height = 78; //216
            NPC.defense = 0;
            NPC.Calamity().DR = 0.99999f;
            NPC.Calamity().unbreakableDR = true;
            NPC.lifeMax = maxLife;

            NPC.aiStyle = -1; //new
            AIType = -1; //new

            NPC.knockBackResist = 0f;
            NPC.scale = 1.35f;

            NPC.alpha = 255;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }

            NPC.chaseable = false;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.netAlways = true;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
        public bool TailSpawned = false;
        public override void AI()
        {
            NPC.damage = 0;

            CIGlobalNPC.LegacySCalWorm = NPC.whoAmI;
            // 目标查找
            if (!Main.player[NPC.target].active || Main.player[NPC.target].dead)
                NPC.TargetClosest(true);

            Player player = Main.player[NPC.target];

            // 身体生成逻辑
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                HandleWormBodySpawning(player);
            }

            // 存在状态检查
            HandleExistenceConditions();

            // 我草，我不管了，懒了
            #region 灾坟虫移动
            // 移动逻辑
            Vector2 vector18 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
            float num191 = Main.player[NPC.target].position.X + Main.player[NPC.target].width / 2;
            float num192 = Main.player[NPC.target].position.Y + Main.player[NPC.target].height / 2;
            float num188 = BossRushEvent.BossRushActive ? 12.5f : 10f;
            float num189 = BossRushEvent.BossRushActive ? 0.125f : 0.1f;

            float num48 = num188 * 1.3f;
            float num49 = num188 * 0.7f;
            float num50 = NPC.velocity.Length();
            if (num50 > 0f)
            {
                if (num50 > num48)
                {
                    NPC.velocity.Normalize();
                    NPC.velocity *= num48;
                }
                else if (num50 < num49)
                {
                    NPC.velocity.Normalize();
                    NPC.velocity *= num49;
                }
            }

            num191 = (int)(num191 / 16f) * 16;
            num192 = (int)(num192 / 16f) * 16;
            vector18.X = (int)(vector18.X / 16f) * 16;
            vector18.Y = (int)(vector18.Y / 16f) * 16;
            num191 -= vector18.X;
            num192 -= vector18.Y;
            float num193 = (float)Math.Sqrt(num191 * num191 + num192 * num192);
            float num196 = Math.Abs(num191);
            float num197 = Math.Abs(num192);
            float num198 = num188 / num193;
            num191 *= num198;
            num192 *= num198;
            if (NPC.velocity.X > 0f && num191 > 0f || NPC.velocity.X < 0f && num191 < 0f || NPC.velocity.Y > 0f && num192 > 0f || NPC.velocity.Y < 0f && num192 < 0f)
            {
                if (NPC.velocity.X < num191)
                {
                    NPC.velocity.X = NPC.velocity.X + num189;
                }
                else
                {
                    if (NPC.velocity.X > num191)
                    {
                        NPC.velocity.X = NPC.velocity.X - num189;
                    }
                }
                if (NPC.velocity.Y < num192)
                {
                    NPC.velocity.Y = NPC.velocity.Y + num189;
                }
                else
                {
                    if (NPC.velocity.Y > num192)
                    {
                        NPC.velocity.Y = NPC.velocity.Y - num189;
                    }
                }
                if (Math.Abs(num192) < num188 * 0.2 && (NPC.velocity.X > 0f && num191 < 0f || NPC.velocity.X < 0f && num191 > 0f))
                {
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y = NPC.velocity.Y + num189 * 2f;
                    }
                    else
                    {
                        NPC.velocity.Y = NPC.velocity.Y - num189 * 2f;
                    }
                }
                if (Math.Abs(num191) < num188 * 0.2 && (NPC.velocity.Y > 0f && num192 < 0f || NPC.velocity.Y < 0f && num192 > 0f))
                {
                    if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X = NPC.velocity.X + num189 * 2f; //changed from 2
                    }
                    else
                    {
                        NPC.velocity.X = NPC.velocity.X - num189 * 2f; //changed from 2
                    }
                }
            }
            else
            {
                if (num196 > num197)
                {
                    if (NPC.velocity.X < num191)
                    {
                        NPC.velocity.X = NPC.velocity.X + num189 * 1.1f; //changed from 1.1
                    }
                    else if (NPC.velocity.X > num191)
                    {
                        NPC.velocity.X = NPC.velocity.X - num189 * 1.1f; //changed from 1.1
                    }
                    if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < num188 * 0.5)
                    {
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y + num189;
                        }
                        else
                        {
                            NPC.velocity.Y = NPC.velocity.Y - num189;
                        }
                    }
                }
                else
                {
                    if (NPC.velocity.Y < num192)
                    {
                        NPC.velocity.Y = NPC.velocity.Y + num189 * 1.1f;
                    }
                    else if (NPC.velocity.Y > num192)
                    {
                        NPC.velocity.Y = NPC.velocity.Y - num189 * 1.1f;
                    }
                    if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < num188 * 0.5)
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X + num189;
                        }
                        else
                        {
                            NPC.velocity.X = NPC.velocity.X - num189;
                        }
                    }
                }
            }
            #endregion
            // 更新旋转
            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public void HandleWormBodySpawning(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!TailSpawned)
                {
                    int Previous = NPC.whoAmI;
                    for (int i = 0; i < maxLength; i++)
                    {
                        int lol;
                        if (i >= 0 && i < minLength && i % 2 == 1)
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<SCalWormBodyWeak>(), NPC.whoAmI);
                            Main.npc[lol].localAI[0] += passedVar;
                            passedVar += 36f;
                        }
                        else if (i >= 0 && i < minLength)
                        {
                            lol = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<SCalWormBody>(), NPC.whoAmI);
                            // localAI[3] 绘制哪个版本的体节
                            Main.npc[lol].localAI[3] = i;
                        }
                        else
                            lol = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<SCalWormTail>(), NPC.whoAmI);

                        Main.npc[lol].realLife = NPC.whoAmI;
                        // AI2，传递RealLife
                        Main.npc[lol].ai[2] = NPC.whoAmI;
                        // AI1，传递是否死亡
                        Main.npc[lol].ai[1] = Previous;
                        Previous = lol;
                    }
                    TailSpawned = true;
                }
                if (!NPC.active && Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                }
            }
        }

        // 检测是否应该存在
        public void HandleExistenceConditions()
        {
            bool shouldDespawn = Main.player[NPC.target].dead || !NPC.AnyNPCs(ModContent.NPCType<SCalWormHeart>()) || CIGlobalNPC.LegacySCal < 0 || !Main.npc[CIGlobalNPC.LegacySCal].active;

            if (shouldDespawn)
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.checkDead();
                return;
            }
            else
            {
                NPC.alpha = Math.Max(NPC.alpha - 42, 0);
            }
        }
        // 直接将所有蠕虫设置为不活跃
        public void DespawnAllWormParts()
        {
            SoundEngine.PlaySound(SepulcherSummonSound, NPC.position);
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                int[] wormTypes = { NPC.type, ModContent.NPCType<SCalWormBody>(), ModContent.NPCType<SCalWormBodyWeak>(), ModContent.NPCType<SCalWormTail>() };

                if (wormTypes.Contains(Main.npc[i].type))
                    Main.npc[i].active = false;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Texture2D texture2D15 = TextureAssets.Npc[NPC.type].Value;
            Vector2 vector11 = texture2D15.Size() / 2;

            Vector2 vector43 = NPC.Center - Main.screenPosition;

            vector43 -= texture2D15.Size() * NPC.scale / 2f;
            vector43 += vector11 * NPC.scale + new Vector2(0f, 4f + NPC.gfxOffY);
            spriteBatch.Draw(texture2D15, vector43, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

            texture2D15 = ModContent.Request<Texture2D>($"CalamityInheritance/NPCs/Boss/SCAL/ScalWorm/SCalWormHeadGlow").Value;
            Color color37 = Color.Lerp(Color.White, Color.Red, 0.5f);

            spriteBatch.Draw(texture2D15, vector43, NPC.frame, color37, NPC.rotation, vector11, NPC.scale, spriteEffects, 0f);

            return false;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (CalamityLists.projectileDestroyExceptionList.TrueForAll(x => projectile.type != x) && projectile.extraUpdates < 50)
            {
                if (projectile.penetrate == -1 && !projectile.minion)
                {
                    projectile.penetrate = 1;
                }
                else if (projectile.penetrate >= 1)
                {
                    projectile.penetrate = 1;
                }
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[dust].velocity *= 3f;
                    if (Main.rand.NextBool(2))
                    {
                        Main.dust[dust].scale = 0.5f;
                        Main.dust[dust].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 5f;
                    dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[dust].velocity *= 2f;
                }
            }
        }
        public override void OnKill()
        {
            SoundEngine.PlaySound(SepulcherSummonSound, NPC.position);
        }
    }
}
