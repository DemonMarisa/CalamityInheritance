using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Misc;
using CalamityInheritance.Content.Projectiles.HeldProj.Melee.Swords;
using CalamityInheritance.Content.Projectiles.Melee.Swords;
using CalamityInheritance.Content.Rarity.ShopValue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.Swords
{
    public class TerraEdge : CIMelee
    {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.BonusAttackSpeedMultiplier[Item.type] = 1.20f;
        }
        public override void SetDefaults()
        {
            Item.width = 58;
            Item.damage = 120;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 14;
            Item.knockBack = 6.25f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.height = 58;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ProjectileType<TerraEdgeEnergySword>();
            Item.shootSpeed = 3f;
            Item.noMelee = true; // This is set the sword itself doesn't deal damage (only the projectile does).
            Item.shootsEveryUse = true; // This makes sure Player.ItemAnimationJustStarted is set when swinging.
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float adjustedItemScale = player.GetAdjustedItemScale(Item);

            Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);

            if (velocity == Vector2.Zero)
            {
                velocity = new Vector2(player.direction, 0f);
            }

            Vector2 beamVelocity = Vector2.Normalize(velocity) * 16f;
            Projectile.NewProjectile(source, position, beamVelocity, ProjectileType<TerraEdgeBeam>(), damage, knockback, player.whoAmI);

            NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);

            return false; // Return false because we've manually created the projectiles.
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.TerraBlade);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<TrueBloodyEdge>().
                AddIngredient(ItemID.TrueExcalibur).
                AddIngredient(ItemID.BrokenHeroSword).
                AddTile(TileID.MythrilAnvil).
                Register();

            CreateRecipe().
                AddIngredient(ItemID.TrueNightsEdge).
                AddIngredient(ItemID.TrueExcalibur).
                AddIngredient(ItemID.BrokenHeroSword).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
