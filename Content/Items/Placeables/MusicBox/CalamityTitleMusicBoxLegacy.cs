using CalamityInheritance.CIPlayer;
using CalamityInheritance.Tiles.MusicBox;
using CalamityInheritance.UI.MusicUI;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Placeables.Crags;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Placeables.MusicBox
{
    public class CalamityTitleMusicBoxLegacy : CIPlaceable, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Local}.MusicBox";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Music/TheTaleofaCruelWorld/TheTaleofaCruelWorldNor"), ItemType<CalamityTitleMusicBoxLegacy>(), TileType<CalamityTitleMusicBoxTitle>());
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(TileType<CalamityTitleMusicBoxTitle>(), 0);
            Item.consumable = false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
            AddIngredient(ItemID.MusicBox).
                AddIngredient(ItemType<BrimstoneSlag>(), 12).
                AddTile(TileType<AshenAltar>()).
                Register();
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool? UseItem(Player player)
        {
            CalamityInheritancePlayer cIPlayer = player.CIMod();

            if (player.altFunctionUse == 2 && MusicChoiceUI.ChangeCd == 0)
                MusicChoiceUI.active = !MusicChoiceUI.active;
            return true;
        }
    }
}
