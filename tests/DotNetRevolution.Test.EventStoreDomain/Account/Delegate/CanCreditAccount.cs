namespace DotNetRevolution.Test.EventStoreDomain.Account.Delegate
{
    public delegate bool CanCreditAccount(AccountAggregateRoot account, decimal amount, out string declinationReason);
}
