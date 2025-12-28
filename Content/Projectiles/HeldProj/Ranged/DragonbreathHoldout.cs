using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.Content.Items.Weapons.Ranged;
using CalamityInheritance.System.Configs;
using CalamityMod;
using CalamityMod.Items.Placeables.FurnitureOtherworldly;
using CalamityMod.Items.Weapons.Melee;
using LAP.Core.Utilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Projectiles.HeldProj.Ranged
{
    public class DragonbreathHoldout : BaseHeldProj, ILocalizedModType
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public new string LocalizationCategory => "Content.Projectiles.Ranged";
        public override string Texture => "CalamityInheritance/Content/Items/Weapons/Ranged/DragonsBreathold";
        public Player Owner => Main.player[Projectile.owner];
        public override bool? CanHitNPC(NPC target) => false;
        public override void HoldoutAI()
        {
            ref float UseCounter = ref Projectile.ai[1];
            Vector2 fireDirection = Vector2.UnitX.RotatedBy(Projectile.rotation);
            fireDirection = fireDirection.SafeNormalize(Vector2.UnitX);
            int ut = Owner.HeldItem.useTime;

            if (UseCounter % ut == 0)
            {
                int PelletsPerShot = DragonsBreathold.PelletsPerShot;
                int[] bulletIDs = new int[PelletsPerShot];
                Owner.PickAmmo(Owner.ActiveItem(), out int proj, out float shootSpeed, out int damage, out float kb, out _);
                damage = (int)(damage * DragonsBreathold.FullAutoDamageMult);
                //初始化子弹ID数组，全部设定为龙息弹
                for (int i = 0; i < PelletsPerShot; ++i)
                    bulletIDs[i] = Owner.HeldItem.shoot;
                int dragonsBreathAdded = 0;
                //然后，随机替换其中的一半为普通的子弹
                while (dragonsBreathAdded < PelletsPerShot / 2)
                {
                    int i = Main.rand.Next(PelletsPerShot);
                    //如果这个位置被替换，跳过
                    if (bulletIDs[i] == Owner.HeldItem.shoot)
                        continue;
                    //将其改为龙息弹
                    bulletIDs[i] = proj;
                    ++dragonsBreathAdded;
                }
                float spreadFactor = 1.22f;
                //最后才是实际发射逻辑，散步转角
                float Spread = 0.018f;
                float angleOffset = Spread * -0.5f * (PelletsPerShot - 1) * spreadFactor;
                for (int i = 0; i < PelletsPerShot; ++i)
                {
                    Vector2 rotatedVel = (fireDirection * shootSpeed).RotatedBy(angleOffset);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Vector2.Zero /*GetBulletFirePos()*/, rotatedVel , bulletIDs[i], damage, kb, Owner.whoAmI);
                    angleOffset += Spread * spreadFactor;
                }
            }
        }
    }
}