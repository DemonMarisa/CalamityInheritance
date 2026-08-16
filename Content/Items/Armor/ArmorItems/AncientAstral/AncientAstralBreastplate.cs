using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.DamageBuffs;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.AncientAstral
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientAstralBreastplate : CIArmor
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 22;
            Item.defense = 30;
            Item.rare = ItemRarityID.Red;
            Item.value = CIShopValue.RarityPriceRed;
        }

        public override void UpdateEquip(Player player)
        {
            player.LAP().MaxLifeAdditive += 30;
            player.GetDamage<ThrowingDamageClass>() += 0.05f;
            player.GetCritChance<ThrowingDamageClass>() += 5;
            player.moveSpeed -= 0.20f;
            player.lifeRegen += 1;
            if (CIUtils.HasCalamity())
                player.buffImmune[CalDeBuff.AstralInfectionDebuff] = true;       
            player.buffImmune[BuffType<CIAstralInfection>()] = true;
            player.buffImmune[BuffID.Rabies] = true;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(ItemID.MeteoriteBar, 15).
                    AddIngredient(CalamityMaterials.LifeAlloy, 5).
                    AddIngredient(CalamityMaterials.StarblightSoot, 15).
                    AddTile(TileID.MythrilAnvil).
                    Register();
            }
            else
            {

            }
        }
    }
}