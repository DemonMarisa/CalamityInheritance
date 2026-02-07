using CalamityInheritance.Content.Projectiles.DraedonsArsenal;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using LAP.Content.RecipeGroupAdd;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.DraedonsArsenal
{
    public class FrequencyManipulator : CIRogueClass, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.DraedonsArsenal";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 44;
            Item.damage = 80;
            Item.DamageType = RogueDamageClass.Instance;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTime = 56;
            Item.useAnimation = 56;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5f;

            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;

            Item.shootSpeed = 16f;
            Item.shoot = ProjectileType<FrequencyManipulatorProjectile>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (proj.WithinBounds(Main.maxProjectiles))
                Main.projectile[proj].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<MysteriousCircuitry>(8).
                AddIngredient<DubiousPlating>(12).
                AddRecipeGroup(LAPRecipeGroup.AnyMythrilBar, 10).
                AddIngredient(ItemID.SoulofSight, 20).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
