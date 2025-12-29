using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;


namespace CalamityInheritance.Content.Items.Armor.Wulfum.NewTexture
{
    [AutoloadEquip(EquipType.Head)]
    public class ANewWulfrumHeadgear : CIArmor, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Wulfrum";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 2; //7
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool wulLegacy = body.type == ItemType<WulfrumArmorLegacy>() && legs.type == ItemType<WulfrumLeggingsLegacy>();
            bool wulNew = body.type == ItemType<ANewWulfrumArmor>() && legs.type == ItemType<ANewWulfrumLeggings>();
            bool wullegacynew = body.type == ItemType<WulfrumArmorLegacy>() && legs.type == ItemType<ANewWulfrumLeggings>();
            bool wulnewlegacy = body.type == ItemType<ANewWulfrumArmor>() && legs.type == ItemType<WulfrumLeggingsLegacy>();
            return wulLegacy || wulNew || wullegacynew || wulnewlegacy;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");
            player.statDefense += 3; //10
            if (player.statLife <= (player.statLifeMax2 * 0.5f))
            {
                player.statDefense += 5; //15
            }
            player.pickSpeed -= 0.10f;
            player.moveSpeed += 0.10f;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.03f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<WulfrumMetalScrap>(8)
            .AddIngredient<EnergyCore>(1)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}