using CalamityInheritance.Content.Items.Accessories;
using CalamityInheritance.System.Configs;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.PermanentBoosters;
using LAP.Assets.TextureRegister;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityInheritance.Texture.OldTextures
{
    public class OldTextureRegister : ModSystem
    {
        public static string TexturesPath => "CalamityInheritance/Texture/OldTextures";
        public static string MiscPath => $"{TexturesPath}/Misc";
        public static Tex2DWithPath BloodOrangeAlter { get; set; }
        public static Tex2DWithPath CometShardAlter { get; set; }
        public static Tex2DWithPath DragonfruitAlter { get; set; }
        public static Tex2DWithPath ElderberryAlter { get; set; }
        public static Tex2DWithPath EtherealCoreAlter { get; set; }
        public static Tex2DWithPath MiracleFruitAlter { get; set; }
        public static Tex2DWithPath PhantomHeartAlter { get; set; }
        public override void PostSetupContent()
        {
            BloodOrangeAlter = new Tex2DWithPath($"{MiscPath}/BloodOrangeAlter");
            MiracleFruitAlter = new Tex2DWithPath($"{MiscPath}/MiracleFruitAlter");
            ElderberryAlter = new Tex2DWithPath($"{MiscPath}/ElderberryAlter");
            DragonfruitAlter = new Tex2DWithPath($"{MiscPath}/DragonfruitAlter");
            CometShardAlter = new Tex2DWithPath($"{MiscPath}/CometShardAlter");
            EtherealCoreAlter = new Tex2DWithPath($"{MiscPath}/EtherealCoreAlter");
            PhantomHeartAlter = new Tex2DWithPath($"{MiscPath}/PhantomHeartAlter");

            if (CIRespriteConfig.Instance.BloodOrange)
                TextureAssets.Item[ItemType<SanguineTangerine>()] = BloodOrangeAlter.Texture;
            if (CIRespriteConfig.Instance.MiracleFruit)
                TextureAssets.Item[ItemType<MiracleFruit>()] = MiracleFruitAlter.Texture;
            if (CIRespriteConfig.Instance.BloodOrange)
                TextureAssets.Item[ItemType<TaintedCloudberry>()] = ElderberryAlter.Texture;
            if (CIRespriteConfig.Instance.BloodOrange)
                TextureAssets.Item[ItemType<SacredStrawberry>()] = DragonfruitAlter.Texture;
            if (CIRespriteConfig.Instance.BloodOrange)
                TextureAssets.Item[ItemType<CometShard>()] = CometShardAlter.Texture;
            if (CIRespriteConfig.Instance.BloodOrange)
                TextureAssets.Item[ItemType<EtherealCore>()] = EtherealCoreAlter.Texture;
            if (CIRespriteConfig.Instance.BloodOrange)
                TextureAssets.Item[ItemType<PhantomHeart>()] = PhantomHeartAlter.Texture;
        }
        public override void Unload()
        {
            BloodOrangeAlter = null;
            MiracleFruitAlter = null;
            ElderberryAlter = null;
            DragonfruitAlter = null;
            CometShardAlter = null;
            EtherealCoreAlter = null;
            PhantomHeartAlter = null;
        }
    }
}
