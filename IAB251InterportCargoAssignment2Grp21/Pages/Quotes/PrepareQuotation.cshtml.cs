using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IAB251InterportCargoAssignment2Grp21.Data;
using IAB251InterportCargoAssignment2Grp21.Models;

namespace IAB251InterportCargoAssignment2Grp21.Pages.Quotes
{
    public class PrepareQuotationModel : PageModel
    {
        private readonly InterportCargoContext _db;
        private const decimal GST_RATE = 0.10m;

        // ── Actual rates from Rate Schedule Extract.xlsx ──
        private static readonly Dictionary<string, decimal> Rates20ft = new()
        {
            ["WarfBooking"]    = 60m,
            ["LiftOnOff"]      = 80m,
            ["Fumigation"]     = 220m,
            ["LCL"]            = 400m,
            ["Tailgate"]       = 120m,
            ["Storage"]        = 240m,
            ["Facility"]       = 70m,
            ["WarfInspection"] = 60m,
        };

        private static readonly Dictionary<string, decimal> Rates40ft = new()
        {
            ["WarfBooking"]    = 70m,
            ["LiftOnOff"]      = 120m,
            ["Fumigation"]     = 280m,
            ["LCL"]            = 500m,
            ["Tailgate"]       = 160m,
            ["Storage"]        = 300m,
            ["Facility"]       = 100m,
            ["WarfInspection"] = 90m,
        };

        public PrepareQuotationModel(InterportCargoContext db) => _db = db;

        // ── Display properties ──
        public List<QuotationRequest> AcceptedRequests { get; set; } = new();
        public QuotationRequest? SelectedRequest { get; set; }
        public bool ShowDiscountAlert { get; set; }
        public bool DiscountApplied { get; set; }
        public string DiscountReason { get; set; } = string.Empty;
        public int SuggestedDiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal PreviewSubtotal { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal PreviewTotal { get; set; }
        public bool SubmitSuccess { get; set; }
        public string SubmittedQuotationNumber { get; set; } = string.Empty;
        public Dictionary<string, string> FormSnapshot { get; set; } = new();

        // ── GET ──
        public IActionResult OnGet(int? requestId)
        {
            if (!IsAuthorised()) return RedirectToPage("/Account/EmployeeLogin");

            if (requestId.HasValue)
                SelectedRequest = LoadRequest(requestId.Value);
            else
                AcceptedRequests = _db.QuotationRequests
                    .Include(r => r.Customer)
                    .Where(r => r.Status == "Accepted")
                    .ToList();

            return Page();
        }

        // ── PREVIEW ──
        public IActionResult OnPostPreview(
            int requestId, string containerType, string scope,
            bool includeWarfBooking, bool includeLiftOnOff, bool includeFumigation,
            bool includeLCL, bool includeTailgate, bool includeStorage,
            bool includeFacility, bool includeWarfInspection,
            decimal discountPercent = 0, string discountReason = "")
        {
            if (!IsAuthorised()) return RedirectToPage("/Account/EmployeeLogin");
            SelectedRequest = LoadRequest(requestId);

            decimal subtotal = CalcSubtotal(containerType, includeWarfBooking,
                includeLiftOnOff, includeFumigation, includeLCL, includeTailgate,
                includeStorage, includeFacility, includeWarfInspection,
                SelectedRequest!.NumberOfContainers);

            HandleDiscount(SelectedRequest, subtotal, discountPercent, discountReason);
            SetPreview(subtotal);

            FormSnapshot = Snapshot(requestId, containerType, scope,
                includeWarfBooking, includeLiftOnOff, includeFumigation,
                includeLCL, includeTailgate, includeStorage,
                includeFacility, includeWarfInspection);

            return Page();
        }

        // ── APPLY DISCOUNT ──
        public IActionResult OnPostApplyDiscount(
            int requestId, string containerType, string scope,
            bool includeWarfBooking, bool includeLiftOnOff, bool includeFumigation,
            bool includeLCL, bool includeTailgate, bool includeStorage,
            bool includeFacility, bool includeWarfInspection)
        {
            if (!IsAuthorised()) return RedirectToPage("/Account/EmployeeLogin");
            SelectedRequest = LoadRequest(requestId);

            decimal subtotal = CalcSubtotal(containerType, includeWarfBooking,
                includeLiftOnOff, includeFumigation, includeLCL, includeTailgate,
                includeStorage, includeFacility, includeWarfInspection,
                SelectedRequest!.NumberOfContainers);

            HandleDiscount(SelectedRequest, subtotal, 0, "");
            DiscountApplied = true;
            SetPreview(subtotal);

            FormSnapshot = Snapshot(requestId, containerType, scope,
                includeWarfBooking, includeLiftOnOff, includeFumigation,
                includeLCL, includeTailgate, includeStorage,
                includeFacility, includeWarfInspection);

            return Page();
        }

        // ── DECLINE DISCOUNT ──
        public IActionResult OnPostDeclineDiscount(
            int requestId, string containerType, string scope,
            bool includeWarfBooking, bool includeLiftOnOff, bool includeFumigation,
            bool includeLCL, bool includeTailgate, bool includeStorage,
            bool includeFacility, bool includeWarfInspection)
        {
            if (!IsAuthorised()) return RedirectToPage("/Account/EmployeeLogin");
            SelectedRequest = LoadRequest(requestId);

            decimal subtotal = CalcSubtotal(containerType, includeWarfBooking,
                includeLiftOnOff, includeFumigation, includeLCL, includeTailgate,
                includeStorage, includeFacility, includeWarfInspection,
                SelectedRequest!.NumberOfContainers);

            ShowDiscountAlert = false;
            DiscountApplied = false;
            SetPreview(subtotal);

            FormSnapshot = Snapshot(requestId, containerType, scope,
                includeWarfBooking, includeLiftOnOff, includeFumigation,
                includeLCL, includeTailgate, includeStorage,
                includeFacility, includeWarfInspection);

            return Page();
        }

        // ── SUBMIT ──
        public IActionResult OnPostSubmit(
            int requestId, string containerType, string scope,
            bool includeWarfBooking, bool includeLiftOnOff, bool includeFumigation,
            bool includeLCL, bool includeTailgate, bool includeStorage,
            bool includeFacility, bool includeWarfInspection,
            decimal discountPercent = 0, string discountReason = "")
        {
            if (!IsAuthorised()) return RedirectToPage("/Account/EmployeeLogin");
            SelectedRequest = LoadRequest(requestId);

            decimal subtotal = CalcSubtotal(containerType, includeWarfBooking,
                includeLiftOnOff, includeFumigation, includeLCL, includeTailgate,
                includeStorage, includeFacility, includeWarfInspection,
                SelectedRequest!.NumberOfContainers);

            decimal discountAmt = discountPercent > 0
                ? Math.Round(subtotal * (discountPercent / 100), 2) : 0;

            decimal afterDiscount = subtotal - discountAmt;
            decimal gst = Math.Round(afterDiscount * GST_RATE, 2);
            decimal total = afterDiscount + gst;

            string qNumber = $"QT-{DateTime.UtcNow:yyyyMMdd}-{requestId}";

            _db.Quotations.Add(new Quotation
            {
                RequestId          = requestId,
                QuotationNumber    = qNumber,
                DateIssued         = DateTime.UtcNow,
                ContainerType      = containerType,
                Scope              = scope,
                BaseRate           = subtotal,
                DepotCharges       = 0,
                LclDeliveryCharges = includeLCL
                    ? (containerType == "40ft" ? Rates40ft["LCL"] : Rates20ft["LCL"])
                    : 0,
                AdditionalCharges  = 0,
                DiscountAmount     = discountAmt,
                TotalAmount        = total,
                DiscountApplied    = discountPercent > 0,
                DiscountReason     = discountReason,
                Status             = "Pending",
                OfficerEmail       = HttpContext.Session.GetString("EmployeeEmail") ?? ""
            });

            QuotationRequest? req = _db.QuotationRequests.Find(requestId);
            if (req != null) req.Status = "QuotationSent";

            _db.SaveChanges();

            SubmitSuccess = true;
            SubmittedQuotationNumber = qNumber;
            return Page();
        }

        // ── HELPERS ──
        private bool IsAuthorised()
        {
            string? role = HttpContext.Session.GetString("EmployeeRole");
            if (string.IsNullOrWhiteSpace(role)) return false;
            return role.Replace(" ", "").Trim().ToLowerInvariant() == "quotationofficer";
        }

        private QuotationRequest? LoadRequest(int id) =>
            _db.QuotationRequests.Include(r => r.Customer)
               .FirstOrDefault(r => r.RequestId == id);

        private decimal CalcSubtotal(
            string containerType,
            bool warfBooking, bool liftOnOff, bool fumigation,
            bool lcl, bool tailgate, bool storage,
            bool facility, bool warfInspection, int containers)
        {
            var rates = containerType == "40ft" ? Rates40ft : Rates20ft;
            decimal total = 0;
            if (warfBooking)    total += rates["WarfBooking"];
            if (liftOnOff)      total += rates["LiftOnOff"];
            if (fumigation)     total += rates["Fumigation"];
            if (lcl)            total += rates["LCL"];
            if (tailgate)       total += rates["Tailgate"];
            if (storage)        total += rates["Storage"];
            if (facility)       total += rates["Facility"];
            if (warfInspection) total += rates["WarfInspection"];
            return total * containers;
        }

        private void SetPreview(decimal subtotal)
        {
            decimal afterDiscount = subtotal;
            if (DiscountApplied && SuggestedDiscountPercent > 0)
            {
                DiscountAmount = Math.Round(subtotal * (SuggestedDiscountPercent / 100m), 2);
                afterDiscount  = subtotal - DiscountAmount;
            }
            GSTAmount      = Math.Round(afterDiscount * GST_RATE, 2);
            PreviewSubtotal = subtotal;
            PreviewTotal    = afterDiscount + GSTAmount;
        }

        private void HandleDiscount(QuotationRequest req, decimal subtotal,
            decimal existingPercent, string existingReason)
        {
            if (existingPercent > 0)
            {
                DiscountApplied = true;
                SuggestedDiscountPercent = (int)existingPercent;
                DiscountReason = existingReason;
                return;
            }

            // Criteria 1: 3+ containers → 5%
            if (req.NumberOfContainers >= 3)
            {
                ShowDiscountAlert = true;
                SuggestedDiscountPercent = 5;
                DiscountReason =
                    $"{req.NumberOfContainers} containers requested — volume discount eligible (5%).";
                return;
            }

            // Criteria 2: High value job → 8%
            if (subtotal > 1000m)
            {
                ShowDiscountAlert = true;
                SuggestedDiscountPercent = 8;
                DiscountReason =
                    $"High-value job (${subtotal:F2}) — competitive pricing discount eligible (8%).";
                return;
            }

            // Criteria 3: Quarantine + packing → 10%
            if (req.RequiresQuarantine && (req.RequiresPacking || req.RequiresUnpacking))
            {
                ShowDiscountAlert = true;
                SuggestedDiscountPercent = 10;
                DiscountReason =
                    "Combined quarantine and packing/unpacking services — discount eligible (10%).";
            }
        }

        private Dictionary<string, string> Snapshot(
            int requestId, string containerType, string scope,
            bool warfBooking, bool liftOnOff, bool fumigation,
            bool lcl, bool tailgate, bool storage,
            bool facility, bool warfInspection) =>
            new()
            {
                ["requestId"]           = requestId.ToString(),
                ["containerType"]       = containerType,
                ["scope"]               = scope,
                ["includeWarfBooking"]  = warfBooking.ToString(),
                ["includeLiftOnOff"]    = liftOnOff.ToString(),
                ["includeFumigation"]   = fumigation.ToString(),
                ["includeLCL"]          = lcl.ToString(),
                ["includeTailgate"]     = tailgate.ToString(),
                ["includeStorage"]      = storage.ToString(),
                ["includeFacility"]     = facility.ToString(),
                ["includeWarfInspection"] = warfInspection.ToString(),
            };
    }
}