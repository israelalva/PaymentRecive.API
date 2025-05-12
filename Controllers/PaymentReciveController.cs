using PaymentRecive.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using PaymentRecive.API.Infrastructure;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentReciveController : ControllerBase
    {
        private readonly ILogger<PaymentReciveController> _logger;
        private readonly PaymentReciveContext _paymentReciveContext;

        public PaymentReciveController(
            ILogger<PaymentReciveController> logger,
            PaymentReciveContext catalogContext
            )
        {
            _logger = logger;
            _paymentReciveContext = catalogContext;
        }

        //[HttpGet]
        //public List<Product> Get()
        //{
        //    var productos = _catalogContext.Product.Include(x => x.ProductInStocks).ToList();

        //    return _catalogContext.Product.ToList();
        //}

        [HttpGet("{id}")]
        public AgreementPayments Get(int id)
        {
            var pagos = _paymentReciveContext.AgreementPayment.Include(x => x.AgreementPaymentId).FirstOrDefault(x => x.AgreementPaymentId == id);

            return pagos;
        }

        [HttpPost]
        public bool Post([FromBody] AgreementPayments value)
        {
            var producto = _paymentReciveContext.AgreementPayment.Add(value);
            _paymentReciveContext.SaveChanges();

            return true;
        }

        //[HttpDelete("{id}")]
        //public bool Delete(int id)
        //{
        //    var product = _catalogContext.Product.Where(x => x.ProductId == id).FirstOrDefault();
        //    if (product != null)
        //    {
        //        var delete = _catalogContext.Product.Remove(product);
        //        _catalogContext.SaveChanges();

        //        return true;
        //    }

        //    return false;
        //}

        //[HttpDelete("{name}")]
        //public bool DeleteByName(string name)
        //{
        //    var product = _catalogContext.Product.Where(x => x.Name == name).FirstOrDefault();
        //    if (product != null)
        //    {
        //        var delete = _catalogContext.Product.Remove(product);
        //        _catalogContext.SaveChanges();

        //        return true;
        //    }

        //    return false;
        //}
    }
}
