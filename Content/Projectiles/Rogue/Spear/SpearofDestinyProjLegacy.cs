using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.Items.Weapons.Rogue.Spear;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Rogue.Spear
{
    public class SpearofDestinyProjLegacy : ModProjectile, ILocalizedModType
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<SpearofDestinyLegacy>();
        public override string Texture => GetInstance<SpearofDestinyLegacy>().Texture;

        private bool initialized = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 600;
            Projectile.DamageType = RogueDamage.Instance;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void SendExtraAI(BinaryWriter writer) => writer.Write(initialized);
        public override void ReceiveExtraAI(BinaryReader reader) => initialized = reader.ReadBoolean();
        public override void AI()
        {
            if (!initialized)
            {
                if (Projectile.CI().Stealth)
                {
                    Projectile.penetrate++;
                    Projectile.tileCollide = false;
                }
                initialized = true;
            }
            if (Main.rand.NextBool(4))
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height,
                DustID.GoldCoin, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            Projectile.HomeInNPC(800f, 20f, 35f);

            Vector2 center = Projectile.Center;
            float maxDistance = 800f;
            bool homeIn = false;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].CanBeChasedBy(Projectile, false))
                {
                    float extraDistance = Main.npc[i].width / 2 + (float)(Main.npc[i].height / 2);
                    bool canHit = Projectile.CI().Stealth || Collision.CanHit(Projectile.Center, 1, 1, Main.npc[i].Center, 1, 1);

                    if (Vector2.Distance(Main.npc[i].Center, Projectile.Center) < maxDistance + extraDistance && canHit)
                    {
                        center = Main.npc[i].Center;
                        homeIn = true;
                        break;
                    }
                }
            }

            if (homeIn)
            {
                Projectile.extraUpdates = 2;
                Vector2 moveDirection = LAPUtilities.GetVector2(Projectile.Center, center);
                Projectile.velocity = (Projectile.velocity * 20f + moveDirection * 12f) / 21f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i <= 10; i++)
            {
                Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height,
                DustID.GoldCoin, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => target.AddBuff(BuffID.Ichor, 120);

        public override void OnHitPlayer(Player target, Player.HurtInfo info) => target.AddBuff(BuffID.Ichor, 120);
    }
}
