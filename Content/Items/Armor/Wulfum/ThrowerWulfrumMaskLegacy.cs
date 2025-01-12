using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.Wulfum
{
    [AutoloadEquip(EquipType.Head)]
    public class ThrowerWulfrumMaskLegacy : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wulfrum Mask");
            // Tooltip.SetDefault("3% increased rogue damage");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 20000;
            Item.rare = 1;
            Item.defense = 1; //6
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<WulfrumArmorLegacy>() && legs.type == ModContent.ItemType<WulfrumLeggingsLegacy>();
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer calp = player.Calamity();
            player.GetDamage<RogueDamageClass>() += 0.03f; //3%盗贼伤害
            calp.rogueStealthMax += 0.5f; //50潜伏值
            player.setBonus = "+3 defense\n" +
                "+5 defense when below 50% life";
            player.statDefense += 3; //9
            if (player.statLife <= (player.statLifeMax2 * 0.5f))
            {
                player.statDefense += 5; //14
            }
            player.pickSpeed -= 0.10f;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 10;
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