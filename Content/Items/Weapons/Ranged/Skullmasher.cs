using Terraria.Audio;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Sounds;
using CalamityMod;
using Terraria.ID;
using Terraria.DataStructures;
using CalamityMod.Buffs.StatDebuffs;
using CalamityInheritance.Utilities;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.LoreItems;
using CalamityMod.Items.LoreItems;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityInheritance.Rarity;
using System.Collections.Generic;
using Terraria.Localization;
using CalamityInheritance.System.Configs;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class Skullmasher : CIRanged, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Ranged";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.damage = 510;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 76;
            Item.crit += 5;
            Item.height = 30;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 12f;
            Item.useAmmo = 97;
            Item.rare = ModContent.RarityType<MaliceChallengeDrop>();
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override Vector2? HoldoutOffset()
        {
            if (CIRespriteConfig.Instance.SkullmasherResprite)
                return new Vector2(-15, 0);
            else
                return new Vector2(-40, 0);
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<MarkedforDeath>(), 300);

            target.Calamity().miscDefenseLoss = 25;
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            SoundEngine.PlaySound(CommonCalamitySounds.LargeWeaponFireSound);
            CalamityInheritancePlayer modPlayer = player.CIMod();

            for (int projectiles = 0; projectiles < 5; projectiles++)
            {
                if (CIConfig.Instance.AmmoConversion == true)
                {
                    float speedX = velocity.X + Main.rand.Next(-40, 41) * 0.01f;
                    float speedY = velocity.Y + Main.rand.Next(-40, 41) * 0.01f;
                    Projectile proj = Projectile.NewProjectileDirect(source, position, new Vector2(speedX, speedY), ModContent.ProjectileType<AMRShot>(), damage, knockback, player.whoAmI);
                }
                if (CIConfig.Instance.AmmoConversion == false)
                {
                    float speedX = velocity.X + Main.rand.Next(-40, 41) * 0.01f;
                    float speedY = velocity.Y + Main.rand.Next(-40, 41) * 0.01f;
                    Projectile proj = Projectile.NewProjectileDirect(source, position, new Vector2(speedX, speedY), type, damage, knockback, player.whoAmI);
                    if (type == ProjectileID.Bullet)
                    {
                        type = ModContent.ProjectileType<AMRShot>();
                    }
                    if (type != ModContent.ProjectileType<AMRShot>())
                    {
                        proj.CalamityInheritance().AMRextra = true;
                    }
                }
            }
            return false;
        }
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
            if (CIServerConfig.Instance.LegendaryitemsRecipes == true)
            {
                Recipe recipe1 = CreateRecipe();
                recipe1.AddIngredient(ModContent.ItemType<KnowledgeDevourerofGods>());
                recipe1.AddTile(ModContent.TileType<CosmicAnvil>());
                recipe1.Register();

                Recipe recipe2 = CreateRecipe();
                recipe2.AddIngredient(ModContent.ItemType<LoreDevourerofGods>());
                recipe2.AddTile(ModContent.TileType<CosmicAnvil>());
                recipe2.Register();
            }
        }
    }
}
