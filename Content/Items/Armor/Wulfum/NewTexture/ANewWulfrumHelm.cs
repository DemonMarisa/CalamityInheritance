using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Items.Armor.Wulfum.NewTexture
{
    [AutoloadEquip(EquipType.Head)]
    public class ANewWulfrumHelm : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Wulfrum";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CIShopValue.RarityPriceBlue;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 3; //8
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<WulfrumArmorLegacy>() && legs.type == ModContent.ItemType<WulfrumLeggingsLegacy>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");
            player.statDefense += 3; //11
            if (player.statLife <= (int)((double)player.statLifeMax2 * 0.5))
            {
                player.statDefense += 5; //16
            }
            player.pickSpeed -= 0.10f;
            player.moveSpeed += 0.10f;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.03f; //伤害看看就好，蚊子腿
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