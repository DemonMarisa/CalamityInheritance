using CalamityInheritance.CIPlayer;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Systems;
using CalamityModMusic.Items.Placeables;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Tools
{
    public class MusicMarkChange : ModItem, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            Item.ResearchUnlockCount = 1;
        }
        public new string LocalizationCategory => "Content.Items.Tools";
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.damage = 1; //55->1 这玩意前期能变真近战武器而非调试工具了
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 42;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.LightRed;
        }
        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            CalamityInheritancePlayer modPlayer1 = player.CIMod();
            return base.CanUseItem(player);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
            AddIngredient<CalamityTitleMusicBox>().
            AddTile(TileID.WorkBenches).
            Register();
        }
    }
}
