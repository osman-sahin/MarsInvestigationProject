using MarsInvestigationProject.Models.Enums;

namespace MarsInvestigationProject.Models
{
    internal class Position
    {
        public Position(int x, int y, Route r)
        {
            this.x = x;
            this.y = y;
            this.r = r;
        }
        public int x { get; set; }
        public int y { get; set; }
        public Route r;
    }
}
