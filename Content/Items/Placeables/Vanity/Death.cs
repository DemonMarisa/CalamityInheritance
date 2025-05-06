using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Tiles.Vanity;
using CalamityInheritance.Rarity;
using CalamityMod.World;
using CalamityMod;
using CalamityInheritance.World;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Items.Placeables.Vanity
{
    public class Death : CIPlaceable, ILocalizedModType
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
            Item.createTile = ModContent.TileType<DeathTiles>();
            Item.rare = ModContent.RarityType<CatalystViolet>();
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
                if (!CalamityWorld.death)
                {
                    if (!CalamityWorld.revenge)
                    {
                        CalamityWorld.revenge = true;
                        CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.RevengeText", Color.Crimson);
                    }
                    CalamityWorld.death = true;
                    CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.DeathText", Color.MediumPurple);
                }
                else
                {
                    if (CIWorld.Malice)
                    {
                        CIWorld.Malice = false;
                        CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.MaliceText2", Color.Crimson);
                    }
                    CalamityWorld.death = false;
                    CIFunction.BroadcastLocalizedText("Mods.CalamityInheritance.Status.DeathText2", Color.MediumPurple);
                }
                CalamityNetcode.SyncWorld();
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
