using System.IO;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.NPCs.Boss.CalamitasClone.Projectiles;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Events;
using CalamityMod.Particles;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Core.Utils;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.NPCs.Boss.CalamitasClone.Brothers
{
    [AutoloadBossHead]
    public class CataclysmLegacy : ModNPC
    {
        #region 杂项初始化
        public enum LegacyCataclysmAttackType
        {
            fireProj,
            charge,
        }

        public static LegacyCataclysmAttackType[] AttackCycle =>
            [
            LegacyCataclysmAttackType.charge,// 这是用来标记的，因为调用的时候会+1，取不到第一个，得取一遍回来才能取到
            LegacyCataclysmAttackType.fireProj,
            ];
        #endregion
        #region 数据
        public int fireDamage = 30;
        #endregion
        #region SSD
        public static string CalSoundPath => "CalamityMod/Sounds/Custom";
        public static string CalScalSoundPath => $"{CalSoundPath}/SCalSounds";
        public static readonly SoundStyle DashSound = new($"{CalScalSoundPath}/SCalDash");
        public string Gen = "CalamityInheritance/NPCs/Boss/CalamitasClone/Brothers";
        public static Asset<Texture2D> GlowMask;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 1;
            NPCID.Sets.BossBestiaryPriority.Add(Type); //录入图鉴
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() //图鉴绘制
            {
                PortraitScale = 0.8f, //预览图大小
                Scale = 0.5f, //图集大小
            };
            NPCID.Sets.NPCBestiaryDrawOffset[Type] = value;
            if (!Main.dedServ)
            {
                GlowMask = ModContent.Request<Texture2D>($"{Gen}/CataclysmLegacy_Glow", AssetRequestMode.AsyncLoad);
            }
        }
        #endregion
        #region SD
        public override void SetDefaults()
        {
            NPC.BossBar = Main.BigBossProgressBar.NeverValid;
            NPC.damage = 60;
            NPC.npcSlots = 5f;
            NPC.width = NPC.height = 116;

            NPC.defense = (CalamityWorld.death || BossRushEvent.BossRushActive) ? 15 : 10;
            NPC.DR_NERD((CalamityWorld.death || BossRushEvent.BossRushActive) ? 0.225f : 0.15f);
            NPC.LifeMaxNERB(11000, 13200, 80000);

            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
        }
        #endregion
        #region 图鉴
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {

            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange([
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				// You can add multiple elements if you really wanted to
				new FlavorTextBestiaryInfoElement($"{GenericNPC.GetNPCBestiaryLocal}.CataclysmLegacy")
            ]);
        }
        #endregion
        public override void FindFrame(int frameHeight)
        {
            //绘制npc的动画
            NPC.frameCounter += 0.15f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }
        #region AI
        public bool start = false;
        public override void AI()
        {
            if (CIGlobalNPC.LegacyCalamitasCloneP2 < 0 || !Main.npc[CIGlobalNPC.LegacyCalamitasCloneP2].active)
            {
                NPC.life = 0;
                NPC.HitEffect();
                NPC.active = false;
                NPC.netUpdate = true;
            }

            if (NPC.rotation < 0f)
                NPC.rotation += MathHelper.TwoPi;
            else if (NPC.rotation > MathHelper.TwoPi)
                NPC.rotation -= MathHelper.TwoPi; //确保转角一直在2pi内

            // 获取目标
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                NPC.TargetClosest(true);

            Player target = Main.player[NPC.target];

            ref float attackType = ref NPC.ai[0];
            ref float attackTimer = ref NPC.ai[1];
            ref float rotationSpeed = ref NPC.CIMod().BossNewAI[1];
            rotationSpeed = 0.25f;

            if (start)
            {
                for (int d = 0; d < 50; d++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                }
                Vector2 velocity = Main.player[NPC.target].Center - NPC.Center;
                velocity.Normalize();
                NPC.rotation = velocity.ToRotation() + MathHelper.Pi;
                start = false;
            }

            // 获取NPC实例
            CIGlobalNPC.LegacyCatalysmClone = NPC.whoAmI;

            switch ((LegacyCataclysmAttackType)attackType)
            {
                case LegacyCataclysmAttackType.fireProj:
                    DoBehavior_FireCataclysmFirer(target, attackTimer);
                    break;
                case LegacyCataclysmAttackType.charge:
                    DoBehavior_Charge(ref attackTimer, ref rotationSpeed);
                    break;
            }
            attackTimer++;
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
        #region 发射火焰
        public void DoBehavior_FireCataclysmFirer(Player target, float attacktimer)
        {
            // 移动速度
            float velocity = 2f;
            float acceleration = 0.18f;
            int PosX = 100;
            int totalFireTime = 145;
            int fireDelay = 5;

            // 尝试悬停在玩家两侧
            Vector2 destination = new Vector2(target.Center.X + PosX , target.Center.Y);

            // 应该在哪
            Vector2 distanceFromDestination = destination - NPC.Center;

            CIFunction.SmoothMovement(NPC, 0f, distanceFromDestination, velocity, acceleration, true);

            if (attacktimer % fireDelay == 0)
            {
                SoundEngine.PlaySound(CISoundID.SoundFlamethrower, NPC.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // 使用旋转角度计算方向
                    Vector2 direction = Vector2.UnitX.RotatedBy(NPC.rotation); // 基础方向根据旋转角度
                    direction = direction.SafeNormalize(Vector2.UnitX);

                    int projType = ModContent.ProjectileType<CataclysmFire>();
                    // 偏移向量
                    Vector2 offset = new Vector2(0, 50).RotatedBy(NPC.rotation);
                    Vector2 projectileVelocity = direction * 6.5f;
                    Vector2 projectileSpawn = NPC.Center + offset;
                    projectileVelocity = projectileVelocity.RotatedBy(MathHelper.PiOver2);

                    Projectile.NewProjectile(NPC.GetSource_FromAI(), projectileSpawn, projectileVelocity, projType, fireDamage, 0f, Main.myPlayer, 0f, 0f);
                }
            }

            if (attacktimer > totalFireTime)
                SelectNextAttack();
        }
        #endregion
        #region 冲刺
        public bool hasCharge = false;
        public int ChargeCount = 0;
        public void DoBehavior_Charge(ref float attacktimer, ref float rotationacc)
        {
            int totalCharge = 4;
            int chargeCount = 25;
            int chargeCooldown = 70;

            if (attacktimer < chargeCount)
                rotationacc = 0.04f;

            if (hasCharge == false)
            {
                float chargeVelocity = 20f;
                chargeVelocity = Main.rand.NextFloat(20f, 25f);
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
            ChargeCount = 0;
            // 获取当前索引（不重置）
            int currentIndex = (int)NPC.ai[3];

            LegacyCataclysmAttackType[] attackCycle = AttackCycle;
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
        #region 绘制
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //绘制残影
            SpriteEffects sEffect = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                sEffect = SpriteEffects.FlipHorizontally;

            Texture2D getTexture = TextureAssets.Npc[NPC.type].Value;
            Vector2 halveTexture = new(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);
            int afterAmt = 7;
            for (int i = 1; i < afterAmt; i += 2)
            {
                Color afterColor = drawColor; afterColor = Color.Lerp(afterColor, Color.Wheat, 0.5f);
                afterColor = NPC.GetAlpha(afterColor);
                afterColor *= (afterAmt - i) / 15f;
                Vector2 afterDrawCenter = NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
                afterDrawCenter -= new Vector2(getTexture.Width, getTexture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                afterDrawCenter += halveTexture * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                spriteBatch.Draw(getTexture, afterDrawCenter, NPC.frame, afterColor, NPC.rotation, halveTexture, NPC.scale, sEffect, 0f);
            }
            Vector2 drawLocation = NPC.Center - screenPos;
            drawLocation -= new Vector2(getTexture.Width, getTexture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
            drawLocation += halveTexture * NPC.scale + new Vector2(0f, NPC.gfxOffY);
            spriteBatch.Draw(getTexture, drawLocation, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, halveTexture, NPC.scale, sEffect, 0f);

            getTexture = GlowMask.Value;
            Color pink = Color.Lerp(Color.White, Color.Red, 0.5f);
            for (int j = 1; j < afterAmt; j++)
            {
                Color afterColorNew = pink;
                afterColorNew = Color.Lerp(afterColorNew, Color.Wheat, 0.5f);
                afterColorNew *= (afterAmt - j) / 15f;
                Vector2 afterDrawCenterNew = NPC.oldPos[j] + new Vector2(NPC.width, NPC.height) / 2f - screenPos;
                afterDrawCenterNew -= new Vector2(getTexture.Width, getTexture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
                afterDrawCenterNew += halveTexture * NPC.scale + new Vector2(0f, NPC.gfxOffY);
                spriteBatch.Draw(getTexture, afterDrawCenterNew, NPC.frame, afterColorNew, NPC.rotation, halveTexture, NPC.scale, sEffect, 0f);
            }
            spriteBatch.Draw(getTexture, drawLocation, NPC.frame, pink, NPC.rotation, halveTexture, NPC.scale, sEffect, 0f);
            return false;
        }
        #endregion
        #region 受击与死亡
        public override bool CheckActive() => false;

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, hit.HitDirection, -1f);
            if (NPC.life <= 0)
            {
                for (int j = 0; j < 40; j++)
                {
                    int dType = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[dType].velocity *= 3f;
                    if (Main.rand.NextBool())
                    {
                        Main.dust[dType].scale = 0.5f;
                        Main.dust[dType].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                    }
                }
                for (int k = 0; k < 70; k++)
                {
                    int dType2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 3f);
                    Main.dust[dType2].velocity *= 5f;
                    Main.dust[dType2].noGravity = true;
                    dType2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, (int)CalamityDusts.Brimstone, 0f, 0f, 100, default, 2f);
                    Main.dust[dType2].velocity *= 2f;
                }
            }

        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (hurtInfo.Damage > 0)
                target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120, true);
        }

        public override void OnKill()
        {
            DeathAshParticle.CreateAshesFromNPC(NPC, Vector2.Zero);
            CalamityNetcode.SyncWorld();
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<HavocsBreathLegacy>(), 4);
        }
        #endregion
    }
}