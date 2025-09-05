using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class CursedDaggerLegacy : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Rogue";
        //会被用于射弹内
        public const float ShootSpeed = 12f;
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.damage = 34;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 16;
            Item.knockBack = 4.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 34;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<CursedDaggerProjLegacy>();
            Item.shootSpeed = ShootSpeed;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo src, Vector2 pos, Vector2 vel, int type, int dmg, float kb)
        {
            bool stealth = player.CheckStealth();
            if (stealth)
                vel *= 2.5f;
            int p = Projectile.NewProjectile(src, pos, vel, type, dmg, kb, player.whoAmI);
            Projectile realProj = Main.projectile[p];
            if (stealth)
            {
                realProj.Calamity().stealthStrike = true;
                realProj.penetrate = -1;
            }
            return false;
        }
    }
}