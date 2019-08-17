using System.Collections.Generic;

namespace graph_master.common.utilities.Extensibility
{
    public class ExtensibleModelGetOptions
    {
        public List<string> Parts { get; set; }

        public bool PartIsUsed(string extension)
        {
            if (Parts == null)
                return false;

            for (int i = 0; i < Parts.Count; i++)
            {
                if (Parts[i] == extension)
                    return true;
            }

            return false;
        }
    }
}