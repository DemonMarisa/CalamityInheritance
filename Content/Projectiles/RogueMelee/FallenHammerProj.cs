using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.RogueMelee;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.Typeless.Explosions;
using LAP.Core.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CalamityInheritance.Content.Projectiles.RogueMelee
{
    public class FallenHammerProj : CIMeleeRogueProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<FallenHammer>();
        public override string Texture => GetInstance<FallenHammer>().Texture;
        public int Timer;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 62;
            Projectile.friendly = true;
            ModItem item = Projectile.Owner()?.HeldItem?.ModItem;
            if (item is not null && item is CIMeleeRogue rogue && rogue.IsMelee)
                Projectile.DamageType = DamageClass.Melee;
            else
                Projectile.DamageType = RogueDamage.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30 * 3;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.35f, 0.35f, 0f);
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 8;
                SoundEngine.PlaySound(CISoundID.SoundBoomerangs, Projectile.position);
            }
            if (Projectile.CI().Stealth)
                Projectile.extraUpdates = 5;
            Timer++;
            if (Timer == 25)
            {
                if (Projectile.CI().Stealth)
                {
                    if (Projectile.IsLocalPlayer())
                    {
                        Vector2 firevel = Main.LocalPlayer.GetPlayerToMouseVector2() * 16f;
                        Projectile.NewProj(ProjectileType<FallenHammerProj_Rogue>(), Projectile.Center, firevel);
                    }
                    SoundEngine.PlaySound(SoundID.Item4 with { Volume = 0.3f }, Projectile.Center); //即将开始追踪前, 播报落星的声音
                    DustCircle(Projectile.Center, 20f, 2.2f, DustID.GemRuby, true, 4.8f);
                }
            }
            int Regturn = 45;
            if (Projectile.CI().Stealth)
                Regturn = 75;
            if (Timer > Regturn)
            {
                Projectile.tileCollide = false;
                float rSpeed = 16f;
                float accele = 3.2f;
                Player plr = Main.player[Projectile.owner];
                Projectile.PredictHomeIn(rSpeed, accele, plr);
                if (Main.myPlayer == Projectile.owner)
                {
                    Rectangle rectangle = new((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
                    Rectangle value2 = new((int)Main.player[Projectile.owner].position.X, (int)Main.player[Projectile.owner].position.Y, Main.player[Projectile.owner].width, Main.player[Projectile.owner].height);
                    if (rectangle.Intersects(value2))
                    {
                        if (Projectile.CI().Stealth && Projectile.IsLocalPlayer())
                        {
                            Vector2 firevel = Main.LocalPlayer.GetPlayerToMouseVector2() * 16f;
                            Projectile.NewProj(ProjectileType<FallenHammerProj_HomeIn>(), Projectile.Center, firevel, 2f);
                        }
                        Projectile.Kill();
                    }
                }
            }
            Projectile.rotation += 0.4f;
            return;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIBrimstoneFlames>(), 240);
            OnHitEffect();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffType<CIBrimstoneFlames>(), 240);
            OnHitEffect();
        }

        private void OnHitEffect()
        {
            if (Projectile.owner == Main.myPlayer)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<FuckYou>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
                if (proj.InBounds(Main.maxProjectiles))
                    Main.projectile[proj].DamageType = DamageClass.MeleeNoSpeed;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor, 1);
            return false;
        }
    }
}
