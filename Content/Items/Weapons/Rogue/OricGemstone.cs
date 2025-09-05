using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity;
using CalamityInheritance.Rarity.Special;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Build.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    public class OricGemstone : RogueWeapon, ILocalizedModType
    {
        public new string LocalizedCategory => $"{Generic.WeaponLocal}.Rogue";
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
            Item.shoot = ModContent.ProjectileType<OriGemstoneProj>();
            Item.shootSpeed = 28f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var modPlayer = player.CIMod();
            int stealth = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[stealth].Calamity().stealthStrike = player.CheckStealth();
        }
    }
}
