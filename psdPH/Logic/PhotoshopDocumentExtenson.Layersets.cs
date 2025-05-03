using Photoshop;
using System.Collections.Generic;
using System.Linq;

namespace psdPH.Logic
{
    public static partial class PhotoshopDocumentExtension
    {
        // Метод для получения имен всех групп слоев
        public static string[] GetLayerSetsNames(LayerSet[] layerSets)
        {
            return layerSets.Select(ls => ls.Name).ToArray();
        }

        // Метод для получения имен всех групп слоев в документе
        public static string[] GetLayerSetsNames(this Document doc, LayerSet[] layerSets)
        {
            return GetLayerSetsNames(layerSets);
        }

        // Метод для получения имен всех групп слоев в документе с учетом вложенности
        public static string[] GetLayerSetsNames(this Document doc, LayerListing listing = DefaultListing)
        {
            return GetLayerSets(doc, listing).Select(ls => ls.Name).ToArray();
        }

        // Метод для получения всех групп слоев в документе
        public static LayerSet[] GetLayerSets(this Document doc, LayerListing listing = DefaultListing)
        {
            List<LayerSet> layerSets = new List<LayerSet>();
            foreach (LayerSet item in doc.LayerSets)
                layerSets.Add(item);

            if (listing == LayerListing.Recursive)
                foreach (LayerSet layerSet in doc.LayerSets)
                    ProcessLayerSet(layerSet, layerSets);

            return layerSets.ToArray();
        }

        // Рекурсивный метод для обработки вложенных групп слоев
        private static void ProcessLayerSet(LayerSet layerSet, List<LayerSet> layerSets)
        {
            foreach (LayerSet nestedLayerSet in layerSet.LayerSets)
            {
                layerSets.Add(nestedLayerSet);
                ProcessLayerSet(nestedLayerSet, layerSets);
            }
        }

        // Метод для поиска группы слоев по ID
        private static LayerSet FindLayerSetById(this Document doc, int layerSetId, LayerListing listing = DefaultListing)
        {
            LayerSet[] layerSets = doc.GetLayerSets(listing);
            return layerSets.First(ls => ls.id == layerSetId);
        }

        // Метод для поиска группы слоев по имени
        public static LayerSet GetLayerSetByName(this Document doc, string layerSetName, LayerListing listing = DefaultListing)
        {
            LayerSet[] layerSets = doc.GetLayerSets(listing);
            return layerSets.First(ls => ls.Name == layerSetName);
        }
    }
}
