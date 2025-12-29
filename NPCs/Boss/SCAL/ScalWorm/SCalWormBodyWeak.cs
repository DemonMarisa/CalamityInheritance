using CalamityInheritance.NPCs.Boss.SCAL.Proj;
using CalamityMod;
using CalamityMod.Dusts;
using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
using LAP.Core.MiscDate;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.SCAL.ScalWorm
{
    public class SCalWormBodyWeak : ModNPC
    {
        private bool setAlpha = false;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Brimstone Heart");
        }

        public override void SetDefaults()
        {
            NPC.damage = 0;
            NPC.npcSlots = 5f;
            NPC.width = 20;
            NPC.height = 20;
            NPC.lifeMax = CalamityWorld.revenge ? 1200000 : 1000000;
            if (CalamityWorld.death)
            {
                NPC.lifeMax = 2000000;
            }
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.Calamity().DR = 0.99999f;
            NPC.Calamity().unbreakableDR = true;

            NPC.scale = 1.35f;

            NPC.alpha = 255;
            NPC.chaseable = false;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.canGhostHeal = false;
            NPC.HitSound = SoundID.NPCHit13;
            NPC.DeathSound = SoundID.NPCDeath13;
            NPC.netAlways = true;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            NPC.dontCountMe = true;
        }

        public override void AI()
        {
            if (NPC.ai[2] > 0f)
            {
                NPC.realLife = (int)NPC.ai[2];
            }

            bool flag = false;
            // �����������ͷ��ʧ/����ʱ��

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

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                HandleProjectileAttack();
            }

            UpdatePositionAndRotation();

        }
        public int BrimstoneDarts = 120;
        public void HandleProjectileAttack()
        {
            const float AttackInterval = 900f;
            NPC.localAI[0]++;

            if (NPC.localAI[0] >= AttackInterval)
            {
                NPC.localAI[0] = 0f;

                Vector2 baseDirection = Vector2.Normalize(new Vector2(-1f, -1f));
                for (int i = 0; i < 4; i++)
                {
                    SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/Item/DeadSunRicochet") with { Pitch = -0.65f, Volume = 1.8f }, NPC.Center);
                    Vector2 direction = baseDirection.RotatedBy(MathHelper.TwoPi / 4 * i);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 2f, ProjectileType<BrimstoneBarrageLegacy>(), BrimstoneDarts, 0f, Main.myPlayer);
                }
            }
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
                // ͬ������͸����
                NPC.alpha = parent.alpha;
            }
        }

        public void UpdatePositionAndRotation()
        {
            NPC parent = Main.npc[(int)NPC.ai[1]];
            Vector2 npcCenter = NPC.Center;
            Vector2 parentCenter = parent.Center;

            // ���㳯�򸸽ڵ�ķ���
            Vector2 directionToParent = parentCenter - npcCenter;

            NPC.rotation = directionToParent.ToRotation() + MathHelper.PiOver2;

            // �����븸�ڵ������
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
            if (LAPList.projectileDestroyExceptionList.TrueForAll(x => projectile.type != x) && projectile.extraUpdates < 50 && ProjectileID.Sets.DrawScreenCheckFluff[projectile.type] > 480)
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
        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreKill()
        {
            return false;
        }
    }
}
