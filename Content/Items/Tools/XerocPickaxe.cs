using CalamityInheritance.Content.Items.Materials;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using MonoMod.ModInterop;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Tools
{
    public class XerocPickaxe: RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Tools";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.height = Item.width = 54;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.pick = 225;
            Item.useTime = 5;
            Item.useAnimation = 10;
            Item.tileBoost += 6;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.damage = 300;
            Item.knockBack = 5f;
            Item.rare = ItemRarityID.Red;
            Item.value = CIShopValue.RarityPriceRed;
            Item.autoReuse = true;
        }
        public override float StealthDamageMultiplier => 50f;
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<NebulaBar>(9).
                AddIngredient<GalacticaSingularity>(1).
                AddIngredient(ItemID.LunarBar, 10).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}