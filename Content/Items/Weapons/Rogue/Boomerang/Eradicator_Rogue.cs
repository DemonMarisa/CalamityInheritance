using CalamityInheritance.Content.Items.Weapons.Melee;
using CalamityInheritance.Content.Projectiles.Melee.Boomerang;
using CalamityInheritance.Content.Projectiles.Rogue.Boomerang;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang
{
    public class Eradicator_Rogue : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.BaseWeaponCategory}.Rogue";
        public static float Speed = 9.0f;
        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 58;
            Item.damage = 100;
            //降低飞盘伤害，提高星云射线的倍率（0.4→0.8），且极大程度地提高了星云射线的索敌范围与蛇毒，与发射频率
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.knockBack = 7f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = RarityType<DeepBlue>();
            Item.shoot = ProjectileType<EradicatorProj_Rogue>();
            Item.shootSpeed = Speed;
            Item.DamageType = GetInstance<RogueDamageClass>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (player.Calamity().StealthStrikeAvailable() && proj.WithinBounds(Main.maxProjectiles))
            {
                Main.projectile[proj].timeLeft += EradicatorProj_Rogue.StealthExtraLifetime;
                Main.projectile[proj].Calamity().stealthStrike = true;
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<CosmiliteBar>(12)
                .AddTile<CosmicAnvil>()
                .Register();
        }
    }
}
