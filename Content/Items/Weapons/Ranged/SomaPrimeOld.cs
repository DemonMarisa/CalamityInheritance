using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Rarity;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class SomaPrimeOld : CIRanged, ILocalizedModType
    {
        
        private static readonly float XYInaccuracy = 0.32f;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<SomaPrime>(false);
        }
        public override void SetDefaults()
        {
            Item.width = 94;
            Item.height = 34;
            Item.damage = 255;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = Item.useAnimation = 2;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item40;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.BulletHighVelocity;
            Item.shootSpeed = 30f;
            Item.useAmmo = AmmoID.Bullet;

            Item.value = CIShopValue.RarityPriceDonatorPink;
            Item.rare = RarityType<DonatorPink>();
            Item.Calamity().devItem = true;
            
        }

        // Terraria seems to really dislike high crit values in SetDefaults
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 30;
        public override void UseItemFrame(Player player)
        {
            CIFunction.NoHeldProjUpdateAim(player, 0, 1);
        }
        public override Vector2? HoldoutOffset() => new Vector2(-25, -5);

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (CIConfig.Instance.AmmoConversion == true)
            {
                type = ProjectileID.BulletHighVelocity;
                damage += 4;
            }
            if (CIConfig.Instance.AmmoConversion == false)
            {
                if (type == ProjectileID.Bullet)
                {
                    type = ProjectileID.BulletHighVelocity;
                    damage += 4;
                }
            }
            velocity.X += Main.rand.NextFloat(-XYInaccuracy, XYInaccuracy);
            velocity.Y += Main.rand.NextFloat(-XYInaccuracy, XYInaccuracy);
            Vector2 vel = velocity;
            Projectile shot = Projectile.NewProjectileDirect(source, position + new Vector2(0 , -6), vel, type, damage, knockback, player.whoAmI);
            CalamityGlobalProjectile cgp = shot.Calamity();
            cgp.appliesSomaShred = true;
            return false;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player) => Main.rand.NextFloat() > 0.8f;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (CIConfig.Instance.AmmoConversion == true)
            {
                string AmmoConversionOn = Language.GetTextValue("Mods.CalamityInheritance.ConfigsMessage.AmmoConversionCIWeapon");

                tooltips.Add(new TooltipLine(Mod, "AmmoConversionCIWeapon", AmmoConversionOn));
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<Minigun>().
                AddIngredient<P90Legacy>().
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
