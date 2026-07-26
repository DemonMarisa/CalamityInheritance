using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Melee.GreatSwords;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using CalamityInheritance.Core.Utils;
using LAP.Common.CalamityModCross;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.GreatSwords
{
    public class GreatswordofBlah : CIMelee
    {
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee;
            Item.width = Item.height = 108;
            Item.damage = 128;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.useTurn = false;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = RarityType<DeepBlue>();
            Item.shoot = ProjectileType<GreatswordofBlahProj>();
            Item.shootSpeed = 6f;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient<GreatswordofJudgementLegacy>().
                    AddIngredient(CalamityMaterials.CosmiliteBar, 8).
                    AddIngredient(CalamityMaterials.EndothermicEnergy, 20).
                    AddTile(CalTileID.CosmicAnvilID).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient<GreatswordofJudgementLegacy>().
                    AddTile<DraedonsForgeoldTile>().
                    Register();
            }
        }
    }
}
