using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Weapons.RogueMelee;
using CalamityInheritance.Content.Misc;
using LAP.Core.Utilities;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.RogueMelee
{
    public class KelvinCatalystProj_Rogue : CIMeleeRogueProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<KelvinCatalyst>();
        public override string Texture => GetInstance<KelvinCatalyst>().Texture;
        public int Timer;
        public int ChargeCount;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 70;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            ModItem item = Projectile.Owner()?.HeldItem?.ModItem;
            if (item is not null && item is CIMeleeRogue rogue && rogue.IsMelee)
                Projectile.DamageType = DamageClass.Melee;
            else
                Projectile.DamageType = RogueDamage.Instance;
            Projectile.coldDamage = true;
            Projectile.extraUpdates = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Timer);
            writer.Write(ChargeCount);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Timer = reader.ReadInt32();
            ChargeCount = reader.ReadInt32();
        }
        public override void AI()
        {
            Timer++;
            VisualAudioEffects();
            if (Timer < 45)
            {
                if (ChargeCount <= 3)
                    Projectile.velocity *= 0.97f;
                else if (ChargeCount == 0)
                    Projectile.velocity *= 0.92f;
            }
            else
            {
                NPC npc = LAPUtilities.FindClosestTarget(Projectile.Center, 1500f, true);
                if (npc is not null)
                {
                    if (ChargeCount < 3)
                        Projectile.HomingTarget(npc.Center, 1500f, 18f, 0f);
                }
                else
                    Projectile.velocity *= 0.92f;
            }
            if (ChargeCount >= 3 && Timer > 45 || Timer > 180)
            {
                Player owner = Main.player[Projectile.owner];
                Projectile.PredictHomeIn(24f, 10f, owner);
                if (Main.myPlayer == Projectile.owner)
                    if (Projectile.Hitbox.Intersects(owner.Hitbox))
                        Projectile.Kill();
            }
        }
        public void VisualAudioEffects()
        {
            Lighting.AddLight(Projectile.Center, Main.DiscoR * 0.3f / 255f, Main.DiscoR * 0.4f / 255f, Main.DiscoR * 0.5f / 255f);

            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 60;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.Center);
            }

            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.IceRod, 0f, 0f, 100, default, 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0f;

            Projectile.rotation += 0.25f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (ChargeCount < 3 && Timer > 45)
            {
                ChargeCount++;
                Timer = 0;
            }
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 velocity = (MathHelper.TwoPi * i / 5f).ToRotationVector2() * 4f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                                             Projectile.Center,
                                             velocity,
                                             ProjectileType<KelvinCatalystProjStar>(),
                                             (int)(Projectile.damage * 0.7f), //冰星伤害0.5→0.7
                                             Projectile.knockBack * 0.5f,
                                             Projectile.owner);
                }
                SoundEngine.PlaySound(SoundID.Item30, Projectile.Center);
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 2);
            return false;
        }
    }
}
