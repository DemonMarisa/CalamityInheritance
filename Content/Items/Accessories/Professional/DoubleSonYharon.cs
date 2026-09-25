using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.SummonBuff.Weapons;
using CalamityInheritance.Content.Items.Weapons.Summon.Normal.CloseRange;
using CalamityInheritance.Content.Projectiles.Summon.Normal.CloseRange;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using Terraria;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Professional
{
    public class DoubleSonYharon : CIAccessories
    {
        public override int AccessoriesStyle => Professional;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 32;
            Item.rare = RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.defense = 5;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lifeRegen += 4;
            player.moveSpeed += 0.1f;
            player.maxMinions += 10;
            if (player.whoAmI == Main.myPlayer)
            {
                if (player.ownedProjectileCounts[ProjectileType<SonYharonAcc>()] < 2)
                    Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, ProjectileType<SonYharonAcc>(), (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(YharonSonStaff.WeaponDamage), 2f, Main.myPlayer, 0f, 0f);
            }
        }
    }
}
