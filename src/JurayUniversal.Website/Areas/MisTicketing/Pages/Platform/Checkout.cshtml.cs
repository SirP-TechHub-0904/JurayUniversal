using JurayUniversal.Application.Services;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisTicketing.Pages.Platform
{
    [AllowAnonymous]
    public class CheckoutModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly IPaymentService _paymentService;
        private readonly PaystackService _paystackService;

        public CheckoutModel(DashboardDbContext context, UserManager<Profile> userManager, IPaymentService paymentService, PaystackService paystackService)
        {
            _context = context;
            _userManager = userManager;
            _paymentService = paymentService;
            _paystackService = paystackService;
        }

        public MisEvent MisEvent { get; set; }
        public MisTicketType TicketType { get; set; }
        public JurayUniversal.Domain.Models.MisSetting Settings { get; set; }

        [BindProperty]
        public string GuestName { get; set; }

        [BindProperty]
        public string GuestEmail { get; set; }

        [BindProperty]
        public string GuestPhone { get; set; }

        [BindProperty]
        public string PaymentMethod { get; set; }

        public async Task<IActionResult> OnGetAsync(long eventId, long ticketTypeId)
        {
            Settings = await _context.MisSettings.FirstOrDefaultAsync();
            TicketType = await _context.MisTicketTypes
                .Include(t => t.MisEvent)
                .ThenInclude(e => e.MisFinanceCategory)
                .FirstOrDefaultAsync(t => t.Id == ticketTypeId && t.MisEventId == eventId);

            if (TicketType == null)
            {
                return NotFound();
            }

            MisEvent = TicketType.MisEvent;

            // Pre-fill user data if logged in
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                GuestName = $"{user.FirstName} {user.LastName}";
                GuestEmail = user.Email;
                GuestPhone = user.PhoneNumber;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long eventId, long ticketTypeId)
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            var ticketType = await _context.MisTicketTypes
                .Include(t => t.MisEvent)
                .ThenInclude(e => e.MisFinanceCategory)
                .FirstOrDefaultAsync(t => t.Id == ticketTypeId && t.MisEventId == eventId);

            if (ticketType == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            // Ensure capacity
            if (ticketType.TotalCapacity.HasValue && ticketType.SoldCount >= ticketType.TotalCapacity.Value)
            {
                TempData["error"] = "Sorry, this ticket type is sold out.";
                return RedirectToPage("./EventDetails", new { id = eventId });
            }

            string refCode = "TKT_" + Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();

            if (ticketType.Price == 0)
            {
                // FREE TICKET - Generate Immediately
                var ticket = new MisEventTicket
                {
                    MisTicketTypeId = ticketTypeId,
                    UserId = userId,
                    GuestName = GuestName,
                    GuestEmail = GuestEmail,
                    GuestPhone = GuestPhone,
                    QrCodeText = Guid.NewGuid().ToString(),
                    IsUsed = false,
                    TransactionReference = "FREE",
                    CreatedAt = DateTime.UtcNow
                };

                ticketType.SoldCount += 1;
                _context.MisEventTickets.Add(ticket);
                await _context.SaveChangesAsync();

                TempData["success"] = "Your free ticket has been secured!";
                return RedirectToPage("./Index");
            }
            else
            {
                // PAID TICKET - Initiate Payment
                if (ticketType.MisEvent.MisFinanceCategoryId == null)
                {
                    TempData["error"] = "Event finance category not configured. Cannot process payment.";
                    return RedirectToPage("./EventDetails", new { id = eventId });
                }

                // If only one provider is enabled, default to it
                if (string.IsNullOrEmpty(PaymentMethod))
                {
                    if (settings != null && settings.EnablePaystack && !settings.EnablePixelPay) PaymentMethod = "Paystack";
                    else if (settings != null && settings.EnablePixelPay) PaymentMethod = "PixelPay";
                    else PaymentMethod = "PixelPay"; // Default fallback
                }

                var transaction = new MisFinanceTransaction
                {
                    MisFinanceCategoryId = ticketType.MisEvent.MisFinanceCategoryId.Value,
                    UserId = userId,
                    DonorName = GuestName,
                    DonorEmail = GuestEmail,
                    DonorPhone = GuestPhone,
                    Amount = ticketType.Price,
                    TransactionReference = refCode,
                    PaymentProvider = PaymentMethod,
                    Status = JurayUniversal.Domain.Enum.Enum.PaymentStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    Notes = $"Ticket Purchase: {ticketType.MisEvent.Title} - {ticketType.Name}"
                };

                _context.MisFinanceTransactions.Add(transaction);
                
                // Temporarily create an unverified ticket linked to transaction
                var pendingTicket = new MisEventTicket
                {
                    MisTicketTypeId = ticketTypeId,
                    UserId = userId,
                    GuestName = GuestName,
                    GuestEmail = GuestEmail,
                    GuestPhone = GuestPhone,
                    QrCodeText = Guid.NewGuid().ToString(),
                    IsUsed = true, // Mark as "Used/Invalid" until payment succeeds
                    TransactionReference = refCode,
                    CreatedAt = DateTime.UtcNow
                };
                
                _context.MisEventTickets.Add(pendingTicket);
                await _context.SaveChangesAsync();

                string callbackUrl = $"{Request.Scheme}://{Request.Host}/MisTicketing/Platform/VerifyPayment?ref={refCode}&provider={PaymentMethod}";

                if (PaymentMethod == "Paystack")
                {
                    var paymentInit = await _paystackService.InitializePaymentAsync(refCode, ticketType.Price, GuestEmail, callbackUrl);
                    if (paymentInit.Success)
                    {
                        return Redirect(paymentInit.PaymentUrl);
                    }
                    else
                    {
                        TempData["error"] = "Failed to initialize Paystack: " + paymentInit.ErrorMessage;
                        return RedirectToPage("./EventDetails", new { id = eventId });
                    }
                }
                else
                {
                    var paymentInit = await _paymentService.InitializePaymentAsync(refCode, ticketType.Price, "NGN", callbackUrl);
                    if (paymentInit.Success)
                    {
                        return Redirect(paymentInit.PaymentUrl);
                    }
                    else
                    {
                        TempData["error"] = "Failed to initialize PixelPay: " + paymentInit.ErrorMessage;
                        return RedirectToPage("./EventDetails", new { id = eventId });
                    }
                }
            }
        }
    }
}
