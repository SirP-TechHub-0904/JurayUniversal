using JurayUniversal.Application.Services;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisTicketing.Pages.Platform
{
    [AllowAnonymous]
    public class VerifyPaymentModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly IPaymentService _paymentService;
        private readonly PaystackService _paystackService;

        public VerifyPaymentModel(DashboardDbContext context, IPaymentService paymentService, PaystackService paystackService)
        {
            _context = context;
            _paymentService = paymentService;
            _paystackService = paystackService;
        }

        public async Task<IActionResult> OnGetAsync(string @ref)
        {
            if (string.IsNullOrEmpty(@ref))
            {
                TempData["error"] = "Invalid payment reference.";
                return RedirectToPage("./Index");
            }

            var transaction = await _context.MisFinanceTransactions
                .FirstOrDefaultAsync(t => t.TransactionReference == @ref);

            if (transaction == null)
            {
                TempData["error"] = "Transaction not found.";
                return RedirectToPage("./Index");
            }

            if (transaction.Status == JurayUniversal.Domain.Enum.Enum.PaymentStatus.Success)
            {
                TempData["success"] = "Payment was already verified. Your ticket is ready.";
                return RedirectToPage("./Index");
            }

            // Call Provider to verify
            bool isPaid = false;
            if (transaction.PaymentProvider == "Paystack")
            {
                isPaid = await _paystackService.VerifyPaymentAsync(@ref);
            }
            else
            {
                isPaid = await _paymentService.VerifyPaymentAsync(@ref);
            }

            var ticket = await _context.MisEventTickets
                .FirstOrDefaultAsync(t => t.TransactionReference == @ref);

            if (isPaid)
            {
                transaction.Status = JurayUniversal.Domain.Enum.Enum.PaymentStatus.Success;
                transaction.PaidAt = DateTime.UtcNow;

                if (ticket != null)
                {
                    ticket.IsUsed = false; // Mark as Valid
                    
                    var ticketType = await _context.MisTicketTypes.FindAsync(ticket.MisTicketTypeId);
                    if (ticketType != null)
                    {
                        ticketType.SoldCount += 1;
                    }
                }

                await _context.SaveChangesAsync();
                TempData["success"] = "Payment successful! Your ticket has been generated.";
                return RedirectToPage("./Index");
            }
            else
            {
                transaction.Status = JurayUniversal.Domain.Enum.Enum.PaymentStatus.Failed;
                await _context.SaveChangesAsync();

                TempData["error"] = "Payment failed or was cancelled.";
                return RedirectToPage("./Index"); // In a real app we might redirect back to event details
            }
        }
    }
}
