using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Materials
{
    public class CoreofEleum : CIMaterials, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Materials";

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
            Lighting.AddLight(Item.Center, 0.15f * brightness, 0.05f * brightness, 0.5f * brightness);
        }

        public override void AddRecipes()
        {
            CreateRecipe(3).
                AddIngredient<EssenceofEleum>().
                AddIngredient(ItemID.Ectoplasm).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
