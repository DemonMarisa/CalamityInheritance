using CalamityInheritance.Common.Blance;
using CalamityInheritance.Common.CalamityModCross;
using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.CDs;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.Content.Projectiles.HeldProj.Melee.GreatSwords.AOTCNew;
using CalamityInheritance.Content.Projectiles.Melee.ArkWeapons;
using CalamityInheritance.Content.Rarity;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Content.Tiles.CraftingStations;
using CalamityInheritance.Core.Utils;

using LAP.Core.SystemsLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee.ArkWeapons
{
    public class ArkoftheCosmosNew : CIMelee
    {
        public int Charge;
        public int Combo;
        public override void SetDefaults()
        {
            Item.width = Item.height = 136;
            Item.damage = 2100;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 9.5f;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 28f;
            Item.rare = RarityType<CatalystViolet>();
        }
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 15;
        public override bool AltFunctionUse(Player player) => true;
        public override void HoldItem(Player player)
        {
            if (!player.HasCD<AOTCCharge>())
                player.AddCD(LAPContent.CDType<AOTCCharge>(), CIWeaponsBlance.MaxAOTCCharge);
        }
        public override bool CanUseItem(Player player)
        {
            return !Main.projectile.Any(n => n.active && n.owner == player.whoAmI && n.type == ProjectileType<AOTCNewLeft>());
        }
        public override float UseSpeedMultiplier(Player player) => player.altFunctionUse == 2 ? 5f : 1f;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                if (player.controlUp && player.CI().CurAOTCCharge > 0)
                {
                    if (player.CI().CurAOTCCharge > CIWeaponsBlance.MaxAOTCCharge)
                        player.CI().CurAOTCCharge = CIWeaponsBlance.MaxAOTCCharge;
                    int chargeCount = 0;
                    while (player.CI().CurAOTCCharge > 0)
                    {
                        player.CI().CurAOTCCharge--;
                        chargeCount++;
                    }
                    Projectile.NewProjectile(source, player.Center, velocity, ProjectileType<AOTCNewBlast>(), (int)(damage * chargeCount * 0.6f), knockback, player.whoAmI);
                }
                else if (!player.HasCD<AOTCParryCD>())
                {
                    player.AddCD(LAPContent.CDType<AOTCParryCD>(), 60 * 5);
                    Projectile.NewProjectile(source, player.Center, velocity, ProjectileType<AOTCNewParry>(), damage, knockback, player.whoAmI);
                }
            }
            else
            {
                float chargeattack = 0f;
                float damagemult = 1f;
                if (player.CI().CurAOTCCharge > 0)
                {
                    chargeattack = 1f;
                    damagemult = 1.2f;
                    player.CI().CurAOTCCharge--;
                }
                if (Combo == 0 || Combo == 2)
                    Projectile.NewProjectile(source, player.Center, velocity, ProjectileType<AOTCNewLeft>(), (int)(damage * damagemult), knockback, player.whoAmI, chargeattack);
                else if (Combo == 1 || Combo == 3)
                    Projectile.NewProjectile(source, player.Center, velocity, ProjectileType<AOTCNewLeft_2>(), (int)(damage * damagemult), knockback, player.whoAmI, chargeattack);
                else if (Combo == 4)
                    Projectile.NewProjectile(source, player.Center, Vector2.Zero, ProjectileType<AOTCNewThrow>(), (int)(damage * damagemult), knockback, player.whoAmI, chargeattack);
                else
                    Projectile.NewProjectile(source, player.Center, velocity, ProjectileType<AOTCNewLeft>(), (int)(damage * damagemult), knockback, player.whoAmI, chargeattack);
                if (Combo != 4)
                {
                    Projectile.NewProjectile(source, player.Center, velocity * 1.5f, ProjectileType<RendingNeedleNew>(), (int)(damage * damagemult), knockback, player.whoAmI, chargeattack);
                }
                Combo += 1;
                if (Combo > 4)
                    Combo = 0;
            }
            return false;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            return true;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (Charge <= 0)
                return;
        }

        public override void AddRecipes()
        {
            if (CIUtils.HasCalamity())
            {
                CreateRecipe().
                    AddIngredient<FourSeasonsGalaxiaold>().
                    AddIngredient<ArkoftheElementsold>().
                    AddIngredient<AuricBarold>().
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();

                CreateRecipe().
                    AddIngredient<FourSeasonsGalaxiaold>().
                    AddIngredient<ArkoftheElementsold>().
                    AddIngredient(CalamityMaterials.AuricBar, 5).
                    AddTile(CalamityTile.CosmicAnvilTile).
                    Register();
            }
            else
            {
                CreateRecipe().
                    AddIngredient<FourSeasonsGalaxiaold>().
                    AddIngredient<ArkoftheElementsold>().
                    AddIngredient<AuricBarold>().
                    AddTile<DraedonsForgeoldTile>().
                    Register();
            }
        }
    }
}
