using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor.Astral;
using CalamityMod.Items.Fishing.AstralCatches;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientAstral
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientAstralBreastplate: ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Armor";
        public override void SetStaticDefaults()
        {
            if(CalamityConditions.DownedAstrumDeus.IsMet())
            {
                ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<AncientAstralBreastplate>()] = ModContent.ItemType<AstralBreastplate>();
            }
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
            player.statLifeMax2 += 30;
            player.GetDamage<RogueDamageClass>() += 0.05f;
            player.GetCritChance<RogueDamageClass>() += 5;
            player.moveSpeed -= 0.20f;
            player.lifeRegen += 1;
            player.buffImmune[ModContent.BuffType<AstralInfectionDebuff>()] = true;
            player.buffImmune[BuffID.Rabies] = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MeteoriteBar, 15).
                AddIngredient<LifeAlloy>(5).
                AddIngredient<StarblightSoot>(15).
                AddIngredient<UrsaSergeant>().
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}