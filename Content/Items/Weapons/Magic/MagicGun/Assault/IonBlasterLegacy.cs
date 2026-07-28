using CalamityInheritance.Content.BaseClass.Weapons;
using CalamityInheritance.Content.Projectiles.Magic.MagicGun.Assault;
using CalamityInheritance.Content.Rarity.ShopValue;
using CalamityInheritance.Core.Utils;
using LAP.Content.RecipeGroupAdd;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Magic.MagicGun.Assault
{
    public class IonBlasterLegacy : CIMagic
    {
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 28;
            Item.damage = 30;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 10;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 5.5f;
            Item.UseSound = SoundID.Item91;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.shoot = ProjectileType<IonBlastLegacy>();
            Item.shootSpeed = 3f;

            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
        }
        public override void UseItemFrame(Player player)
        {
            CIUtils.NoHeldProjUpdateAim(player, 0, 1);
        }
        public override Vector2? HoldoutOffset() => new Vector2(-5, 0);

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            float manaAmount = player.statMana * 0.01f;
            float damageMult = manaAmount * 0.75f;
            float injectionNerf = player.CalPlayerInfo().astralInjection ? 0.6f : 1f;
            damage = (int)(damage * damageMult * injectionNerf);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float manaRatio = (float)player.statMana / player.statManaMax2;
            bool injectionNerf = player.CalPlayerInfo().astralInjection;
            if (injectionNerf)
                manaRatio = MathHelper.Min(manaRatio, 0.65f);

            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            proj.scale = 0.75f + 0.75f * manaRatio;
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(LAPRecipeGroup.AnyAdamantiteBar, 7).
                AddIngredient(ItemID.SoulofFright, 10).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
