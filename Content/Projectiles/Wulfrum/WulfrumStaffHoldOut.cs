using CalamityInheritance.Content.Items.Weapons.Wulfrum;
using LAP.Core.BaseClass.Projectiles;
using LAP.Core.SystemsLoader;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.Wulfrum
{
    public class WulfrumStaffHoldOut : BaseHeldProj
    {
        public override LocalizedText DisplayName => LAPUtilities.GetItemName<WulfrumStaff>();
        public override string Texture => GetInstance<WulfrumStaff>().Texture;
        public int NeedFire = 0;
        public int SecondDely = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
            ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
            Projectile.AddHeldProj();
        }
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            RotAmount = 0.25f;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void ExAI()
        {
            Owner.SetArmRot(Projectile.rotation, false);
            DrawRotOffset = 0.35f * Projectile.direction;
            DrawPosOffset = Projectile.rotation.ToRotationVector2() * 30;
            if (SecondDely > 0)
                SecondDely--;
            if (UseDelay <= 0 && Owner.CheckMana(Owner.ActiveItem(), (int)(Owner.HeldItem.mana * Owner.manaCost), true, false) && Owner.LAP().MouseLeft)
            {
                NeedFire = 3;
                UseDelay = 60;
            }
            if (NeedFire > 0 && SecondDely <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item43, Projectile.Center);
                Vector2 firedirection = Projectile.rotation.ToRotationVector2();
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, firedirection * 9f, ProjectileType<WulfrumBoltOld>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                SecondDely = 10;
                NeedFire--;
            }
        }
    }
}
