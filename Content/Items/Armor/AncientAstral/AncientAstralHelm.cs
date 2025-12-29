using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using Humanizer;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.AncientAstral
{
    [AutoloadEquip(EquipType.Head)]
    public class AncientAstralHelm: CIArmor, ILocalizedModType
    {
        private const int Crits = 5;
        private const int LifeMax = 20;
        private const float LifeSpeed = 0.5f;
        private const float StealthPoint = 1.15f;
        private const float DR = 0.12f;
        private const int LifeMaxSetBonus = 40;
        public const int CritsRegen = 25;
        public const int RogueCritsTimes = 25;
        public const float DefenseAndDR = 0.3f;
        public const float DefenseDamageReduction = 0.5f;
        public const int MaxStealthLifeRegenSpeed = 6;
        public const int LifeRegenSpeedResetCD = 15;
        public const float StealthRegen = 0.25f;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 22;
            Item.value = CIShopValue.RarityPriceRed;
            Item.rare =  ItemRarityID.Red;
            Item.defense = 22;
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Crits.ToPercent(), LifeMax, LifeSpeed);
        public override void UpdateEquip(Player player)
        {
            player.statLifeMax2 += LifeMax;
            player.GetCritChance<RogueDamageClass>() += Crits;
            player.lifeRegen += (int)(LifeSpeed * 2);
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<AncientAstralBreastplate>() && legs.type == ItemType<AncientAstralLeggings>();

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer calPlayer = player.Calamity();
            var usPlayer = player.CIMod(); 
            player.pStone = true;
            calPlayer.wearingRogueArmor = true;

            player.lifeRegen += LifeSpeed.ToInnerLifeRegen();
            player.statLifeMax2 += LifeMaxSetBonus;
            calPlayer.rogueStealthMax += StealthPoint;
            player.endurance += DR;

            calPlayer.stealthStrikeHalfCost = true;
            usPlayer.AncientAstralSet = true;

            //灾厄你全家死完了吧为什么潜伏值要用的1.15f
            player.setBonus = this.GetLocalizedValue("SetBonus").FormatWith(
                StealthPoint.ToStealthInt(),
                LifeSpeed,
                LifeMaxSetBonus,
                DR.ToPercent(),
                CritsRegen,
                RogueCritsTimes,
                DefenseAndDR.ToPercent(),
                DefenseDamageReduction.ToPercent(),
                MaxStealthLifeRegenSpeed,
                LifeRegenSpeedResetCD,
                StealthRegen.ToPercent());

            player.Calamity().WearingPostMLSummonerSet = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.MeteoriteBar, 10).
                AddIngredient<LifeAlloy>(5).
                AddIngredient<StarblightSoot>(10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }

    }
}