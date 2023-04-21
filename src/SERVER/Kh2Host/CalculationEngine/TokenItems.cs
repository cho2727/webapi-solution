using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculationEngine
{
    public class TokenItems : IEnumerable<TokenItem>
    {
        #region 로컬 변수

        // 계산식을 구성하고 있는 각 토큰들을 저장한다.
        private System.Collections.Generic.List<TokenItem> items;

        // TokenItems를 가지고 있는 부모 오브젝트
        private Formula parent = null;

        #endregion

        #region 생성자

        public TokenItems(Formula Parent)
        {
            parent = Parent;
            items = new List<TokenItem>();
        }

        #endregion

        #region 프로퍼티

        /// <summary>
        /// The tokenitems collection object has a formular as a parent
        /// </summary>
        public Formula Parent
        {
            get
            {
                return parent;
            }
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        #endregion

        #region 함수

        public void Add(TokenItem item)
        {
            items.Add(item);
            item.parent = this; 
        }

        public void AddToFront(TokenItem item)
        {
            items.Insert(0, item);
            item.parent = this;
        }

        #endregion

        #region 인덱서

        public TokenItem this[int index]
        {
            get
            {
                return this.items[index];
            }
        }

        #endregion

        #region IEnumerable<TokenItem> 함수

        public IEnumerator<TokenItem> GetEnumerator()
        {
            return new TokemItemsEnumerator(items);
        }

        #endregion

        #region IEnumerable 멤버

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new TokemItemsEnumerator(items);
        }

        #endregion
    }

    public class TokemItemsEnumerator : IEnumerator<TokenItem>
    {
        #region 로컬 변수

        private System.Collections.Generic.List<TokenItem> items;
        int location;

        #endregion

        #region 생성자

        public TokemItemsEnumerator(System.Collections.Generic.List<TokenItem> Items)
        {
            items = Items;
            location = -1;
        }

        #endregion

        #region IEnumerator<TokenItem> 멤버

        public TokenItem Current
        {
            get
            {
                if (location > 0 || location < items.Count)
                {
                    return items[location];
                }
                else
                {
                    // we are outside the bounds					
                    throw new InvalidOperationException("The enumerator is out of bounds");
                }

            }
        }

        #endregion

        #region IDisposable 멤버

        public void Dispose()
        {
            // do nothing
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get
            {
                if (location > 0 || location < items.Count)
                {
                    return (object)items[location];
                }
                else
                {
                    // we are outside the bounds					
                    throw new InvalidOperationException("The enumerator is out of bounds");
                }

            }
        }

        public bool MoveNext()
        {
            location++;
            return (location < items.Count);
        }

        public void Reset()
        {
            location = -1;
        }

        #endregion
    }
}
