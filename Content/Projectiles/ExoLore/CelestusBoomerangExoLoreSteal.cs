using CalamityMod.Buffs.DamageOverTime;
using CalamityMod;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons;
using Microsoft.Xna.Framework.Graphics;
using CalamityInheritance.Sounds.Custom;
using CalamityMod.Projectiles.Rogue;
using System.IO;

namespace CalamityInheritance.Content.Projectiles.ExoLore
{
    public class CelestusBoomerangExoLoreSteal : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => $"{Generic.WeaponPath}/Rogue/Celestusold";

        private bool initialized = false;
        private float speed = 25f;
        private int counter;
        #region 别名
        public ref float AttackType => ref Projectile.ai[0];
        public ref float AttackTimer => ref Projectile.ai[1];
        public int TargetIndex
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }
        public Player Owner => Main.player[Projectile.owner];
        #endregion
        #region 攻击枚举
        const float IsShooted = 0f;
        const float IsReturning = 1f;
        const float IsStealth = 2f;
        const float IsHanging = 3f;
        const float IsFading = 4f;
        #endregion
        #region 射弹属性
        const float ReturnTime = 40f;
        #endregion
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 94;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.MaxUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.noEnchantmentVisuals = true;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            Projectile.DoSyncHandlerWrite(ref writer);
            writer.Write(counter);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.DoSyncHandlerRead(ref reader);
            counter = reader.ReadInt32();
            
        }
        public override void AI()
        {
            DoGeneral();
            switch (AttackType)
            {
                case IsShooted:
                    DoShooted();
                    break;
                case IsReturning:
                    DoReturning(Projectile.Calamity().stealthStrike);
                    break;
                case IsStealth:
                    DoStealth();
                    break;
            }
        }

        //I don't know how to code.
        //这个本来准备用于悬挂回收AI，但是后来想了想执行起来会有问题，注释掉了
        //我不建议删掉这段，因为可能后面会用得上，这里写的其实挺有条理的，如果需要参考的话确实可以参考的。
        #region 注释AI
        /*
        private void DoHanging()
        {
            //设定目标位置
            float hangingX = Owner.MountedCenter.X;
            float hangingY = Owner.MountedCenter.Y - 100f;
            Vector2 hangingCenter = new Vector2(hangingX, hangingY);
            //设定加速度
            float acceleration = 0.35f;
            //将角度不断指向这个敌怪，
            NPC facingTarget = Main.npc[TargetIndex];
            float rotAngle =  Projectile.AngleTo(Vector2.UnitX);
            //除非target不再活动，不然镰刀都会朝向这个target的
            if (facingTarget is not null && facingTarget.active)
                rotAngle = Projectile.AngleTo(facingTarget.Center);
            //将射弹返回，同时在此期间不断修正射弹的朝向
            Projectile.rotation = Utils.AngleLerp(Projectile.rotation, rotAngle, 0.2f);
            Projectile.velocity = (Projectile.velocity * (15f + acceleration) + Projectile.DirectionTo(hangingCenter)) / 15f;
            //遍历场上可能存在的幻影镰刀，如果不存在则执行返回程式
            int searchProj = ModContent.ProjectileType<CelestusBoomerangExoLoreHomeIn>();
            bool atLeastOne = false;
            //但凡搜索到一个我们都直接break出去，我们不会在这里执行幻影镰刀的返程AI的
            for (int search = 0; search < Main.projectile.Length; search++)
            {
                Projectile wantedProj = Main.projectile.[search];
                if (wantedProj.type != searchProj && wantedProj.owner != Owner.whoAmI)
                    continue;
                else
                {
                    atLeastOne = true;
                    break;
                }
            }
            //场上但凡有一个幻影镰刀，这里的AI都会在这里停止并返回。
            if (atLeastOne)
                return;
            //现在才开始执行回收AI
            AttackTimer += 1f;
            if (AttackTimer > 15f)
            {
                //执行返回AI。重置计时器
                AttackTimer = 0f;
                AttackType = IsFading;
                Projectile.netUpdate = true;
            }
        }
        */
        #endregion
        private void DoGeneral()
        {
            if (!initialized)
            {
                speed = Projectile.velocity.Length();
                initialized = true;
            }
            Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.5f / 255f, Main.DiscoG * 0.5f / 255f, Main.DiscoB * 0.5f / 255f);
            if (AttackType != IsStealth)
                Projectile.rotation = Projectile.velocity.ToRotation();
            else
                Projectile.rotation += 1f;
        }

        private void SpawnExtraProj()
        {
            counter++;
            if(counter == 12)
            {
                float randomAngle = Main.rand.NextBool() ? MathHelper.PiOver2 + Main.rand.NextFloat(-3f, 4f) : -MathHelper.PiOver2 + Main.rand.NextFloat(-3f, 4f);
                int t = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotatedBy(randomAngle), ModContent.ProjectileType<CelestusBoomerangExoLoreHomeIn>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
                Main.projectile[t].scale *= 0.9f;
                counter = 0;
            }
        }

        private void DoStealth()
        {
            CIFunction.HomeInOnNPC(Projectile, true, 1250f, speed, 20f);
        }

        private void DoReturning(bool stealthStrike)
        {
            float returnSpeed = 25f;
            float acceleration = 5f;
            CIFunction.BoomerangReturningAI(Owner, Projectile, returnSpeed, acceleration);
            SpawnExtraProj();
            if (Main.myPlayer != Projectile.owner)
                return;
            if (Projectile.Hitbox.Intersects(Owner.Hitbox))
            {
                if (stealthStrike)
                {
                    Projectile.velocity *= -1f;
                    Projectile.timeLeft = 600;
                    Projectile.penetrate = -1;
                    Projectile.localNPCHitCooldown = -1;
                    AttackType = IsStealth;
                    Projectile.netUpdate = true;
                }
                else
                    Projectile.Kill();

            }
        }

        private void DoShooted()
        {
            AttackTimer += 1f;
            if (AttackTimer > ReturnTime)
            {
                AttackType = IsReturning;
                AttackTimer = 0f;
                Projectile.netUpdate = true;
            }
            else
            {
                SpawnExtraProj();
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.ExoDebuffs();
            OnHitEffects();
            if (AttackType == IsStealth)
            {
                AttackType = IsHanging;
                //标记这个敌怪
                TargetIndex = target.whoAmI;
                Projectile.netUpdate = true;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            OnHitEffects();
        }

        private void OnHitEffects()
        {
            if (Projectile.owner == Main.myPlayer)
            {
                float spread = 45f * 0.0174f;
                double startAngle = Math.Atan2(Projectile.velocity.X, Projectile.velocity.Y) - spread / 2;
                double deltaAngle = spread / 8f;
                double offsetAngle;
                for (int i = 0; i < 4; i++)
                {
                    offsetAngle = startAngle + deltaAngle * (i + i * i) / 2f + 32f * i;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 2f), (float)(Math.Cos(offsetAngle) * 2f), ModContent.ProjectileType<Celestus2ExoLore>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 2f), (float)(-Math.Cos(offsetAngle) * 2f), ModContent.ProjectileType<Celestus2ExoLore>(), (int)(Projectile.damage * 0.7), Projectile.knockBack, Projectile.owner);
                }
            }
            SoundStyle[] getSound =
            [
                CISoundMenu.CelestusOnHit1,
                CISoundMenu.CelestusOnHit2,
                CISoundMenu.CelestusOnHit3
            ];
            SoundEngine.PlaySound(Utils.SelectRandom(Main.rand, getSound), Projectile.position);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            CalamityUtils.DrawAfterimagesCentered(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Rectangle frame = new Rectangle(0, 0, 106, 94);
            Main.EntitySpriteDraw(ModContent.Request<Texture2D>($"{Generic.WeaponPath}/Rogue/CelestusoldGlow").Value,
                Projectile.Center - Main.screenPosition,
                frame,
                Color.White,
                Projectile.rotation,
                Projectile.Size / 2,
                1f,
                SpriteEffects.None,
                0);
        }
    }
}
