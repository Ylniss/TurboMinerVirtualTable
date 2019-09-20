using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utils
{
    public interface IConverter<TSource, TDestination>
    {
        TDestination Convert(TSource source);
    }
}
