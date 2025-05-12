using PaymentRecive.API.Model;
using Microsoft.EntityFrameworkCore;

namespace PaymentRecive.API.Infrastructure
{
    public class PaymentReciveContext : DbContext
    {
        public PaymentReciveContext(DbContextOptions<PaymentReciveContext> options) : base(options)
        {
        }

        public DbSet<AgreementPayments> AgreementPayment { get; set; }
    }
}
