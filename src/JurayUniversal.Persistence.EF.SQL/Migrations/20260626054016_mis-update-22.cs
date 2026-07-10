using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurayUniversal.Persistence.EF.SQL.Migrations
{
    public partial class misupdate22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MisManuals",
                columns: new[] { "Id", "ModuleFolder", "Overview", "Title" },
                values: new object[] { 6L, "MisFinances", "Complete guide for managing compulsory dues and voluntary donations, including payment setup, tracking, and PixelPay integration.", "Dues & Donations Guide" });

            migrationBuilder.InsertData(
                table: "MisManualPages",
                columns: new[] { "Id", "MisManualId", "PageDescription", "PageTitle", "RouteKey" },
                values: new object[,]
                {
                    { 43L, 6L, "This is where you define and manage all your organization's payment categories — both compulsory dues (e.g. annual subscriptions) and optional donations (e.g. building fund, charity).", "Payment Categories", "/MisFinances/Dashboard/Categories" },
                    { 44L, 6L, "The Ledger gives you a complete financial picture for a single payment category — total revenue collected, who has paid, and (for compulsory dues) a full defaulters list.", "Payment Ledger", "/MisFinances/Dashboard/Ledger" },
                    { 45L, 6L, "A chronological master log of every single payment transaction recorded in the system across all categories. Filter by category, status, or date range.", "All Transactions", "/MisFinances/Dashboard/Transactions" },
                    { 46L, 6L, "Your personal financial dashboard. Shows all payments you owe to the organization, causes you can support, and your complete payment history.", "My Finances", "/MisFinances/Platform/MyFinances" }
                });

            migrationBuilder.InsertData(
                table: "MisManualElements",
                columns: new[] { "Id", "Description", "ElementName", "ElementType", "MisManualPageId", "RequiredRole" },
                values: new object[,]
                {
                    { 84L, "Opens the 'New Payment Category' modal. Fill in the category title, description, type (compulsory or optional), frequency (One-Time, Monthly, Yearly, etc.), and optionally a fixed amount. If you leave the amount blank, the user or donor can choose their own amount when paying.", "New Category Button", "Button", 43L, "GlobalEditor" },
                    { 85L, "When set to 'Compulsory Due', ALL active members are expected to pay this. It will appear in every user's 'Outstanding Payables' on their platform dashboard. Defaulters will automatically appear in the Ledger's Defaulters tab.", "Payment Type: Compulsory Due", "Setting", 43L, "GlobalEditor" },
                    { 86L, "When set to 'Optional Donation', the payment is voluntary. It shows in the 'Support a Cause' section on the user's platform. No defaulter tracking is applied. Perfect for tithes, offerings, project-specific fundraising.", "Payment Type: Optional Donation", "Setting", 43L, "GlobalEditor" },
                    { 87L, "Defines how often this payment is expected: One-Time (single occurrence), Daily, Weekly, Monthly, or Yearly. The system uses this for reporting and defaulter tracking.", "Frequency", "Setting", 43L, "GlobalEditor" },
                    { 88L, "Each active category has a unique public payment link (e.g. /pay/5) that anyone — even visitors without an account — can use to make a payment. Copy the link and share it on WhatsApp, email, social media, or embed it on your website.", "Public Link / Share Link", "Column", 43L, "GlobalViewer" },
                    { 89L, "Opens the payment ledger for this specific category. Shows who has paid, how much was collected, and (for compulsory dues) who the defaulters are.", "Ledger Button", "Button", 43L, "GlobalViewer" },
                    { 90L, "Inactive categories no longer appear on the public payment portal or on the user's platform. Use this to pause a category without deleting its payment history.", "Active / Inactive Toggle", "Setting", 43L, "GlobalEditor" },
                    { 91L, "Displays four key metrics: Total Payments, Revenue Collected, Users Paid, and Defaulters (only for compulsory categories). These are real-time calculations from the transaction log.", "Summary Cards", "Dashboard", 44L, "GlobalViewer" },
                    { 92L, "Lists all users or guests who have successfully made a payment for this category, including the amount, date, and PixelPay transaction reference.", "Paid Tab", "Tab", 44L, "GlobalViewer" },
                    { 93L, "Available only for Compulsory categories. Lists all registered platform members who have NOT yet paid. Use this to identify members to follow up with or remind via Mass Broadcast.", "Defaulters Tab", "Tab", 44L, "GlobalViewer" },
                    { 94L, "Allows you to manually record a cash or offline payment on behalf of a member (e.g. someone paid at the office). This creates a verified payment entry in the ledger without going through PixelPay. Enter the payer's name, email, amount, and any relevant notes.", "Record Manual Payment", "Button", 44L, "GlobalEditor" },
                    { 95L, "A unique code (e.g. TX-20260626-5-ABC123) that identifies every payment. Use this to trace a specific payment in your PixelPay merchant dashboard or resolve disputes.", "Transaction Reference", "Column", 45L, "GlobalViewer" },
                    { 96L, "Three possible statuses: Pending (user started but hasn't completed payment), Paid (PixelPay confirmed success), Failed (payment was declined or cancelled). Status is updated automatically via the PixelPay webhook.", "Payment Status", "Column", 45L, "GlobalViewer" },
                    { 97L, "If the payer was a registered member, their name links to their profile. If they were a guest (no account), their name and email as typed at checkout are displayed.", "Donor vs. Member", "Column", 45L, "GlobalViewer" },
                    { 98L, "This section shows all compulsory dues that you have not yet paid. Each card shows the title, frequency, and fixed amount (if applicable). Click 'Pay Now' to be taken to the secure PixelPay checkout. Once your payment is confirmed, the card disappears automatically.", "Outstanding Payables", "Section", 46L, "" },
                    { 99L, "Takes you to the PixelPay hosted checkout page. Because you are already logged in, your name and email are automatically filled in. You can pay by Card, Bank Transfer, USSD, or PayAttitude — all supported by PixelPay.", "Pay Now Button", "Button", 46L, "" },
                    { 100L, "Copies the unique payment link to your clipboard. Share it with someone who wants to pay on your behalf, or use it to pay from a different device.", "Share Button", "Button", 46L, "" },
                    { 101L, "Shows all optional donation categories set up by the admin. Contributions here are completely voluntary. Click 'Donate' to make a free-will contribution of any amount.", "Support a Cause Section", "Section", 46L, "" },
                    { 102L, "A complete chronological log of all payments you have made. Shows the category, amount paid, transaction reference, payment status, and date. Pending payments will update to 'Paid' automatically once PixelPay confirms.", "Payment History", "Table", 46L, "" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 84L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 85L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 86L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 87L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 88L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 89L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 90L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 91L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 92L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 93L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 94L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 95L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 96L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 97L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 98L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 99L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 100L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 101L);

            migrationBuilder.DeleteData(
                table: "MisManualElements",
                keyColumn: "Id",
                keyValue: 102L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 43L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 44L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 45L);

            migrationBuilder.DeleteData(
                table: "MisManualPages",
                keyColumn: "Id",
                keyValue: 46L);

            migrationBuilder.DeleteData(
                table: "MisManuals",
                keyColumn: "Id",
                keyValue: 6L);
        }
    }
}
