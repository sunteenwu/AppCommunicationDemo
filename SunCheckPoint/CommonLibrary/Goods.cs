using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
  public  class Goods
    {
        string goodsname;
        double price;
        bool isSelected=false;

        public Goods()
        {
        }

        public Goods(String good)
        {
            this.goodsname = good;
        }
        
        public string Goodsname
        {
            get
            {
                return goodsname;
            }

            set
            {
                goodsname = value;
            }
        }

        public double Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
            }
        }
    }
}
