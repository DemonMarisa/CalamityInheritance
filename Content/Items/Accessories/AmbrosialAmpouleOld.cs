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

namespace CalamityInheritance.Content.Items.Accessories
{
    public class AmbrosialAmpouleOld : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Accessories";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.defense = 4;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            CalamityPlayer calPlayer = player.Calamity();
            CalamityInheritancePlayer usPlayer = player.CalamityInheritance();
            player.endurance = 0.05f;
            //我忽然发现白草瓶是不是没生命恢复?
            player.lifeRegen += 2;
            
            usPlayer.beeResist = true;
            usPlayer.AmbrosialAmpouleOld = true;
            usPlayer.AmbrosialImmnue = true;
            usPlayer.AmbrosialStats = true;

            if (!(calPlayer.rOoze || calPlayer.purity) && !hideVisual)
                Lighting.AddLight(player.Center, new Vector3(1.2f, 1.2f, 0.72f));
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<CrimsonFlask>().
                AddIngredient<ArchaicPowder>().
                AddIngredient<RadiantOoze>().
                AddIngredient<HoneyDew>().
                AddIngredient<StarblightSoot>(15).
                AddIngredient<CryonicBar>(5).
                AddTile(TileID.MythrilAnvil).
                Register();

            CreateRecipe().
                AddIngredient<CorruptFlask>().
                AddIngredient<ArchaicPowder>().
                AddIngredient<RadiantOoze>().
                AddIngredient<HoneyDew>().
                AddIngredient<StarblightSoot>(15).
                AddIngredient<CryonicBar>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
