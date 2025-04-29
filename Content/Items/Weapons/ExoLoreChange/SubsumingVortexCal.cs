using Terraria.ModLoader;
using Terraria;
using CalamityInheritance.Utilities;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Projectiles.Magic;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.DataStructures;
using CalamityMod.Projectiles.Melee;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Terraria.Audio;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using rail;
using CalamityInheritance.Texture;
using CalamityInheritance.System.Configs;
using CalamityInheritance.Content.Projectiles.ExoLore;
using CalamityInheritance.Content.Items.Weapons.Magic;
using Terraria.ID;
using CalamityInheritance.Sounds.Custom;

namespace CalamityInheritance.Content.Items.Weapons.ExoLoreChange
{
    public class SubsumingVortexCal : GlobalItem
    {
        public static Player Fucker => Main.LocalPlayer;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item item, bool lateInstatiation) => item.type == ModContent.ItemType<SubsumingVortex>();
        #region Fuck掉原灾的发光贴图
        public delegate void PostDrawInWorldDelegate(SubsumingVortex self, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI);
        public static void PostDrawInWorld(PostDrawInWorldDelegate orig, SubsumingVortex self, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            self.Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>($"{CIResprite.CIExtraRoute}/FuckGlowMask").Value);
        }
        #endregion
        public const float SmallVortexSpeedFac = 1.3f;
        public const int SmallVortexCounts = 4;
        public const float SmallVortexDamageFac = 0.3f;
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var usPlayer = player.CIMod();
            if (usPlayer.LoreExo || usPlayer.PanelsLoreExo)
            {
                damage.Base = 1165;
                item.mana = 20;
                item.useTime = 20;
                item.useAnimation = 20;
            }
        }
        public override bool CanUseItem(Item item, Player player)
        {
            if (player.CheckExoLore())
            {
                if (player.altFunctionUse != 2)
                    item.UseSound = Utils.SelectRandom(Main.rand, SubsumingVortexold.TossSound);
                else
                    item.UseSound = null;
            }
            else
                item.UseSound = SoundID.Item84;

            return true;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            string t = Main.LocalPlayer.CIMod().PanelsLoreExo || Main.LocalPlayer.CIMod().LoreExo ? Language.GetTextValue($"{Generic.GetWeaponLocal}.Magic.SubsumingVortexChange") : null;
            if (t != null) tooltips.Add(new TooltipLine(Mod, "Name", t));
        }
        public override Vector2? HoldoutOffset(int type)
        {
            //画师跟写代码的打一架吧，原灾的贴图要-6才能正常手持
            if (CIRespriteConfig.Instance.FuckAllExo)
                return Vector2.Zero;
            else
                return new Vector2(-6, 0);
        }
        //We need to fuck the original shoot method, so we can use our own projectile.
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //Check if using lore.
            bool isLore = player.CheckExoLore();
            //if using lore, switch the proj.
            int altVortex = isLore ? ModContent.ProjectileType<SubsumingVortexProjBig>() : ModContent.ProjectileType<ExoVortex2>();
            int bigVortex = isLore ? ModContent.ProjectileType<SubsumingVortexProjGiant>() : ModContent.ProjectileType<EnormousConsumingVortex>();
            if (player.altFunctionUse != 2)
            {
                for (int i = 0; i < SmallVortexCounts; i++)
                {
                    float hue = (i / (float)(SmallVortexCounts - 1f) + Main.rand.NextFloat(0.3f)) % 1f;
                    Vector2 vortexVelocity = velocity * SmallVortexSpeedFac + Main.rand.NextVector2Square(-2.5f, 2.5f);
                    Projectile.NewProjectile(source, position, vortexVelocity, altVortex, (int)(damage * SmallVortexDamageFac), knockback, player.whoAmI, hue);
                }
                return false;
            }
            Projectile.NewProjectile(source, position, velocity, bigVortex, damage, knockback, player.whoAmI);
            return false;
        }
    }

    //暂时注释掉了。没啥用。
    //没删是因为不知道什么时候会用到这个钩子
    public class FuckSubsumingGlowMask : GlobalItem
    {
        public static void Load(Mod mod)
        {
            if (!CIRespriteConfig.Instance.FuckAllExo)
                return;
            MethodInfo fuckSubsumingGlow = typeof(SubsumingVortex).GetMethod("PostDrawInWorld", BindingFlags.Instance | BindingFlags.Public);
            MonoModHooks.Add(fuckSubsumingGlow, SubsumingVortexCal.PostDrawInWorld);
        }
    }
}
