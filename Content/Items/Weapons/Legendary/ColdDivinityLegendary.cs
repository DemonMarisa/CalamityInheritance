using CalamityInheritance.Content.Buff.SummonBuff.Weapons;
using CalamityInheritance.Content.Projectiles.Legendary;
using CalamityInheritance.Core.Utils;
using LAP.Core.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Content.Items.Weapons.Legendary
{
    //我本人完全看不懂这个代码，如果需要的话可能重写？
    public class ColdDivinityLegendary : LegendaryWeapon
    {
        public override Color DrawColor => new(30, 144, 255);
        public static readonly float ShootSpeed = 10f;
        public override int BaseDamage => 48;
        public static int TrueDamage = 0;
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = false;
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.DamageType = DamageClass.Summon;
            Item.width = 52;
            Item.height = 50;
            Item.damage = BaseDamage;
            Item.mana = 10;
            Item.useTime = Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.knockBack = 4.5f;
            Item.UseSound = SoundID.Item30;
            Item.autoReuse = true;
            Item.shootSpeed = ShootSpeed;
            Item.shoot = ProjectileType<ColdDivinityLegendProj>();
        }
        //wtf is this
        public override bool AltFunctionUse(Player player)
        {
            if (player.ActiveItem().type == ItemType<ColdDivinityLegendary>())
            {
                bool canContinue = true;
                int count = 0;
                foreach (Projectile p in Main.ActiveProjectiles)
                {
                    if (p.type == ProjectileType<ColdDivinityLegendProj>() && p.owner == player.whoAmI)
                    {
                        if (p.ai[1] > 1f)
                        {
                            canContinue = false;
                            break;
                        }
                        else if (p.ai[1] == 0f)
                        {
                            if (((ColdDivinityLegendProj)p.ModProjectile).Idle)
                                count++;
                        }
                    }
                }
                if (canContinue && count > 0)
                {
                    NPC tar = CIUtils.MinionHoming(Main.MouseWorld, 1000f, player);
                    if (tar != null)
                    {
                        int pAmt = count;
                        float angleVariance = MathHelper.TwoPi / pAmt;
                        float angle = 0f;

                        var source = player.GetSource_ItemUse(player.ActiveItem());
                        for (int i = 0; i < pAmt; i++)
                        {
                            if (Main.projectile.Length == Main.maxProjectiles)
                                break;
                            int pDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(BaseDamage);
                            int projj = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ProjectileType<ColdDivinityLegendProj>(), pDmg, 1f, player.whoAmI, angle, 2f);
                            // Main.projectile[projj].originalDamage = item.damage;

                            angle += angleVariance;
                            for (int j = 0; j < 22; j++)
                            {
                                Dust dust = Dust.NewDustDirect(Main.projectile[projj].position, Main.projectile[projj].width, Main.projectile[projj].height, DustID.Ice);
                                dust.velocity = Vector2.UnitY * Main.rand.NextFloat(3f, 5.5f) * Main.rand.NextBool().ToDirectionInt();
                                dust.noGravity = true;
                            }
                        }
                    }
                }
                return false;
            }
            return base.AltFunctionUse(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            TrueDamage = damage;
            //搜寻世界上存在可能的由本人提供的召唤物
            float allMinions = 0f;
            foreach (Projectile pro in Main.ActiveProjectiles)
            {
                //如果是本人的召唤物，把这个召唤物的栏位需求赋值
                if (pro.minion && pro.owner == player.whoAmI)
                    allMinions += pro.minionSlots;
            }
            //用剩下的召唤栏召唤左键的召唤物 
            if (player.altFunctionUse != 2 && allMinions < player.maxMinions)
            {
                //给自己发送一个buff
                player.AddBuff(BuffType<ColdDivinityLegendBuff>(), 120, true);
                //将鼠标位置赋值
                position = player.LocalMouseWorld();
                int p = Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
                //实时刷新伤害
                Main.projectile[p].originalDamage = Item.damage;

                //查询当前在玩家身上持有的射弹数量
                int ptr = 0;
                foreach (Projectile pro in Main.ActiveProjectiles)
                {
                    if (pro.type == type && pro.owner == player.whoAmI)
                    {
                        if (!(pro.ModProjectile as ColdDivinityLegendProj).Idle)
                            continue;
                        ptr++;
                    }
                }
                float varAngle = MathHelper.TwoPi / ptr;
                float angle = 0f;
                foreach (Projectile pro in Main.ActiveProjectiles)
                {
                    if (pro.type == type && pro.owner == player.whoAmI && pro.ai[1] == 0f)
                    {
                        if (!(pro.ModProjectile as ColdDivinityLegendProj).Idle)
                            continue;
                        pro.ai[0] = angle;
                        pro.netUpdate = true;
                        angle += varAngle;
                        for (int j = 0; j < 22; j++)
                        {
                            Dust d = Dust.NewDustDirect(pro.position, pro.width, pro.height, DustID.Ice);
                            d.velocity = Vector2.UnitY * Main.rand.NextFloat(3f, 5.5f) * Main.rand.NextBool().ToDirectionInt();
                            d.noGravity = true;
                        }
                    }
                }
            }
            return false;
        }
    }
}