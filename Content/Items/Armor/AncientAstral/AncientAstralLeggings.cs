using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientAstral
{
    [AutoloadEquip(EquipType.Legs)]
    public class AncientAstralLeggings: CIArmor, ILocalizedModType
    {
        private const int LifeMax = 20;
        private const int Crits = 5;
        private const float Regen = 0.5f;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(LifeMax,Crits.ToPercent(),Regen.ToTooltipHP());

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 14;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare = ItemRarityID.Red;
            Item.defense = 16;
        }

        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += LifeMax;
            player.GetCritChance<RogueDamageClass>() += Crits;
            player.lifeRegen += Regen.ToInnerLifeRegen();
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MeteoriteBar, 10).
                AddIngredient<LifeAlloy>(5).
                AddIngredient<StarblightSoot>(10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}