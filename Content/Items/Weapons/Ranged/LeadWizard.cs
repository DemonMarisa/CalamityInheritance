using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityMod.Items.LoreItems;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class LeadWizard : CIRanged, ILocalizedModType 
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 78;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 66;
            Item.height = 34;
            Item.useTime = 3;
            Item.reuseDelay = 8;
            Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = RarityType<MaliceChallengeDrop>();
            Item.UseSound = SoundID.Item31;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.BulletHighVelocity;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.Bullet;
            
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 30;

        public override Vector2? HoldoutOffset() => new Vector2(-5, -3);

        public override void UseItemFrame(Player player)
        {
            CIFunction.NoHeldProjUpdateAim(player, 0, 1);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float rotation = MathHelper.ToRadians(6);
            for (int i = 0; i < 2; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedBy(i == 1 ? rotation : -rotation);
                Projectile.NewProjectile(source, position, perturbedSpeed, ProjectileID.BulletHighVelocity, damage, knockback, player.whoAmI, 0f, 0f);
            }
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            if (Main.rand.Next(0, 100) < 33)
                return false;
            return true;
        }
        public override void AddRecipes()
        {
            if (CIServerConfig.Instance.LegendaryitemsRecipes == true)
            {
                CreateRecipe()
                    .AddIngredient<KnowledgeGolem>()
                    .DisableDecraft()
                    .AddTile(TileID.MythrilAnvil)
                    .Register();
            }
        }
    }
}
