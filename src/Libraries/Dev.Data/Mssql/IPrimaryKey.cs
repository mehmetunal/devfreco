using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Data.Mssql
{
    public interface IPrimaryKey
    {
    }

    public interface IPrimaryKey<TKey> : IPrimaryKey
    {
    }
}
