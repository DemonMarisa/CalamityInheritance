using System;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class ShizukuEdgeMoon: ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Typeless/ShizukuItem/ShizukuEdge";
        public Player Owner => Main.player[Projectile.owner];
        public ref float AttackTimer => ref Projectile.ai[0];
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.height = 108;
            Projectile.width = 84;
            Projectile.scale *= 2f;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 90000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 1;
        }
        public override void AI()
        {
            float spinTime = 50f;
            if (Owner.dead || !Owner.controlUseTile)
            {
                // Main.NewText("FuckedMoon, Velocity:" + Projectile.velocity.Length().ToString());
                Projectile.Kill();
                Owner.reuseDelay = 2;
                return;
            }
            int spinDir = Math.Sign(Projectile.velocity.X);
            Projectile.velocity = new Vector2(spinDir, 0f);
            if (AttackTimer == 0f)
            {
                Projectile.rotation = new Vector2(Projectile.velocity.X, -Owner.gravDir).ToRotation() + MathHelper.ToRadians(135f);
                if (Projectile.velocity.X < 0f)
                {
                    Projectile.rotation -= MathHelper.PiOver2;
                }
            }
            AttackTimer += 1f;
            Projectile.rotation += MathHelper.TwoPi * 2f / spinTime * spinDir;
            int wantedDire = (Owner.SafeDirectionTo(Main.MouseWorld).X > 0f).ToDirectionInt();
            if (AttackTimer % spinTime > spinTime * 0.5f && wantedDire != Projectile.velocity.X)
            {
                Owner.ChangeDir(wantedDire);
                Projectile.velocity = Vector2.UnitX * wantedDire;
                Projectile.rotation -= MathHelper.Pi;
                Projectile.netUpdate = true;
            }
            PosAndRot();
            HoldoutAI();
        }

        private void PosAndRot()
        {
            Vector2 ifPlayerControl = Owner.RotatedRelativePoint(Owner.MountedCenter, true);
            Projectile.Center = new Vector2(ifPlayerControl.X, ifPlayerControl.Y - 180f);
            Projectile.spriteDirection = Projectile.direction;
            Projectile.timeLeft = 2;
            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = Owner.itemAnimation = 2;
            Owner.itemRotation = MathHelper.WrapAngle(Projectile.rotation);
        }

        private DamageClass BestCrit()
        {
            float statCrit = 0f;
            DamageClass best = DamageClass.Generic;
            float meleeCrits = Owner.GetCritChance<MeleeDamageClass>();
            float rangedCrits = Owner.GetCritChance<RangedDamageClass>();
            float magicCrits = Owner.GetCritChance<MagicDamageClass>();
            float rogueCrits = Owner.GetCritChance<RogueDamageClass>();
            if (meleeCrits > statCrit)
            {
                statCrit = meleeCrits;
                best = DamageClass.Melee;
            }
            if (rangedCrits > statCrit)
            {
                statCrit = rangedCrits;
                best = DamageClass.Ranged;
            }
            if (magicCrits > statCrit)
            {
                statCrit = magicCrits;
                best = DamageClass.Magic;
            }
            if (rogueCrits > statCrit)
            {
                best = ModContent.GetInstance<RogueDamageClass>();
            }
            return best;
        }

        public DamageClass BestClass()
        {
            float statDamage = 1f;
            DamageClass best = DamageClass.Generic;
            float melee = Owner.GetTotalDamage<MeleeDamageClass>().Additive;
            float ranged = Owner.GetTotalDamage<RangedDamageClass>().Additive;
            float magic = Owner.GetTotalDamage<MagicDamageClass>().Additive;
            //召唤加成相对更高
            float summon = Owner.GetTotalDamage<SummonDamageClass>().Additive / 1.5f;
            if (melee > statDamage)
            {
                statDamage = melee;
                best = DamageClass.Melee;
            }
            if (ranged > statDamage)
            {
                statDamage = ranged;
                best = DamageClass.Ranged;
            }
            if (magic > statDamage)
            {
                statDamage = magic;
                best = DamageClass.Magic;
            }
            if (summon > statDamage)
            {
                best = DamageClass.Summon;
            }
            return best;
        }
        public void HoldoutAI()
        {
 
            //发射的射弹类型
            int[] ghostType =
            [
                ModContent.ProjectileType<SoulSmallPlaceholder>(),
                ModContent.ProjectileType<SoulMidPlaceholder>(),
                ModContent.ProjectileType<SoulLargePlaceholder>()
            ];
            #region 伤害倍率计算
            //1.获取穿甲量
            float AP = Owner.GetArmorPenetration<GenericDamageClass>();
            //2.获取玩家（除盗贼外）最高伤害职业
            DamageClass damageClass = BestClass();
            //3.获取加算伤害加成
            float additiveDamageMult = Owner.GetTotalDamage(damageClass).Additive;
            //4.获取当前暴击概率最高的职业
            DamageClass critClass = BestCrit();
            //5.获取暴击加成
            float critsAdd = Owner.GetCritChance(critClass);
            //6.获取玩家当前防御力
            int baseDef = Owner.statDefense;
            //7. 假定穿甲量 + 伤害加成 < 200%(2f)，给予保底2f倍率
            float damageMult = (AP + additiveDamageMult);
            if (damageMult < 2f)
                damageMult = 2f;
            //8. 假定暴击概率+当前防御力 低于 500, 给予保底500
            int damageBase = (int)critsAdd + baseDef;
            if (damageBase < 500)
                damageBase = 500;
            //9.将上述所有计算代入下方自定义公式：(穿甲倍率加算+伤害加成) * 武器伤害 + (暴击概率 + 当前防御力) * 5 = 最终的射弹伤害
            int finalProjDamage = (int)(damageMult * Projectile.damage + damageBase * 5);

            //10.根据玩家当前的仆从和哨兵上限，获得射弹数量加成
            int baseProjCounts = 5;
            int extraProjCounts = Owner.maxMinions + Owner.maxTurrets;
            int finalProjCounts = baseProjCounts + extraProjCounts;
            #endregion
            //开始攻击
            AttackTimer += 1f;
            IEntitySource src = Projectile.GetSource_FromThis();
            if (AttackTimer % 30 == 0)
            {
                for (int i = 0; i < finalProjCounts; i++)
                {
                    //随机取用一个射弹
                    int realProj = Utils.SelectRandom(Main.rand, ghostType);
                    float speedX = Main.rand.NextFloat(12f, 16f);
                    //发射方向
                    Vector2 fireDir = new Vector2(speedX, 0f).RotatedByRandom(MathHelper.TwoPi);
                    int p = Projectile.NewProjectile(src, Projectile.Center, fireDir, realProj, finalProjDamage, 0f, Owner.whoAmI);
                    //给予所有射弹盗贼潜伏标记
                    Main.projectile[p].Calamity().stealthStrike = true;
                }
            }
            //释放冲击波，灭弹效果
            if (AttackTimer % 180 == 0)
            {
                Projectile.NewProjectile(src, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShizukuShockwave>(), finalProjDamage / 10, 0f, Owner.whoAmI);
            }
            //重置一轮计数器
            if (AttackTimer > 240)
            {
                //在玩家的位置释放飞剑
                // Vector2 spawnPos = new Vector2(Owner.Center.X, Owner.MountedCenter.Y - 24f)
                AttackTimer = 0;
            }   
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }
            float spinning = Projectile.rotation - MathHelper.PiOver4 * (float)Math.Sign(Projectile.velocity.X);
            float staffRadiusHit = 110f;
            float useless = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + spinning.ToRotationVector2() * -staffRadiusHit, Projectile.Center + spinning.ToRotationVector2() * staffRadiusHit, 23f * Projectile.scale, ref useless))
            {
                return true;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //手动接管绘制
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
            Rectangle rec = new Rectangle(0, 0, texture.Width, texture.Height);
            Vector2 ori = texture.Size() / 2f;
            Color color = Color.White;
            SpriteEffects sE = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                sE = SpriteEffects.FlipHorizontally;
            Main.EntitySpriteDraw(texture, drawPos, new Rectangle?(rec), color, Projectile.rotation, ori, Projectile.scale, sE, 0);
            return false;
        }
    }
}