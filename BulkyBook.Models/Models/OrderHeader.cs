using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.Models
{
    public class OrderHeader
    {
        /*Where is the order shipped 
         * what is the payment status 
         * order date
         * payment Id
         */
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } // each order belong to ID
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public string? OrderStatus { get; set; }
        public double OrderTotal { get; set; }
        public string? PaymentStatus { get; set; } // pending / completed
        public string? TrackingNumber { get; set; }
        public string?  Carrier { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentDueDate { get; set; } // For company users -- supposed to be DateOnly but it works only with .net8
        public string? PaymentIntentId { get; set; } // identify each payment for order

        [Required]
        public string Name { get; set; }
        [Required]
        public string StreetAdress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
