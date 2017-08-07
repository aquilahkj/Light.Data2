namespace Light.Data
{
	public interface IConnectionSetting
	{
		string ConnectionString { get; }
		string Name { get; }
		string ProviderName { get; }
		ConfigParamSet ConfigParam { get; }
	}
}
