namespace DotNetRevolution.Test.Domain.Account.Delegate
{
    public delegate bool CanDebitAccount(AccountAggregateRoot account, decimal amount, out string declinationReason);
}