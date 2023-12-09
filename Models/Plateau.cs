namespace MarsInvestigationProject.Models
{
    internal class Plateau
    {
        public Plateau(int maxX, int maxY)
        {
            this.maxX = maxX;
            this.maxY = maxY;
        }
        public int minX { get; set; } = 0;
        public int minY { get; set; } = 0;
        public int maxX { get; set; }
        public int maxY { get; set; }
    }
}
