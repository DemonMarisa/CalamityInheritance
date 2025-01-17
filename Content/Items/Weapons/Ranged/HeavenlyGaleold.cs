using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityInheritance.Content.Projectiles.Ranged;
using CalamityInheritance.Rarity;
using Microsoft.Xna.Framework.Graphics;

namespace CalamityInheritance.Content.Items.Weapons.Ranged
{
    public class HeavenlyGaleold : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Content.Items.Weapons.Ranged";
        public const float NormalArrowDamageMult = 1.25f;
        private static int[] ExoArrows;
        public override void SetStaticDefaults()
        {
            ExoArrows = new int[]
            {
            ModContent.ProjectileType<ExoArrowTeal>(),
            ModContent.ProjectileType<OrangeExoArrow>(),
            ModContent.ProjectileType<ExoArrowGreen>(),
            ModContent.ProjectileType<ExoArrowBlue>()
            };
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 198;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 98;
            Item.useTime = 9;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Arrow;
            Item.rare = ModContent.RarityType<CatalystViolet>();
            Item.value = CIShopValue.RarityPriceCatalystViolet;
            Item.Calamity().canFirePointBlankShots = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo spawnSource, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 source = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 baseOffset = velocity;
            baseOffset.Normalize();
            baseOffset *= 40f;

            float piOver10 = MathHelper.Pi / 10f;
            bool againstWall = !Collision.CanHit(source, 0, 0, source + baseOffset, 0, 0);

            int numArrows = 5;
            float dmgMult = 1f;

            for (int i = 0; i < numArrows; i++)
            {
                float offsetAmt = i - (numArrows - 1f) / 2f;
                Vector2 offset = baseOffset.RotatedBy(piOver10 * offsetAmt);

                if (againstWall)
                    offset -= baseOffset;

                int thisArrowType = type;
            if (CalamityInheritanceConfig.Instance.AmmoConversion == false)
            {
                if (type == ProjectileID.WoodenArrowFriendly)
                {
                    thisArrowType = Main.rand.Next(ExoArrows);

                    dmgMult = thisArrowType == ModContent.ProjectileType<ExoArrowTeal>() ? 0.66f : 1f;
                }
                else
                {
                    dmgMult = NormalArrowDamageMult;
                }
            }
            if (CalamityInheritanceConfig.Instance.AmmoConversion == true)
            {
                thisArrowType = Main.rand.Next(ExoArrows);
                dmgMult = thisArrowType == ModContent.ProjectileType<ExoArrowTeal>() ? 0.66f : 1f;
            }

                int finalDamage = (int)(damage * dmgMult);
                int proj = Projectile.NewProjectile(spawnSource, source + offset, velocity, thisArrowType, finalDamage, knockback, player.whoAmI);

                if (type != ProjectileID.WoodenArrowFriendly)
                {
                    Main.projectile[proj].noDropItem = true;
                }
            }

            return false;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        => Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalamityInheritance/Content/Items/Weapons/Ranged/HeavenlyGaleoldGlow").Value);

        public override bool CanConsumeAmmo(Item ammo, Player player)
            {
                if (Main.rand.Next(0, 100) < 66)
                    return false;
                return true;
            }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PlanetaryAnnihilation>().
                AddIngredient<TelluricGlare>().
                AddIngredient<ClockworkBow>().
                AddIngredient<Alluvion>().
                AddIngredient<AstrealDefeat>().
                AddIngredient<FlarewingBow>().
                AddIngredient<Phangasm>().
                AddIngredient<TheBallista>().
                AddIngredient<MiracleMatter>().
                AddTile<CalamityMod.Tiles.Furniture.CraftingStations.DraedonsForge>().
                Register();
        }
    }
}
