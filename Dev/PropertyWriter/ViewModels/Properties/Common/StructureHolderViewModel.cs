using PropertyWriter.Models.Properties.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyWriter.ViewModels.Properties.Common
{
    class StructureHolderViewModel
    {
        private StructureHolder holder_;

        public IEnumerable<IPropertyViewModel> ViewModels { get; }

        public StructureHolderViewModel(StructureHolder holder)
        {
            holder_ = holder;
        }
    }
}
