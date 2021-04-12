using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    class NativeAsset
    {
        public string AssetName { get; set; }

        public string TxHash { get; set; }

        public DateTime Time { get; set; }

        List<string> MetaData { get; set; }

        public long Quantity { get; set; }
    }
}
