using CalamityInheritance.Tiles.Vanity;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Rarity;
using CalamityInheritance.World;
using CalamityMod;
using CalamityMod.World;
using Microsoft.Xna.Framework.Input;
using Terraria.Localization;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Items.Placeables.Vanity
{
    public class Revenge : CIPlaceable, ILocalizedModType
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
            Item.createTile = ModContent.TileType<RevengeTiles>();
            Item.rare = ModContent.RarityType<PureRed>();
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
        {
            if (CalamityUtils.AnyBossNPCS())
                return false;
            if (player.altFunctionUse == 2)
            {
                Item.consumable = true;
                Item.UseSound = SoundID.Item1;
            }
            else
            {
                Item.useAnimation = 45;
                Item.useTime = 45;
                Item.useStyle = ItemUseStyleID.HoldUp;
                Item.UseSound = SoundID.Item119;
            }
            return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                if (!CalamityWorld.revenge)
                {
                    CalamityWorld.revenge = true;
                    CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.RevengeText", Color.Crimson);
                }
                else
                {
                    CalamityWorld.revenge = false;
                    CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.RevengeText2", Color.Crimson);
                    if (CalamityWorld.death)
                    {
                        CalamityWorld.death = false;
                        CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.DeathText2", Color.Crimson);
                    }
                    if (CIWorld.Malice)
                    {
                        CIWorld.Malice = false;
                        CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.MaliceText2", Color.LightGoldenrodYellow);
                    }
                }
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
