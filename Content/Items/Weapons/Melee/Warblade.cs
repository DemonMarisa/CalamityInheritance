using CalamityInheritance.Content.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Melee
{
    public class Warblade : CIMelee, ILocalizedModType
    {
        //返厂的哥布林战刀得到了一定的重置
        public new string LocalizationCategory => "Content.Items.Weapons.Melee";
        //攻击Types:
        public int Attackstyle = 0;
        public int GapTime = 0;
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.DamageType = DamageClass.Melee;
            Item.width = 32;
            Item.height = 48;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 4.25f;
            Item.value = CIShopValue.RarityPriceGreen;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            //干掉本体的动画
            Item.noMelee = true;
            Item.noUseGraphic = true;
            //“发射弹幕”
            Item.shoot = ModContent.ProjectileType<WarbladeProj>();
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                
            }
        }
    }
}
