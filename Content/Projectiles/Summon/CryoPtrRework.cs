using System;
using CalamityInheritance.Buffs.Legendary;
using CalamityInheritance.Content.Items;
using CalamityInheritance.Content.Items.Weapons.Legendary;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Summon
{
    public class CryoPtrRework : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Summon";
        public override string Texture => $"{GenericProjRoute.ProjRoute}/Summon/CryogenPtr";
        public float FireDelay = 0f;
        public float HomingDelay = 0f;
        const int AroundingAI = 0;
        const int ProjPhase = 1;
        const int Target = 2;
        const float ReadyToFire = 0f;
        const float HomingToTarget = 1f;
        public bool AroundPlayer = false;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 60;
            Projectile.netImportant = true;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.minionSlots = 1f;
            Projectile.timeLeft = 18000;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
        }
        public override bool? CanDamage()
        {
            return FireDelay <= 0 && HomingDelay > 15f && Projectile.ai[ProjPhase] != ReadyToFire;
        }
        public override void AI()
        {
            //标记召唤物使用
            Player p = Main.player[Projectile.owner];
            var mp = p.CIMod();
            var src = Projectile.GetSource_FromThis();
            if (p.dead)
                mp.IsColdDivityActiving = false;
            if (!mp.IsColdDivityActiving)
            {
                Projectile.active = false;
                return;
            }

            //计时器自转
            if (FireDelay > 0)
                FireDelay--;
            
            if (FireDelay == 0)
            {
                SpawnDust(Projectile, 20);
                SoundEngine.PlaySound(CISoundID.SoundIceRodBlockPlaced with {Pitch = 0.2f}, Projectile.Center);
                Projectile.netUpdate = true;
            }
            //执行AI逻辑
            switch (Projectile.ai[ProjPhase])
            {
                case ReadyToFire:
                    ReadyFire(p);
                    break;
                case HomingToTarget:
                    //转角，一定要保住转角啊啊啊啊啊啊啊啊啊啊
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                    HomingDelay += 1f;
                    //获得这个Target,
                    int getTar = (int)Projectile.ai[2];
                    NPC realTar = Main.npc[getTar];
                    //索敌
                    if (HomingDelay > 15f)
                        CIFunction.HomingNPCBetter(Projectile, realTar, 3200f, CyrogenLegendary.ShootSpeed, 20f);
                    break;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //命中敌人给debuff
            target.AddBuff(BuffType<CryoDrain>(), 300);
            SpawnDust(Projectile, 20);
            base.OnHitNPC(target, hit, damageDone);
        }
        //发射弹幕
        public void ReadyFire(Player p, bool upgrade = false)
        {
            float num = FireDelay == 0 ? 90f : (300 - FireDelay) / 3;
            float stdDist = num > 90f ? 90f : num;
            //取玩家中心点旋转
            Projectile.Center = p.Center + Projectile.ai[AroundingAI].ToRotationVector2() * stdDist;
            //别忘了转角
            Projectile.rotation = Projectile.ai[AroundingAI] + (float)Math.Atan(90);
            Projectile.ai[AroundingAI] -= MathHelper.ToRadians(4f);
            //寻找附近的单位
            NPC aliveTarget = CIFunction.FindClosestTarget(Projectile, 800f, true);
            //搜索到单位，发射弹幕
            if (aliveTarget != null && Projectile.owner == Main.myPlayer)
            {
                FireDelay = upgrade ? 90f : 180f;
                Vector2 vel = Projectile.ai[AroundingAI].ToRotationVector2().RotatedBy(Math.Atan(0));
                vel.Normalize();
                vel *= 30f;
                int shard = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, vel, Projectile.type, (int)(Projectile.damage * 1.1f), Projectile.knockBack, Projectile.owner, Projectile.ai[AroundingAI], HomingToTarget, aliveTarget.whoAmI);
                //动态变化其伤害
                if (Main.projectile.IndexInRange(shard))
                    Main.projectile[shard].originalDamage = (int)(Projectile.originalDamage * 1.1f);
            }
            //多人同步
            Projectile.netUpdate = Projectile.owner == Main.myPlayer;
        }
        public static void SpawnDust(Projectile proj, int dAmt)
        {
            for (int i = 0; i < dAmt; i++)
                Dust.NewDust(proj.Center, proj.width, proj.height, DustID.Ice, Main.rand.NextFloat(1, 3), Main.rand.NextFloat(1, 3), 0, Color.Cyan, Main.rand.NextFloat(0.5f, 1.5f));
        }
    }
}