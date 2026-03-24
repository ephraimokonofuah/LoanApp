namespace LoanApp.Utility
{
    public static class EmailTemplates
    {
        private static string BaseLayout(string title, string iconHtml, string bodyContent, string? footerNote = null)
        {
            return $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>{title}</title>
</head>
<body style=""margin:0; padding:0; background-color:#f4f6f9; font-family:'Segoe UI',Roboto,Helvetica,Arial,sans-serif;"">
    <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#f4f6f9; padding:32px 16px;"">
        <tr>
            <td align=""center"">
                <table role=""presentation"" width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#ffffff; border-radius:12px; overflow:hidden; box-shadow:0 2px 12px rgba(0,0,0,0.08);"">
                    <!-- Header -->
                    <tr>
                        <td style=""background:linear-gradient(135deg,#0d1b2a 0%,#1b2838 40%,#0a4d68 100%); padding:32px 40px; text-align:center;"">
                            <h1 style=""color:#ffffff; margin:0; font-size:24px; font-weight:700; letter-spacing:0.5px;"">LoanApp</h1>
                            <p style=""color:rgba(255,255,255,0.7); margin:6px 0 0; font-size:13px;"">Your Trusted Loan Management Platform</p>
                        </td>
                    </tr>
                    <!-- Icon -->
                    <tr>
                        <td align=""center"" style=""padding:28px 0 0;"">
                            {iconHtml}
                        </td>
                    </tr>
                    <!-- Title -->
                    <tr>
                        <td style=""padding:16px 40px 0; text-align:center;"">
                            <h2 style=""margin:0; font-size:22px; color:#1a1a2e; font-weight:700;"">{title}</h2>
                        </td>
                    </tr>
                    <!-- Body -->
                    <tr>
                        <td style=""padding:20px 40px 32px;"">
                            {bodyContent}
                        </td>
                    </tr>
                    <!-- Footer Note -->
                    {(footerNote != null ? $@"
                    <tr>
                        <td style=""padding:0 40px 24px;"">
                            <div style=""background-color:#f0f4ff; border-left:4px solid #0d6efd; border-radius:6px; padding:14px 18px;"">
                                <p style=""margin:0; font-size:13px; color:#495057;"">{footerNote}</p>
                            </div>
                        </td>
                    </tr>" : "")}
                    <!-- Divider -->
                    <tr>
                        <td style=""padding:0 40px;"">
                            <hr style=""border:none; border-top:1px solid #e9ecef; margin:0;"" />
                        </td>
                    </tr>
                    <!-- Footer -->
                    <tr>
                        <td style=""padding:24px 40px; text-align:center;"">
                            <p style=""margin:0; font-size:12px; color:#adb5bd;"">This is an automated notification from LoanApp.</p>
                            <p style=""margin:6px 0 0; font-size:12px; color:#adb5bd;"">Please do not reply directly to this email.</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
        }

        private static string Icon(string emoji, string bgColor) =>
            $@"<div style=""width:64px; height:64px; border-radius:50%; background-color:{bgColor}; display:inline-block; text-align:center; line-height:64px; font-size:30px;"">{emoji}</div>";

        private static string InfoRow(string label, string value) =>
            $@"<tr>
                <td style=""padding:8px 12px; font-size:14px; color:#6c757d; border-bottom:1px solid #f0f0f0; white-space:nowrap;"">{label}</td>
                <td style=""padding:8px 12px; font-size:14px; color:#1a1a2e; font-weight:600; border-bottom:1px solid #f0f0f0;"">{value}</td>
            </tr>";

        private static string InfoTable(params (string label, string value)[] rows)
        {
            var rowsHtml = string.Join("", rows.Select(r => InfoRow(r.label, r.value)));
            return $@"<table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#f8f9fa; border-radius:8px; margin:16px 0; overflow:hidden;"">
                {rowsHtml}
            </table>";
        }

        private static string StatusBadge(string text, string bgColor, string textColor = "#ffffff") =>
            $@"<span style=""display:inline-block; padding:4px 14px; border-radius:20px; background-color:{bgColor}; color:{textColor}; font-size:13px; font-weight:600;"">{text}</span>";

        // ==========================================
        // LOAN APPLICATION NOTIFICATIONS
        // ==========================================

        public static string LoanApplicationSubmitted(string userName, int applicationId, decimal amount, string purpose, int duration)
        {
            return BaseLayout("New Loan Application Submitted",
                Icon("&#128203;", "#e8f4fd"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>Admin</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">A new loan application has been submitted and requires your review.</p>
                {InfoTable(
                    ("Applicant", userName),
                    ("Application ID", $"#{applicationId}"),
                    ("Loan Amount", $"${amount:N2}"),
                    ("Purpose", purpose),
                    ("Duration", $"{duration} months")
                )}
                <p style=""font-size:14px; color:#6c757d;"">Please log in to the admin panel to review this application.</p>",
                "A new application is waiting for your review.");
        }

        public static string LoanApplicationApproved(string userName, int applicationId, decimal amount)
        {
            return BaseLayout("Loan Application Approved! &#127881;",
                Icon("&#9989;", "#d4edda"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{userName}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Great news! Your loan application has been <strong style=""color:#198754;"">approved</strong>.</p>
                {InfoTable(
                    ("Application ID", $"#{applicationId}"),
                    ("Approved Amount", $"${amount:N2}"),
                    ("Status", "Approved ✓")
                )}
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Your next step is to set up your disbursement details. Please log in to your account to provide your bank information.</p>",
                "Log in to your account to set up disbursement and receive your funds.");
        }

        public static string LoanApplicationRejected(string userName, int applicationId)
        {
            return BaseLayout("Loan Application Update",
                Icon("&#128308;", "#f8d7da"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{userName}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">We regret to inform you that your loan application has been <strong style=""color:#dc3545;"">declined</strong> at this time.</p>
                {InfoTable(
                    ("Application ID", $"#{applicationId}"),
                    ("Status", "Rejected")
                )}
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">This decision may be based on the information provided. You are welcome to apply again or contact our support team for more details.</p>",
                "You can submit a new application or reach out to support for assistance.");
        }

        public static string DocumentsRequested(string userName, int applicationId, string? note)
        {
            var noteHtml = !string.IsNullOrEmpty(note)
                ? $@"<div style=""background-color:#fff3cd; border-radius:8px; padding:14px 18px; margin:16px 0;"">
                    <p style=""margin:0; font-size:14px; color:#856404;""><strong>Admin Note:</strong> {note}</p>
                </div>"
                : "";

            return BaseLayout("Documents Required for Your Application",
                Icon("&#128196;", "#fff3cd"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{userName}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Additional documents are needed to process your loan application.</p>
                {InfoTable(
                    ("Application ID", $"#{applicationId}"),
                    ("Status", "Documents Requested")
                )}
                {noteHtml}
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Please log in and upload the required documents as soon as possible to avoid delays.</p>",
                "Upload your documents promptly to keep your application moving forward.");
        }

        // ==========================================
        // DISBURSEMENT NOTIFICATIONS
        // ==========================================

        public static string DisbursementBankDetailsSubmitted(string userName, int disbursementId, decimal amount)
        {
            return BaseLayout("Bank Details Submitted for Disbursement",
                Icon("&#127974;", "#e8f4fd"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>Admin</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">A user has submitted their bank details for loan disbursement.</p>
                {InfoTable(
                    ("User", userName),
                    ("Disbursement ID", $"#{disbursementId}"),
                    ("Amount", $"${amount:N2}"),
                    ("Status", "Account Details Submitted")
                )}
                <p style=""font-size:14px; color:#6c757d;"">Please review the details and proceed with the disbursement process.</p>",
                "Bank details are waiting for your review in the admin panel.");
        }

        public static string DisbursementStatusUpdate(string userName, string statusText, int disbursementId, decimal amount)
        {
            var icon = statusText switch
            {
                "Paid" => Icon("&#128176;", "#d4edda"),
                "ReadyForPayment" => Icon("&#128178;", "#cce5ff"),
                "Failed" => Icon("&#10060;", "#f8d7da"),
                _ => Icon("&#128260;", "#e2e3e5")
            };

            var message = statusText switch
            {
                "Paid" => "Your funds have been successfully disbursed! The repayment schedule has been generated for your loan.",
                "ReadyForPayment" => "Your disbursement has been approved and is ready for payment. Funds will be sent to your bank account shortly.",
                "Failed" => "Unfortunately, there was an issue with your disbursement. Please contact support or check your bank details.",
                _ => $"Your disbursement status has been updated to: {statusText}."
            };

            return BaseLayout($"Disbursement Update: {statusText}",
                icon,
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{userName}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">{message}</p>
                {InfoTable(
                    ("Disbursement ID", $"#{disbursementId}"),
                    ("Amount", $"${amount:N2}"),
                    ("Status", statusText)
                )}",
                "Log in to your account for full details.");
        }

        // ==========================================
        // REPAYMENT NOTIFICATIONS
        // ==========================================

        public static string PaymentRequested(string userName, int installmentNumber, decimal amount, string paymentMethod)
        {
            return BaseLayout("New Payment Request Received",
                Icon("&#128179;", "#cce5ff"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>Admin</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">A user has requested payment details for a repayment installment.</p>
                {InfoTable(
                    ("User", userName),
                    ("Installment", $"#{installmentNumber}"),
                    ("Amount", $"${amount:N2}"),
                    ("Payment Method", paymentMethod)
                )}
                <p style=""font-size:14px; color:#6c757d;"">Please provide the payment details through the admin panel.</p>",
                "A user is waiting for payment details. Please respond promptly.");
        }

        public static string PaymentDetailsSent(string userName, int installmentNumber, decimal amount, string paymentMethod)
        {
            return BaseLayout("Payment Details Are Ready!",
                Icon("&#128233;", "#cce5ff"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{userName}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">The admin has sent you the payment details for your repayment.</p>
                {InfoTable(
                    ("Installment", $"#{installmentNumber}"),
                    ("Amount Due", $"${amount:N2}"),
                    ("Payment Method", paymentMethod)
                )}
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Please log in to view the payment details and make your payment. After paying, submit your transaction reference for confirmation.</p>",
                "Complete your payment and submit the transaction reference to confirm.");
        }

        public static string TransactionReferenceSubmitted(string userName, int installmentNumber, decimal amount, string? transactionRef)
        {
            return BaseLayout("Transaction Reference Submitted",
                Icon("&#128203;", "#fff3cd"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>Admin</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">A user has submitted a transaction reference for payment verification.</p>
                {InfoTable(
                    ("User", userName),
                    ("Installment", $"#{installmentNumber}"),
                    ("Amount", $"${amount:N2}"),
                    ("Transaction Ref", transactionRef ?? "N/A")
                )}
                <p style=""font-size:14px; color:#6c757d;"">Please verify the payment and confirm it in the admin panel.</p>",
                "Payment is awaiting your verification.");
        }

        public static string PaymentConfirmed(string userName, int installmentNumber, decimal amount)
        {
            return BaseLayout("Payment Confirmed! &#127881;",
                Icon("&#9989;", "#d4edda"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{userName}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Your payment for installment <strong>#{installmentNumber}</strong> has been <strong style=""color:#198754;"">confirmed</strong> successfully!</p>
                {InfoTable(
                    ("Installment", $"#{installmentNumber}"),
                    ("Amount Paid", $"${amount:N2}"),
                    ("Status", "Paid ✓")
                )}
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Thank you for your timely payment. You can view your updated repayment schedule in your account.</p>",
                "Keep up the great payment track record!");
        }

        // ==========================================
        // ELIGIBILITY NOTIFICATIONS
        // ==========================================

        public static string EligibilityCheckSubmitted(string userName, int checkId, decimal amount, string loanType)
        {
            return BaseLayout("New Eligibility Check Submitted",
                Icon("&#128270;", "#e8f4fd"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>Admin</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">A new eligibility check has been submitted for review.</p>
                {InfoTable(
                    ("User", userName),
                    ("Check ID", $"#{checkId}"),
                    ("Loan Type", loanType),
                    ("Desired Amount", $"${amount:N2}")
                )}
                <p style=""font-size:14px; color:#6c757d;"">Please review the eligibility check in the admin panel.</p>",
                "An eligibility check is waiting for your review.");
        }

        public static string EligibilityApproved(string userName, int checkId, decimal amount)
        {
            return BaseLayout("Eligibility Check Approved! &#127881;",
                Icon("&#9989;", "#d4edda"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{userName}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Your eligibility check has been <strong style=""color:#198754;"">approved</strong> by an admin!</p>
                {InfoTable(
                    ("Check ID", $"#{checkId}"),
                    ("Amount", $"${amount:N2}"),
                    ("Status", "Approved ✓")
                )}
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">You can now proceed to submit a full loan application. Log in to get started!</p>",
                "You're eligible! Apply for your loan now.");
        }

        public static string EligibilityRejected(string userName, int checkId, string? adminNotes)
        {
            var notesHtml = !string.IsNullOrEmpty(adminNotes)
                ? $@"<div style=""background-color:#f8d7da; border-radius:8px; padding:14px 18px; margin:16px 0;"">
                    <p style=""margin:0; font-size:14px; color:#842029;""><strong>Admin Notes:</strong> {adminNotes}</p>
                </div>"
                : "";

            return BaseLayout("Eligibility Check Update",
                Icon("&#128308;", "#f8d7da"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{userName}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Your eligibility check has been reviewed. Unfortunately, it was <strong style=""color:#dc3545;"">not approved</strong> at this time.</p>
                {InfoTable(
                    ("Check ID", $"#{checkId}"),
                    ("Status", "Rejected")
                )}
                {notesHtml}
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">You may try again with different parameters or contact support for assistance.</p>",
                "Contact our support team if you have questions about this decision.");
        }

        // ==========================================
        // SUPPORT TICKET NOTIFICATIONS
        // ==========================================

        public static string SupportTicketCreated(string ticketNumber, string subject, string category)
        {
            return BaseLayout("New Support Ticket Created",
                Icon("&#127384;", "#e8f4fd"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>Admin</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">A new support ticket has been created and needs attention.</p>
                {InfoTable(
                    ("Ticket #", ticketNumber),
                    ("Subject", subject),
                    ("Category", category)
                )}
                <p style=""font-size:14px; color:#6c757d;"">Please log in to the admin panel to review and respond to this ticket.</p>",
                "A user needs your help! Please respond as soon as possible.");
        }

        public static string SupportTicketReply(string userName, string ticketNumber, string subject, bool isAdminReply)
        {
            var greeting = isAdminReply ? userName : "Admin";
            var sender = isAdminReply ? "The support team" : userName;

            return BaseLayout($"New Reply on Ticket #{ticketNumber}",
                Icon("&#128172;", "#e8f4fd"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{greeting}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;""><strong>{sender}</strong> has replied to support ticket <strong>#{ticketNumber}</strong>.</p>
                {InfoTable(
                    ("Ticket #", ticketNumber),
                    ("Subject", subject)
                )}
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Log in to your account to view the full conversation and respond.</p>",
                "Continue the conversation by logging in to your account.");
        }

        public static string SupportTicketStatusUpdate(string userName, string ticketNumber, string newStatus)
        {
            var icon = newStatus switch
            {
                "Resolved" => Icon("&#9989;", "#d4edda"),
                "Closed" => Icon("&#128274;", "#e2e3e5"),
                _ => Icon("&#128260;", "#cce5ff")
            };

            return BaseLayout($"Ticket #{ticketNumber} Status Updated",
                icon,
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{userName}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Your support ticket status has been updated.</p>
                {InfoTable(
                    ("Ticket #", ticketNumber),
                    ("New Status", newStatus)
                )}
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Log in to view details or continue the conversation if needed.</p>",
                newStatus == "Resolved" ? "If this didn't resolve your issue, you can reopen the ticket." : null);
        }

        // ==========================================
        // USER MANAGEMENT NOTIFICATIONS
        // ==========================================

        public static string AccountBanned(string userName, string? reason)
        {
            return BaseLayout("Account Suspended",
                Icon("&#128683;", "#f8d7da"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{userName}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Your account has been <strong style=""color:#dc3545;"">suspended</strong>.</p>
                {(!string.IsNullOrEmpty(reason) ? $@"<div style=""background-color:#f8d7da; border-radius:8px; padding:14px 18px; margin:16px 0;"">
                    <p style=""margin:0; font-size:14px; color:#842029;""><strong>Reason:</strong> {reason}</p>
                </div>" : "")}
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">If you believe this was done in error, please contact our support team.</p>",
                "Contact support if you have questions about this action.");
        }

        public static string AccountActivated(string userName)
        {
            return BaseLayout("Account Reactivated! &#127881;",
                Icon("&#9989;", "#d4edda"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{userName}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Your account has been <strong style=""color:#198754;"">reactivated</strong>. You can now log in and use all platform features.</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Welcome back!</p>",
                "You now have full access to your account.");
        }

        public static string RolePromotion(string userName, string newRole)
        {
            return BaseLayout($"Role Updated to {newRole}",
                Icon("&#128081;", "#fff3cd"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{userName}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Your account role has been upgraded to <strong style=""color:#0d6efd;"">{newRole}</strong>.</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">You now have access to additional features and responsibilities. Please log out and log back in for changes to take effect.</p>",
                "Log out and log back in to access your new role features.");
        }

        // ==========================================
        // DOCUMENT NOTIFICATIONS
        // ==========================================

        public static string DocumentUploaded(string userName, int applicationId, string documentType)
        {
            return BaseLayout("New Document Uploaded",
                Icon("&#128206;", "#e8f4fd"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>Admin</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">A user has uploaded a new document for their loan application.</p>
                {InfoTable(
                    ("User", userName),
                    ("Application ID", $"#{applicationId}"),
                    ("Document Type", documentType)
                )}
                <p style=""font-size:14px; color:#6c757d;"">Please review the document in the admin panel.</p>",
                "A new document is ready for your review.");
        }

        public static string DocumentApproved(string userName, int applicationId, string documentType, string? reviewNotes)
        {
            var notesSection = !string.IsNullOrEmpty(reviewNotes)
                ? $@"<p style=""font-size:15px; color:#495057; line-height:1.6;""><strong>Reviewer Notes:</strong> {reviewNotes}</p>"
                : "";
            return BaseLayout("Document Approved &#9989;",
                Icon("&#9989;", "#d4edda"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{userName}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Great news! Your document has been <strong style=""color:#198754;"">approved</strong>.</p>
                {InfoTable(
                    ("Application ID", $"#{applicationId}"),
                    ("Document Type", documentType),
                    ("Status", "Approved")
                )}
                {notesSection}
                <p style=""font-size:14px; color:#6c757d;"">You can view the status of all your documents in your application details.</p>",
                "Your document has passed review successfully.");
        }

        public static string DocumentRejected(string userName, int applicationId, string documentType, string? reviewNotes)
        {
            var notesSection = !string.IsNullOrEmpty(reviewNotes)
                ? $@"<p style=""font-size:15px; color:#495057; line-height:1.6;""><strong>Reason:</strong> {reviewNotes}</p>"
                : "";
            return BaseLayout("Document Rejected &#10060;",
                Icon("&#10060;", "#f8d7da"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{userName}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Unfortunately, your document has been <strong style=""color:#dc3545;"">rejected</strong>.</p>
                {InfoTable(
                    ("Application ID", $"#{applicationId}"),
                    ("Document Type", documentType),
                    ("Status", "Rejected")
                )}
                {notesSection}
                <p style=""font-size:14px; color:#6c757d;"">Please upload a corrected document from your application page.</p>",
                "If you have questions about this decision, please contact support.");
        }

        // ==========================================
        // WELCOME EMAIL
        // ==========================================

        public static string WelcomeEmail(string userName)
        {
            return BaseLayout("Welcome to LoanApp! &#127881;",
                Icon("&#128075;", "#d4edda"),
                $@"<p style=""font-size:15px; color:#495057; line-height:1.6;"">Hello <strong>{userName}</strong>,</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Welcome to <strong>LoanApp</strong>! Your account has been created successfully.</p>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Here's what you can do:</p>
                <ul style=""font-size:15px; color:#495057; line-height:1.8; padding-left:20px;"">
                    <li>&#128269; Check your loan eligibility</li>
                    <li>&#128196; Apply for a loan</li>
                    <li>&#128176; Track your disbursements</li>
                    <li>&#128197; Manage your repayments</li>
                    <li>&#127384; Get support anytime</li>
                </ul>
                <p style=""font-size:15px; color:#495057; line-height:1.6;"">Get started by checking your eligibility today!</p>",
                "Need help? Our support team is here for you.");
        }
    }
}
