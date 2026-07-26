using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Items.Weapons.Melee.Swords;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Core.Enums;
using LAP.Core.SystemsLoader;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.GreatSwords
{
    public class Animus : CIMelee
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.BonusAttackSpeedMultiplier[Type] = 1.50f;
        }

        public override void SetDefaults()
        {
            Item.width = 82;
            Item.height = 84;
            Item.scale = 2f;
            Item.damage = 400;
            Item.useTurn = true;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 11;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 11;
            Item.useTurn = true;
            Item.knockBack = 20f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.rare = RarityType<DonatorPink>();
            Item.value = CIShopValue.RarityPriceDonatorPink;

            Item.SetCalStatInflation(AllWeaponTier.DemonShadow);
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            int damageRan = Main.rand.Next(195); //0 to 194
            float damageMult = 1f;
            if (damageRan >= 50 && damageRan <= 99) //25%
            {
                damageMult = 1.5f;
            }
            else if (damageRan >= 100 && damageRan <= 139) //20%
            {
                damageMult = 2.25f;
            }
            else if (damageRan >= 140 && damageRan <= 169) //15%
            {
                damageMult = 3.75f;
            }
            else if (damageRan >= 170 && damageRan <= 189) //10%
            {
                damageMult = 7.5f;
            }
            else if (damageRan >= 190 && damageRan <= 194) //5%
            {
                damageMult = 12.5f;
            }
            else
            {
                damageMult = 1f;
            }
            damageMult -= 1f;
            damage += damageMult;
        }
        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient<BladeofEnmity>().
                    AddIngredient(CalamityMaterials.ShadowspecBar, 5).
                    AddTile(CalamityTile.DraedonsForgeTile).
                    Register();
            }
            CreateRecipe().
                AddIngredient<CalamitousEssence>().
                DisableDecraft().
                Register();
        }
    }
}
