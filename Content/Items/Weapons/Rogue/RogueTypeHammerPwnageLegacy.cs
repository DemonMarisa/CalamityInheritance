using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class RogueTypeHammerPwnageLegacy: RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory =>"Content.Items.Weapons.Rogue";
        public static readonly float Speed = 12f;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 68;
            Item.height = 68;
            Item.damage = 90;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.knockBack = 10f;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<RogueTypeHammerPwnageLegacyProj>();
            Item.shootSpeed = 12f;
            Item.value = CIShopValue.RarityPriceYellow;
            Item.rare = ItemRarityID.Yellow;
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if(player.Calamity().StealthStrikeAvailable())//如果允许潜伏攻击
            {
                int stealth = Projectile.NewProjectile(source, position, velocity * 1.2f ,type, (int)(damage*1.25f), knockback, player.whoAmI, 0f, 0f, 0f);
                if(stealth.WithinBounds(Main.maxProjectiles))
                    Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Pwnhammer).
                //移除神圣锭的需求，将神圣锭修改为5个飞行魂
                AddIngredient(ItemID.SoulofFlight, 5).
                AddIngredient(ItemID.SoulofFright, 5).
                AddIngredient(ItemID.SoulofMight, 5).
                AddIngredient(ItemID.SoulofSight, 5).
                //修改为铁砧教主
                AddTile(TileID.Anvils).
                Register();
        }
    }
}