using System;

namespace PaymentRecive.API.Model
{
    public class AgreementPayments
    {
        public long AgreementPaymentId { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public long AgreementId { get; set; }
        public long AgreementNumber { get; set; }
        public bool? Insurance { get; set; }
        public int? BranchNumber { get; set; }
        public string BranchAddress { get; set; }
        public long RepresentativeId { get; set; }
        public string RepresentativeName { get; set; }
        public int WeekNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime TransactionCreationDate { get; set; }
        public string TransactionType { get; set; }
        public decimal TransactionValue { get; set; }
        public string ReceiptNumber { get; set; }
        public int? Sequence { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
