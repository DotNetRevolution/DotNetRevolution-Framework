declare @id UniqueIdentifier = '86873b4f-209a-4d42-a147-a60c013b3086'
      , @type varchar(512) = 'DotNetRevolution.Test.EventStoreDomain.Account.AccountAggregateRoot'
	  
exec dbo.GetDomainEvents @eventProviderId = @id, @eventProviderType = @type