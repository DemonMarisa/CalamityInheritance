using CalamityMod.Items.Weapons.Rogue;
using CalamityMod;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class PhantomLance : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.knockBack = 5f;

            Item.width = 62;
            Item.height = 68;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();

            Item.autoReuse = true;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<PhantomLanceProj>();
        }

        public override float StealthDamageMultiplier => 1.75f;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int s = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[s].Calamity().stealthStrike = player.CheckStealth(); 
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.SpectreBar, 12).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
