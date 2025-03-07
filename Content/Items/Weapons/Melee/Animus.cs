using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Utilities;
using CalamityInheritance.Rarity;
using CalamityInheritance.Content.Items.Materials;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class Animus : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 82;
            Item.height = 84;
            Item.scale = 1.5f;
            Item.damage = 2000;
            Item.useTurn = true;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.useAnimation = 11;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 11;
            Item.knockBack = 20f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.rare = ModContent.RarityType<DonatorPink>();
            Item.value = CIShopValue.RarityPriceDonatorPink;
        }

        

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            float damageMult = player.CalamityInheritance().AnimusDamage;
            damageMult -= 1f;
            damage += damageMult;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
            int damageRan = Main.rand.Next(195); //0 to 194
            if (damageRan >= 50 && damageRan <= 99) //25%
            {
                player.CalamityInheritance().AnimusDamage = 1.5f;
            }
            else if (damageRan >= 100 && damageRan <= 139) //20%
            {
                player.CalamityInheritance().AnimusDamage = 2.25f;
            }
            else if (damageRan >= 140 && damageRan <= 169) //15%
            {
                player.CalamityInheritance().AnimusDamage = 3.75f;
            }
            else if (damageRan >= 170 && damageRan <= 189) //10%
            {
                player.CalamityInheritance().AnimusDamage = 7.5f;
            }
            else if (damageRan >= 190 && damageRan <= 194) //5%
            {
                player.CalamityInheritance().AnimusDamage = 12.5f;
            }
            else
            {
                player.CalamityInheritance().AnimusDamage = 1f;
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
            int damageRan = Main.rand.Next(195); //0 to 194
            if (damageRan >= 50 && damageRan <= 99) //25%
            {
                player.CalamityInheritance().AnimusDamage = 1.5f;
            }
            else if (damageRan >= 100 && damageRan <= 139) //20%
            {
                player.CalamityInheritance().AnimusDamage = 2.25f;
            }
            else if (damageRan >= 140 && damageRan <= 169) //15%
            {
                player.CalamityInheritance().AnimusDamage = 3.75f;
            }
            else if (damageRan >= 170 && damageRan <= 189) //10%
            {
                player.CalamityInheritance().AnimusDamage = 7.5f;
            }
            else if (damageRan >= 190 && damageRan <= 194) //5%
            {
                player.CalamityInheritance().AnimusDamage = 12.5f;
            }
            else
            {
                player.CalamityInheritance().AnimusDamage = 1f;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<BladeofEnmity>().
                AddIngredient<ShadowspecBar>(5).
                AddTile<DraedonsForge>().
                Register();

            CreateRecipe().
                AddIngredient<CalamitousEssence>().
                DisableDecraft().
                Register();
        }
    }
}
