namespace LoanApp.Utility
{
    public static class LoanCalculator
    {
        /// <summary>
        /// Calculates EMI (Equated Monthly Installment) using the reducing balance method.
        /// Formula: EMI = P × r × (1+r)^n / ((1+r)^n − 1)
        /// </summary>
        public static (decimal MonthlyPayment, decimal TotalRepayment, decimal TotalInterest) Calculate(
            decimal loanAmount, decimal annualInterestRate, int durationMonths)
        {
            if (durationMonths <= 0 || loanAmount <= 0)
                return (0m, 0m, 0m);

            if (annualInterestRate == 0m)
            {
                decimal monthly = Math.Round(loanAmount / durationMonths, 2);
                decimal total = monthly * durationMonths;
                return (monthly, total, 0m);
            }

            // Use double only for Math.Pow, keep everything else in decimal
            double r = (double)(annualInterestRate / 100m / 12m);
            double n = durationMonths;
            double P = (double)loanAmount;

            double powerTerm = Math.Pow(1.0 + r, n);
            double emi = P * (r * powerTerm) / (powerTerm - 1.0);

            // Round only the final EMI to 2 decimal places
            decimal monthlyPayment = Math.Round((decimal)emi, 2);
            decimal totalRepayment = monthlyPayment * durationMonths;
            decimal totalInterest = totalRepayment - loanAmount;

            return (monthlyPayment, totalRepayment, totalInterest);
        }
    }
}
