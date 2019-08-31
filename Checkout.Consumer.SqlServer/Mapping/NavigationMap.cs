using Checkout.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Consumer.SqlServer.Mapping
{
    public class NavigationMap : IEntityTypeConfiguration<Navigation>
    {
        public void Configure(EntityTypeBuilder<Navigation> builder)
        {
            builder.ToTable("Navigation");
            builder.HasKey(nav => nav.Id);

            builder.Property(nav => nav.Ip);
            builder.Property(nav => nav.PageName);
            builder.Property(nav => nav.Browser);
            builder.Property(nav => nav.Parameters);
        }
    }
}
