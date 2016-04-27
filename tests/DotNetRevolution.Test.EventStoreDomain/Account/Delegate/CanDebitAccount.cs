namespace DotNetRevolution.Test.EventStoreDomain.Account.Delegate
{
    public delegate bool CanDebitAccount(AccountAggregateRoot account, decimal amount, out string declinationReason);
}