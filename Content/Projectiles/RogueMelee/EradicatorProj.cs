using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.BaseClass.Projectiles;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Items.Weapons.RogueMelee;
using CalamityInheritance.Content.Projectiles.Melee.Yoyos;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.RogueMelee
{
    public class EradicatorProj : CIMeleeRogueProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<Eradicator>();
        public override string Texture => GetInstance<Eradicator>().Texture;
        private static float RotationIncrement = 0.15f;
        private static int Lifetime = 350;
        private static int ReboundTime = 60;
        public int Time;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 62;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 2;
            Projectile.timeLeft = Lifetime;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 3;
            ModItem item = Projectile.Owner()?.HeldItem?.ModItem;
            if (item is not null && item is CIMeleeRogue rogue && rogue.IsMelee)
                Projectile.DamageType = DamageClass.Melee;
            else
                Projectile.DamageType = RogueDamage.Instance;
        }

        public override void AI()
        {
            Time++;
            if (Projectile.timeLeft == Lifetime - ReboundTime)
                Projectile.netUpdate = true;
            if (Projectile.timeLeft <= Lifetime - ReboundTime)
            {
                Player owner = Main.player[Projectile.owner];

                Projectile.PredictHomeIn(12f, 12f, owner);

                if (Main.myPlayer == Projectile.owner)
                    if (Projectile.Hitbox.Intersects(owner.Hitbox))
                        Projectile.Kill();
            }
            Lighting.AddLight(Projectile.Center, 0.35f, 0f, 0.25f);
            float spin = Projectile.direction <= 0 ? -1f : 1f;
            Projectile.rotation += spin * RotationIncrement;
            if (Time % 10 == 0)
            {
                if (LAPUtilities.IsLocalPlayer(Projectile.owner))
                {
                    NPC npc = LAPUtilities.FindClosestTarget(Projectile.Center, 900);
                    if (npc is not null)
                    {
                        int damage = (int)(Projectile.damage * 0.8f);
                        Vector2 vel = LAPUtilities.GetVector2(Projectile.Center, npc.Center) * 9f;
                        Projectile proj = LAPUtilities.NewProjWithClass(Projectile.GetSource_FromThis(), Projectile.Center, vel, ProjectileType<NebulaShotLegacy>(), damage, Projectile.knockBack, Projectile.owner, DamageClass.Melee);
                    }
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffType<CIGodSlayerInferno>(), 180);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            LAPUtilities.DrawAfterimages(Projectile, ProjectileID.Sets.TrailingMode[Projectile.type], lightColor);
            return false;
        }
    }
}
