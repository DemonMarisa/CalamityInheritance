using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Ranged.TrueScarlet
{
    public class R99ChlorophyteBullet : ModProjectile, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => $"{GetType().Namespace}.ScarletHomingAmmo".Replace('.', '/');
        public int AttackHit
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 2;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 2;
        }
        public override void AI()
        {
            Projectile.light = 1f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.localAI[0]++;
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 50;
            }
            if (Projectile.alpha < 170)
            {
                if (Projectile.localAI[0] > 1f)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        float x2 = Projectile.Center.X - Projectile.velocity.X / 10f * i;
                        float y2 = Projectile.Center.Y - Projectile.velocity.Y / 10f * i;
                        Vector2 newVec = new(x2, y2);
                        int dust = Dust.NewDust(newVec, 1, 1, DustID.CursedTorch);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= 0f;
                        Main.dust[dust].position = newVec;
                        Main.dust[dust].alpha = Projectile.alpha;
                    }
                }
            }
            NPC target = Projectile.FindClosestTarget(1800f);
            if (target != null)
                Projectile.HomingNPCBetter(target, 1800f, 18f, 20f, ignoreDist: true);
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DefenseEffectiveness *= 0f;
            modifiers.FinalDamage *= 1f / (1f - target.Calamity().dragonFire);
            Player r99Owner = Main.player[Projectile.owner];
            if (r99Owner.ActiveItem().type != ModContent.ItemType<R99>())
                return;
            ref int isShooted = ref r99Owner.CIMod().R99Shooting;
            ref int targetIndex = ref r99Owner.CIMod().R99TargetWhoAmI;
            if (targetIndex != target.whoAmI)
                isShooted = 0;
            else
            {
                isShooted += 1;
                if (isShooted > R99.CrackedShieldTime)
                {
                    if (Main.rand.Next(15) - Projectile.CalamityInheritance().CurR99Chance <= 0)
                    {
                        modifiers.FinalDamage *= 600;
                        Projectile.CalamityInheritance().CurR99Chance = 0;
                    }
                    else
                        Projectile.CalamityInheritance().CurR99Chance += 1;
                }
            } 
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            Player player = Main.player[Projectile.owner];
            if (player.ActiveItem().type == ModContent.ItemType<R99>())
            {
                //处理声音
                //大残！
                SoundStyle[] R99HitShield =
                [
                    CISoundMenu.R99ShieldHit1,
                    CISoundMenu.R99ShieldHit2

                ];
                //碎甲！！
                SoundStyle[] R99ShieldCracked =
                [
                    CISoundMenu.R99ShieldCracked1,
                    CISoundMenu.R99ShieldCracked2,
                    CISoundMenu.R99ShieldCracked3,
                    CISoundMenu.R99ShieldCracked4
                ];
                //一丝！！！
                SoundStyle[] R99FleshHit =
                [
                    CISoundMenu.R99FleshHit1,
                    CISoundMenu.R99FleshHit2,
                    CISoundMenu.R99FleshHit3,
                    CISoundMenu.R99FleshHit4
                ];
                SoundStyle crackedShield = Utils.SelectRandom(Main.rand, R99ShieldCracked);
                SoundStyle hitSoundShield = Utils.SelectRandom(Main.rand, R99HitShield);
                SoundStyle fleshHit = Utils.SelectRandom(Main.rand, R99FleshHit);
                ref int shootedTime = ref player.CIMod().R99Shooting;
                ref int soundDelay = ref player.CIMod().GlobalSoundDelay;
                ref int targetIndex = ref player.CIMod().R99TargetWhoAmI;
                if (AttackHit == 0f)
                {
                    for (int i = 0; i < 2; i++)
                        Dust.NewDust(Projectile.position, 10, 10, shootedTime < R99.CrackedShieldTime ? DustID.CursedTorch: DustID.Blood, Projectile.velocity.X * 0.2f, Projectile.velocity.Y / 4f, 0, default, 1f);

                    AttackHit = 1;
                }
                //不是同一个目标就刷新一次攻击总数
                if (targetIndex == -1)
                    targetIndex = target.whoAmI;
                if (targetIndex != target.whoAmI)
                {
                    shootedTime = 0;
                    targetIndex = target.whoAmI;
                }

                //专门处理碎甲音效
                if (shootedTime > R99.CrackedShieldTime && shootedTime < R99.FleshHitTime)
                {
                    if (Main.zenithWorld)
                        SoundEngine.PlaySound(CISoundMenu.Pipes with { MaxInstances = 0, Volume = 0.85f }, target.Center);

                    else
                        SoundEngine.PlaySound(crackedShield with { Volume = 1.15f }, target.Center);
                    shootedTime = R99.FleshHitTime;
                    soundDelay = 60;
                }
                if (soundDelay != 0)
                    return;

                soundDelay = 4;
                //打肉音效
                if (shootedTime > R99.FleshHitTime)
                {
                    shootedTime = R99.FleshHitTime;
                    SoundEngine.PlaySound(fleshHit with { MaxInstances = 0, Volume = 0.75f }, target.Center);
                }
                //打甲音效
                else if (shootedTime < R99.CrackedShieldTime)
                {
                    shootedTime++;
                    SoundEngine.PlaySound(hitSoundShield with { MaxInstances = 0, Volume = 0.75f }, target.Center);
                }
            }
            //不手持R99就固定使用铂金粒子
            else
            {
                if (AttackHit == 0f)
                {
                    for (int i = 0; i < 2; i++)
                        Dust.NewDust(Projectile.position, 10, 10, DustID.PlatinumCoin, Projectile.velocity.X * 0.2f, Projectile.velocity.Y / 4f, 0, default, 1f);
                    AttackHit = 1;
                }
            }
        }
    }
}