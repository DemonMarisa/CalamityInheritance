using CalamityInheritance.Common.CalamityModCross.CalDamageClass;
using CalamityInheritance.Content.Items.Accessories.Professional;
using CalamityInheritance.Content.Projectiles.Typeless.Explosions;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Core.GlobalInstance.Players
{
    public partial class CIPlayer : ModPlayer
    {
        public int InvincibleTimer;
        public bool FreeDodgeThisDamage;
        public bool NormalDoge;
        public void ResetDodge()
        {
            if (InvincibleTimer > 0)
                InvincibleTimer--;
            FreeDodgeThisDamage = false;
            NormalDoge = false;
        }
        public override bool ConsumableDodge(Player.HurtInfo info)
        {
            if (NormalDoge)
            {
                if (Main.rand.NextBool(5))
                {
                    Player.SetImmuneTimeForAllTypes(120);
                    FreeDodgeThisDamage = true;
                    return true;
                }
            }
            if (EclispeMirror)
            {
                if (Main.rand.NextBool(7))
                {
                    Player.SetImmuneTimeForAllTypes(120);
                    FreeDodgeThisDamage = true;
                    SoundEngine.PlaySound(SoundID.Item68, Player.Center);
                    //计算闪避时提供的射弹伤。
                    var source = Player.GetSource_Accessory(ItemLoader.GetItem(ItemType<EclispeMirrorLegacy>()).Item);
                    int damage = (int)Player.GetTotalDamage<RogueDamage>().ApplyTo(5000);
                    Projectile.NewProjectile(source, Player.Center, Vector2.Zero, ProjectileType<EclipseMirrorBurst>(), damage, 0, Player.whoAmI);
                    return true;
                }
            }
            return false;
        }
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (FreeDodgeThisDamage)
                return true;
            if (InvincibleTimer > 0)
                return true;
            if (GodSlayerSet && info.Damage < 80)
                return true;
            return false;
        }
    }
}
