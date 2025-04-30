using CalamityInheritance.Buffs.StatDebuffs;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Dusts;
using MonoMod.Core.Utils;
using CalamityInheritance.Buffs.Potions;
using static CalamityInheritance.NPCs.Boss.Yharon.YharonLegacy;
using Terraria.Audio;
using CalamityInheritance.Utilities;
using static CalamityInheritance.NPCs.Boss.SCAL.SupremeCalamitasLegacy;
using CalamityInheritance.NPCs.Boss.SCAL.Proj;
using CalamityInheritance.Content.Items;
using CalamityInheritance.NPCs.Boss.CalamitasClone.Projectiles;
using Terraria.GameContent;

namespace CalamityInheritance.NPCs.Boss.CalamitasClone
{
    [AutoloadBossHead]
    public class CalamitasCloneLegacy : ModNPC
    {

        #region 杂项初始化
        public enum LegacyCCloneAttackType
        {
            fireAbyssalLaser,
            fireFireBall,
            charge,

            PhaseTransition,

            // 召唤招式
            SummonBrother,
            SummonSoulSeeker,
        }
        // 获取NPC实例
        public static NPC LegacyCalamitasClone
        {
            get
            {
                if (CIGlobalNPC.LegacyCalamitasClone == -1)
                    return null;

                return Main.npc[CIGlobalNPC.LegacyCalamitasClone];
            }
        }
        // 普灾的攻击循环
        public static LegacyCCloneAttackType[] AttackCycle =>
            [
            LegacyCCloneAttackType.fireAbyssalLaser,// 这是用来标记的，因为调用的时候会+1，取不到第一个，得取一遍回来才能取到
            LegacyCCloneAttackType.fireAbyssalLaser,
            LegacyCCloneAttackType.charge,
            LegacyCCloneAttackType.fireFireBall,
            LegacyCCloneAttackType.charge,
            ];
        #endregion
        #region 阶段
        public const float stage1LifeRatio = 1f;
        public const float stage2LifeRatio = 0.7f;
        #endregion
        #region 数据
        public int laserDamage = 30;
        public int fireDamage = 40;
        #endregion
        #region 杂项bool
        // 用于在AI的初始化
        public bool initialized = false;
        // 判定是否进入二阶段
        public bool isStage2 = false;
        #endregion
        #region SSD
        public string Gen = "CalamityInheritance/NPCs/Boss/CalamitasClone";
        public static Asset<Texture2D> P1GlowTexture;
        public static Asset<Texture2D> P2Texture;
        public static Asset<Texture2D> P2GlowTexture;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Calamitas");
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            if (!Main.dedServ)
            {
                P1GlowTexture = ModContent.Request<Texture2D>($"{Gen}/CalamitasCloneLegacy_Glow", AssetRequestMode.AsyncLoad);
                P2Texture = ModContent.Request<Texture2D>($"{Gen}/CalamitasCloneLegacy_Phase2", AssetRequestMode.AsyncLoad);
                P2GlowTexture = ModContent.Request<Texture2D>($"{Gen}/CalamitasCloneLegacy_Phase2_Glow", AssetRequestMode.AsyncLoad);
            }
        }
        #endregion
        #region SD
        public override void SetDefaults()
        {
            NPC.damage = 50;
            NPC.npcSlots = 14f;
            NPC.width = 116;
            NPC.height = 172;
            NPC.defense = 15;

            NPC.value = 0f;

            NPC.lifeMax = 100000;
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;

            NPC.knockBackResist = 0f;

            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }

            NPC.buffImmune[BuffID.Ichor] = false;
            NPC.buffImmune[ModContent.BuffType<MarkedforDeath>()] = false;
            NPC.buffImmune[BuffID.Frostburn] = false;
            NPC.buffImmune[BuffID.CursedInferno] = false;
            NPC.buffImmune[BuffID.Daybreak] = false;
            NPC.buffImmune[BuffID.BetsysCurse] = false;
            NPC.buffImmune[BuffID.StardustMinionBleed] = false;
            NPC.buffImmune[BuffID.DryadsWardDebuff] = false;
            NPC.buffImmune[BuffID.Oiled] = false;
            NPC.buffImmune[BuffID.BoneJavelin] = false;
            NPC.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = false;
            NPC.buffImmune[ModContent.BuffType<ArmorCrunch>()] = false;
            NPC.buffImmune[ModContent.BuffType<GodSlayerInferno>()] = false;
            NPC.buffImmune[ModContent.BuffType<HolyFlames>()] = false;
            NPC.buffImmune[ModContent.BuffType<Nightwither>()] = false;
            NPC.buffImmune[ModContent.BuffType<Plague>()] = false;
            NPC.buffImmune[ModContent.BuffType<Shred>()] = false;
            NPC.buffImmune[ModContent.BuffType<WhisperingDeath>()] = false;
            NPC.buffImmune[ModContent.BuffType<SilvaStun>()] = false;
            NPC.buffImmune[ModContent.BuffType<SulphuricPoisoning>()] = false;
            NPC.buffImmune[ModContent.BuffType<StepToolDebuff>()] = false;

            NPC.boss = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.HitSound = SoundID.NPCHit4;
        }
        #endregion
        #region AI
        public override void AI()
        {
            if (NPC.rotation < 0f)
                NPC.rotation += MathHelper.TwoPi;
            else if (NPC.rotation > MathHelper.TwoPi)
                NPC.rotation -= MathHelper.TwoPi; //确保转角一直在2pi内

            if (initialized == false)
            {
                Main.player[NPC.target].Calamity().GeneralScreenShakePower = 4;
                SpawnDust();
                initialized = true;
            }

            // 获取目标
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest(true);

            Player target = Main.player[NPC.target];

            if (Main.slimeRain)
            {
                Main.StopSlimeRain(true);
                CalamityNetcode.SyncWorld();
            }

            ref float attackType = ref NPC.ai[0];
            ref float attackTimer = ref NPC.ai[1];
            ref float currentPhase = ref NPC.ai[2];
            // NPC.ai[3]用于招式选择
            ref float rotationSpeed = ref NPC.CIMod().BossNewAI[1];

            // 进入新的阶段
            float lifeRatio = NPC.life / (float)NPC.lifeMax;

            // Set the whoAmI variable.
            CIGlobalNPC.LegacyCalamitasClone = NPC.whoAmI;
            if (lifeRatio <= stage1LifeRatio && currentPhase == 0f)
            {
                attackTimer = 0;
                SelectNextAttack();
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            if (lifeRatio <= stage2LifeRatio && currentPhase == 1f)
            {
                attackTimer = 0;
                SelectNextAttack();
                currentPhase++;
                NPC.netUpdate = true;
                return;
            }
            // 重置旋转速度
            rotationSpeed = 0.25f;

            switch ((LegacyCCloneAttackType)attackType)
            {
                case LegacyCCloneAttackType.fireAbyssalLaser:
                    DoBehavior_FireAbyssaLaser(target, attackTimer);
                    break;
                case LegacyCCloneAttackType.charge:
                    DoBehavior_Charge(target,ref attackTimer, currentPhase,ref rotationSpeed);
                    break;
                case LegacyCCloneAttackType.fireFireBall:
                    DoBehavior_FireBall(target, attackTimer);
                    break;
            }

            LookAtTarget(target, rotationSpeed);
        }
        #endregion
        #region 技能
        #region 看向目标
        public void LookAtTarget(Player player, float rotationSpeed)
        {
            NPC.rotation = NPC.rotation.AngleLerp(NPC.AngleTo(player.Center) - MathHelper.PiOver2, rotationSpeed);
        }
        #endregion
        #region 发射激光
        public void DoBehavior_FireAbyssaLaser(Player target, float attacktimer)
        {
            // 移动速度
            float velocity = 16f;
            float acceleration = 1f;
            int distanceX = 600;
            int distanceY = 250;

            int totalFireTime = 360;
            int fireDelay =  30;
            // 如果玩家手持真近战武器，那么降低加速度

            int posX = 1;
            if (NPC.Center.X < target.Center.X)
                posX = -1;

            // 尝试悬停在玩家左上方或右上方
            Vector2 destination = new Vector2(target.Center.X + posX * distanceX, target.Center.Y - distanceY);
            // 大于一半时间后向下移动
            if(attacktimer > totalFireTime / 2)
                destination = new Vector2(target.Center.X + posX * distanceX, target.Center.Y + distanceY);
            // 应该在哪
            Vector2 distanceFromDestination = destination - NPC.Center;

            CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, velocity, acceleration, true);

            if (attacktimer % fireDelay == 0)
            {
                SoundEngine.PlaySound(CISoundID.SoundLaserDestroyer, NPC.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // 使用旋转角度计算方向
                    Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation); // 基础方向根据旋转角度
                    direction = direction.SafeNormalize(Vector2.UnitX);

                    int projType = ModContent.ProjectileType<BrimstoneLaser>();
                    // 偏移向量
                    Vector2 offset = new Vector2(0, 50).RotatedBy(NPC.rotation);
                    Vector2 projectileVelocity = direction * 12.5f;
                    Vector2 projectileSpawn = NPC.Center + offset;
                    projectileVelocity = projectileVelocity.RotatedBy(MathHelper.PiOver2);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, projectileVelocity, projType, laserDamage, 0f, Main.myPlayer, 0f, 0f);
                }
            }

            if (attacktimer > totalFireTime)
                SelectNextAttack();
        }
        #endregion
        #region 冲刺
        public bool hasCharge = false;
        public int ChargeCount = 0;
        public void DoBehavior_Charge(Player target, ref float attacktimer, float currentPhase, ref float rotationacc)
        {
            int totalCharge = 2;
            int chargeCount = 25;
            int chargeCooldown = 70;

            if (attacktimer < chargeCount)
                rotationacc = 0.04f;


            if (hasCharge == false)
            {
                float chargeVelocity = 20f;
                chargeVelocity += 1f * currentPhase;

                Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation + MathHelper.PiOver2);
                direction = direction.SafeNormalize(Vector2.UnitX);
                NPC.velocity = direction * chargeVelocity;

                NPC.netUpdate = true;
                SoundEngine.PlaySound(DashSound, NPC.Center);
                hasCharge = true;
            }
            else
            {
                if (attacktimer > chargeCount)
                {
                    rotationacc += 0.15f;

                    NPC.velocity *= 0.96f;

                    if (NPC.velocity.X > -0.1 && NPC.velocity.X < 0.1)
                        NPC.velocity.X = 0f;
                    if (NPC.velocity.Y > -0.1 && NPC.velocity.Y < 0.1)
                        NPC.velocity.Y = 0f;
                }
                if (attacktimer >= chargeCooldown)
                {
                    hasCharge = false;
                    attacktimer = 0;
                    ChargeCount++;
                }
                NPC.netUpdate = true;
            }

            if (ChargeCount > totalCharge - 1)
                SelectNextAttack();
        }
        #endregion
        #region 火球
        public void DoBehavior_FireBall(Player target, float attacktimer)
        {
            int fireBallCount = 4;
            int fireBallDelay = 45;

            // Scal的加速度和速度
            float velocity = 12f;
            float acceleration = 0.24f;

            // 終灾应该在哪
            Vector2 destination = new Vector2(target.Center.X, target.Center.Y - 550f);
            // 离应该在哪的距离
            Vector2 distanceFromDestination = destination - NPC.Center;
            // 移动
            CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, velocity, acceleration, true);


            if (attacktimer % fireBallDelay == 0)
            {
                SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // 使用旋转角度计算方向
                    Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation); // 基础方向根据旋转角度
                    direction = direction.SafeNormalize(Vector2.UnitX);
                    int projType = ModContent.ProjectileType<HellfireballReborn>();
                    // 偏移向量
                    Vector2 offset = new Vector2(0, 50).RotatedBy(NPC.rotation);
                    Vector2 projectileVelocity = direction * 12.5f;
                    Vector2 projectileSpawn = NPC.Center + offset;
                    projectileVelocity = projectileVelocity.RotatedBy(MathHelper.PiOver2);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, projectileVelocity, projType, fireDamage, 0f, Main.myPlayer, 0f, 0f);
                }
            }

            if (attacktimer > fireBallCount * fireBallDelay)
                SelectNextAttack();
        }
        #endregion
        #region 选择下一个攻击
        public void SelectNextAttack(int? Skip = null)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;
            // ai1对应的啥
            // attackType = NPC.ai[0];
            // attackTimer = NPC.ai[1];
            // currentPhase = NPC.ai[2];

            // 置零攻击计时器和选择攻击类型
            NPC.ai[1] = 0f;
            hasCharge = false;

            // 获取当前索引（不重置）
            int currentIndex = (int)NPC.ai[3];

            LegacyCCloneAttackType[] attackCycle = AttackCycle;
            if (Skip == null)
            {
                // 递增索引
                currentIndex++;
                if (currentIndex >= attackCycle.Length)
                    currentIndex = 0;
            }
            else
            {
                currentIndex = (int)Skip;
                if (currentIndex >= attackCycle.Length)
                    currentIndex = 0;
            }
            // 更新索引和攻击类型
            NPC.ai[3] = currentIndex;
            NPC.ai[0] = (int)attackCycle[currentIndex];

            // 多人游戏同步
            if (Main.netMode == NetmodeID.Server)
                NPC.netUpdate = true;
        }
        #endregion
        #endregion
        #region 召唤粒子环
        public void SpawnDust()
        {
            int dustAmt = 50;
            int random = 10;

            for (int j = 0; j < 10; j++)
            {
                random += j;
                int dustAmtSpawned = 0;
                int scale = random * 12;
                float dustPositionX = NPC.Center.X - (scale / 2);
                float dustPositionY = NPC.Center.Y - (scale / 2);
                while (dustAmtSpawned < dustAmt)
                {
                    float dustVelocityX = Main.rand.Next(-random, random);
                    float dustVelocityY = Main.rand.Next(-random, random);
                    float dustVelocityScalar = random * 2f;
                    float dustVelocity = (float)Math.Sqrt(dustVelocityX * dustVelocityX + dustVelocityY * dustVelocityY);
                    dustVelocity = dustVelocityScalar / dustVelocity;
                    dustVelocityX *= dustVelocity;
                    dustVelocityY *= dustVelocity;
                    int dust = Dust.NewDust(new Vector2(dustPositionX, dustPositionY), scale, scale, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].position.X = NPC.Center.X;
                    Main.dust[dust].position.Y = NPC.Center.Y;
                    Main.dust[dust].position.X += Main.rand.Next(-10, 11);
                    Main.dust[dust].position.Y += Main.rand.Next(-10, 11);
                    Main.dust[dust].velocity.X = dustVelocityX;
                    Main.dust[dust].velocity.Y = dustVelocityY;
                    Main.dust[dust].scale = 3f;
                    dustAmtSpawned++;
                }
            }
        }
        #endregion
        #region 飞走
        public void DeSpawn(float attacktimer)
        {
            //奶奶的不要走之前创思我好不好
            NPC.damage = 0;
            if (attacktimer < 60)
            {
                NPC.velocity *= 0.96f;
            }
            else
            {
                NPC.velocity.X *= 0.96f;
                NPC.velocity.Y -= 0.4f;
                if (attacktimer == 90)
                {
                    NPC.active = false;
                }
                NPC.Opacity -= 0.04f;
            }
        }
        #endregion
        #region 绘制
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            // 在图鉴中使用默认绘制
            if (NPC.IsABestiaryIconDummy)
                return true;

            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Texture2D texture = isStage2 ? P2Texture.Value : TextureAssets.Npc[NPC.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount[NPC.type] / 2);
            Color white = Color.White;
            float colorLerpAmt = 0.5f;
            int afterimageAmt = 7;

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int i = 1; i < afterimageAmt; i += 2)
                {
                    Color afterimageColor = drawColor;
                    afterimageColor = Color.Lerp(afterimageColor, white, colorLerpAmt);
                    afterimageColor = NPC.GetAlpha(afterimageColor);
                    afterimageColor *= (afterimageAmt - i) / 15f;
                    Vector2 offset = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
                    offset -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    offset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture, offset, NPC.frame, afterimageColor, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
                }
            }

            Vector2 npcOffset = NPC.Center - screenPos;
            npcOffset -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
            npcOffset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
            spriteBatch.Draw(texture, npcOffset, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, origin, NPC.scale, spriteEffects, 0f);

            texture = isStage2 ? P2GlowTexture.Value : P1GlowTexture.Value;
            Color color = Color.Lerp(Color.White, Color.Red, 0.5f);

            if (CalamityConfig.Instance.Afterimages)
            {
                for (int i = 1; i < afterimageAmt; i++)
                {
                    Color extraAfterimageColor = color;
                    extraAfterimageColor = Color.Lerp(extraAfterimageColor, white, colorLerpAmt);
                    extraAfterimageColor *= (afterimageAmt - i) / 15f;
                    Vector2 offset = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
                    offset -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                    offset += origin * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                    spriteBatch.Draw(texture, offset, NPC.frame, extraAfterimageColor, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);
                }
            }

            spriteBatch.Draw(texture, npcOffset, NPC.frame, color, NPC.rotation, origin, NPC.scale, spriteEffects, 0f);

            return false;
        }
        #endregion
    }
}
