declare @id UniqueIdentifier = 'aa1ce259-e135-4d8b-acd6-a60c007d3dd8'
      , @type varchar(512) = 'DotNetRevolution.Test.EventStoreDomain.Account.AccountAggregateRoot'

exec dbo.GetDomainEvents @eventProviderId = @id, @eventProviderType = @type