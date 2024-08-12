using System;
using System.Data;

namespace EntityFrameworkVsDapper.Data
{
	public interface ISqlConnectionFactory
	{
		IDbConnection CreateConnection();
	}
}

