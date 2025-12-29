using CalamityInheritance.Content.Items.Weapons.Melee.Boomerang;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue.Boomerang
{
    public class RoguePwnagehammer: RogueWeapon, ILocalizedModType
    {
        public override string Texture => GetInstance<MeleePwnagehammer>().Texture;
        public new string LocalizationCategory =>$"{Generic.BaseWeaponCategory}.Rogue";
        public static readonly float Speed = 12f;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 68;
            Item.height = 68;
            Item.damage = 70;
            Item.DamageType = GetInstance<RogueDamageClass>();
            Item.knockBack = 10f;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.shoot = ProjectileType<RoguePwnagehammerProj>();
            Item.shootSpeed = 12f;
            Item.value = CIShopValue.RarityPriceLightRed;
            Item.rare = ItemRarityID.LightRed;
            Item.autoReuse = true;
        }
        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 24  ;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool isStealth = player.CheckStealth();
            if (!isStealth)
                return true;
                
            int stealth = Projectile.NewProjectile(source, position, velocity * 1.2f ,type, damage, knockback, player.whoAmI, 0f, 0f, 0f);
            if(stealth.WithinBounds(Main.maxProjectiles))
                Main.projectile[stealth].Calamity().stealthStrike = true;
            return false;
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