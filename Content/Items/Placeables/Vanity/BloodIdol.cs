using CalamityInheritance.Rarity;
using CalamityInheritance.Tiles.Vanity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Placeables.Vanity
{
    public class BloodIdol : CIPlaceable, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Local}.Vanity";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = false;
            Item.createTile = ModContent.TileType<BloodIdolTiles>();
            Item.rare = ModContent.RarityType<PureRed>();
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.consumable = true;
                Item.UseSound = SoundID.Item1;
            }
            else
            {
                if (Main.IsItDay())
                    return false;
                Item.useAnimation = 45;
                Item.useTime = 45;
                Item.useStyle = ItemUseStyleID.HoldUp;
                Item.UseSound = SoundID.Item119;
            }
            return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse != 2)
            {
                if(!Main.IsItDay())
                {
                    if (!Main.bloodMoon)
                    {
                        Main.bloodMoon = true;
                        CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.BloodMoon", Color.Crimson);
                    }
                    if (Main.bloodMoon)
                    {
                        CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.BloodMoon2", Color.Crimson);
                    }
                }
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BloodOrb>(10).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
