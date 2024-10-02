using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TpSlManager
{
    public struct SlTpCondictionHolder<T>
    {
        public delegate double DefineSl(T obj);
        public delegate double DefineTp(T obj);
        public DefineSl[] SlDelegate { get; set; }
        public DefineTp[] TpDelegate { get; set; }
        public T[] TpDelegateObj { get; set; }
        public T[] SlDelegateObj { get; set; }
        public bool Validate { get; set; } = true;
        public TpSlComputator<T> Computator { get; set; }

        public SlTpCondictionHolder(T[] slObj, T[] tpObj, DefineSl[] slcond, DefineTp[] tpcond)
        {
            //TODO: i m using different size for tp and sl
            if (slObj.Length != slcond.Length || tpcond.Length != tpObj.Length)
                this.Validate = false;

            else
            {
                TpDelegateObj = new T[tpObj.Length];
                SlDelegateObj = new T[slcond.Length];
                SlDelegate = new DefineSl[slcond.Length];
                TpDelegate = new DefineTp[tpObj.Length];

                this.Computator = new TpSlComputator<T>(this);
            }
           
        }
    }
}
