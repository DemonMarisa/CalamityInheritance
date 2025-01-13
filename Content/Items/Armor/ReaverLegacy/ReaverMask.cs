using CalamityInheritance;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ReaverLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class ReaverMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Reaver Mask");
            /* Tooltip.SetDefault("15% increased magic damage, 12% reduced mana cost, and 5% increased magic critical strike chance\n" +
                "10% increased movement speed, can move freely through liquids, and +80 max mana"); */
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 22;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 7; //40
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ReaverScaleMail>() && legs.type == ModContent.ItemType<ReaverCuisses>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            var modPlayer1 = player.CalamityInheritance();
            modPlayer1.reaverMageBurst = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            player.GetDamage<MagicDamageClass>() += 0.10f; //35魔法伤害，30+10(buff)=40暴击率,20魔力上限加成
            player.GetCritChance<MagicDamageClass>() += 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.ignoreWater = true;
            player.GetDamage<MagicDamageClass>() += 0.10f;
            player.GetCritChance<MagicDamageClass>() += 5;
            player.manaCost *= 0.88f;
            player.moveSpeed += 0.1f;
            player.statManaMax2 += 80;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<PerennialBar>(),10)
            .AddIngredient(ItemID.JungleSpores, 8)
            .AddIngredient(ModContent.ItemType<EssenceofEleum>(), 2)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
