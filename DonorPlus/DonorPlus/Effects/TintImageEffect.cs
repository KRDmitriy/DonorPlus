using Xamarin.Forms;

namespace DonorPlus.Effects
{
    public class TintImageEffect : RoutingEffect
    {
        public const string GroupName = "DonorPlus";
        public const string Name = "TintImageEffect";

        public Color TintColor { get; set; }

        public TintImageEffect() : base($"{GroupName}.{Name}") { }
    }
}
