using BarterExchange.Shared;

namespace BarterExchange.Data.Classes
{
    public static class Storage
    {
        public static NavMenu Nav { get; set; }

        public static ushort Key { get; private set; } = 0x0077;
    }
}
