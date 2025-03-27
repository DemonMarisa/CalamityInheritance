using System.Security.Cryptography.X509Certificates;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity;
using CalamityMod;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class EclipseSpear: RogueWeapon , ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Rogue";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 82;
            Item.height = 88;
            Item.damage = 500;
            Item.knockBack = 3.5f;
            Item.useAnimation = Item.useTime = 22;
            Item.autoReuse = true;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            //23f，不然这也太没手感了
            Item.shootSpeed = 23f;
            Item.shoot = ModContent.ProjectileType<EclipseSpearProj>();

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = ModContent.RarityType<DeepBlue>();
        }
        public override float StealthDamageMultiplier => 1.15f;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool ifStealth = player.Calamity().StealthStrikeAvailable();
            if (ifStealth)
                type = ModContent.ProjectileType<EclipseSpearProjStealth>();

            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[p].Calamity().stealthStrike = ifStealth;
            return false;
        }
    }
}