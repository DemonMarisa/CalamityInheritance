using CalamityMod;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Materials;
using CalamityInheritance.Content.Projectiles.Wulfrum;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Melee;

namespace CalamityInheritance.Content.Items.Weapons.Wulfrum
{
    public class WulfrumKnifeOld : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<WulfrumKnife>(false);
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.damage = 11;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 15;
            Item.knockBack = 1f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.autoReuse = true;
            Item.height = 38;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(0, 0, 0, 5);
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<WulfrumKnifeProjOld>();
            Item.shootSpeed = 12f;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int p = Projectile.NewProjectile(source, position, velocity * 1.3f, ModContent.ProjectileType<WulfrumKnifeProjOld>(), damage, knockback, player.whoAmI);
                Projectile proj = Main.projectile[p];
                if (p.WithinBounds(Main.maxProjectiles))
                {
                    proj.Calamity().stealthStrike = true;
                    proj.penetrate = 4;
                    proj.usesLocalNPCImmunity = true;
                    proj.localNPCHitCooldown = 1;
                }
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).
                AddIngredient<WulfrumMetalScrap>().
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
