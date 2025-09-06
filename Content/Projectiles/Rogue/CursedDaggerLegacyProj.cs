using System;
using CalamityInheritance.Content.Items.Weapons.Rogue;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue
{
    //要是想重置下特效也行，不过我暂时找不到这武器需要重置特效的必要
    //这里有一个基于射弹速度修改生成频率时间戳的方式。用来尽可能解决射弹速度越快反而会导致总衍生射弹量减少的问题
    public class CursedDaggerProjLegacy : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Rogue";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Rogue/CursedDaggerLegacy";
        
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 8;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.timeLeft = 240;
            Projectile.penetrate = 8;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!Projectile.Calamity().stealthStrike)
            {
                Projectile.penetrate--;
                if (Projectile.penetrate <= 0)
                {
                    Projectile.Kill();
                }
                else
                {
                    Projectile.ai[0] += 0.1f;
                    if (Projectile.velocity.X != oldVelocity.X)
                    {
                        Projectile.velocity.X = -oldVelocity.X;
                    }
                    if (Projectile.velocity.Y != oldVelocity.Y)
                    {
                        Projectile.velocity.Y = -oldVelocity.Y;
                    }
                }
            }
            else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                    Projectile.velocity.X = -oldVelocity.X;

                if (Projectile.velocity.Y != oldVelocity.Y)
                    Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Projectile.BaseProjPreDraw(tex, 4, lightColor);
            return false;
        }
        //基础生成间隔
        const int BasicSpawnRate = 8;
        //基础发射的速度
        const float BaseSpawnSpeed = CursedDaggerLegacy.ShootSpeed;
        //最小生成间隔（避免频率过高）
        const float MinSpawnRate = 4;
        //最大生成间隔（避免频率过低）
        const float MaxSpawnRate = 14;
        public override void AI()
        {
            //计算当前速度的模长
            float curSpeed = Projectile.velocity.Length();
            //速度越快，间隔越小；速度越慢，间隔越大。
            //这里基于基础速度鱼基础间隔进行比例计算，还有不要删除这个“不必要”的括号
            float dynamicSpawnRate = (BaseSpawnSpeed / curSpeed) * BasicSpawnRate;
            //将其控制在一个合理值，防止极端
            dynamicSpawnRate = MathHelper.Clamp(dynamicSpawnRate, MinSpawnRate, MaxSpawnRate);
            //最后转化为整数
            int spawnRates = (int)Math.Round(dynamicSpawnRate);

            
            if (Main.rand.NextBool(4))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, (int)CalamityDusts.SulphurousSeaAcid, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }

            if (!Projectile.Calamity().stealthStrike && Projectile.owner == Main.myPlayer)
            {
                if (Projectile.timeLeft % spawnRates == 0)
                {
                    Vector2 velo = new(0f, Main.rand.NextFloat(10f, 14f));
                    int flame = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velo, ProjectileID.CursedDartFlame, Projectile.damage, Projectile.knockBack * 0.5f, Projectile.owner);
                    if (flame.WithinBounds(Main.maxProjectiles))
                    {
                        Main.projectile[flame].DamageType = ModContent.GetInstance<RogueDamageClass>();
                        Main.projectile[flame].usesLocalNPCImmunity = true;
                        Main.projectile[flame].localNPCHitCooldown = 10;
                    }
                    return;
                }
            }
            if (Projectile.timeLeft % spawnRates == 0)
            {
                Vector2 velocity = new(Main.rand.NextFloat(-14f, 14f), Main.rand.NextFloat(-14f, 14f));
                int flame = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, Main.rand.NextBool(2) ? ProjectileID.CursedFlameFriendly : ProjectileID.CursedDartFlame, Projectile.damage, Projectile.knockBack * 0.5f, Projectile.owner);
                if (flame.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[flame].DamageType = ModContent.GetInstance<RogueDamageClass>();
                    Main.projectile[flame].usesLocalNPCImmunity = true;
                    Main.projectile[flame].localNPCHitCooldown = 10;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.CursedInferno, 600);
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 10; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, (int)CalamityDusts.SulphurousSeaAcid, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }
    }
}