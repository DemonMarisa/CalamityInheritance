using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.GlobalInstance.Players;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Accessories.Restorative
{
    public class DeificAmuletLegacy : CIAccessories
    {
        public override int AccessoriesStyle => Restorative;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = Item.height = 48;
            Item.rare = ItemRarityID.LightRed;
            Item.value = CIShopValue.RarityPriceLightRed;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.CI().DeificAmuletFallenStar = true;
            player.LAP().ExImmuneTime += 25;
            player.pStone = true;
            player.LAP().LifeRegen += 1;
            player.GetArmorPenetration<GenericDamageClass>() += 10;
            if (player.IsUnderwater())
                Lighting.AddLight(player.Center, new Vector3(1, 1, 1));
        }
        public static void DeificAmuletLegacy_OnHurt(CIPlayer player)
        {
            var source = player.Player.GetSource_Accessory(ItemLoader.GetItem(ItemType<DeificAmuletLegacy>()).Item);
            for (int n = 0; n < 3; n++)
            {
                int deificStarDamage = (int)player.Player.GetTotalDamage<GenericDamageClass>().ApplyTo(230);

                Projectile star = CIUtils.ProjectileRain(source, player.Player.Center, 400f, 100f, 500f, 800f, 29f,
                ProjectileID.StarVeilStar, deificStarDamage, 4f, player.Player.whoAmI);
                star.DamageType = DamageClass.Generic;
                star.usesLocalNPCImmunity = true;
                star.localNPCHitCooldown = 5;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.StarVeil).
                AddIngredient(ItemID.CharmofMyths).
                AddIngredient(ItemID.MeteoriteBar, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
