using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Aerospec;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientAero
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientAeroHelm :CIArmor, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 20;
            Item.rare = ItemRarityID.Orange;
            Item.value = CIShopValue.RarityPriceOrange;
            Item.defense = 5;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AncientAeroArmor>() && legs.type == ModContent.ItemType<AncientAeroLeggings>();
        }
        public override void UpdateEquip(Player p)
        {
            p.moveSpeed += 0.1f;
            p.jumpSpeedBoost += 0.5f;
        }
        public override void UpdateArmorSet(Player player)
        {
            var usPlayer = player.CIMod();
            player.setBonus = this.GetLocalizedValue("SetBonus");
            usPlayer.AncientAeroSet = true; 
            //增加玩家最大飞行时间的180(3秒), 我没说错.
            player.wingTimeMax += 180;
            bool usingAeroStoneLegacy = player.CIMod().AeroStonePower;
            bool usingAeroStoneCalamity = player.Calamity().aeroStone;
            //如果玩家佩戴旧天蓝石, 额外追加3秒
            if (usingAeroStoneLegacy)
                player.wingTimeMax += 180;
            //如果玩家佩戴原灾天蓝石，则使其加成追加到6秒
            if (usingAeroStoneCalamity)
                player.wingTimeMax += 310;
            //如果玩家一起佩戴，再追加6秒飞行时间
            if (usingAeroStoneCalamity && usingAeroStoneLegacy)
            {
                player.wingTimeMax += 360;
            }

        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<AerialiteBar>(10).
                AddIngredient(ItemID.FallenStar, 5).
                AddIngredient<HarpyRing>().
                AddTile(TileID.SkyMill).
                Register();
        }
    }
}