using System.Collections.Generic;
using CalamityInheritance.Content.Items.Materials;
using CalamityInheritance.CIPlayer;
using CalamityInheritance.Content.Items.Weapons.Magic;
using CalamityInheritance.Content.Projectiles.Melee;
using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityInheritance.Utilities;
using Terraria.Localization;
using CalamityInheritance.NPCs.Boss.Calamitas;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    [LegacyName("KelvinCatalystMelee")]
    public class MeleeTypeKelvinCatalyst : CIMelee, ILocalizedModType
    {
        
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.damage = 50;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.UseSound = CISoundID.SoundWeaponSwing;
            Item.value = CIShopValue.RarityPricePink;
            Item.rare = ItemRarityID.Pink;
            Item.Calamity().donorItem = true;
            Item.shoot = ModContent.ProjectileType<MeleeTypeKelvinCatalystProj>();
            Item.shootSpeed = 15f;
            Item.DamageType = DamageClass.MeleeNoSpeed;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            if(CIFunction.IsThereNpcNearby(ModContent.NPCType<CalamitasRebornPhase2>(), player, 7200f))
            {
                string empoweredWeapon = Language.GetTextValue("Mods.CalamityInheritance.Content.Items.Weapons.EmpoweredTooltip.BuffKelvin");
                tooltips.Add(new TooltipLine(Mod, "WeaponBuff", empoweredWeapon));
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<IceStar>(200).
                AddIngredient<Avalanche>(2).
                AddIngredient<GlacialCrusher>(2).
                AddIngredient<HoarfrostBow>(2).
                AddIngredient<Icebreaker>(2).
                AddIngredient<SnowstormStaff>(2).
                AddIngredient<BittercoldStaff>(2).
                AddIngredient<SoulofCryogen>(2).
                AddIngredient<FrostFlare>(2).
                AddIngredient<CryoStone>(2).
                AddIngredient(ItemID.FrostCore, 2).
                AddIngredient(ItemID.FrozenKey, 2).
                AddIngredient<EssenceofEleum>(20).
                AddIngredient<CryoBar>(20).
                AddTile(TileID.IceMachine).
                Register();
        }
    }
}
