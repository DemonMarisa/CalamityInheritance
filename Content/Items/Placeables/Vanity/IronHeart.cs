using CalamityInheritance.Tiles.Vanity;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.World;
using CalamityInheritance.Utilities;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Items.Placeables.Vanity
{
    public class IronHeart : CIPlaceable, ILocalizedModType
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
            Item.createTile = ModContent.TileType<IronHeartTiles>();
            Item.rare = ItemRarityID.Blue;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
        {
            if (CalamityUtils.AnyBossNPCS())
                return false;
            if (player.altFunctionUse == 2)
            {
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
            // This world syncing code should only be run by one entity- the server, to prevent a race condition
            // with the packets.
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return true;
            if (player.altFunctionUse != 2)
            {
                CIWorld world = ModContent.GetInstance<CIWorld>();
                world.UpdateIronHeart();
                return true;
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
