using Lunalipse.Common.Data;

namespace Lunalipse.Presentation.Generic
{
    public delegate void RemoveItem(object datacontext);
    public delegate void RemoveFromCatalogue(MusicEntity Entity);
    public delegate void AudioPanelDelegation<T, I>(T identifier, I args);
    public delegate void OnItemSelected<T>(T selected, object tag = null);
    internal class Delegation
    {
        public static RemoveItem RemovingItem;
        public static RemoveFromCatalogue CatalogueUpdated;
    }
}
