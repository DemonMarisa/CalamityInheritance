using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Armor.ReaverLegacy
{
    [AutoloadEquip(EquipType.Head)]
    public class ReaverCap : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item./*  */height = 22;
            Item.value = CIShopValue.RarityPriceLime; 
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
            player.setBonus = this.GetLocalizedValue("SetBonus");
            CalamityPlayer modPlayer = player.Calamity();
            var modPlayer1 = player.CalamityInheritance();
            modPlayer.rogueStealthMax += 1.15f;
            modPlayer1.reaverRogueExProj = true;
            player.Calamity().wearingRogueArmor = true;
            //25盗贼暴击，25盗贼伤害,115潜伏值
            //完全不是了，15伤5爆，5速，20移动速度，攻击滞留生命裂片
        }

        public override void UpdateEquip(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            player.ignoreWater = true;
            player.GetDamage<RogueDamageClass>() += 0.15f;
            player.GetCritChance<RogueDamageClass>() += 0.5f;
            player.moveSpeed += 0.2f;
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
