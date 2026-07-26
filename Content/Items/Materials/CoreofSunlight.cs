using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass;
using CalamityInheritance.Core.Utils;
using Terraria;
using Terraria.ID;

namespace CalamityInheritance.Content.Items.Materials
{
    public class CoreofSunlight : CIMaterials
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 40);
            Item.rare = ItemRarityID.Yellow;
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float brightness = Main.essScale * Main.rand.NextFloat(0.9f, 1.1f);
            Lighting.AddLight(Item.Center, 0.3f * brightness, 0.3f * brightness, 0.05f * brightness);
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe(3).
                    AddIngredient(CalamityMaterials.EssenceofEleum).
                    AddIngredient(ItemID.Ectoplasm).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {
                CreateRecipe(3).
                    AddIngredient(ItemID.SoulofFlight).
                    AddIngredient(ItemID.Ectoplasm).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
        }
    }
}
