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
    public class OricGemstone : RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 32;
            Item.damage = 28;
            Item.noMelee = true;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.noUseGraphic = true;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = CIShopValue.RarityPricePink;
            Item.shoot = ModContent.ProjectileType<OricGemstoneProj>();
            Item.shootSpeed = 16f;
        }
        public override bool AltFunctionUse(Player player) => true;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            damage /= 1 + player.CheckStealth().ToInt();
            int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Projectile proj = Main.projectile[stealth];
            if (player.altFunctionUse is 2 && !player.CheckStealth())
                proj.CalamityInheritance().MouseRight = true;
                
            if (player.CheckStealth())
            {
                proj.Calamity().stealthStrike = true;
                proj.timeLeft = 480;
                proj.penetrate = -1;
            }

            return false;
        }
    }
}
