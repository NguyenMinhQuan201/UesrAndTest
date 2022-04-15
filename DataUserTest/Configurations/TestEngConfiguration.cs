using DataUserTest.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUserTest.Configurations
{
    public class TestEngConfiguration : IEntityTypeConfiguration<TestEng>
    {
        public void Configure(EntityTypeBuilder<TestEng> builder)
        {
            builder.ToTable("TestEngs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).IsRequired(true);

        }
    }
}
