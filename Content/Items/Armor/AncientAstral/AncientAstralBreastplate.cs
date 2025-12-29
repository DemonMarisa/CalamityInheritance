using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Fishing.AstralCatches;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientAstral
{
    [AutoloadEquip(EquipType.Body)]
    public class AncientAstralBreastplate: CIArmor, ILocalizedModType
    {
        private const int LifeMax = 30;
        private const float Damage = 0.05f;
        private const int Crits = 5;
        private const float MoveSpeed = 0.20f;
        private const float RegenSpeed = 0.5f;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Damage.ToPercent(), LifeMax, RegenSpeed, MoveSpeed.ToPercent());
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
            player.statLifeMax2 += LifeMax;
            player.GetDamage<RogueDamageClass>() += Damage;
            player.GetCritChance<RogueDamageClass>() += Crits;
            player.moveSpeed -= MoveSpeed;
            player.lifeRegen += RegenSpeed.ToInnerLifeRegen();
            player.buffImmune[BuffType<AstralInfectionDebuff>()] = true;
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