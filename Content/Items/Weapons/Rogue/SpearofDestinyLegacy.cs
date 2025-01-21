using Terraria.DataStructures;
using CalamityMod.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod;
using CalamityInheritance.Rarity;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class SpearofDestinyLegacy :RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Rogue";
        public override void SetDefaults()
        {
            //Scarlet:将命运投矛的数值回滚至1457前，并修改稀有度为恶意专属掉落的稀有度颜色。
            //这一做法仅仅是为了作为对恶意模式的一个记忆点。
            Item.width = 52;
            Item.damage = 32;
            // Item.damage = 26;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 52;
            Item.rare = ModContent.RarityType<MaliceChallengeDrop>();   
            Item.shoot = ModContent.ProjectileType<SpearofDestinyProjectileLegacy>();
            Item.shootSpeed = 20f;
            Item.value = CIShopValue.RarityMaliceDrop;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int index = 7;
            for (int i = -index; i <= index; i += index)
            {
                int projType = (i != 0 || player.Calamity().StealthStrikeAvailable()) ? type : ModContent.ProjectileType<IchorSpearProj>();
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(i));
                int spear = Projectile.NewProjectile(source, position, perturbedSpeed, projType, damage, knockback, player.whoAmI);
                if (spear.WithinBounds(Main.maxProjectiles))
                    Main.projectile[spear].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            }
            return false;
        }
        //2025.1.13 DemonMarisa 改为灵液怪掉落
        /*
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SpearofDestiny>(). 
                Register();
        }
        */
    }
}
