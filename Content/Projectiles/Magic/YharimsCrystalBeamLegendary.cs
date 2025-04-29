using System;
using System.IO;
using CalamityInheritance.Content.Items.Weapons;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityInheritance.NPCs.Boss.SCAL;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Magic
{
    public class YharimsCrystalBeamLegendary : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Magic";
        #region 射弹属性
        private const float BeamDivisor = MathHelper.Pi / YharimsCrystalPrismLegendary.BeamCounts;

        private const float DmgMulti = 3f;

        private const float BeamPosOffset = 16f;
        private const float ScaleMax = 1.8f;

        private const float Lengthmax = 2400f;
        private const float HitTileWidth = 1f;
        private const float HitboxHitTileWidth = 22f;
        private const int PointsNum = 3;
        private const float LengthChanger = 0.75f;

        private const float VEThreshold = 0.1f;

        private const float OuterBeamOpacity = 0.75f;
        private const float InnerBeamOpacity = 0.1f;
        private const float Lights = 0.75f;

        private const float DustEnd = 14.5f;
        private const float DustSideEnd = 4f;
        private const float TileOffset = 10.5f;
        private const float Reduction = 14.5f;
        #endregion
        private const short BeamID = 0;
        private const short OwnerIndex = 1;
        private const short BeamLengthChecker = 1;

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
            //光束本身会撞墙消失，但是……也无所谓
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        //我们这里需要手动发送AI，这是为了确保netUpdate
        public override void SendExtraAI(BinaryWriter writer) => writer.Write(Projectile.localAI[BeamLengthChecker]);
        public override void ReceiveExtraAI(BinaryReader reader) => Projectile.localAI[BeamLengthChecker] = reader.ReadSingle();

        /*
        *Projectile.ai[BeamID] -> 0 ~ N-1, 在射弹生成时赋值。即射弹ID。因为每个“Beam”都有不同的转角与色彩
        *Projectile.ai[OwnerIndex] -> 用于标记是“哪个”棱镜用有这道光束
        *Projectile.localAI[BeamLengthChecker] -> 用于标记光束的长度
        */
        public override void AI()
        {
            //给判定，如果发现自己的射弹连自己都不是，我们直接干掉这个包括水晶在内的射弹。
            Projectile hostCrystal = Main.projectile[(int)Projectile.ai[OwnerIndex]];
            if (Projectile.type != ModContent.ProjectileType<YharimsCrystalBeamLegendary>() || !hostCrystal.active || hostCrystal.type != ModContent.ProjectileType<YharimsCrystalPrismLegendary>())
            {
                Projectile.Kill();
                return;
            }
            //激活终灾加强时我们将射弹更新为2无敌帧。
            Player player = Main.player[Projectile.owner];
            if (player.CIMod().YharimsKilledScal)
                Projectile.localNPCHitCooldown = 2;

            Vector2 hostCrystalDir = Vector2.Normalize(hostCrystal.velocity);
            float chargeRatio = MathHelper.Clamp(hostCrystal.ai[BeamID] / YharimsCrystalPrism.MaxCharge, 0f, 1f);

            //更新光束伤害。暴君水晶的伤害增加会比较顺滑一点
            Projectile.damage = (int)(hostCrystal.damage * GetDamageMultiplier(chargeRatio, player));

            //除非达到充能时间，不然不允许射弹造成伤害
            Projectile.friendly = hostCrystal.ai[BeamID] > YharimsCrystalPrism.DamageStart;

            //这个差值会让组成光束的”射弹“都略微不同
            float beamIdOffset = (int)Projectile.ai[BeamID] - YharimsCrystalPrism.NumBeams / 2f + 0.5f;
            float beamSpread;
            float spinRate;
            float beamStartSidewaysOffset;
            float beamStartForwardsOffset;

            //让伤害随着充能的过程中得到提升。满充能后这里将不会在进行判定
            if (chargeRatio < 1f)
            {
                Projectile.scale = MathHelper.Lerp(0f, ScaleMax, chargeRatio);
                beamSpread = MathHelper.Lerp(1.22f, 0f, chargeRatio);
                beamStartSidewaysOffset = MathHelper.Lerp(20f, 6f, chargeRatio);
                beamStartForwardsOffset = MathHelper.Lerp(-17f, -13f, chargeRatio);

                //在第一个2/3充能时间，射弹的透明度由0~40%。旋转强度也会逐渐提升
                if (chargeRatio <= 0.66f)
                {
                    float phaseRatio = chargeRatio * 1.5f;
                    Projectile.Opacity = MathHelper.Lerp(0f, 0.4f, phaseRatio);
                    spinRate = MathHelper.Lerp(20f, 16f, phaseRatio);
                }

                //在最后的1/3充能时间，射弹透明度会剧增到100%，旋转强度也同理
                else
                {
                    float phaseRatio = (chargeRatio - 0.66f) * 3f;
                    Projectile.Opacity = MathHelper.Lerp(0.4f, 1f, phaseRatio);
                    spinRate = MathHelper.Lerp(16f, 6f, phaseRatio);
                }
            }
            else
            {
                Projectile.scale = ScaleMax;
                Projectile.Opacity = 1f;
                beamSpread = 0f;
                spinRate = 6f;
                beamStartSidewaysOffset = 6f;
                beamStartForwardsOffset = -13f;
            }

            //保住光束的”转角“，使其看起来像是一条线。
            float deviationAngle = (hostCrystal.ai[BeamID] + beamIdOffset * spinRate) / (spinRate * YharimsCrystalPrism.NumBeams) * MathHelper.TwoPi;
            Vector2 unitRot = Vector2.UnitY.RotatedBy(deviationAngle);
            float sinusoidYOffset = unitRot.Y * BeamDivisor * beamSpread;
            float hostCrystalAngle = hostCrystal.velocity.ToRotation();
            Vector2 yVec = new Vector2(4f, beamStartSidewaysOffset);
            Vector2 beamSpanVector = (unitRot * yVec).RotatedBy(hostCrystalAngle);

            //保住光束的起始位置。让其在水晶的中心
            Projectile.Center = hostCrystal.Center;
            //补一个差值，让其对其水晶的贴图
            Projectile.position += hostCrystalDir * BeamPosOffset + new Vector2(0f, -hostCrystal.gfxOffY);
            //补另一个差值，让其对其水晶的尖端
            Projectile.position += hostCrystalDir * beamStartForwardsOffset;
            //最后的差值
            Projectile.position += beamSpanVector;

            //将光束的速度设置为指针指向。
            Projectile.velocity = hostCrystalDir.RotatedBy(sinusoidYOffset);
            if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
                Projectile.velocity = -Vector2.UnitY;
            Projectile.rotation = Projectile.velocity.ToRotation();

            //默认情况下，插入的光束应该从射弹中心点开始。如果水晶满蓄力的话，这个插入会从水晶重心开始
            //如果玩家把水晶的贴图塞到了墙里面。直接重写这个，让其从玩家中心点开始
            Vector2 samplingPoint = Projectile.Center;
            if (hostCrystal.ai[BeamID] >= YharimsCrystalPrism.MaxCharge)
                samplingPoint = hostCrystal.Center;
            if (!Collision.CanHitLine(Main.player[Projectile.owner].Center, 0, 0, hostCrystal.Center, 0, 0))
                samplingPoint = Main.player[Projectile.owner].Center;

            //我们发送一个“激光扫射”来校准光束的长度
            //如果光束无视物块，直接固定光束长度就行了
            float[] laserScanResults = new float[PointsNum];
            Collision.LaserScan(samplingPoint, Projectile.velocity, HitTileWidth * Projectile.scale, Lengthmax, laserScanResults);
            float avg = 0f;
            for (int i = 0; i < laserScanResults.Length; ++i)
                avg += laserScanResults[i];
            avg /= PointsNum;
            Projectile.localAI[BeamLengthChecker] = MathHelper.Lerp(Projectile.localAI[BeamLengthChecker], avg, LengthChanger);

            //X -> 光束长度， Y -> 光束宽度
            Vector2 beamDims = new Vector2(Projectile.velocity.Length() * Projectile.localAI[BeamLengthChecker], Projectile.width * Projectile.scale);

            //我们只在特定充能阶段开始时生成粒子
            Color beamColor = GetBeamColor();
            if (chargeRatio >= VEThreshold)
            {
                ProduceBeamDust(beamColor);

                if (Main.netMode != NetmodeID.Server)
                {
                    WaterShaderData wsd = (WaterShaderData)Filters.Scene["WaterDistortion"].GetShader();
                    float waveSine = 0.1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f);
                    Vector2 ripplePos = Projectile.position + new Vector2(beamDims.X * 0.5f, 0f).RotatedBy(Projectile.rotation);
                    Color waveData = new Color(0.5f, 0.1f * Math.Sign(waveSine) + 0.5f, 0f, 1f) * Math.Abs(waveSine);
                    wsd.QueueRipple(ripplePos, waveData, beamDims, RippleShape.Square, Projectile.rotation);
                }
            }

            //让光束发光
            DelegateMethods.v3_1 = beamColor.ToVector3() * Lights * chargeRatio;
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[BeamLengthChecker], beamDims.Y, DelegateMethods.CastLight);
        }

        private float GetDamageMultiplier(float chargeRatio, Player p)
        {
            float f = chargeRatio * chargeRatio * chargeRatio;
            //激活终灾加强时，将伤害倍率调整为1000%
            float setMult = p.CIMod().YharimsKilledScal ? DmgMulti + 7f : DmgMulti;
            return MathHelper.Lerp(1f, setMult, f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var usPlayer = player.CIMod();
            var src = Projectile.GetSource_FromThis();
            if (usPlayer.GlobalFireDelay == 0)
            {
                if (usPlayer.YharimsKilledExo)
                    DoExoUpgrade(target, src);
                if (usPlayer.YharimsKilledScal)
                    DoScalUpgrade(target, src);
                    DoDragonUpgrade(target, src);
            }
        }

        private void DoScalUpgrade(NPC target, IEntitySource src)
        {
            Player player = Main.player[Projectile.owner];
            var usPlayer = player.CIMod();
            //这里会跳过指针，即光柱的位置
            if (usPlayer.GlobalMiscCounter > 7)
                usPlayer.GlobalMiscCounter = 1;
            //设置初始向量为指向鼠标的向量
            Vector2 getFireSpeed = Main.MouseWorld - player.Center;
            //设定一个速度，这里用的27f
            float setSpeed = 27f;
            //将距离向量转速度向量
            float dist = getFireSpeed.Length();
            dist = setSpeed / dist;
            getFireSpeed.X *= dist;
            getFireSpeed.Y *= dist;
            //设置方向
            Vector2 fireSupporter = ((MathHelper.TwoPi * usPlayer.GlobalMiscCounter / 8f) + getFireSpeed.ToRotation()).ToRotationVector2() * getFireSpeed.Length() * 0.8f;
            Projectile.NewProjectile(src, player.Center, fireSupporter, ModContent.ProjectileType<YharimsScalSupport>(), Projectile.damage, 0f, player.whoAmI, 0f, 0f, target.whoAmI);
            usPlayer.GlobalFireDelay = 8;
            usPlayer.GlobalMiscCounter++;
        }


        private void DoExoUpgrade(NPC target, IEntitySource src)
        {
            Player player = Main.player[Projectile.owner];
            for (int i = -1; i < 2; i += 2)
            {
                //设置生成位置
                float projX = target.Center.X + Main.rand.NextFloat(-50f, 51f);
                float projY = target.Center.Y + 800f;  
                Vector2 spawnPos = new (projX, projY);
                //-1 -> 1, 这样会使其从上下方，两个方向进行进攻
                Vector2 spawnVel = new Vector2(10f, 0f).RotatedBy(-MathHelper.PiOver2);
                int damage = Projectile.damage /4;
                //生成流星
                Projectile.NewProjectile(src, spawnPos, spawnVel, ModContent.ProjectileType<CIExocometMagic>(), damage, 0f, player.whoAmI);
            }
        }


        private void DoDragonUpgrade(NPC target, IEntitySource src)
        {
            Player player = Main.player[Projectile.owner];
            for (int i = -1; i < 2; i += 2)
            {
                //设置生成位置
                float projX = target.Center.X + Main.rand.NextFloat(-50f, 51f);
                float projY = target.Center.Y - 800f;  
                Vector2 spawnPos = new (projX, projY);
                //-1 -> 1, 这样会使其从上下方，两个方向进行进攻
                Vector2 spawnVel = new Vector2(10f, 0f).RotatedBy(MathHelper.PiOver2);
                int damage = Projectile.damage /4;
                //生成火球
                Projectile.NewProjectile(src, spawnPos, spawnVel, ModContent.ProjectileType<YharimsDragonSupport>(), damage, 0f, player.whoAmI, 0f, 0f, target.whoAmI);
            }
        }


        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(ModContent.BuffType<Dragonfire>(), 180);

        //决定目标hitbox与射弹hitbox接触时的行为
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            //如果正常接触，那就接触
            if (projHitbox.Intersects(targetHitbox))
                return true;
            //否则采用AABBline接触，我也不知道是啥
            float _ = float.NaN;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[BeamLengthChecker], HitboxHitTileWidth * Projectile.scale, ref _);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //如果光束都没确定一个方向，ban掉下方的绘制
            if (Projectile.velocity == Vector2.Zero)
                return false;

            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            float beamLength = Projectile.localAI[BeamLengthChecker];
            Vector2 centerFloored = Projectile.Center.Floor() + Projectile.velocity * Projectile.scale * TileOffset;
            Vector2 scaleVec = new(Projectile.scale);

            beamLength -= Reduction * Projectile.scale * Projectile.scale;

            DelegateMethods.f_1 = 1f; 
            Vector2 beamStartPos = centerFloored - Main.screenPosition;
            Vector2 beamEndPos = beamStartPos + Projectile.velocity * beamLength;
            Utils.LaserLineFraming llf = new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw);

            //绘制外围光束
            Color outerBeamColor = GetBeamColor();
            DelegateMethods.c_1 = outerBeamColor * OuterBeamOpacity * Projectile.Opacity;
            Utils.DrawLaser(Main.spriteBatch, tex, beamStartPos, beamEndPos, scaleVec, llf);

            //绘制内部光束
            scaleVec *= 0.5f;
            Color innerBeamColor = Color.White;
            DelegateMethods.c_1 = innerBeamColor * InnerBeamOpacity * Projectile.Opacity;
            Utils.DrawLaser(Main.spriteBatch, tex, beamStartPos, beamEndPos, scaleVec, llf);
            return false;
        }

        private void ProduceBeamDust(Color beamColor)
        {
            //粒子
            Vector2 laserEndPos = Projectile.Center + Projectile.velocity * (Projectile.localAI[BeamLengthChecker] - DustEnd * Projectile.scale);
            for (int i = 0; i < 2; ++i)
            {
                float dustAngle = Projectile.rotation + (Main.rand.NextBool() ? 1f : -1f) * MathHelper.PiOver2;
                float dustStartDist = Main.rand.NextFloat(1f, 1.8f);
                Vector2 dustVel = dustAngle.ToRotationVector2() * dustStartDist;
                int d = Dust.NewDust(laserEndPos, 0, 0, DustID.CopperCoin, dustVel.X, dustVel.Y, 0, beamColor, 3.3f);
                Main.dust[d].color = beamColor;
                Main.dust[d].noGravity = true;
                Main.dust[d].scale = 1.2f;

                if (Projectile.scale > 1f)
                {
                    Main.dust[d].velocity *= Projectile.scale;
                    Main.dust[d].scale *= Projectile.scale;
                }

                if (Projectile.scale != ScaleMax)
                {
                    Dust smallDust = Dust.CloneDust(d);
                    smallDust.scale /= 2f;
                }
            }

            if (Main.rand.NextBool(5))
            {
                Vector2 dustOffset = Projectile.velocity.RotatedBy(MathHelper.PiOver2) * (Main.rand.NextFloat() - 0.5f) * Projectile.width;
                Vector2 dustPos = laserEndPos + dustOffset - Vector2.One * DustSideEnd;
                int dustID = 244;
                int d = Dust.NewDust(dustPos, 8, 8, dustID, 0f, 0f, 100, beamColor, 5f);
                Main.dust[d].velocity *= 0.5f;

                Main.dust[d].velocity.Y = -Math.Abs(Main.dust[d].velocity.Y);
            }
        }

        private Color GetBeamColor()
        {
            float customHue = GetHue(Projectile.ai[BeamID]);
            float sat = 0.66f;
            float light = 0.53f;
            Color c = Main.hslToRgb(customHue, sat, light);
            c.A = 64;
            return c;
        }

        //这个用于绘制名字彩蛋。
        private float GetHue(float indexing)
        {
            Player p = Main.player[Projectile.owner];
            var up = p.CIMod();
            if (up.YharimsKilledExo)
                indexing = 1452;

            return indexing / YharimsCrystalPrism.NumBeams % 0.12f;
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.TileActionAttempt cut = DelegateMethods.CutTiles;
            Vector2 beamStartPos = Projectile.Center;
            Vector2 beamEndPos = beamStartPos + Projectile.velocity * Projectile.localAI[BeamLengthChecker];
            Utils.PlotTileLine(beamStartPos, beamEndPos, Projectile.width * Projectile.scale, cut);
        }
    }
}
