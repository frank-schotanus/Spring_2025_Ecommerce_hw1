using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Services;
using Spring2025_Ecommerce.Models;

namespace Maui.eCommerce.ViewModels
{
    public class ProductViewModel
    {
        public string? Name {
            get
            {
                return Model?.Name ?? string.Empty;
            }

            set
            {
                if(Model.Name != value && Model != null)
                {
                    Model.Name = value;
                }
            }
        }

        public Product? Model { get; set; }

        public void AddOrUpdate()
        {
            ProductServiceProxy.Current.AddOrUpdate(Model);
        }

        public ProductViewModel()
        {
            Model = new Product();
        }

        public ProductViewModel(Product? model)
        {
            Model = model;
        }
    }
}
