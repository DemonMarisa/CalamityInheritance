using System.Security.Cryptography.X509Certificates;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity;
using CalamityInheritance.Sounds.Custom;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class EclipseSpear: RogueWeapon , ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
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
            Item.shootSpeed = 16f;
            Item.shoot = ModContent.ProjectileType<EclipseSpearProj>();

            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = ModContent.RarityType<DeepBlue>();
        }
        public override float StealthDamageMultiplier => 1.15f;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool ifStealth = player.CheckStealth();
            if (ifStealth)
            {
                SoundEngine.PlaySound(CISoundMenu.EclipseSpearAttackStealth, player.Center);
                type = ModContent.ProjectileType<EclipseSpearProjStealth>();
            }
            else
                SoundEngine.PlaySound(CISoundMenu.EclipseSpearAttackNor, player.Center);

            int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[p].Calamity().stealthStrike = ifStealth;

            if (!ifStealth)
                Projectile.NewProjectile(source, position, -velocity, ModContent.ProjectileType<EclipseSpearBack>(), damage, knockback, player.whoAmI);
            else
                Projectile.NewProjectile(source, position, -velocity * 1.5f, ModContent.ProjectileType<EclipseSpearBack>(), damage, knockback, player.whoAmI);
            return false;
        }
    }
}