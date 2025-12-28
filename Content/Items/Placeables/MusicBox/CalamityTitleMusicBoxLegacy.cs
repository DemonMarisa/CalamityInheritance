using CalamityInheritance.CIPlayer;
using CalamityInheritance.Tiles.MusicBox;
using CalamityInheritance.UI.MusicUI.MusicButton;
using CalamityInheritance.UI.MusicUI;
using CalamityMod.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Accessories;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items.Placeables;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Placeables.Crags;

namespace CalamityInheritance.Content.Items.Placeables.MusicBox
{
    public class CalamityTitleMusicBoxLegacy : CIPlaceable, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Local}.MusicBox";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Music/TheTaleofaCruelWorld/TheTaleofaCruelWorldNor"), ModContent.ItemType<CalamityTitleMusicBoxLegacy>(), ModContent.TileType<CalamityTitleMusicBoxTitle>());
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(ModContent.TileType<CalamityTitleMusicBoxTitle>(), 0);
            Item.consumable = false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
            AddIngredient(ItemID.MusicBox).
                AddIngredient(ModContent.ItemType<BrimstoneSlag>(), 12).
                AddIngredient(ModContent.ItemType<EssenceofHavoc>(), 3).
                AddTile(ModContent.TileType<AshenAltar>()).
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
