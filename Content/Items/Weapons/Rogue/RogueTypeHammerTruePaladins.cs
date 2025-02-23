using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityMod;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod.Items.Weapons.Rogue;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    [LegacyName("RogueTypeFallenPaladinsHammer")]
    public class RogueTypeHammerTruePaladins: RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Rogue";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.damage = 87;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 13;
            Item.knockBack = 20f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.height = 28;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = ModContent.ProjectileType<RogueTypeHammerTruePaladinsProj>();
            Item.shootSpeed = 14f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.Calamity().StealthStrikeAvailable())//如果允许潜伏攻击
            {
                int stealth = Projectile.NewProjectile(source, position, velocity*1.6f ,type, (int)(damage*1.14f), knockback, player.whoAmI, 0f, 0f, -3f);
                int stealth2 = Projectile.NewProjectile(source, position, velocity*2.8f ,type, (int)(damage*1.14f), knockback, player.whoAmI, 0f, 0f, -3f);
                
                if(stealth.WithinBounds(Main.maxProjectiles))
                {
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                    Main.projectile[stealth2].Calamity().stealthStrike = true;
                }
                return false;
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.PaladinsHammer).
                AddIngredient<RogueTypeHammerPwnageLegacy>().
                AddIngredient<ScoriaBar>(5).
                AddIngredient<AshesofCalamity>(5).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
