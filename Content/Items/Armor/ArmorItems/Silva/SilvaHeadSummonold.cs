using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Items;
using CalamityInheritance.Content.Buff.SummonBuff.Armor;
using CalamityInheritance.Content.CDs;
using CalamityInheritance.Content.Items.Accessories.Misc;
using CalamityInheritance.Content.Projectiles.Armor.Summon.Header;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.SystemsLoader;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Armor.ArmorItems.Silva
{
    [AutoloadEquip(EquipType.Head)]
    public class SilvaHeadSummonold : CIArmor
    {

        public static readonly SoundStyle ActivationSound = new("CalamityMod/Sounds/Custom/AbilitySounds/SilvaActivation");
        public static readonly SoundStyle DispelSound = new("CalamityMod/Sounds/Custom/AbilitySounds/SilvaDispel");
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = RarityType<DeepBlue>();
            Item.defense = 13; //110
        }
        public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ItemType<SilvaArmorold>() && legs.type == ItemType<SilvaLeggingsold>();
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.CI().SilvaSet = true;
            player.CI().SilvaReborn = true;
            player.CI().SilvaSummon = true;
            player.setBonus = this.GetLocalizedValue("SetBonus");
            if (player.whoAmI == Main.myPlayer)
            {
                var source = player.GetSource_ItemUse(Item);
                if (player.FindBuffIndex(BuffType<SilvaCrystalBuff>()) == -1)
                {
                    player.AddBuff(BuffType<SilvaCrystalBuff>(), 2, true);
                }
                if (player.ownedProjectileCounts[ProjectileType<SilvaCrystal>()] < 1)
                {
                    var damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(10000);

                    var p = Projectile.NewProjectile(source, player.Center.X, player.Center.Y, 0f, -1f, ProjectileType<SilvaCrystal>(), damage, 0f, Main.myPlayer, -20f, 0f);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 10000;
                }
            }
            player.GetDamage<SummonDamageClass>() += 0.75f;
            player.GetAttackSpeed<SummonMeleeSpeedDamageClass>() += 0.15f;
            if (player.HasCD<SilvaReviveCD>())
                player.GetDamage<ThrowingDamageClass>() *= 1.10f;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 5;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient(CalamityMaterials.PlantyMush, 6).
                    AddIngredient(CalamityMaterials.EffulgentFeather, 5).
                    AddIngredient(CalamityMaterials.DarksunFragment, 10).
                    AddIngredient<LeadCore>().
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {

            }
        }
    }

}
