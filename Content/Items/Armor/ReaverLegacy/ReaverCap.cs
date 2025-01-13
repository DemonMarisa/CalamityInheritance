using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Materials;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items;
using CalamityMod.Items.Accessories;
using CalamityInheritance.Utilities;


namespace CalamityInheritance.Content.Items.Armor.ReaverLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class ReaverCap : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Reaver Headgear");
            /* Tooltip.SetDefault("15% increased rogue damage, 5% increased rogue velocity and critical strike chance\n" +
                "20% increased movement speed and can move freely through liquids"); */
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.defense = 10; //43
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
            player.setBonus = this.GetLocalizedValue("SetBonus");
            modPlayer.rogueStealthMax += 1.15f;
            modPlayer1.reaverRogueExProj = true;
            player.moveSpeed += 0.2f;
            player.GetCritChance<RogueDamageClass>() += 0.10f;
            player.Calamity().wearingRogueArmor = true;
            //25盗贼暴击，25盗贼伤害,115潜伏值
        }

        public override void UpdateEquip(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            player.ignoreWater = true;
            player.GetDamage<RogueDamageClass>() += 0.10f;
            modPlayer.rogueVelocity += 0.05f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<PerennialBar>(),8)
            .AddIngredient(ItemID.JungleSpores, 8)
            .AddIngredient(ModContent.ItemType<EssenceofEleum>(), 2)
            .AddTile(TileID.MythrilAnvil)
            .Register();
        }
    }
}
