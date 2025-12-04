using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Infrastructure.Persistence.Configuration
{
	public class IdempotencyRequestConfiguration : IEntityTypeConfiguration<IdempotencyRequest>
	{
		public void Configure(EntityTypeBuilder<IdempotencyRequest> b)
		{
			b.HasKey(x => x.Id);

			b.HasIndex(x => x.Key)
				.IsUnique();

			b.Property(x => x.RequestHash)
				.HasMaxLength(200)
				.IsRequired();

			b.Property(x => x.ResponseData)
				.HasColumnType("nvarchar(max)")
				.IsRequired();
		}
	}
}
