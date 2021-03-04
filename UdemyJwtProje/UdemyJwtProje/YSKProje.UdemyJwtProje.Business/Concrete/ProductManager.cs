using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YSKProje.UdemyJwtProje.Business.Interfaces;
using YSKProje.UdemyJwtProje.DataAccess.Interfaces;
using YSKProje.UdemyJwtProje.Entities.Concrete;

namespace YSKProje.UdemyJwtProje.Business.Concrete
{
    public class ProductManager : GenericManager<Product>, IProductService
    {
        private readonly IProductDal _productDal;
        public ProductManager(IGenericDal<Product> genericDal,
            IProductDal productDal) : base(genericDal)
        {
            _productDal = productDal;
        }

       

    }
}
