using CalamityInheritance.Tiles.Vanity;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Rarity;
using CalamityInheritance.World;
using CalamityMod;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;
using CalamityMod.World;

namespace CalamityInheritance.Content.Items.Placeables.Vanity
{
    public class MLGRune : CIPlaceable, ILocalizedModType
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
            Item.createTile = ModContent.TileType<MLGRuneTiles>();
            Item.rare = ModContent.RarityType<PureRed>();
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool CanUseItem(Player player)
        {
            if (CalamityUtils.AnyBossNPCS())
                return false;
            if (player.altFunctionUse == 2)
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
            CalamityInheritancePlayer modPlayer = player.CIMod();
            if (player.altFunctionUse == 2)
            {
                if (!modPlayer.MLG)
                {
                    modPlayer.MLG = true;
                }
                else
                {
                    modPlayer.MLG = false;
                }
                Main.NewText($" MLG = {modPlayer.MLG}");
            }
            return true;
        }
    }
}
