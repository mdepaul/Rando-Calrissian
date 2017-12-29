/* Attributions
 * Mike DePaul
 * https://github.com/mdepaul/Rando-Calrissian.git
 * **/

namespace MD.RandoCalrissian
{
    public class CharacterMix
    {
        public bool UseUpper { get; set; }
        public bool UseLower { get; set; }
        public bool UseDigits { get; set; }
        public bool UseSpecial { get; set; }

        public CharacterMix()
        {
            UseUpper = true;
            UseLower = true;
            UseDigits = true;
            UseSpecial = true;
        }

        public override string ToString()
        {
            return (UseUpper ? "U" : "") +
                (UseLower ? "L" : "") +
                (UseDigits ? "D" : "") +
                (UseSpecial ? "S" : "");
        }
    }
}