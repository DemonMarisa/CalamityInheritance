using System;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged
{
    public class ProfanedNuke: ModProjectile, ILocalizedModType
    {
        //删除残影
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public int SummonFlaresTimer = 0;
        private ref float WhatRocket => ref Projectile.ai[0];
        //从原灾抄过来的
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 155;
            Projectile.DamageType = DamageClass.Ranged; 
        }
        public override void AI()
        {
            FlightAnimation();
            ProjectileFlying();
            LiquidRocket();
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(CISoundID.SoundBomb, Projectile.position);
            UpdateProjectile();
            OnKillDust();
            OnKillGore();
            BreakTiles();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 300);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ProfanedNukeDust>(), (int)(Projectile.damage * 0.25), Projectile.knockBack, Projectile.owner, 0f, 2f);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 300);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ProfanedNukeDust>(), (int)(Projectile.damage * 0.25), Projectile.knockBack, Projectile.owner, 0f, 2f);
        }
        private void FlightAnimation()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 7)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame > 2)
                Projectile.frame = 0;
        }
        private void ProjectileFlying()
        {
            SpawnDamageDust();
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1? 0f : MathHelper.Pi) + MathHelper.ToRadians(90) * Projectile.direction;
            //飞行到一定速度时追踪
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 5f)
                CIFunction.HomeInOnNPC(Projectile, false, 1800f, 22f, 15f);
        }
        private void SpawnDamageDust()
        {
            SummonFlaresTimer++;
            if (SummonFlaresTimer % 12 == 0)
                if (Projectile.owner == Main.myPlayer)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<ProfanedNukeDust>(), Projectile.damage/2, Projectile.knockBack, Projectile.owner, 0f, 0f);
            if (Math.Abs(Projectile.velocity.X) >= 8f || Math.Abs(Projectile.velocity.Y) >= 8f)
                SpawnDust();
        }
        //优先更新射弹属性
        private void UpdateProjectile()
        {
            Projectile.ExpandHitboxBy(192);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.Damage();
        }
        private void LiquidRocket()
        {
            if (WhatRocket == ItemID.DryRocket ||
                WhatRocket == ItemID.WetRocket ||
                WhatRocket == ItemID.LavaRocket||
                WhatRocket == ItemID.HoneyRocket)
            {
                Projectile.ignoreWater = false;
                if (Projectile.wet)
                    Projectile.timeLeft = 1;
            }
        }
        
        //如果可能的话尝试破坏物块
        private void BreakTiles()
        {
             if (Projectile.owner != Main.myPlayer)
                return;

            int blastRadius = 0;
            switch (WhatRocket)
            {
                case ItemID.RocketII: 
                    blastRadius = 6;
                    break;
                case ItemID.RocketIV:
                    blastRadius = 12;
                    break;
                case ItemID.MiniNukeII:
                    blastRadius = 18;
                    break;
                default:
                    break;
            }

            Projectile.ExpandHitboxBy(22);

            if (blastRadius > 0)
                Projectile.ExplodeTiles(blastRadius);

            Point center = Projectile.Center.ToTileCoordinates();
            DelegateMethods.v2_1 = center.ToVector2();
            DelegateMethods.f_1 = 4f;
            if (WhatRocket == ItemID.DryRocket)
            {
                DelegateMethods.f_1 = 4.5f;
                Utils.PlotTileArea(center.X, center.Y, DelegateMethods.SpreadDry);
            }
            else if (WhatRocket == ItemID.WetRocket)
            {
                Utils.PlotTileArea(center.X, center.Y, DelegateMethods.SpreadWater);
            }
            else if (WhatRocket == ItemID.LavaRocket)
            {
                Utils.PlotTileArea(center.X, center.Y, DelegateMethods.SpreadLava);
            }
            else if (WhatRocket == ItemID.HoneyRocket)
            {
                Utils.PlotTileArea(center.X, center.Y, DelegateMethods.SpreadHoney);
            }
        }
        
        //粒子
        private void OnKillDust()
        { 
            for (int i = 0; i < 40; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 2f);
                Main.dust[d].velocity *= 3f;
                if (Main.rand.NextBool())
                {
                    Main.dust[d].scale = 0.5f;
                    Main.dust[d].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
                }
            }
            for (int i = 0; i < 60; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 3f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 5f;
                d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.CopperCoin, 0f, 0f, 100, default, 2f);
                Main.dust[d].velocity *= 2f;
            }
        }
        //烟雾
        private void OnKillGore()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Vector2 goreSource = Projectile.Center;
                int goreAmt = 10;
                Vector2 source = new(goreSource.X - 24f, goreSource.Y - 24f);
                for (int goreIndex = 0; goreIndex < goreAmt; goreIndex++)
                {
                    float velocityMult = 0.33f;
                    if (goreIndex < (goreAmt / 3))
                    {
                        velocityMult = 0.66f;
                    }
                    if (goreIndex >= (2 * goreAmt / 3))
                    {
                        velocityMult = 1f;
                    }
                    Mod mod = ModContent.GetInstance<CalamityInheritance>();
                    int type = Main.rand.Next(61, 64);
                    int smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    Gore gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X += 1f;
                    gore.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X -= 1f;
                    gore.velocity.Y += 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X += 1f;
                    gore.velocity.Y -= 1f;
                    type = Main.rand.Next(61, 64);
                    smoke = Gore.NewGore(Projectile.GetSource_Death(), source, default, type, 1f);
                    gore = Main.gore[smoke];
                    gore.velocity *= velocityMult;
                    gore.velocity.X -= 1f;
                    gore.velocity.Y -= 1f;
                }
            }
        }
        private void SpawnDust()
        {
            float halveX = Projectile.velocity.X / 2;
            float halveY = Projectile.velocity.Y / 2;
            int d = Dust.NewDust(new Vector2(Projectile.position.X + 3f + halveX, Projectile.position.Y + 3f + halveY) - Projectile.velocity /2, Projectile.width - 8, Projectile.height - 8, DustID.CopperCoin, 0f, 0f, 100, default, 1f);
            Main.dust[d].scale *= 2f + Main.rand.Next(10) * 0.1f;
            Main.dust[d].velocity *= 0.2f;
            Main.dust[d].noGravity = true;
            d = Dust.NewDust(new Vector2(Projectile.position.X + 3f + halveX, Projectile.position.Y + 3f + halveY) - Projectile.velocity /2, Projectile.width - 8, Projectile.height - 8, DustID.CopperCoin, 0f, 0f, 100, default, 0.5f);
            Main.dust[d].fadeIn = 1f + Main.rand.Next(5) * 0.1f;
            Main.dust[d].velocity *= 0.05f;
        }
    }
}