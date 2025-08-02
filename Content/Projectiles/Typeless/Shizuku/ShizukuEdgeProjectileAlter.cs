using System;
using CalamityMod;
using Microsoft.Build.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Typeless.Shizuku
{
    public class ShizukuEdgeProjectileAlter : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Typeless";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Typeless/ShizukuItem/ShizukuEdge";
        public Player Owner => Main.player[Projectile.owner];
        public static Vector2 HoldingOffset => new (-5, 10f);
        public ref float AttackTimer => ref Projectile.ai[1];
        public ref float AttackType => ref Projectile.ai[0];
        public float YOffset = 0f;
        public const float IsSpawned = 0f;
        public const float IsSpining = 1f;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.height = 108;
            Projectile.width = 84;
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
        // public override void AI()
        // {
        //     KillProj();
        //     PlayerPostionRotation();
        //     AddLight();
        //     ShootProjectile();
        // }
        //重新制作AI
        public override void AI()
        {
            Projectile.extraUpdates = 0;
            //刚生成的时候，水平固定，并使其向上攀升
            if (AttackType == IsSpawned)
            {
                Projectile.rotation += 0.25f;
                Projectile.velocity *= 0.95f;
                //期间射弹的水平位置跟随玩家位置
                Projectile.position.X = Owner.Center.X;
                if (Projectile.velocity.Length() == 0)
                {
                    //记录此时高度，开始自转
                    YOffset = Projectile.Center.Y - Owner.Center.Y;
                    AttackType = IsSpining;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                Vector2 offset = new(0, YOffset);
                Vector2 rrp = Owner.RotatedRelativePoint(Owner.MountedCenter, true) - offset;
                UpdateVisual(rrp);
                //开始处理AI
                if (Projectile.owner == Main.myPlayer)
                {
                    bool stillInUse = (Owner.channel || Owner.controlUseTile) && !Owner.noItems && !Owner.CCed;
                    if (stillInUse)
                    {
                        HoldoutAI();
                    }
                    else
                    {
                        DeleteAI();
                    }

                }
            }
        }

        private void HoldoutAI()
        {
            //开始处理攻击AI
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
            if (AttackTimer % 15 == 0)
            {
                for (int i = 0; i < finalProjCounts; i++)
                {
                    //随机取用一个射弹
                    int realProj = Utils.SelectRandom(Main.rand, ghostType);
                    float speedX = Main.rand.NextFloat(Projectile.rotation * 1.1f, Projectile.rotation * 2.4f);
                    //发射方向
                    Vector2 fireDir = new Vector2(speedX, 0f).RotatedByRandom(MathHelper.TwoPi);
                    int p = Projectile.NewProjectile(src, Projectile.Center, fireDir, realProj, finalProjDamage, 0f, Owner.whoAmI);
                    //给予所有射弹盗贼潜伏标记
                    Main.projectile[p].Calamity().stealthStrike = true;
                }
            }
            //释放冲击波，灭弹效果
            if (AttackTimer % 90 == 0)
            {
                Projectile.NewProjectile(src, Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShizukuShockwave>(), finalProjDamage / 10, 0f, Owner.whoAmI);
            }
            //重置一轮计数器
            if (AttackTimer > 180)
                AttackTimer = 0;
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

        private void DeleteAI()
        {
        }

        private void UpdateVisual(Vector2 rrp)
        {
            //自转
            Projectile.rotation += 0.25f;
            //我只需要让射弹中心位于头顶上方，不考虑其他
            Projectile.Center = rrp;
            Projectile.spriteDirection = Projectile.direction;
            Owner.ChangeDir(Projectile.direction);
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = 2;
            Owner.itemAnimation = 2;
            Owner.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();

        }

        public void ShootProjectile()
        {
            #region 初始化
            int scytherProj = ModContent.ProjectileType<ScythePlaceholder>();
            int ghostRandom = Main.rand.Next(0, 3);
            ghostRandom = ghostRandom switch
            {
                0 => ModContent.ProjectileType<SoulSmallPlaceholder>(),
                1 => ModContent.ProjectileType<SoulMidPlaceholder>(),
                _ => ModContent.ProjectileType<SoulLargePlaceholder>(),
            };
            var projSrc = Projectile.GetSource_FromThis();
            float scytheDmg = Projectile.damage * 1.2f;
            float ghostDmg = Projectile.damage * 0.4f;
            #endregion
            AttackTimer += 1f;
            if (AttackTimer % 15f == 0)
            {
                ShootGhost(ghostRandom, ghostDmg, projSrc);
                ShootScythe(scytherProj, scytheDmg, projSrc);
            }
        }

        public void ShootScythe(int scytherProj, float scytheDmg, IEntitySource projSrc)
        {
            Vector2 srcPos = Owner.MountedCenter - HoldingOffset;
            //最后给予一定的随机度
            Vector2 finalPos = new(srcPos.X, srcPos.Y + 5f);
            //最终位置
            Vector2 velocity = srcPos - finalPos;
            //转速度向量
            float projSpeed = 16f;
            float length = velocity.Length();
            length = projSpeed / length;
            velocity.X *= length;
            velocity.Y *= length;
            Projectile.NewProjectile(projSrc, srcPos, velocity, scytherProj, (int)scytheDmg, 0f, Owner.whoAmI);
        }

        public void ShootGhost(int ghostType, float ghostDmg, IEntitySource projSrc)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 srcPos = Owner.MountedCenter - HoldingOffset;
                //最后给予一定的随机度
                Vector2 finalPos = new(srcPos.X + Main.rand.NextFloat(-8f, 9f), srcPos.Y + 5f);
                //最终位置
                Vector2 velocity = srcPos - finalPos;
                //转速度向量
                float projSpeed = 16f;
                float length = velocity.Length();
                length = projSpeed / length;
                velocity.X *= length;
                velocity.Y *= length;
                Projectile.NewProjectile(projSrc, srcPos, velocity, ghostType, (int)ghostDmg, 0f, Owner.whoAmI);
            }
        }

        public void AddLight()
        {
            //补光
            Lighting.AddLight(Projectile.Center, TorchID.White);
        }

        //让我编译一下看看怎么个事
        private void PlayerPostionRotation()
        {
            //干掉Rotation，只让他水平旋转
            Projectile.rotation = MathHelper.PiOver4 + MathHelper.Pi;
            //更新射弹位置到对应的位置
            Vector2 pos = Owner.RotatedRelativePoint(Owner.MountedCenter, true);
            //我一想到我要为了这个编译十几次就想笑
            Projectile.Center = pos - HoldingOffset;
            Owner.heldProj = Projectile.whoAmI;
            Owner.itemTime = Owner.itemAnimation = 2;
            Owner.altFunctionUse = 2;
            Owner.ChangeDir(Projectile.direction);
            //将隐形的物品投射至玩家头顶上
            Owner.itemRotation = MathHelper.WrapAngle(MathHelper.PiOver2);
        }
        #region 方法列表
        public void KillProj()
        {
            if (Owner.dead)
            {
                Projectile.Kill();
                Owner.reuseDelay = 2;
                return;
            }
        }
        #endregion
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (projHitbox.Intersects(targetHitbox))
                return true;
            float spinning = Projectile.rotation - MathHelper.PiOver4 * Math.Sign(Projectile.velocity.X);
            float staffRadiusHit = 110f;
            float useless = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + spinning.ToRotationVector2() * -staffRadiusHit, Projectile.Center + spinning.ToRotationVector2() * staffRadiusHit, 23f * Projectile.scale, ref useless))
            {
                return true;
            }
            return false;
        }
        //绘制需要考虑描边
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