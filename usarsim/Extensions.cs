namespace usarsim
{
    using System;

    public static class Extensions
    {
        public static void Raise(this EventHandler handler, object sender, EventArgs args)
        {
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        public static void Raise<T>(this EventHandler<T> handler, object sender, T args) where T : EventArgs
        {
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        public static void Raise(this Action handler)
        {
            if (handler != null)
            {
                handler();
            }
        }

        public static void Raise<T>(this Action<T> handler, T args)
        {
            if (handler != null)
            {
                handler(args);
            }
        }
    }
}
