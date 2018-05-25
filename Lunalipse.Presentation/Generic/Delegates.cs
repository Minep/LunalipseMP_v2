using Lunalipse.Common.Data;

namespace Lunalipse.Presentation.Generic
{
    public delegate void RemoveItem(object datacontext);
    public delegate void RemoveFromCatalogue(MusicEntity Entity);
    internal class Delegates
    {
        public static RemoveItem RemovingItem;
        public static RemoveFromCatalogue CatalogueUpdated;
    }
}
