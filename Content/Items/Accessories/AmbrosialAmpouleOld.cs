using CalamityMod.CalPlayer;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using Microsoft.Xna.Framework;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityInheritance.Content.Items.Materials;

namespace CalamityInheritance.Content.Items.Accessories
{
    public class AmbrosialAmpouleOld : CIAccessories, ILocalizedModType
    {
        protected override BaseSetDefault BaseSD => new
        (
            itemWidth:20,
            itemHeight:20,
            itemRare:ItemRarityID.Red,
            itemValue:CIShopValue.RarityPriceRed,
            itemDefense:4
        );
        public override void ExSSD() => Type.ShimmerEach<AmbrosialAmpoule>();
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer calPlayer = player.Calamity();
            CalamityInheritancePlayer usPlayer = player.CIMod();
            player.endurance += 0.05f;
            
            usPlayer.FuckYouBees = true;
            usPlayer.AmbrosialAmpouleOld = true;
            usPlayer.AmbrosialImmnue = true;
            usPlayer.AmbrosialStats = true;

            if (!(calPlayer.rOoze || calPlayer.purity) && !hideVisual)
                Lighting.AddLight(player.Center, new Vector3(1.2f, 1.2f, 0.72f));
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(CIRecipeGroup.AnyEvilFlask).
                AddIngredient<ArchaicPowder>().
                AddIngredient<RadiantOoze>().
                AddIngredient<HoneyDew>().
                AddIngredient<StarblightSoot>(15).
                AddIngredient<CryoBar>(5). //修改为冰灵锭
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
