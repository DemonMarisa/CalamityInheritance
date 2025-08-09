using System;
using CalamityInheritance.Content.Projectiles.Rogue;
using CalamityInheritance.Rarity;
using CalamityInheritance.Utilities;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Rogue
{
    [LegacyName("EmpyreanKnivesLegacyRogue")]
    public class RogueTypeKnivesEmpyrean: RogueWeapon, ILocalizedModType
    {
        public new string LocalizationCategory => $"{Generic.WeaponLocal}.Rogue";
        public static int BaseDamage = 250;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            Type.ShimmerEach<EmpyreanKnives>(false);
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 20;
            Item.damage = BaseDamage;
            Item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10; //使用时间15->8, 面板伤害360->200
            //使用时间8-10，伤害200-400
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item39;
            Item.autoReuse = true;
            Item.value = CIShopValue.RarityPriceDeepBlue;
            Item.rare = ModContent.RarityType<DeepBlue>();
            Item.shoot = ModContent.ProjectileType<RogueTypeKnivesEmpyreanProj>();
            Item.shootSpeed = 15f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int knifeAmt = 4;
            for (int j = 2; j <= 16; j *= 2)
            {
                if (Main.rand.NextBool(j))
                    knifeAmt++;
            }
            bool stealth = player.CheckStealth();
            int pType = stealth ? ModContent.ProjectileType<RogueTypeKnivesEmpyreanProjClone>() : type;
            for (int i = 0; i < knifeAmt; i++)
            {
                float spreadX = Main.rand.Next(-25, 26) * 0.05f * i;
                float spreadY = Main.rand.Next(-25, 26) * 0.05f * i;
                Vector2 tarPos = new (spreadX, spreadY);
                Vector2 distVec = velocity + tarPos;
                float tarDist = distVec.Length();
                tarDist = Item.shootSpeed / tarDist;
                tarPos.X *= tarDist;
                tarPos.Y *= tarDist;
                int p =Projectile.NewProjectile(source, position, distVec, pType, damage, knockback, Main.myPlayer); 
                Main.projectile[p].Calamity().stealthStrike = stealth;
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.VampireKnives).
                AddIngredient<MonstrousKnives>().
                AddIngredient<CosmiliteBar>(8).
                AddIngredient<DarksunFragment>(8).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
